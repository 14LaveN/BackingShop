using Microsoft.Extensions.DependencyInjection;
using BackingShop.Application.Common;
using BackingShop.Application.Core.Abstractions.Common;
using BackingShop.Application.Core.Abstractions.Helpers.JWT;
using BackingShop.Application.Core.Helpers.JWT;
using BackingShop.Application.Core.Helpers.Metric;

namespace BackingShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentException();

        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<CreateMetricsHelper>();
        services.AddScoped<IDateTime, MachineDateTime>();
        
        return services;
    }
}