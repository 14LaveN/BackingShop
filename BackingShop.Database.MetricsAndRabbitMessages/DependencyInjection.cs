using BackingShop.Application.Core.Settings;
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

        services.AddHealthChecks()
            .AddMongoDb(configuration.GetConnectionString("MongoConnection")!);

        return services;
    }
}