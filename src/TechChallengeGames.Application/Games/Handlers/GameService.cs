using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Logging;
using OperationResult;
using TechChallengeGames.Application.Games.Commands;
using TechChallengeGames.Application.Games.Interfaces;
using TechChallengeGames.Domain.Dto;
using TechChallengeGames.Domain.Interfaces;
using TechChallengeGames.Domain.Models;
using TechChallengeGames.Elasticsearch;
using Result = OperationResult.Result;

namespace TechChallengeGames.Application.Games.Handlers;

public class GameService(IGameRepository repository, IUnitOfWork unitOfWork, IElasticClient<GameLog> elastic, ILogger<GameService> logger) : IGameService
{
    public async Task<IEnumerable<GameDto>> Recommendations(Guid userId)
    {
        logger.LogInformation("Generating game recommendations for user {UserId}", userId);
        var user = userId.ToString();
        var indexName = nameof(PaymentLog).ToLower();
        
        // 1. Buscar os gameIds comprados pelo usuário atual
        logger.LogInformation("Fetching purchased games for user {UserId}", userId);
        var purchasedGameIds = (await GetPurchasedGameIds(user, indexName)).ToList();
        if (purchasedGameIds.Count == 0)
        {
            logger.LogInformation("User {UserId} has no purchased games for recommendations", userId);
            return [];
        }
        
        // 2. Buscar jogos recomendados baseados em usuários similares
        logger.LogInformation("Fetching recommended games for user {UserId}", userId);
        var recommendedGameIds = (await GetRecommendedGameIds(user, indexName, purchasedGameIds)).ToList();
        if (recommendedGameIds.Count == 0)
        {
            logger.LogWarning("No recommendations were found for user {UserId}", userId);
            return [];
        }
        
        // 3. Buscar os detalhes dos jogos recomendados no repositório
        logger.LogInformation("Fetching details for recommended games for user {UserId}", userId);
        var games = repository.Find(recommendedGameIds).ToList();
        
        return games.Select(game => game.MapToDto());
    }
    
    public async Task<IEnumerable<TopGameDto>> Top10()
    {
        logger.LogInformation("Fetching Top 10 most purchased games");
        var indexName = nameof(PaymentLog).ToLower();
        
        // Buscar agregação dos gameIds mais comprados
        logger.LogInformation("Fetching Top 10 aggregation from Elasticsearch");
        var response = await elastic.Search<PaymentLog>(s => s
            .Indices(indexName)
            .Size(0)
            .Aggregations(a => a
                .Add("top_games", agg => agg
                    .Terms(t => t
                        .Field("gameId.keyword")
                        .Size(10)))));
        
        var topGameIds = new List<(Guid Id, long Count)>();
        
        logger.LogInformation("Processing Top 10 aggregation results");
        var topGamesAgg = response.Aggregations!.GetStringTerms("top_games");
        if (topGamesAgg?.Buckets is not null)
        {
            topGameIds = topGamesAgg.Buckets
                .OrderByDescending(b => b.DocCount)
                .Select(b => (Guid.Parse(b.Key.ToString()), b.DocCount))
                .ToList();
        }
        
        if (topGameIds.Count == 0)
        {
            logger.LogInformation("No games found in Top10 aggregation");
            return [];
        }
        
        // Buscar os detalhes dos jogos no repositório
        logger.LogInformation("Fetching game details for Top 10 games");
        var games = repository.Find(topGameIds.Select(g => g.Id)).ToList();
        
        // Ordenar os jogos conforme a ordem do ranking (mais comprados primeiro)
        logger.LogInformation("Ordering Top 10 games");
        var orderedGames = topGameIds
            .Select(tg => (games.FirstOrDefault(g => g.Id == tg.Id), tg.Count))
            .Where(game => game.Item1 is not null)
            .Select(game => new TopGameDto(game.Item1!.Id, game.Item1!.Name, game.Item1!.Description, game.Item1!.Category, game.Count));
        
        return orderedGames;
    }

    public GameDto? Find(Guid id)
    {
        var game = repository.Find(id);
        return game?.MapToDto();
    }

    public IEnumerable<GameDto> Find()
    {
        var games = repository.Find();
        return games.Select(game => game.MapToDto());
    }

    public async Task<Result<Guid>> CreateAsync(CreateGameRequest request)
    {
        logger.LogInformation("Creating a new game with name: {GameName}", request.Name);
        var game = (Game)request;
        repository.Add(game);
        await unitOfWork.CommitAsync(CancellationToken.None);
        logger.LogInformation("Game {GameName} created. Saving game in elasticsearch", game.Name);
        await elastic.AddOrUpdate(new GameLog(game.Id, game.Name, game.Description, game.Category), nameof(GameLog).ToLower());

        return Result.Success(game.Id);
    }

    public async Task<Result> UpdateAsync(UpdateGameRequest request)
    {
        logger.LogInformation("Updating game with ID: {GameId}", request.Id);
        var game = repository.Find(request.Id);
        if (game is null)
            return Result.Error(new Exception("Game not found."));

        logger.LogInformation("Game found. Updating details.");
        game.Update(request.Name, request.Description, request.ImageUrl, request.Category, request.ReleaseDate, request.Price);
        repository.Update(game);
        await unitOfWork.CommitAsync(CancellationToken.None);
        logger.LogInformation("Game {GameName} updated. Updating game in elasticsearch", game.Name);
        await elastic.AddOrUpdate(new GameLog(game.Id, game.Name, game.Description, game.Category), nameof(GameLog).ToLower());

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        logger.LogInformation("Deleting game with ID: {GameId}", id);
        var game = repository.Find(id);
        if (game is null)
            return Result.Error(new Exception("Game not found."));
        
        logger.LogInformation("Game found. Marking as deleted.");
        if (!game.Active)
            return Result.Error(new Exception("Game has been deleted."));

        game.MarkAsDeleted();
        repository.Update(game);
        await unitOfWork.CommitAsync(CancellationToken.None);

        return Result.Success();
    }
    
    private async Task<IEnumerable<Guid>> GetPurchasedGameIds(string userId, IndexName indexName)
    {
        var response = await elastic.Search<PaymentLog>(s => s
            .Indices(indexName)
            .Query(q => q.Term(t => t.Field("userId.keyword").Value(userId)))
            .Size(1000));
        
        return response.Documents.Select(p => p.GameId).Distinct();
    }

    private async Task<IEnumerable<Guid>> GetRecommendedGameIds(string currentUser, IndexName indexName, IList<Guid> purchasedGameIds)
    {
        var purchasedGameIdsArray = purchasedGameIds
            .Select(id => FieldValue.String(id.ToString()))
            .ToArray();
        
        // Buscar outros usuários que compraram os mesmos jogos
        var otherUsersResponse = await elastic.Search<PaymentLog>(s => s
            .Indices(indexName)
            .Query(q => q.Bool(b => b
                .Must(m => m.Terms(t => t.Field("gameId.keyword").Terms(new TermsQueryField(purchasedGameIdsArray))))
                .MustNot(mn => mn.Term(t => t.Field("userId.keyword").Value(currentUser)))))
            .Size(1000));
        
        var otherUserIds = otherUsersResponse.Documents.Select(u => u.UserId).Distinct().ToHashSet();
        
        if (otherUserIds.Count == 0)
            return [];
        
        // Buscar jogos que esses usuários compraram (excluindo os já comprados pelo usuário atual)
        var otherUserIdsArray = otherUserIds
            .Select(id => FieldValue.String(id.ToString()))
            .ToArray();
        
        var recommendationsResponse = await elastic.Search<PaymentLog>(s => s
            .Indices(indexName)
            .Query(q => q.Bool(b => b
                .Must(m => m.Terms(t => t.Field("userId.keyword").Terms(new TermsQueryField(otherUserIdsArray))))
                .MustNot(mn => mn.Terms(t => t.Field("gameId.keyword").Terms(new TermsQueryField(purchasedGameIdsArray))))))
            .Size(1000));
        
        return recommendationsResponse.Documents.Select(r => r.GameId).Distinct();
    }
}