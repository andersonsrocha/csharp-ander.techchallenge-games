using OperationResult;
using TechChallengeGames.Application.Games.Commands;
using TechChallengeGames.Domain.Dto;

namespace TechChallengeGames.Application.Games.Interfaces;

public interface IGameService : IService
{
    GameDto? Find(Guid id);
    IEnumerable<GameDto> Find();
    Task<Result<Guid>> CreateAsync(CreateGameRequest request);
    Task<Result> UpdateAsync(UpdateGameRequest request);
    Task<Result> DeleteAsync(Guid id);
}