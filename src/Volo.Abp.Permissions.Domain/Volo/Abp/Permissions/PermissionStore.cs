using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.Permissions
{
    /* TODOS:
     * - Wrap distributed cache?
     *   - Add multi-tenancy
     *   - Add _cancellationTokenProvider support
     *   - Add object serialization support
     *   - Add cache invalidation support..? Maybe it's not cache's job!
     */

    public class PermissionStore : AbpServiceBase, IPermissionStore, ITransientDependency
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
            return await _permissionGrantRepository.FindAsync(name, providerName, providerKey) != null;
            //return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted; //TODO: Use cache when invalidation is possible!
        }

        private string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            var key = "P:" + providerName + "_K:" + providerKey + "N:" + name;

            if (_currentTenant.Id.HasValue)
            {
                key = "T:" + _currentTenant.Id + "_" + key;
            }

            return key;
        }

        private async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);
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
    }
}
