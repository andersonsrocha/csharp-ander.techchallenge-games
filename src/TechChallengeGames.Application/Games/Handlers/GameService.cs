using OperationResult;
using TechChallengeGames.Application.Games.Commands;
using TechChallengeGames.Application.Games.Interfaces;
using TechChallengeGames.Domain.Dto;
using TechChallengeGames.Domain.Interfaces;
using TechChallengeGames.Domain.Models;

namespace TechChallengeGames.Application.Games.Handlers;

public class GameService(IGameRepository repository, IUnitOfWork unitOfWork) : IGameService
{
    public GameDto? Find(Guid id)
    {
        var game = repository.Find(id);
        if (game is null)
            return null;

        return new GameDto(game.Id, game.Active, game.CreatedIn, game.UpdatedIn, game.Name, game.Description, game.ImageUrl, game.Category, game.ReleaseDate);
    }

    public IEnumerable<GameDto> Find()
    {
        var games = repository.Find();
        return games.Select(game => new GameDto(game.Id, game.Active, game.CreatedIn, game.UpdatedIn, game.Name, game.Description, game.ImageUrl, game.Category, game.ReleaseDate));
    }

    public async Task<Result<Guid>> CreateAsync(CreateGameRequest request)
    {
        var game = (Game)request;
        repository.Add(game);
        await unitOfWork.CommitAsync(CancellationToken.None);
        
        return Result.Success(game.Id);
    }

    public async Task<Result> UpdateAsync(UpdateGameRequest request)
    {
        var game = repository.Find(request.Id);
        if (game is null)
            return Result.Error(new Exception("Game not found."));
        
        game.Update(request.Name, request.Description, request.ImageUrl, request.Category, request.ReleaseDate);
        repository.Update(game);
        await unitOfWork.CommitAsync(CancellationToken.None);
        
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var game = repository.Find(id);
        if (game is null)
            return Result.Error(new Exception("Game not found."));
        
        if (!game.Active)
            return Result.Error(new Exception("Game has been deleted."));
        
        game.MarkAsDeleted();
        repository.Update(game);
        await unitOfWork.CommitAsync(CancellationToken.None);
        
        return Result.Success();
    }
}