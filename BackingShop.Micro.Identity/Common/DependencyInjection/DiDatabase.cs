using BackingShop.Database.Attendee;
using BackingShop.Database.Common;
using BackingShop.Database.GroupEvent;
using BackingShop.Database.Identity;
using BackingShop.Database.Invitation;
using BackingShop.Database.Notification;
using BackingShop.Database.PersonalEvent;

namespace BackingShop.Micro.Identity.Common.DependencyInjection;

public static class DiDatabase
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        services.AddBaseDatabase(configuration);
        services.AddUserDatabase();
        services.AddInvitationsDatabase(configuration);
        services.AddGroupEventDatabase(configuration);
        services.AddPersonalEventDatabase(configuration);
        services.AddAttendeesDatabase(configuration);
        services.AddNotificationsDatabase(configuration);
        
        return services;
    }
}