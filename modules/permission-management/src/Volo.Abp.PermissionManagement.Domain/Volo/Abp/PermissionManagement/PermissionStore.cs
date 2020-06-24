using System.Linq;
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

        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

        public PermissionStore(
            IPermissionGrantRepository permissionGrantRepository,
            IDistributedCache<PermissionGrantCacheItem> cache,
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            PermissionGrantRepository = permissionGrantRepository;
            Cache = cache;
            PermissionDefinitionManager = permissionDefinitionManager;
            Logger = NullLogger<PermissionStore>.Instance;
        }

        public virtual async Task<bool> IsGrantedAsync(string name, string providerName, string providerKey)
        {
            return (await GetCacheItemAsync(name, providerName, providerKey)).IsGranted;
        }

        protected virtual async Task<PermissionGrantCacheItem> GetCacheItemAsync(
            string name,
            string providerName,
            string providerKey)
        {
            var cacheKey = CalculateCacheKey(name, providerName, providerKey);

            Logger.LogDebug($"PermissionStore.GetCacheItemAsync: {cacheKey}");

            var cacheItem = await Cache.GetAsync(cacheKey);

            if (cacheItem != null)
            {
                Logger.LogDebug($"Found in the cache: {cacheKey}");
                return cacheItem;
            }

            Logger.LogDebug($"Not found in the cache: {cacheKey}");
            
            cacheItem = new PermissionGrantCacheItem(name, false);
            
            await SetCacheItemsAsync(providerName, providerKey, name, cacheItem);

            return cacheItem;
        }

        protected virtual async Task SetCacheItemsAsync(
            string providerName,
            string providerKey,
            string currentName,
            PermissionGrantCacheItem currentCacheItem)
        {
            var permissions = PermissionDefinitionManager.GetPermissions();
            
            Logger.LogDebug($"Getting all granted permissions from the repository for this provider name,key: {providerName},{providerKey}");

            var permissionGrants = await PermissionGrantRepository.GetListAsync(providerName, providerKey);

            Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            foreach (var permission in permissions)
            {
                var isGranted = permissionGrants.Any(pg => pg.Name == permission.Name); //TODO: Optimize? Dictionary/Hash
                
                await Cache.SetAsync(
                    CalculateCacheKey(permission.Name, providerName, providerKey),
                    new PermissionGrantCacheItem(permission.Name, isGranted)
                );
                
                if (permission.Name == currentName)
                {
                    currentCacheItem.IsGranted = isGranted;
                }
            }
            
            Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}