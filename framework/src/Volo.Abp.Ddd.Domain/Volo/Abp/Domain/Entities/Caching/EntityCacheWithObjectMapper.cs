using System;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Caching;

public class EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey> :
    EntityCacheBase<TEntity, TEntityCacheItem, TKey> 
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
{
    protected IObjectMapper ObjectMapper { get; }

    public EntityCacheWithObjectMapper(
        IReadOnlyRepository<TEntity, TKey> repository, 
        IDistributedCache<TEntityCacheItem, TKey> cache,
        IObjectMapper objectMapper)
        : base(
            repository,
            cache)
    {
        ObjectMapper = objectMapper;
    }

    protected override TEntityCacheItem MapToCacheItem(TEntity entity)
    {
        if (typeof(TEntity) == typeof(TEntityCacheItem))
        {
            return entity.As<TEntityCacheItem>();
        }

        return ObjectMapper.Map<TEntity, TEntityCacheItem>(entity);
    }
}