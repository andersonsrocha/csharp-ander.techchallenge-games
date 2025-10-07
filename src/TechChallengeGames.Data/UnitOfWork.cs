using TechChallengeGames.Domain.Interfaces;

namespace TechChallengeGames.Data;

public sealed class UnitOfWork(TechChallengeGamesContext usersContext) : IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        => await usersContext.SaveChangesAsync(cancellationToken) > 0;
}