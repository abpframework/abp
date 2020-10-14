using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.Domain.Entities.Caching
{
    public abstract class AbstractKeyEntityCache<TEntity, TCacheItem, TKey> :
        IDistributedEventHandler<EntityChangedEventData<TEntity>>,
        IEntityCache<TCacheItem, TKey>
        where TEntity : class, IEntity
        where TCacheItem : class
    {
        public IServiceProvider ServiceProvider { get; set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return LazyGetRequiredService(typeof(TService), ref reference);
        }

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected Type ObjectMapperContext { get; set; }
        protected IObjectMapper ObjectMapper
        {
            get
            {
                if (_objectMapper != null)
                {
                    return _objectMapper;
                }

                if (ObjectMapperContext == null)
                {
                    return LazyGetRequiredService(ref _objectMapper);
                }

                return LazyGetRequiredService(
                    typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext),
                    ref _objectMapper
                );
            }
        }
        private IObjectMapper _objectMapper;

        protected ICurrentTenant CurrentTenant => LazyGetRequiredService(ref _currentTenant);
        private ICurrentTenant _currentTenant;

        public IDistributedCache<TCacheItem> InternalCache { get; }

        protected IRepository<TEntity> Repository { get; }

        protected AbstractKeyEntityCache(
            IRepository<TEntity> repository,
            IDistributedCache<TCacheItem> internalCache)
        {
            InternalCache = internalCache;
            Repository = repository;
        }

        public virtual async Task<TCacheItem> GetAsync(TKey key)
        {
            return await InternalCache.GetOrAddAsync(GetCacheKey(key), async () => await GetCacheItemFromDataSourceAsync(key));
        }

        public virtual async Task HandleEventAsync(EntityChangedEventData<TEntity> eventData)
        {
            await InternalCache.RemoveAsync(GetCacheKey(eventData.Entity));
        }

        protected virtual async Task<TCacheItem> GetCacheItemFromDataSourceAsync(TKey key)
        {
            return MapToCacheItem(await GetEntityFromDataSourceAsync(key));
        }

        protected abstract Task<TEntity> GetEntityFromDataSourceAsync(TKey key);

        protected virtual string GetCacheKey(TKey key)
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return GetCacheKey(key.ToString(), CurrentTenant.Id);
            }

            return key.ToString();
        }

        protected virtual string GetCacheKey(TEntity entity)
        {
            var key = string.Join("", entity.GetKeys());

            if (entity is IMultiTenant multiTenant)
            {
                return GetCacheKey(key, multiTenant.TenantId);
            }

            return key;
        }

        protected virtual string GetCacheKey(string key, Guid? tenantId)
        {
            return key + "@" + tenantId;
        }

        protected virtual TCacheItem MapToCacheItem(TEntity entity)
        {
            return ObjectMapper.Map<TEntity, TCacheItem>(entity);
        }
    }
}
