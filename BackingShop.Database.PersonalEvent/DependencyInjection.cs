using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.Database.Common;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.PersonalEvent.Data.Interfaces;
using BackingShop.Database.PersonalEvent.Data.Repositories;

namespace BackingShop.Database.PersonalEvent;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddPersonalEventDatabase(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IPersonalEventRepository, PersonalEventRepository>();

        return services;
    }
}