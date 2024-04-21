using BackingShop.Domain.Identity.Events.User;
using BackingShop.Domain.Product.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.RabbitMq.Messaging;
using BackingShop.RabbitMq.Messaging.Product.ProductCreated;
using BackingShop.RabbitMq.Messaging.Settings;
using BackingShop.RabbitMq.Messaging.User.Events.UserCreated;

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

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(typeof(UserCreatedDomainEvent).Assembly,
                typeof(PublishIntegrationEventOnUserCreatedDomainEventHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ProductCreatedDomainEvent).Assembly,
                typeof(PublishIntegrationEventOnProductCreatedDomainEventHandler).Assembly);
        });
        
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));
        
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHealthChecks()
            .AddRabbitMQ(new Uri(MessageBrokerSettings.AmqpLink));
        
        return services; 
    }
}