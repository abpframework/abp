using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Permissions
{
    public class PermissionStore : AbpServiceBase, IPermissionStore, IAsyncEventHandler<EntityChangedEventData<PermissionGrant>>, ITransientDependency
    {
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;
        private readonly IDistributedCache _distributedCache;
        private readonly IJsonSerializer _jsonSerializer;

        public PermissionStore(
            IPermissionGrantRepository permissionGrantRepository,
            IDistributedCache distributedCache,
            ICancellationTokenProvider cancellationTokenProvider,
            ICurrentTenant currentTenant,
            IJsonSerializer jsonSerializer)
        {
            _permissionGrantRepository = permissionGrantRepository;
            _distributedCache = distributedCache;
            _cancellationTokenProvider = cancellationTokenProvider;
            _currentTenant = currentTenant;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }
        
        public virtual Task HandleEventAsync(EntityChangedEventData<PermissionGrant> eventData)
        {
            return _distributedCache.RemoveAsync(
                CalculateCacheKey(
                    eventData.Entity.Name,
                    eventData.Entity.ProviderName,
                    eventData.Entity.ProviderKey,
                    eventData.Entity.TenantId
                )
            );
        }

        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey, _currentTenant.Id);
            var cachedString = await _distributedCache.GetStringAsync(cacheKey, _cancellationTokenProvider.Token);

            if (cachedString != null)
            {
                return _jsonSerializer.Deserialize<PermissionGrantCacheItem>(cachedString);
            }

            var cacheItem = new PermissionGrantCacheItem(
                name,
                await _permissionGrantRepository.FindAsync(name, providerName, providerKey) != null
            );

            await _distributedCache.SetStringAsync(
                cacheKey,
                _jsonSerializer.Serialize(cacheItem),
                new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) },
                _cancellationTokenProvider.Token
            );

            return cacheItem;
        }
        
        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey, Guid? tenantId)
        {
            var key = "N:" + "PermissionGrant" + "#P:" + providerName + "#K:" + providerKey + "#N:" + name;

            if (tenantId.HasValue)
            {
                key += "#T:" + tenantId.Value;
            }

            return key;
        }
    }
}
