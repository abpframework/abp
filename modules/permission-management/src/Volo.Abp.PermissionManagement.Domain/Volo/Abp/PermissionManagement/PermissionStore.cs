using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement
{
    public class PermissionStore : IPermissionStore, ITransientDependency
    {
        public ILogger<PermissionStore> Logger { get; set; }
        
        protected IPermissionGrantRepository PermissionGrantRepository { get; }

        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }
        
        public PermissionStore(
            IPermissionGrantRepository permissionGrantRepository,
            IDistributedCache<PermissionGrantCacheItem> cache)
        {
            PermissionGrantRepository = permissionGrantRepository;
            Cache = cache;
            Logger = NullLogger<PermissionStore>.Instance;
        }

        public async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }

        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(string name, string providerName, string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);

            Logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache, getting from the repository: {cacheKey}");

            cacheItem = new PermissionGrantCacheItem(
                name,
                (await PermissionGrantRepository.FindAsync(name, providerName, providerKey)) != null
            );

            Logger.LogDebug($"Setting the cache item: {cacheKey}");

            await Cache.SetAsync(
                cacheKey,
                cacheItem
            );

            Logger.LogDebug($"Finished setting the cache item: {cacheKey}");

            return cacheItem;
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}
