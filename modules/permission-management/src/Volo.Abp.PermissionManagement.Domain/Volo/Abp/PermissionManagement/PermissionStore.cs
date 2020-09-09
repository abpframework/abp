using System.Collections.Generic;
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
            
            cacheItem = new PermissionGrantCacheItem(false);
            
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

            var grantedPermissionsHashSet = new HashSet<string>(
                (await PermissionGrantRepository.GetListAsync(providerName, providerKey)).Select(p => p.Name)
            );

            Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            var cacheItems = new List<KeyValuePair<string, PermissionGrantCacheItem>>();

            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

                cacheItems.Add(new KeyValuePair<string, PermissionGrantCacheItem>(
                    CalculateCacheKey(permission.Name, providerName, providerKey),
                    new PermissionGrantCacheItem(isGranted))
                );
                
                if (permission.Name == currentName)
                {
                    currentCacheItem.IsGranted = isGranted;
                }
            }

            await Cache.SetManyAsync(cacheItems);
            
            Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
        }

        protected virtual string CalculateCacheKey(string name, string providerName, string providerKey)
        {
            return PermissionGrantCacheItem.CalculateCacheKey(name, providerName, providerKey);
        }
    }
}