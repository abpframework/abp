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
        public TCacheItem this[TKey key] => Get(key);

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

        public virtual TCacheItem Get(TKey key)
        {
            return InternalCache.GetOrAdd(GetCacheKey(key), () => GetCacheItemFromDataSource(key));
        }

        public virtual async Task<TCacheItem> GetAsync(TKey key)
        {
            return await InternalCache.GetOrAddAsync(GetCacheKey(key), async () => await GetCacheItemFromDataSourceAsync(key));
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<TEntity> eventData)
        {
            await InternalCache.RemoveAsync(GetCacheKey(eventData.Entity));
        }

        protected virtual TCacheItem GetCacheItemFromDataSource(TKey key)
        {
            return MapToCacheItem(GetEntityFromDataSource(key));
        }

        protected abstract TEntity GetEntityFromDataSource(TKey key);

        protected virtual async Task<TCacheItem> GetCacheItemFromDataSourceAsync(TKey key)
        {
            return MapToCacheItem(await GetEntityFromDataSourceAsync(key));
        }

        protected abstract Task<TEntity> GetEntityFromDataSourceAsync(TKey key);

        protected abstract string GetCacheKey(TKey key);

        protected virtual string GetCacheKey(TEntity entity)
        {
            return string.Join("", entity.GetKeys());
        }

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
