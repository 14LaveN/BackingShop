using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.RabbitMq.Messaging;
using BackingShop.RabbitMq.Messaging.Settings;

namespace BackingShop.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
        
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHealthChecks()
            .AddRabbitMQ(new Uri(MessageBrokerSettings.AmqpLink));
        
        return services; 
    }
}