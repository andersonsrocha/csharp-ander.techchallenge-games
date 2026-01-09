using OperationResult;
using TechChallengeGames.Application.RabbitMQ.Commands;

namespace TechChallengeGames.Application.RabbitMQ.Interfaces;

public interface IRabbitMqService : IService
{
    Task<Result> SendPurchaseAsync(PurchaseGameRequest request);
}