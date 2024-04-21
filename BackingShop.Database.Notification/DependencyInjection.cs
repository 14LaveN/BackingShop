using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.Database.Common;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Notification.Data.Interfaces;
using BackingShop.Database.Notification.Data.Repositories;

namespace BackingShop.Database.Notification;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddNotificationsDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddTransient<BaseDbContext>();
        
        services.AddScoped<INotificationRepository, NotificationRepository>();

        return services;
    }
}