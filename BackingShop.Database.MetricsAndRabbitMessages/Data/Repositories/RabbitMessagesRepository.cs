using BackingShop.Application.Core.Settings;
using BackingShop.Database.MetricsAndRabbitMessages.Data.Interfaces;
using BackingShop.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BackingShop.Database.MetricsAndRabbitMessages.Data.Repositories;

/// <summary>
/// Represents the generic metrics repository class.
/// </summary>
public sealed class RabbitMessagesRepository
    : IMongoRepository<RabbitMessage>
{
    private readonly IMongoCollection<RabbitMessage> _rabbitMessagesCollection;

    /// <summary>
    /// Create new instance of <see cref="RabbitMessagesRepository"/>.
    /// </summary>
    /// <param name="dbSettings"></param>
    public RabbitMessagesRepository(
        IOptions<MongoSettings> dbSettings)
    {
        var mongoClient = new MongoClient(
            dbSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            dbSettings.Value.Database);
        
        _rabbitMessagesCollection = mongoDatabase.GetCollection<RabbitMessage>(
            dbSettings.Value.RabbitMessagesCollectionName);
    }

    /// <inheritdoc />
    public async Task<List<RabbitMessage>> GetAllAsync() =>
        await _rabbitMessagesCollection.Find(_ => true).ToListAsync();

    /// <inheritdoc />
    public async Task InsertAsync(RabbitMessage type) =>
        await _rabbitMessagesCollection.InsertOneAsync(type);

    /// <inheritdoc />
    public async Task InsertRangeAsync(IEnumerable<RabbitMessage> types) =>
        await _rabbitMessagesCollection.InsertManyAsync(types);

    /// <inheritdoc />
    public async Task RemoveAsync(string id) =>
        await _rabbitMessagesCollection.DeleteOneAsync(x => x.Id == id);
}