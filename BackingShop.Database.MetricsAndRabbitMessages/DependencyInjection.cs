using BackingShop.Application.Core.Settings;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Interfaces;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;
using BackingShop.Domain.Common.Entities;
using BackingShop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackingShop.Database.MetricsAndRabbitMessages;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMongoDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.Configure<MongoSettings>(configuration.GetSection(MongoSettings.MongoSettingsKey));

        services.AddSingleton<IMetricsRepository, MetricsRepository>();
        services.AddScoped<IMongoRepository<RabbitMessage>, RabbitMessagesRepository>();

        services.AddTransient<MongoSettings>();
        
        services.AddHealthChecks()
            .AddMongoDb(configuration.GetConnectionString("MongoConnection")!);

        return services;
    }
}