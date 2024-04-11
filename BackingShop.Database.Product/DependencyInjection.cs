using BackingShop.Database.Common;
using BackingShop.Database.Common.Abstractions;
using BackingShop.Database.Product.Data.Interfaces;
using BackingShop.Database.Product.Data.Repositories;
using BackingShop.Domain.Core.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackingShop.Database.Product;

/// <summary>
/// Represents the product database dependency injection static class.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddProductDatabase(this IServiceCollection services)
    {
        Ensure.NotNull(services, "Services is required.", nameof(services));
        
        services.AddScoped<IProductsRepository, ProductsRepository>();
        
        return services;
    }
}