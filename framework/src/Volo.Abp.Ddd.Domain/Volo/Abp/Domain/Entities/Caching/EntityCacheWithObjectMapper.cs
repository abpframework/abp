using System;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Caching;

public class EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey> :
    EntityCacheBase<TEntity, TEntityCacheItem, TKey>
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
{
    protected IObjectMapper ObjectMapper { get; }

    public EntityCacheWithObjectMapper(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<EntityCacheItemWrapper<TEntityCacheItem>, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager,
        IObjectMapper objectMapper)
        : base(repository, cache, unitOfWorkManager)
    {
        ObjectMapper = objectMapper;
    }

    protected override EntityCacheItemWrapper<TEntityCacheItem>? MapToCacheItem(TEntity? entity)
    {
        if (entity == null)
        {
            return null;
        }

        if (typeof(TEntity) == typeof(TEntityCacheItem))
        {
            return new EntityCacheItemWrapper<TEntityCacheItem>(entity.As<TEntityCacheItem>());
        }

        return new EntityCacheItemWrapper<TEntityCacheItem>(ObjectMapper.Map<TEntity, TEntityCacheItem>(entity));
    }
}
