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
    where TKey : notnull
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

        return new EntityCacheItemWrapper<TEntityCacheItem>(MapToValue(entity));
    }

    protected virtual TEntityCacheItem MapToValue(TEntity entity)
    {
        if (typeof(TEntity) == typeof(TEntityCacheItem))
        {
            return entity.As<TEntityCacheItem>();
        }

        return ObjectMapper.Map<TEntity, TEntityCacheItem>(entity);
    }
}
