using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.BackgroundTasks.Services;
using BackingShop.BackgroundTasks.Tasks;
using BackingShop.RabbitMq.Messaging.Settings;
using Microsoft.Extensions.Configuration;

namespace BackingShop.BackgroundTasks;

public static class DiIntegrationEvent
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddRabbitBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();
        
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingsKey));

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        return services;
    }
}