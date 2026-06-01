using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionStore : IResourcePermissionStore, ITransientDependency
{
    public ILogger<ResourcePermissionStore> Logger { get; set; }

    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    protected IDistributedCache<ResourcePermissionGrantCacheItem> Cache { get; }

    public ResourcePermissionStore(
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IDistributedCache<ResourcePermissionGrantCacheItem> cache,
        IPermissionDefinitionManager permissionDefinitionManager)
    {
        ResourcePermissionGrantRepository = resourcePermissionGrantRepository;
        Cache = cache;
        PermissionDefinitionManager = permissionDefinitionManager;
        Logger = NullLogger<ResourcePermissionStore>.Instance;
    }

    public virtual async Task<bool> IsGrantedAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return (await GetCacheItemAsync(name, resourceName, resourceKey, providerName, providerKey)).IsGranted;
    }

    protected virtual async Task<ResourcePermissionGrantCacheItem> GetCacheItemAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var cacheKey = CalculateCacheKey(name, providerName, providerKey, resourceName, resourceKey);

        Logger.LogDebug($"ResourcePermissionStore.GetCacheItemAsync: {cacheKey}");

        var cacheItem = await Cache.GetAsync(cacheKey);

        if (cacheItem != null)
        {
            Logger.LogDebug($"Found in the cache: {cacheKey}");
            return cacheItem;
        }

        Logger.LogDebug($"Not found in the cache: {cacheKey}");

        cacheItem = new ResourcePermissionGrantCacheItem(false);

        await SetCacheItemsAsync(resourceName, resourceKey, providerName, providerKey, name, cacheItem);

        return cacheItem;
    }

    protected virtual async Task SetCacheItemsAsync(string resourceName, string resourceKey, string providerName, string providerKey, string currentName, ResourcePermissionGrantCacheItem currentCacheItem)
    {
        using (ResourcePermissionGrantRepository.DisableTracking())
        {
            var permissions = await PermissionDefinitionManager.GetResourcePermissionsAsync();

            Logger.LogDebug($"Getting all granted resource permissions from the repository for resource name,key:{resourceName},{resourceKey} and provider name,key: {providerName},{providerKey}");

            var grantedPermissionsHashSet = new HashSet<string>(
                (await ResourcePermissionGrantRepository.GetListAsync(resourceName, resourceKey, providerName, providerKey)).Select(p => p.Name)
            );

            Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            var cacheItems = new List<KeyValuePair<string, ResourcePermissionGrantCacheItem>>();

            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

                cacheItems.Add(new KeyValuePair<string, ResourcePermissionGrantCacheItem>(
                    CalculateCacheKey(permission.Name, resourceName, resourceKey, providerName, providerKey),
                    new ResourcePermissionGrantCacheItem(isGranted))
                );

                if (permission.Name == currentName)
                {
                    currentCacheItem.IsGranted = isGranted;
                }
            }

            await Cache.SetManyAsync(cacheItems);

            Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");
        }
    }

    public virtual async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        Check.NotNullOrEmpty(names, nameof(names));

        var result = new MultiplePermissionGrantResult();

        if (names.Length == 1)
        {
            var name = names.First();
            result.Result.Add(name,
                await IsGrantedAsync(names.First(), resourceName, resourceKey, providerName, providerKey)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
            return result;
        }

        var cacheItems = await GetCacheItemsAsync(names, resourceName, resourceKey, providerName, providerKey);
        foreach (var item in cacheItems)
        {
            result.Result.Add(GetPermissionNameFormCacheKeyOrNull(item.Key),
                item.Value != null && item.Value.IsGranted
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined);
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<string, ResourcePermissionGrantCacheItem>>> GetCacheItemsAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var cacheKeys = names.Select(x => CalculateCacheKey(x, resourceName, resourceKey, providerName, providerKey)).ToList();

        Logger.LogDebug($"ResourcePermissionStore.GetCacheItemAsync: {string.Join(",", cacheKeys)}");

        var cacheItems = (await Cache.GetManyAsync(cacheKeys)).ToList();
        if (cacheItems.All(x => x.Value != null))
        {
            Logger.LogDebug($"Found in the cache: {string.Join(",", cacheKeys)}");
            return cacheItems;
        }

        var notCacheKeys = cacheItems.Where(x => x.Value == null).Select(x => x.Key).ToList();

        Logger.LogDebug($"Not found in the cache: {string.Join(",", notCacheKeys)}");

        var newCacheItems = await SetCacheItemsAsync(resourceName, resourceKey, providerName, providerKey, notCacheKeys);

        var result = new List<KeyValuePair<string, ResourcePermissionGrantCacheItem>>();
        foreach (var key in cacheKeys)
        {
            var item = newCacheItems.FirstOrDefault(x => x.Key == key);
            if (item.Value == null)
            {
                item = cacheItems.FirstOrDefault(x => x.Key == key);
            }

            result.Add(new KeyValuePair<string, ResourcePermissionGrantCacheItem>(key, item.Value));
        }

        return result;
    }

    protected virtual async Task<List<KeyValuePair<string, ResourcePermissionGrantCacheItem>>> SetCacheItemsAsync(string resourceName, string resourceKey, string providerName, string providerKey, List<string> notCacheKeys)
    {
        using (ResourcePermissionGrantRepository.DisableTracking())
        {
            var permissionNames = new HashSet<string>(notCacheKeys.Select(GetPermissionNameFormCacheKeyOrNull));
            var permissions = (await PermissionDefinitionManager.GetResourcePermissionsAsync())
                .Where(x => permissionNames.Contains(x.Name))
                .ToList();

            Logger.LogDebug($"Getting not cache granted permissions from the repository for resource name,key:{resourceName},{resourceKey} and provider name,key: {providerName},{providerKey}");

            var grantedPermissionsHashSet = new HashSet<string>(
                (await ResourcePermissionGrantRepository.GetListAsync(permissionNames.ToArray(), resourceName, resourceKey, providerName, providerKey)).Select(p => p.Name)
            );

            Logger.LogDebug($"Setting the cache items. Count: {permissions.Count}");

            var cacheItems = new List<KeyValuePair<string, ResourcePermissionGrantCacheItem>>();

            foreach (var permission in permissions)
            {
                var isGranted = grantedPermissionsHashSet.Contains(permission.Name);

                cacheItems.Add(new KeyValuePair<string, ResourcePermissionGrantCacheItem>(
                    CalculateCacheKey(permission.Name, resourceName, resourceKey, providerName, providerKey),
                    new ResourcePermissionGrantCacheItem(isGranted))
                );
            }

            await Cache.SetManyAsync(cacheItems);

            Logger.LogDebug($"Finished setting the cache items. Count: {permissions.Count}");

            return cacheItems;
        }
    }

    public virtual async Task<MultiplePermissionGrantResult> GetPermissionsAsync(string resourceName, string resourceKey)
    {
        using (ResourcePermissionGrantRepository.DisableTracking())
        {
            var result = new MultiplePermissionGrantResult();

            var resourcePermissions = (await PermissionDefinitionManager.GetResourcePermissionsAsync()).Where(x => x.ResourceName == resourceName).ToList();
            var permissionGrants = await ResourcePermissionGrantRepository.GetPermissionsAsync(resourceName, resourceKey);
            foreach (var resourcePermission in resourcePermissions)
            {
                var isGranted = permissionGrants.Any(x => x.Name == resourcePermission.Name);
                result.Result.Add(resourcePermission.Name, isGranted ? PermissionGrantResult.Granted : PermissionGrantResult.Undefined);
            }

            return result;
        }
    }

    public virtual async Task<string[]> GetGrantedPermissionsAsync(string resourceName, string resourceKey)
    {
        var resourcePermissions = (await PermissionDefinitionManager.GetResourcePermissionsAsync()).Where(x => x.ResourceName == resourceName).ToList();
        var grantedPermissions = await ResourcePermissionGrantRepository.GetPermissionsAsync(resourceName, resourceKey);

        var result = new List<string>();
        foreach (var grantedPermission in grantedPermissions)
        {
            if (resourcePermissions.Any(x => x.Name == grantedPermission.Name))
            {
                result.Add(grantedPermission.Name);
            }
        }

        return result.ToArray();
    }

    public virtual async Task<string[]> GetGrantedResourceKeysAsync(string resourceName, string name)
    {
        return (await ResourcePermissionGrantRepository.GetResourceKeys(resourceName, name)).Select(x => x.ResourceKey).ToArray();
    }

    protected virtual string GetPermissionNameFormCacheKeyOrNull(string key)
    {
        //TODO: throw ex when name is null?
        return ResourcePermissionGrantCacheItem.GetPermissionNameFormCacheKeyOrNull(key);
    }

    protected virtual string CalculateCacheKey(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        return ResourcePermissionGrantCacheItem.CalculateCacheKey(name,  resourceName, resourceKey, providerName, providerKey);
    }
}
