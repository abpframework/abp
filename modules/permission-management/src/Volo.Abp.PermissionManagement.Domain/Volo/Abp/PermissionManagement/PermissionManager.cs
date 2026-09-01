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

public class PermissionManager : IPermissionManager, ISingletonDependency
{
    protected IPermissionGrantRepository PermissionGrantRepository { get; }

    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    protected IGuidGenerator GuidGenerator { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IReadOnlyList<IPermissionManagementProvider> ManagementProviders => _lazyProviders.Value;

    protected PermissionManagementOptions Options { get; }

    protected IDistributedCache<PermissionGrantCacheItem> Cache { get; }

    private readonly Lazy<List<IPermissionManagementProvider>> _lazyProviders;

    public PermissionManager(
        IPermissionDefinitionManager permissionDefinitionManager,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IPermissionGrantRepository permissionGrantRepository,
        IServiceProvider serviceProvider,
        IGuidGenerator guidGenerator,
        IOptions<PermissionManagementOptions> options,
        ICurrentTenant currentTenant,
        IDistributedCache<PermissionGrantCacheItem> cache)
    {
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
        Cache = cache;
        SimpleStateCheckerManager = simpleStateCheckerManager;
        PermissionGrantRepository = permissionGrantRepository;
        PermissionDefinitionManager = permissionDefinitionManager;
        Options = options.Value;

        _lazyProviders = new Lazy<List<IPermissionManagementProvider>>(
            () => Options
                .ManagementProviders
                .Select(c => serviceProvider.GetRequiredService(c) as IPermissionManagementProvider)
                .ToList(),
            true
        );
    }

    public virtual async Task<PermissionWithGrantedProviders> GetAsync(string permissionName, string providerName, string providerKey)
    {
        var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
        if (permission == null)
        {
            return new PermissionWithGrantedProviders(permissionName, false);
        }
        
        return await GetInternalAsync(
            permission,
            providerName,
            providerKey
        );
    }

    public virtual async Task<MultiplePermissionWithGrantedProviders> GetAsync(
        string[] permissionNames, 
        string providerName,
        string providerKey)
    {
        var permissions = new List<PermissionDefinition>();
        var undefinedPermissions = new List<string>();

        foreach (var permissionName in permissionNames)
        {
            var permission = await PermissionDefinitionManager.GetOrNullAsync(permissionName);
            if (permission != null)
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
            providerName,
            providerKey
        );

        foreach (var undefinedPermission in undefinedPermissions)
        {
            result.Result.Add(new PermissionWithGrantedProviders(undefinedPermission, false));
        }

        return result;
    }

    public virtual async Task<List<PermissionWithGrantedProviders>> GetAllAsync(string providerName, string providerKey)
    {
        var permissionDefinitions = (await PermissionDefinitionManager.GetPermissionsAsync()).ToArray();

        var multiplePermissionWithGrantedProviders = await GetInternalAsync(permissionDefinitions, providerName, providerKey);

        return multiplePermissionWithGrantedProviders.Result;

    }

    public virtual async Task SetAsync(string permissionName, string providerName, string providerKey, bool isGranted)
    {
        await SetAsync(
            new[] { new KeyValuePair<string, bool>(permissionName, isGranted) },
            providerName,
            providerKey
        );
    }

    public virtual async Task SetAsync(
        IEnumerable<KeyValuePair<string, bool>> permissions,
        string providerName,
        string providerKey)
    {
        Check.NotNull(permissions, nameof(permissions));

        /* Each name is resolved only once, even if it is passed more than once. */
        var permissionDefinitions = new Dictionary<string, PermissionDefinition>();
        var permissionsToSet = new List<KeyValuePair<PermissionDefinition, bool>>();
        foreach (var permission in permissions)
        {
            Check.NotNull(permission.Key, "permissionName");

            if (!permissionDefinitions.TryGetValue(permission.Key, out var permissionDefinition))
            {
                permissionDefinition = await PermissionDefinitionManager.GetOrNullAsync(permission.Key);
                permissionDefinitions[permission.Key] = permissionDefinition;
            }

            if (permissionDefinition == null)
            {
                /* Silently ignore undefined permissions,
                   maybe they were removed from dynamic permission definition store */
                continue;
            }

            permissionsToSet.Add(new KeyValuePair<PermissionDefinition, bool>(permissionDefinition, permission.Value));
        }

        if (!permissionsToSet.Any())
        {
            return;
        }

        var distinctPermissions = permissionsToSet.Select(x => x.Key).Distinct().ToArray();

        var enabledPermissions = distinctPermissions.Where(x => x.IsEnabled).ToArray();
        var stateCheckResult = enabledPermissions.Any()
            ? await SimpleStateCheckerManager.IsEnabledAsync(enabledPermissions)
            : new SimpleStateCheckerResult<PermissionDefinition>();

        foreach (var permission in distinctPermissions)
        {
            if (!permission.IsEnabled || !stateCheckResult[permission])
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' is disabled!");
            }

            if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' is not compatible with the provider named '{providerName}'");
            }

            if (!permission.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
            {
                //TODO: BusinessException
                throw new ApplicationException($"The permission named '{permission.Name}' has multitenancy side '{permission.MultiTenancySide}' which is not compatible with the current multitenancy side '{CurrentTenant.GetMultiTenancySide()}'");
            }
        }

        var currentGrantInfo = await GetInternalAsync(distinctPermissions, providerName, providerKey);
        var currentGrants = currentGrantInfo.Result.ToDictionary(x => x.Name, x => x.IsGranted);

        /* The last state wins when the same permission is passed more than once. */
        var requestedGrants = new Dictionary<string, bool>();
        foreach (var permission in permissionsToSet)
        {
            requestedGrants[permission.Key.Name] = permission.Value;
        }

        var changedPermissions = distinctPermissions
            .Select(x => x.Name)
            .Where(x => currentGrants[x] != requestedGrants[x])
            .Select(x => new KeyValuePair<string, bool>(x, requestedGrants[x]))
            .ToList();

        if (!changedPermissions.Any())
        {
            return;
        }

        var provider = ManagementProviders.FirstOrDefault(m => m.Name == providerName);
        if (provider == null)
        {
            //TODO: BusinessException
            throw new AbpException("Unknown permission management provider: " + providerName);
        }

        foreach (var changedPermission in changedPermissions)
        {
            await provider.SetAsync(changedPermission.Key, providerKey, changedPermission.Value);
        }
    }

    public virtual async Task<PermissionGrant> UpdateProviderKeyAsync(PermissionGrant permissionGrant, string providerKey)
    {
        using (CurrentTenant.Change(permissionGrant.TenantId))
        {
            //Invalidating the cache for the old key
            await Cache.RemoveAsync(
                PermissionGrantCacheItem.CalculateCacheKey(
                    permissionGrant.Name,
                    permissionGrant.ProviderName,
                    permissionGrant.ProviderKey
                )
            );
        }

        permissionGrant.ProviderKey = providerKey;
        return await PermissionGrantRepository.UpdateAsync(permissionGrant, true);
    }

    public virtual async Task DeleteAsync(string providerName, string providerKey)
    {
        var permissionGrants = await PermissionGrantRepository.GetListAsync(providerName, providerKey);
        foreach (var permissionGrant in permissionGrants)
        {
            await PermissionGrantRepository.DeleteAsync(permissionGrant, true);
        }
    }

    protected virtual async Task<PermissionWithGrantedProviders> GetInternalAsync(
        PermissionDefinition permission,
        string providerName,
        string providerKey)
    {
        var multiplePermissionWithGrantedProviders = await GetInternalAsync(
            new[] { permission },
            providerName,
            providerKey
        );

        return multiplePermissionWithGrantedProviders.Result.First();
    }

    protected virtual async Task<MultiplePermissionWithGrantedProviders> GetInternalAsync(
        PermissionDefinition[] permissions,
        string providerName,
        string providerKey)
    {
        var permissionNames = permissions.Select(x => x.Name).ToArray();
        var multiplePermissionWithGrantedProviders = new MultiplePermissionWithGrantedProviders(permissionNames);

        var neededCheckPermissions = new List<PermissionDefinition>();

        foreach (var permission in permissions
                                    .Where(x => x.IsEnabled)
                                    .Where(x => x.MultiTenancySide.HasFlag(CurrentTenant.GetMultiTenancySide()))
                                    .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName)))
        {
            if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
            {
                neededCheckPermissions.Add(permission);
            }
        }

        if (!neededCheckPermissions.Any())
        {
            return multiplePermissionWithGrantedProviders;
        }

        foreach (var provider in ManagementProviders)
        {
            permissionNames = neededCheckPermissions.Select(x => x.Name).ToArray();
            var multiplePermissionValueProviderGrantInfo = await provider.CheckAsync(permissionNames, providerName, providerKey);

            foreach (var providerResultDict in multiplePermissionValueProviderGrantInfo.Result)
            {
                if (providerResultDict.Value.IsGranted)
                {
                    var permissionWithGrantedProvider = multiplePermissionWithGrantedProviders.Result
                        .First(x => x.Name == providerResultDict.Key);

                    permissionWithGrantedProvider.IsGranted = true;
                    permissionWithGrantedProvider.Providers.Add(new PermissionValueProviderInfo(provider.Name, providerResultDict.Value.ProviderKey));
                }
            }
        }

        return multiplePermissionWithGrantedProviders;
    }
}
