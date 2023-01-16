using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Caching;

public class EntityCacheWithObjectMapperContext<TObjectMapperContext, TEntity, TEntityCacheItem, TKey> :
    EntityCacheWithObjectMapper<TEntity, TEntityCacheItem, TKey>
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
{
    public EntityCacheWithObjectMapperContext(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<TEntityCacheItem, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager,
        IObjectMapper objectMapper)// Intentionally injected with TContext
        : base(repository, cache, unitOfWorkManager, objectMapper)
    {

    }
}
