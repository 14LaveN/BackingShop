using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using BackingShop.BackgroundTasks.QuartZ.Jobs;

namespace BackingShop.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the save metrics scheduler class.
/// </summary>
public sealed class SaveMetricsScheduler
    : AbstractScheduler<SaveMetricsJob>
{
    /// <summary>
    /// Starts the job.
    /// </summary>
    public override async void Start(IServiceCollection serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<SaveMetricsJob>()
            .WithIdentity("SaveMetricsJob")
            .StoreDurably()
            .Build();
        
        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{nameof(SaveMetricsJob)}Trigger", "default")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(300)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}