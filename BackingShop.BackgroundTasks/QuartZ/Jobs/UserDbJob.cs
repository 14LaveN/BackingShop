using BackingShop.Database.Common;
using Quartz;
using BackingShop.Database.Identity;
using BackingShop.Domain.Identity.Entities;
using static System.Console;

namespace BackingShop.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the base database job.
/// </summary>
public sealed class BaseDbJob : IJob
{
    private readonly BaseDbContext _appDbContext = new();

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        await _appDbContext.SaveChangesAsync();
        WriteLine($"All.SaveChanges - {DateTime.UtcNow}");
    }
}