using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Caching
{
    public abstract class AbstractKeyEntityCache<TEntity, TCacheItem, TKey> :
        IDistributedEventHandler<EntityChangedEventData<TEntity>>,
        IEntityCache<TCacheItem, TKey>
        where TEntity : class, IEntity
        where TCacheItem : class
    {
        public TCacheItem this[TKey id] => Get(id);

        public string CacheName { get; }

        public IObjectMapper ObjectMapper { get; set; }

        protected IRepository<TEntity> Repository { get; }

        public IDistributedCache<TCacheItem> InternalCache { get; }

        protected AbstractKeyEntityCache(
            IRepository<TEntity> repository,
            IDistributedCache<TCacheItem> internalCache,
            string cacheName = null)
        {
            InternalCache = internalCache;
            Repository = repository;
            CacheName = cacheName ?? GenerateDefaultCacheName();
        }

        public virtual TCacheItem Get(TKey id)
        {
            return InternalCache.GetOrAdd(GetCacheKey(id), () => GetCacheItemFromDataSource(id));
        }

        public virtual async Task<TCacheItem> GetAsync(TKey id)
        {
            return await InternalCache.GetOrAddAsync(GetCacheKey(id), async () => await GetCacheItemFromDataSourceAsync(id));
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<TEntity> eventData)
        {
            await InternalCache.RemoveAsync(GetCacheKey(eventData.Entity));
        }

        protected virtual TCacheItem GetCacheItemFromDataSource(TKey id)
        {
            return MapToCacheItem(GetEntityFromDataSource(id));
        }

        protected abstract TEntity GetEntityFromDataSource(TKey id);

        protected virtual async Task<TCacheItem> GetCacheItemFromDataSourceAsync(TKey id)
        {
            return MapToCacheItem(await GetEntityFromDataSourceAsync(id));
        }

        protected abstract Task<TEntity> GetEntityFromDataSourceAsync(TKey id);

        protected abstract string GetCacheKey(TKey id);

        protected abstract string GetCacheKey(TEntity entity);

        protected virtual TCacheItem MapToCacheItem(TEntity entity)
        {
            return ObjectMapper.Map<TEntity, TCacheItem>(entity);
        }

        protected virtual string GenerateDefaultCacheName()
        {
            return GetType().FullName;
        }

        public override string ToString()
        {
            return $"EntityCache {CacheName}";
        }
    }
}
