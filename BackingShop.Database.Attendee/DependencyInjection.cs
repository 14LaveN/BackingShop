using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetNetwork.Database.Attendee.Data.Interfaces;
using AspNetNetwork.Database.Attendee.Data.Repositories;
using AspNetNetwork.Database.Common;
using AspNetNetwork.Database.Common.Abstractions;

namespace AspNetNetwork.Database.Attendee;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddAttendeesDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IUnitOfWork<Domain.Identity.Entities.Attendee>, UnitOfWork<Domain.Identity.Entities.Attendee>>();

        return services;
    }
}