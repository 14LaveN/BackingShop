using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackingShop.Database.Common;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Common.Interceptors;
using BackingShop.Database.GroupEvent.Data.Interfaces;
using BackingShop.Database.GroupEvent.Data.Repositories;

namespace BackingShop.Database.GroupEvent;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddGroupEventDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddScoped<IGroupEventRepository, GroupEventRepository>();

        return services;
    }
}