using BackingShop.Application.Core.Behaviours;
using BackingShop.Micro.Identity.Mediatr.Behaviors;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangeName;
using BackingShop.Micro.Identity.Mediatr.Commands.ChangePassword;
using BackingShop.Micro.Identity.Mediatr.Commands.Login;
using BackingShop.Micro.Identity.Mediatr.Commands.Register;
using MediatR.NotificationPublishers;

namespace BackingShop.Micro.Identity.Common.DependencyInjection;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
                typeof(RegisterCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
                typeof(LoginCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangePasswordCommand).Assembly,
                typeof(ChangePasswordCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangeNameCommand).Assembly,
                typeof(ChangeNameCommandHandler).Assembly);
            
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.AddOpenBehavior(typeof(UserTransactionBehavior<,>));
            x.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            x.AddOpenBehavior(typeof(MetricsBehaviour<,>));
            
            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        return services;
    }
}