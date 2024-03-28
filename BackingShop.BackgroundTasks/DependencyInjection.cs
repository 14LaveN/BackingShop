using System.Reflection;
using Microsoft.Extensions.Configuration;
using AspNetNetwork.BackgroundTasks.QuartZ;
using AspNetNetwork.BackgroundTasks.QuartZ.Jobs;
using AspNetNetwork.BackgroundTasks.QuartZ.Schedulers;
using AspNetNetwork.BackgroundTasks.Services;
using AspNetNetwork.BackgroundTasks.Settings;
using AspNetNetwork.BackgroundTasks.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace AspNetNetwork.BackgroundTasks;

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

        //TODO services.Configure<BackgroundTaskSettings>(configuration.GetSection(BackgroundTaskSettings.SettingsKey);
        
        services.AddHostedService<GroupEventNotificationsProducerBackgroundService>();

        services.AddHostedService<PersonalEventNotificationsProducerBackgroundService>();

        services.AddHostedService<EmailNotificationConsumerBackgroundService>();

        services.AddHostedService<IntegrationEventConsumerBackgroundService>();


        services.AddScoped<IPersonalEventNotificationsProducer, PersonalEventNotificationsProducer>();

        services.AddScoped<IEmailNotificationsConsumer, EmailNotificationsConsumer>();

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(SaveMetricsJob));

            configure
                .AddJob<SaveMetricsJob>(jobKey);
            
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        services.AddSingleton(_ =>
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            
            return scheduler;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        services.AddSingleton(_ =>
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            
            return scheduler;
        });
        services.AddTransient<UserDbScheduler>();
        services.AddTransient<SaveMetricsScheduler>();
        
        var scheduler = new SaveMetricsScheduler();
        scheduler.Start(services);
        
        return services;
    }
}