using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Domain.Entities.Caching;

public interface IEntityCache<TEntityCacheItem, in TKey>
    where TEntityCacheItem : class
{
    /// <summary>
    /// Gets the entity with given <paramref name="id"/>,
    /// or returns null if the entity was not found.
    /// </summary>
    [ItemCanBeNull] 
    Task<TEntityCacheItem> FindAsync(TKey id);
 
    /// <summary>
    /// Gets the entity with given <paramref name="id"/>,
    /// or throws <see cref="EntityNotFoundException"/> if the entity was not found.
    /// </summary>
    [ItemNotNull] 
    Task<TEntityCacheItem> GetAsync(TKey id);
}