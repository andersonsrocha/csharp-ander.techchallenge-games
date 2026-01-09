using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OperationResult;
using RabbitMQ.Client;
using TechChallengeGames.Application.RabbitMQ.Commands;
using TechChallengeGames.Application.RabbitMQ.Interfaces;

namespace TechChallengeGames.Application.RabbitMQ.Handlers;

public class RabbitMqService(ILogger<RabbitMqService> logger, IConfiguration configuration) : IRabbitMqService
{
    private readonly string _hostName = configuration["RabbitMQ:HostName"] ?? "localhost";
    private readonly string _userName = configuration["RabbitMQ:UserName"] ?? "admin";
    private readonly string _password = configuration["RabbitMQ:Password"] ?? "";
    private readonly string _queueName = configuration["RabbitMQ:QueueName"] ?? "queue";

    public async Task<Result> SendPurchaseAsync(PurchaseGameRequest request)
    {
        logger.LogInformation("Creating a new payment for game: {GameId}", request.GameId);
        logger.LogInformation("Creating a RabbitMQ Connection with HostName = {HostName}, UserName = {UserName}, Password = {Password}, QueueName = {QueueName}", _hostName, _userName, _password, _queueName);
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password,
        };

        await using var connection = await factory.CreateConnectionAsync();
        logger.LogInformation("Creating a RabbitMQ Channel");
        await using var channel = await connection.CreateChannelAsync();
        logger.LogInformation("Declaring queue with name: {Name}", _queueName);
        await channel.QueueDeclareAsync(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        logger.LogInformation("Serialize request data");
        var message = JsonSerializer.Serialize(request);
        logger.LogInformation("Get bytes from message: {Message}", message);
        var body = Encoding.UTF8.GetBytes(message);
        logger.LogInformation("Sending message: {Message}", message);
        await channel.BasicPublishAsync(exchange: "", routingKey: _queueName, body: body);
        
        return Result.Success();
    }
}