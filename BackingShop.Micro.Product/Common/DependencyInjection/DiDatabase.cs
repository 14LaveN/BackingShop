using BackingShop.Database.Attendee;
using BackingShop.Database.Common;
using BackingShop.Database.GroupEvent;
using BackingShop.Database.Identity;
using BackingShop.Database.Invitation;
using BackingShop.Database.MetricsAndRabbitMessages;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Interfaces;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;
using BackingShop.Database.Notification;
using BackingShop.Database.PersonalEvent;
using BackingShop.Database.Product;
using Firebase.Storage;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace BackingShop.Micro.Product.Common.DependencyInjection;

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

        services.AddMongoDatabase(configuration);
        services.AddScoped<IMetricsRepository, MetricsRepository>();
        services.AddBaseDatabase(configuration);
        services.AddProductDatabase();
        services.AddUserDatabase();
        services.AddAttendeesDatabase();
        services.AddGroupEventDatabase();
        services.AddPersonalEventDatabase();
        services.AddInvitationsDatabase();
        services.AddNotificationsDatabase();
        
        string pathToFirebaseConfig = @"G:\DotNetProjects\BackingShop\firebase.json";

        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(pathToFirebaseConfig),
        });
        
        return services;
    }
}