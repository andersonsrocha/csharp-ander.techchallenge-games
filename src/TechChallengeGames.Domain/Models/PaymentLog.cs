namespace TechChallengeGames.Domain.Models;

public class PaymentLog(Guid id, Guid userId, Guid gameId)
{
    public Guid Id { get; private set; } = id;
    public Guid UserId { get; private set; } = userId;
    public Guid GameId { get; private set; } = gameId;
}