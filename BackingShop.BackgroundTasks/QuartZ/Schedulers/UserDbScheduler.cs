using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using BackingShop.BackgroundTasks.QuartZ.Jobs;

namespace BackingShop.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the user database scheduler class.
/// </summary>
public sealed class UserDbScheduler
    : AbstractScheduler<BaseDbJob>;