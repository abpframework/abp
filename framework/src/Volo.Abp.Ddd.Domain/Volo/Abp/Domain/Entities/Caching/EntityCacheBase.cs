using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Caching;

public abstract class EntityCacheBase<TEntity, TEntityCacheItem, TKey> :
    IEntityCache<TEntityCacheItem, TKey>,
    ILocalEventHandler<EntityChangedEventData<TEntity>>
    where TEntity : Entity<TKey>
    where TEntityCacheItem : class
    where TKey : notnull
{
    protected IReadOnlyRepository<TEntity, TKey> Repository { get; }
    protected IDistributedCache<EntityCacheItemWrapper<TEntityCacheItem>, TKey> Cache { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected EntityCacheBase(
        IReadOnlyRepository<TEntity, TKey> repository,
        IDistributedCache<EntityCacheItemWrapper<TEntityCacheItem>, TKey> cache,
        IUnitOfWorkManager unitOfWorkManager)
    {
        Repository = repository;
        Cache = cache;
        UnitOfWorkManager = unitOfWorkManager;
    }

    public virtual async Task<TEntityCacheItem?> FindAsync(TKey id)
    {
        return (await Cache.GetOrAddAsync(
            id,
            async () =>
            {
                if (HasObjectExtensionInfo())
                {
                    Repository.EnableTracking();
                }

                return MapToCacheItem(await Repository.FindAsync(id))!;
            }))?.Value;
    }

    public virtual async Task<List<TEntityCacheItem?>> FindManyAsync(IEnumerable<TKey> ids)
    {
        var idArray = ids.ToArray();
        var cacheItemDict = await GetCacheItemDictionaryAsync(idArray.Distinct().ToArray());
        return idArray
            .Select(id => cacheItemDict.TryGetValue(id, out var item) ? item : null)
            .ToList();
    }

    public virtual async Task<Dictionary<TKey, TEntityCacheItem?>> FindManyAsDictionaryAsync(IEnumerable<TKey> ids)
    {
        return await GetCacheItemDictionaryAsync(ids.Distinct().ToArray());
    }

    public virtual async Task<TEntityCacheItem> GetAsync(TKey id)
    {
        return (await Cache.GetOrAddAsync(
            id,
            async () =>
            {
                if (HasObjectExtensionInfo())
                {
                    Repository.EnableTracking();
                }

                return MapToCacheItem(await Repository.GetAsync(id))!;
            }))!.Value!;
    }

    public virtual async Task<List<TEntityCacheItem>> GetManyAsync(IEnumerable<TKey> ids)
    {
        var idArray = ids.ToArray();
        var cacheItemDict = await GetCacheItemDictionaryAsync(idArray.Distinct().ToArray());
        return idArray
            .Select(id =>
            {
                if (!cacheItemDict.TryGetValue(id, out var item) || item == null)
                {
                    throw new EntityNotFoundException<TEntity>(id);
                }
                return item;
            })
            .ToList();
    }

    public virtual async Task<Dictionary<TKey, TEntityCacheItem>> GetManyAsDictionaryAsync(IEnumerable<TKey> ids)
    {
        var distinctIds = ids.Distinct().ToArray();
        var cacheItemDict = await GetCacheItemDictionaryAsync(distinctIds);
        var result = new Dictionary<TKey, TEntityCacheItem>();
        foreach (var id in distinctIds)
        {
            if (!cacheItemDict.TryGetValue(id, out var item) || item == null)
            {
                throw new EntityNotFoundException<TEntity>(id);
            }
            result[id] = item;
        }
        return result;
    }

    protected virtual async Task<Dictionary<TKey, TEntityCacheItem?>> GetCacheItemDictionaryAsync(TKey[] distinctIds)
    {
        var cacheItems = await GetOrAddManyCacheItemsAsync(distinctIds);
        return cacheItems.ToDictionary(x => x.Key, x => x.Value?.Value);
    }

    protected virtual async Task<KeyValuePair<TKey, EntityCacheItemWrapper<TEntityCacheItem>?>[]> GetOrAddManyCacheItemsAsync(TKey[] ids)
    {
        return await Cache.GetOrAddManyAsync(
            ids,
            async missingKeys =>
            {
                if (HasObjectExtensionInfo())
                {
                    Repository.EnableTracking();
                }

                var missingKeyArray = missingKeys.ToArray();
                var entities = await Repository.GetListAsync(
                    x => missingKeyArray.Contains(x.Id),
                    includeDetails: true
                );
                var entityDict = entities.ToDictionary(e => e.Id);

                return missingKeyArray
                    .Select(key =>
                    {
                        entityDict.TryGetValue(key, out var entity);
                        return new KeyValuePair<TKey, EntityCacheItemWrapper<TEntityCacheItem>>(
                            key,
                            MapToCacheItem(entity)!
                        );
                    })
                    .ToList();
            });
    }

    protected virtual bool HasObjectExtensionInfo()
    {
        return typeof(IHasExtraProperties).IsAssignableFrom(typeof(TEntity)) &&
               ObjectExtensionManager.Instance.GetOrNull(typeof(TEntity)) != null;
    }

    protected abstract EntityCacheItemWrapper<TEntityCacheItem>? MapToCacheItem(TEntity? entity);

    public async Task HandleEventAsync(EntityChangedEventData<TEntity> eventData)
    {
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
