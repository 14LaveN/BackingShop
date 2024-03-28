using AspNetNetwork.Application.Core.Settings;
using AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;
using AspNetNetwork.Domain.Common.Core.Primitives.Maybe;
using AspNetNetwork.Domain.Common.Entities;
using MongoDB.Driver;

namespace BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
public sealed class MetricsRepository
    : IMongoRepository<MetricEntity>, IMetricsRepository
{
    private readonly IMongoCollection<MetricEntity> _metricsCollection;

    /// <summary>
    /// Create new instance of metrics repository.
    /// </summary>
    /// <param name="dbSettings">The mongo database settings</param>
    public MetricsRepository(
        MongoSettings dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Database);
        
        _metricsCollection = mongoDatabase.GetCollection<MetricEntity>(
            dbSettings.MetricsCollectionName);
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