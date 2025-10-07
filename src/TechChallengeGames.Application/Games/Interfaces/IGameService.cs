using OperationResult;
using TechChallengeGames.Application.Games.Commands;
using TechChallengeGames.Domain.Dto;

namespace TechChallengeGames.Application.Games.Interfaces;

public interface IGameService : IService
{
    Task<IEnumerable<GameDto>> Recommendations(Guid userId);
    Task<IEnumerable<TopGameDto>> Top10();
    GameDto? Find(Guid id);
    IEnumerable<GameDto> Find();
    Task<Result<Guid>> CreateAsync(CreateGameRequest request);
    Task<Result> UpdateAsync(UpdateGameRequest request);
    Task<Result> DeleteAsync(Guid id);
}