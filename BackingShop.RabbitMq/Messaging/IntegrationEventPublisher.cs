using System.Text;
using System.Text.Json;
using BackingShop.Application.Core.Abstractions.Messaging;
using BackingShop.RabbitMq.Messaging.Settings;
using RabbitMQ.Client;

namespace BackingShop.RabbitMq.Messaging;

/// <summary>
/// Represents the integration event publisher.
/// </summary>
public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    /// <summary>
    /// Initialize connection.
    /// </summary>
    /// <returns>Returns connection to RabbitMQ.</returns> 
    private static async Task<IConnection> CreateConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            Uri = new Uri(MessageBrokerSettings.AmqpLink)
        };

        var connection = await connectionFactory.CreateConnectionAsync();

        return connection;
    }

    /// <inheritdoc />
    public async Task Publish(IIntegrationEvent integrationEvent)
    {
        using var connection = await CreateConnection();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(MessageBrokerSettings.QueueName, false, false, false);

        await channel.ExchangeDeclareAsync(MessageBrokerSettings.QueueName + "Exchange", ExchangeType.Direct, durable: false);
        
        await channel.QueueBindAsync(MessageBrokerSettings.QueueName,
            exchange: MessageBrokerSettings.QueueName + "Exchange",
            routingKey: MessageBrokerSettings.QueueName);

        string payload = JsonSerializer.Serialize(integrationEvent, typeof(IIntegrationEvent));

        var body = Encoding.UTF8.GetBytes(payload);

        if (MessageBrokerSettings.QueueName is not null)
            await channel.BasicPublishAsync(MessageBrokerSettings.QueueName + "Exchange",
                MessageBrokerSettings.QueueName, body: body);
    }
}