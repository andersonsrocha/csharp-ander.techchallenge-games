namespace TechChallengeGames.Application.RabbitMQ.Commands;

public class PurchaseGameRequest
{
    public Guid GameId { get; init; } = Guid.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
    public double Price { get; init; } = 0;
}