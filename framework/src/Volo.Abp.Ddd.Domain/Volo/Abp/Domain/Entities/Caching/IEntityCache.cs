using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities.Caching;

public interface IEntityCache<TEntityCacheItem, TKey>
    where TEntityCacheItem : class
    where TKey : notnull
{
    /// <summary>
    /// Gets the entity with given <paramref name="id"/>,
    /// or returns null if the entity was not found.
    /// </summary>
    Task<TEntityCacheItem?> FindAsync(TKey id);

    /// <summary>
    /// Gets multiple entities with the given <paramref name="ids"/>.
    /// Returns a list where each entry corresponds to the given id in the same order.
    /// An entry will be null if the entity was not found for the corresponding id.
    /// </summary>
    Task<List<TEntityCacheItem?>> FindManyAsync(IEnumerable<TKey> ids);

    /// <summary>
    /// Gets multiple entities with the given <paramref name="ids"/> as a dictionary keyed by id.
    /// An entry will be null if the entity was not found for the corresponding id.
    /// </summary>
    Task<Dictionary<TKey, TEntityCacheItem?>> FindManyAsDictionaryAsync(IEnumerable<TKey> ids);

    /// <summary>
    /// Gets the entity with given <paramref name="id"/>,
    /// or throws <see cref="EntityNotFoundException"/> if the entity was not found.
    /// </summary>
    [ItemNotNull]
    Task<TEntityCacheItem> GetAsync(TKey id);

    /// <summary>
    /// Gets multiple entities with the given <paramref name="ids"/>.
    /// Returns a list where each entry corresponds to the given id in the same order.
    /// Throws <see cref="EntityNotFoundException"/> if any entity was not found.
    /// </summary>
    Task<List<TEntityCacheItem>> GetManyAsync(IEnumerable<TKey> ids);

    /// <summary>
    /// Gets multiple entities with the given <paramref name="ids"/> as a dictionary keyed by id.
    /// Throws <see cref="EntityNotFoundException"/> if any entity was not found.
    /// </summary>
    Task<Dictionary<TKey, TEntityCacheItem>> GetManyAsDictionaryAsync(IEnumerable<TKey> ids);
}
