using AspNetNetwork.Domain.Common.Entities;
using BackingShop.Domain.Common.Entities;

namespace AspNetNetwork.Database.MetricsAndMessages.Data.Interfaces;

/// <summary>
/// Represents the generic mongo repository interface.
/// </summary>
/// <typeparam name="T">The <see cref="BaseMongoEntity"/> type.</typeparam>
internal interface IMongoRepository<T> 
    where T : BaseMongoEntity
{
    /// <summary>
    /// Get all mongo entity async.
    /// </summary>
    /// <returns>List by <see cref="BaseMongoEntity"/> classes.</returns>
    Task<List<T>> GetAllAsync();
  
    /// <summary>
    /// Insert in database the entity.
    /// </summary>
    /// <param name="type">The generic type.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
    Task InsertAsync(T type);
    
    /// <summary>
    /// Insert any entities in database.
    /// </summary>
    /// <param name="types">The enumerable of generic types classes.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
    Task InsertRangeAsync(IEnumerable<T> types);
  
    /// <summary>
    /// Remove from database the entity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>.</returns>
    Task RemoveAsync(string id);
}