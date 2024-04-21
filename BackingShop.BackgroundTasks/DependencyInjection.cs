using System.Reflection;
using Microsoft.Extensions.Configuration;
using BackingShop.BackgroundTasks.QuartZ;
using BackingShop.BackgroundTasks.QuartZ.Jobs;
using BackingShop.BackgroundTasks.QuartZ.Schedulers;
using BackingShop.BackgroundTasks.Services;
using BackingShop.BackgroundTasks.Settings;
using BackingShop.BackgroundTasks.Tasks;
using BackingShop.Database.MetricsAndRabbitMessages;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Interfaces;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace BackingShop.BackgroundTasks;

public static class BDependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey));
        
        //TODO 
        //TODO services.AddHostedService<GroupEventNotificationsProducerBackgroundService>();
//TODO 
        //TODO services.AddHostedService<PersonalEventNotificationsProducerBackgroundService>();
//TODO 
        //TODO services.AddHostedService<EmailNotificationConsumerBackgroundService>();
//TODO 
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();
        services.AddHostedService<SaveMetricsBackgroundService>();
//TODO 
        //TODO services.AddScoped<IGroupEventNotificationsProducer, GroupEventNotificationsProducer>();
        //TODO 
        //TODO services.AddScoped<IPersonalEventNotificationsProducer, PersonalEventNotificationsProducer>();
//TODO 
        //TODO services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();
//TODO 
        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        services.AddQuartz(configure =>
        {
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        
        services.AddTransient<UserDbScheduler>();
        
        var scheduler = new UserDbScheduler();
        //scheduler.Start(services);

        services.AddSingleton<IMetricsRepository, MetricsRepository>();
        
        return services;
    }
}