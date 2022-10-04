using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.Domain.Entities.Caching;

public class EntityCacheWithoutCacheItem<TEntity, TKey> :
    EntityCacheBase<TEntity, TEntity, TKey> 
    where TEntity : Entity<TKey>
{
    public EntityCacheWithoutCacheItem(
        IReadOnlyRepository<TEntity, TKey> repository, 
        IDistributedCache<TEntity, TKey> cache)
        : base(
            repository,
            cache)
    {
        
    }

    protected override TEntity MapToCacheItem(TEntity entity)
    {
        return entity;
    }
}