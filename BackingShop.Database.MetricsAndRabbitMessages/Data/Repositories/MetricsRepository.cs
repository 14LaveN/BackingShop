using BackingShop.Application.Core.Settings;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Interfaces;
using BackingShop.Domain.Common.Core.Primitives.Maybe;
using BackingShop.Domain.Common.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
public sealed class MetricsRepository
    : IMongoRepository<MetricEntity>, IMetricsRepository
{
    private readonly IMongoCollection<MetricEntity> _metricsCollection;
    private readonly MongoSettings _mongoSettings;
    
    /// <summary>
    /// Create new instance of metrics repository.
    /// </summary>
    /// <param name="dbSettings">The mongo database settings</param>
    public MetricsRepository(
        IOptions<MongoSettings> dbSettings)
    {
        _mongoSettings = dbSettings.Value;
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);
        
        _metricsCollection = mongoDatabase.GetCollection<MetricEntity>(
            dbSettings.Value.MetricsCollectionName);
    }

    /// <inheritdoc />
    public async Task<List<MetricEntity>> GetAllAsync() =>
        await _metricsCollection.Find(_ => true).ToListAsync();

    /// <inheritdoc />
    public async Task InsertAsync(MetricEntity type) =>
        await _metricsCollection.InsertOneAsync(type);

    /// <inheritdoc />
    public async Task<Maybe<List<MetricEntity>>> GetMetricsByTime(
        string metricName,
        int time)
    {
        var filter = Builders<MetricEntity>.Filter.And(
            Builders<MetricEntity>.Filter.Eq(x => x.Name, metricName),
            Builders<MetricEntity>.Filter.Gt(x => x.CreatedAt, DateTime.UtcNow.AddMinutes(time)));
        
        var metrics = await _metricsCollection.FindAsync<MetricEntity>(filter).Result.ToListAsync();

        return metrics;
    }

    /// <inheritdoc />
    public async Task InsertRangeAsync(IEnumerable<MetricEntity> types) =>
        await _metricsCollection.InsertManyAsync(types);

    /// <inheritdoc />
    public async Task RemoveAsync(string id) =>
        await _metricsCollection.DeleteOneAsync(x => x.Id == id);
}