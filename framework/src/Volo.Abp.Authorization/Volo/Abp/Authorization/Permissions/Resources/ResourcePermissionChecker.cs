using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ResourcePermissionChecker : IResourcePermissionChecker, ITransientDependency
{
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IResourcePermissionValueProviderManager PermissionValueProviderManager { get; }
    protected ISimpleStateCheckerManager<PermissionDefinition> StateCheckerManager { get; }
    protected IPermissionChecker PermissionChecker { get; }

    public ResourcePermissionChecker(
        ICurrentPrincipalAccessor principalAccessor,
        IPermissionDefinitionManager permissionDefinitionManager,
        ICurrentTenant currentTenant,
        IResourcePermissionValueProviderManager permissionValueProviderManager,
        ISimpleStateCheckerManager<PermissionDefinition> stateCheckerManager,
        IPermissionChecker permissionChecker)
    {
        PrincipalAccessor = principalAccessor;
        PermissionDefinitionManager = permissionDefinitionManager;
        CurrentTenant = currentTenant;
        PermissionValueProviderManager = permissionValueProviderManager;
        StateCheckerManager = stateCheckerManager;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task<bool> IsGrantedAsync(string name, string resourceName, string resourceKey)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, name, resourceName, resourceKey);
    }

    public virtual async Task<bool> IsGrantedAsync(
        ClaimsPrincipal? claimsPrincipal,
        string name,
        string resourceName,
        string resourceKey)
    {
        Check.NotNull(name, nameof(name));

        var permission = await PermissionDefinitionManager.GetResourcePermissionOrNullAsync(resourceName, name);
        if (permission == null)
        {
            return false;
        }

        if (!permission.IsEnabled)
        {
            return false;
        }

        if (!await StateCheckerManager.IsEnabledAsync(permission))
        {
            return false;
        }

        var multiTenancySide = claimsPrincipal?.GetMultiTenancySide()
                               ?? CurrentTenant.GetMultiTenancySide();

        if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
        {
            return false;
        }

        var isGranted = false;
        var context = new ResourcePermissionValueCheckContext(permission, claimsPrincipal, resourceName, resourceKey);
        foreach (var provider in PermissionValueProviderManager.ValueProviders)
        {
            if (context.Permission.Providers.Any() &&
                !context.Permission.Providers.Contains(provider.Name))
            {
                continue;
            }

            var result = await provider.CheckAsync(context);

            if (result == PermissionGrantResult.Granted)
            {
                isGranted = true;
            }
            else if (result == PermissionGrantResult.Prohibited)
            {
                return false;
            }
        }

        return isGranted;
    }

    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names, string resourceName, string resourceKey)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, names, resourceName, resourceKey);
    }

    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string[] names, string resourceName, string resourceKey)
    {
        Check.NotNull(names, nameof(names));

        var result = new MultiplePermissionGrantResult();
        if (!names.Any())
        {
            return result;
        }

        var multiTenancySide = claimsPrincipal?.GetMultiTenancySide() ??
                               CurrentTenant.GetMultiTenancySide();

        var allResourcePermissions = (await PermissionDefinitionManager.GetResourcePermissionsAsync())
            .Where(p => p.ResourceName == resourceName)
            .ToDictionary(p => p.Name, StringComparer.Ordinal);

        var pendingStateCheck = new List<PermissionDefinition>();
        var permissionDefinitions = new List<PermissionDefinition>();

        foreach (var name in names)
        {
            if (!allResourcePermissions.TryGetValue(name, out var permission))
            {
                result.Result.Add(name, PermissionGrantResult.Prohibited);
                continue;
            }

            result.Result.Add(name, PermissionGrantResult.Undefined);

            if (!permission.IsEnabled || !permission.MultiTenancySide.HasFlag(multiTenancySide))
            {
                continue;
            }

            pendingStateCheck.Add(permission);
        }

        if (pendingStateCheck.Count > 0)
        {
            var stateCheckResult = await StateCheckerManager.IsEnabledAsync(pendingStateCheck.ToArray());
            foreach (var item in stateCheckResult)
            {
                if (item.Value)
                {
                    permissionDefinitions.Add(item.Key);
                }
            }
        }

        foreach (var provider in PermissionValueProviderManager.ValueProviders)
        {
            var permissions = permissionDefinitions
                .Where(x => !x.Providers.Any() || x.Providers.Contains(provider.Name))
                .ToList();

            if (permissions.IsNullOrEmpty())
            {
                continue;
            }

            var context = new ResourcePermissionValuesCheckContext(
                permissions,
                claimsPrincipal,
                resourceName,
                resourceKey);

            var multipleResult = await provider.CheckAsync(context);
            foreach (var grantResult in multipleResult.Result.Where(grantResult =>
                result.Result.ContainsKey(grantResult.Key) &&
                result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                grantResult.Value != PermissionGrantResult.Undefined))
            {
                result.Result[grantResult.Key] = grantResult.Value;
                permissionDefinitions.RemoveAll(x => x.Name == grantResult.Key);
            }

            if (result.AllGranted || result.AllProhibited)
            {
                break;
            }
        }

        return result;
    }
}
