using System.Globalization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Prometheus;
using Quartz;
using Quartz.Util;
using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Cache.Service;
using AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Exceptions;
using AspNetNetwork.Domain.Common.Entities;
using Microsoft.Extensions.Logging;

namespace AspNetNetwork.BackgroundTasks.QuartZ.Jobs;

/// <summary>
/// Represents the save metrics job class.
/// </summary>
public sealed class SaveMetricsJob
    : IJob
{
    private readonly IMetricsRepository _metricsCollection;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<SaveMetricsJob> _logger;

    /// <summary>
    /// Initialize a new instance of the <see cref="SaveMetricsJob"/>
    /// </summary>
    /// <param name="metricsCollection">The metrics collection.</param>
    /// <param name="distributedCache">The distributed cache.</param>
    /// <param name="logger">The logger.</param>
    public SaveMetricsJob(
        IMetricsRepository metricsCollection,
        IDistributedCache distributedCache,
        ILogger<SaveMetricsJob> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
        _metricsCollection = metricsCollection;
    }
    
    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Request to save metrics in {nameof(SaveMetricsJob)}.");

            string counterName = "metrics_counter-key";
            
            Counter counter = await _distributedCache
                .GetRecordAsync<Counter>(counterName);

            string histogramName = "metrics_request_duration_seconds-key";
            
            string millisecondsInString =
                await _distributedCache.GetRecordAsync<string>(histogramName);
            
            if (counter is null )
            {
                _logger.LogWarning($"Counter with same name - {counterName} not found");
                throw new NotFoundException(nameof(Counter), counterName);
            }

            if (millisecondsInString.IsNullOrWhiteSpace())
            {
                _logger.LogWarning($"Histogram with same name - {histogramName} not found");
                throw new NotFoundException("Milliseconds", histogramName);
            }
            
            var metrics = new List<MetricEntity>
            { 
                new("BackingShop_request_duration_seconds",
                    millisecondsInString),
                new(counter.Name,
                    counter.Value.ToString(CultureInfo.CurrentCulture))
            };
            
            await _metricsCollection.InsertRangeAsync(metrics);

            _logger.LogInformation($"Insert in MongoDb metrics at - {DateTime.UtcNow}");
        }
        catch (Exception exception)
        {
            _logger.LogError($"[SaveMetricsJob]: {exception.Message}");
            throw;
        }
    }
}