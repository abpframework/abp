using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.PermissionManagement;

public class ResourcePermissionManager : IResourcePermissionManager, ISingletonDependency
{
    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    protected IGuidGenerator GuidGenerator { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IReadOnlyList<IResourcePermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

    protected PermissionManagementOptions Options { get; }

    protected IDistributedCache<ResourcePermissionGrantCacheItem> Cache { get; }

    private readonly Lazy<List<IResourcePermissionManagementProvider>> _lazyProviders;

    private readonly Lazy<List<IResourcePermissionProviderKeyLookupService>> _lazyProviderKeyLookupServices;

    public ResourcePermissionManager(
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IServiceProvider serviceProvider,
        IGuidGenerator guidGenerator,
        IOptions<PermissionManagementOptions> options,
        ICurrentTenant currentTenant,
        IDistributedCache<ResourcePermissionGrantCacheItem> cache)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        Cache = cache;
        SimpleStateCheckerManager = simpleStateCheckerManager;
        ResourcePermissionGrantRepository = resourcePermissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
        Options = options.Value;

        _lazyProviders = new Lazy<List<IResourcePermissionManagementProvider>>(
            () => Options
                .ResourceManagementProviders
                .Select(c => serviceProvider.GetRequiredService(c) as IResourcePermissionManagementProvider)
                .ToList(),
            true
        );

        _lazyProviderKeyLookupServices = new Lazy<List<IResourcePermissionProviderKeyLookupService>>(
            () => Options
                .ResourcePermissionProviderKeyLookupServices
                .Select(c => serviceProvider.GetRequiredService(c) as IResourcePermissionProviderKeyLookupService)
                .ToList(),
            true
        );
    }

    public virtual Task<List<IResourcePermissionProviderKeyLookupService>> GetProviderKeyLookupServicesAsync()
    {
        return Task.FromResult(_lazyProviderKeyLookupServices.Value);
    }

    public virtual  Task<IResourcePermissionProviderKeyLookupService> GetProviderKeyLookupServiceAsync(string serviceName)
    {
        var service = _lazyProviderKeyLookupServices.Value.FirstOrDefault(s => s.Name == serviceName);
        return service == null
            ? throw new AbpException("Unknown resource permission provider key lookup service: " + serviceName)
            : Task.FromResult(service);
    }

    public virtual async Task<List<PermissionDefinition>> GetAvailablePermissionsAsync(string resourceName)
    {
        var multiTenancySide = CurrentTenant.GetMultiTenancySide();
        var resourcePermissions = new List<PermissionDefinition>();
        foreach (var resourcePermission in (await PermissionDefinitionManager.GetResourcePermissionsAsync())
                 .Where(x => x.IsEnabled && x.MultiTenancySide.HasFlag(multiTenancySide) && x.ResourceName == resourceName))
        {
            if (await SimpleStateCheckerManager.IsEnabledAsync(resourcePermission))
            {
                resourcePermissions.Add(resourcePermission);
            }
        }

        return resourcePermissions;
    }

    public virtual async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permission = await PermissionDefinitionManager.GetResourcePermissionOrNullAsync(resourceName, permissionName);
        if (permission == null || permission.ResourceName != resourceName)
        {
            return new PermissionWithGrantedProviders(permissionName, false);
        }

        return await GetInternalAsync(
            permission,
            resourceName,
            resourceKey,
            providerName,
            providerKey
        );
    }

    public virtual async Task<MultiplePermissionWithGrantedProviders> GetAsync(string[] permissionNames, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permissions = new List<PermissionDefinition>();
        var undefinedPermissions = new List<string>();

        foreach (var permissionName in permissionNames)
        {
            var permission = await PermissionDefinitionManager.GetResourcePermissionOrNullAsync(resourceName, permissionName);
            if (permission != null && permission.ResourceName == resourceName)
            {
                permissions.Add(permission);
            }
            else
            {
                undefinedPermissions.Add(permissionName);
            }
        }

        if (!permissions.Any())
        {
            return new MultiplePermissionWithGrantedProviders(undefinedPermissions.ToArray());
        }

        var result = await GetInternalAsync(
            permissions.ToArray(),
            resourceName,
            resourceKey,
            providerName,
            providerKey
        );

        foreach (var undefinedPermission in undefinedPermissions)
        {
            result.Result.Add(new PermissionWithGrantedProviders(undefinedPermission, false));
        }

        return result;
    }

    public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string resourceName, string resourceKey)
    {
        var resourcePermissionDefinitions = await GetAvailablePermissionsAsync(resourceName);
        var resourcePermissionGrants = await ResourcePermissionGrantRepository.GetPermissionsAsync(resourceName, resourceKey);
        var result = new List<PermissionWithGrantedProviders>();
        foreach (var resourcePermissionDefinition in resourcePermissionDefinitions)
        {
            var permissionWithGrantedProviders = new PermissionWithGrantedProviders(resourcePermissionDefinition.Name, false);

            var grantedPermissions = resourcePermissionGrants
                .Where(x => x.Name == resourcePermissionDefinition.Name && x.ResourceName == resourceName && x.ResourceKey == resourceKey)
                .ToList();

            if (grantedPermissions.Any())
            {
                permissionWithGrantedProviders.IsGranted = true;
                foreach (var grantedPermission in grantedPermissions)
                {
                    permissionWithGrantedProviders.Providers.Add(new PermissionValueProviderInfo(grantedPermission.ProviderName, grantedPermission.ProviderKey));
                }
            }

            result.Add(permissionWithGrantedProviders);
        }

        return result;
    }

    public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permissionDefinitions = await GetAvailablePermissionsAsync(resourceName);
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(permissionDefinitions.ToArray(), resourceName, resourceKey, providerName, providerKey);
        return multiplePermissionWithGrantedProviders.Result;
    }

    public virtual async Task<List<PermissionProviderWithPermissions>> GetAllGroupAsync(string resourceName, string resourceKey)
    {
        var resourcePermissions = await GetAvailablePermissionsAsync(resourceName);
        var resourcePermissionGrants = await ResourcePermissionGrantRepository.GetPermissionsAsync(resourceName, resourceKey);
        resourcePermissionGrants = resourcePermissionGrants.Where(x => resourcePermissions.Any(rp => rp.Name == x.Name)).ToList();
        var resourcePermissionGrantsGroup = resourcePermissionGrants.GroupBy(x => new { x.ProviderName, x.ProviderKey });
        var result = new List<PermissionProviderWithPermissions>();
        foreach (var resourcePermissionGrant in resourcePermissionGrantsGroup)
        {
            result.Add(new PermissionProviderWithPermissions(resourcePermissionGrant.Key.ProviderName, resourcePermissionGrant.Key.ProviderKey, resourcePermissionGrant.Key.ProviderKey)
            {
                Permissions = resourcePermissionGrant.Select(x => x.Name).ToList()
            });
        }

        if (result.Any())
        {
            var providerKeyInfos = new Dictionary<string, List<ResourcePermissionProviderKeyInfo>>();
            var resourcePermissionProviderGroup = resourcePermissionGrants.GroupBy(x => x.ProviderName);
            var providerKeyLookupServices = await GetProviderKeyLookupServicesAsync();
            foreach (var resourcePermissionProvider in resourcePermissionProviderGroup)
            {
                var providerKeyLookupService = providerKeyLookupServices.FirstOrDefault(s => s.Name == resourcePermissionProvider.Key);
                if (providerKeyLookupService == null)
                {
                    continue;
                }
                var keys = resourcePermissionProvider.Select(rp => rp.ProviderKey).Distinct().ToList();
                providerKeyInfos.Add(resourcePermissionProvider.Key, await providerKeyLookupService.SearchAsync(keys.ToArray()));
            }

            foreach (var item in result)
            {
                if (!providerKeyInfos.TryGetValue(item.ProviderName, out var providerKeyInfoList))
                {
                    continue;
                }

                var providerKeyInfo = providerKeyInfoList.FirstOrDefault(p => p.ProviderKey == item.ProviderKey);
                if (providerKeyInfo != null)
                {
                    item.ProviderDisplayName = providerKeyInfo.ProviderDisplayName;
                    item.ProviderNameDisplayName = providerKeyLookupServices
                        .FirstOrDefault(s => s.Name == item.ProviderName)?.DisplayName;
                }
            }
        }

        return result;
    }

    public virtual async Task SetAsync(string permissionName, string resourceName, string resourceKey, string providerName, string providerKey, bool isGranted)
    {
        var permission = await PermissionDefinitionManager.GetResourcePermissionOrNullAsync(resourceName, permissionName);
        if (permission == null || permission.ResourceName != resourceName)
        {
            /* Silently ignore undefined permissions,
               maybe they were removed from dynamic permission definition store */
            return;
        }

        if (!permission.IsEnabled || !await SimpleStateCheckerManager.IsEnabledAsync(permission))
        {
            //TODO: BusinessException
            throw new ApplicationException($"The resource permission named '{permission.Name}' is disabled!");
        }

        if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
        {
            //TODO: BusinessException
            throw new ApplicationException($"The resource permission named '{permission.Name}' has not compatible with the provider named '{providerName}'");
        }

        if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
        {
            //TODO: BusinessException
            throw new ApplicationException($"The resource permission named '{permission.Name}' has multitenancy side '{permission.MultiTenancySide}' which is not compatible with the current multitenancy side '{CurrentTenant.GetMultiTenancySide()}'");
        }

        var currentGrantInfo = await GetInternalAsync(permission, resourceName, resourceKey, providerName, providerKey);
        if (currentGrantInfo.IsGranted == isGranted && currentGrantInfo.Providers.Any(x => x.Name == providerName && x.Key == providerKey))
        {
            return;
        }

        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
        if (provider == null)
        {
            //TODO: BusinessException
            throw new AbpException("Unknown resource permission management provider: " + providerName);
        }

        await provider.SetAsync(permissionName, resourceName, resourceKey, providerKey, isGranted);
    }

    public virtual async Task<ResourcePermissionGrant> UpdateProviderKeyAsync(ResourcePermissionGrant resourcePermissionGrant, string providerKey)
    {
        using (CurrentTenant.Change(resourcePermissionGrant.TenantId))
        {
            //Invalidating the cache for the old key
            await Cache.RemoveAsync(
                ResourcePermissionGrantCacheItem.CalculateCacheKey(
                    resourcePermissionGrant.Name,
                    resourcePermissionGrant.ResourceName,
                    resourcePermissionGrant.ResourceKey,
                    resourcePermissionGrant.ProviderName,
                    resourcePermissionGrant.ProviderKey
                )
            );
        }

        resourcePermissionGrant.ProviderKey = providerKey;
        return await ResourcePermissionGrantRepository.UpdateAsync(resourcePermissionGrant, true);
    }

    public virtual async Task<ResourcePermissionGrant> UpdateProviderKeyAsync(ResourcePermissionGrant resourcePermissionGrant, string resourceName, string resourceKey, string providerKey)
    {
        using (CurrentTenant.Change(resourcePermissionGrant.TenantId))
        {
            //Invalidating the cache for the old key
            await Cache.RemoveAsync(
                ResourcePermissionGrantCacheItem.CalculateCacheKey(
                    resourcePermissionGrant.Name,
                    resourcePermissionGrant.ResourceName,
                    resourcePermissionGrant.ResourceKey,
                    resourcePermissionGrant.ProviderName,
                    resourcePermissionGrant.ProviderKey
                )
            );
        }

        resourcePermissionGrant.ProviderKey = providerKey;
        return await ResourcePermissionGrantRepository.UpdateAsync(resourcePermissionGrant, true);
    }

    public virtual async Task DeleteAsync(string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permissionGrants = await ResourcePermissionGrantRepository.GetListAsync(resourceName, resourceKey, providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await ResourcePermissionGrantRepository.DeleteAsync(permissionGrant, true);
        }
    }

    public virtual async Task DeleteAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permissionGrant = await ResourcePermissionGrantRepository.FindAsync(name, resourceName, resourceKey, providerName, providerKey);
        if (permissionGrant != null)
        {
            await ResourcePermissionGrantRepository.DeleteAsync(permissionGrant, true);
        }
    }

    public virtual async Task DeleteAsync(string providerName, string providerKey)
    {
        var permissionGrants = await ResourcePermissionGrantRepository.GetListAsync(providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await ResourcePermissionGrantRepository.DeleteAsync(permissionGrant, true);
        }
    }

    protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition permission, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(
            new[] { permission },
            resourceName,
            resourceKey,
            providerName,
            providerKey
        );

        return multiplePermissionWithGrantedProviders.Result.First();
    }

    protected virtual async Task<MultiplePermissionWithGrantedProviders> GetInternalAsync(PermissionDefinition[] permissions, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var permissionNames = permissions.Select(x => x.Name).ToArray();
        var multiplePermissionWithGrantedProviders = new MultiplePermissionWithGrantedProviders(permissionNames);

        var resourcePermissions = await GetAvailablePermissionsAsync(resourceName);
        if (!resourcePermissions.Any())
        {
            return multiplePermissionWithGrantedProviders;
        }

        foreach (var provider in ManagementProviders)
        {
            permissionNames = resourcePermissions.Select(x => x.Name).ToArray();
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(permissionNames, resourceName, resourceKey, providerName, providerKey);

            foreach (var providerResultDict in multiplePermissionValueProviderGrantInfo.Result)
            {
                if (providerResultDict.Value.IsGranted)
                {
                    var permissionWithGrantedProvider = multiplePermissionWithGrantedProviders.Result
                        .FirstOrDefault(x => x.Name == providerResultDict.Key);

                    if (permissionWithGrantedProvider == null)
                    {
                        continue;
                    }

                    permissionWithGrantedProvider.IsGranted = true;
                    permissionWithGrantedProvider.Providers.Add(
                        new PermissionValueProviderInfo(provider.Name, providerResultDict.Value.ProviderKey));
                }
            }
        }

        return multiplePermissionWithGrantedProviders;
    }
}
