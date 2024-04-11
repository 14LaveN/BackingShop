using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Entities;

namespace BackingShop.Database.MetricsAndMessages.Data.Interfaces;

/// <summary>
/// Represents the metrics repository interface.
/// </summary>
public interface IMetricsRepository
{
    /// <summary>
    /// Insert in database the metric entity.
    /// </summary>
    /// <param name="metricEntity">The metric entity.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
    Task InsertAsync(MetricEntity metricEntity);

    /// <summary>
    /// Get metrics entity by time.
    /// </summary>
    /// <param name="metricName">The metric name.</param>
    /// <param name="time">The time.</param>
    /// <returns>List by <see cref="MetricEntity"/> classes.</returns>
    Task<Maybe<List<MetricEntity>>> GetMetricsByTime(
        string metricName,
        int time);
    
    /// <summary>
    /// Insert any metrics entities in database.
    /// </summary>
    /// <param name="metrics">The enumerable of metrics classes.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
    Task InsertRangeAsync(IEnumerable<MetricEntity> metrics);
}