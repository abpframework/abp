using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Caching;

public abstract class EntityCacheBase<TEntity, TEntityCacheItem, TKey> :
    IEntityCache<TEntityCacheItem, TKey>,
    ILocalEventHandler<EntityChangedEventData<TEntity>>
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
{
    protected IReadOnlyRepository<TEntity, TKey> Repository { get; }
    protected IDistributedCache<TEntityCacheItem, TKey> Cache { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected EntityCacheBase(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<TEntityCacheItem, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager)
    {
        Repository = repository;
        Cache = cache;
        UnitOfWorkManager = unitOfWorkManager;
    }
    
    public virtual async Task<TEntityCacheItem> FindAsync(TKey id)
    {
        return await Cache.GetOrAddAsync(
            id,
            async () => MapToCacheItem(await Repository.FindAsync(id))
        );
    }

    public virtual async Task<TEntityCacheItem> GetAsync(TKey id)
    {
        return await Cache.GetOrAddAsync(
            id,
            async () => MapToCacheItem(await Repository.GetAsync(id))
        );
    }

    protected abstract TEntityCacheItem MapToCacheItem(TEntity entity);
    
    public async Task HandleEventAsync(EntityChangedEventData<TEntity> eventData)
    {
        if (eventData is EntityCreatedEventData<TEntity>)
        {
            return;
        }
        
        /* Why we are using double remove:
         * First Cache.RemoveAsync drops the cache item in a unit of work.
         * Some other application / thread may read the value from database and put it to the cache again
         * before the UOW completes.
         * The second Cache.RemoveAsync drops the cache item after the database transaction is complete.
         * Only the second Cache.RemoveAsync may not be enough if the application crashes just after the UOW completes.
         */
        
        await Cache.RemoveAsync(eventData.Entity.Id);
        
        if(UnitOfWorkManager.Current != null)
        {
            await Cache.RemoveAsync(eventData.Entity.Id, considerUow: true);
        }
    }
}