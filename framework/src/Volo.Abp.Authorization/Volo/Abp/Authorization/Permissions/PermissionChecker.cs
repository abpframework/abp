using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionChecker : IPermissionChecker, ITransientDependency
{
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected IPermissionValueProviderManager PermissionValueProviderManager { get; }
    protected ISimpleStateCheckerManager<PermissionDefinition> StateCheckerManager { get; }

    public PermissionChecker(
        ICurrentPrincipalAccessor principalAccessor,
        IPermissionDefinitionManager permissionDefinitionManager,
        ICurrentTenant currentTenant,
        IPermissionValueProviderManager permissionValueProviderManager,
        ISimpleStateCheckerManager<PermissionDefinition> stateCheckerManager)
    {
        PrincipalAccessor = principalAccessor;
        PermissionDefinitionManager = permissionDefinitionManager;
        CurrentTenant = currentTenant;
        PermissionValueProviderManager = permissionValueProviderManager;
        StateCheckerManager = stateCheckerManager;
    }

    public virtual async Task<bool> IsGrantedAsync(string name)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, name);
    }

    public virtual async Task<bool> IsGrantedAsync(
        ClaimsPrincipal claimsPrincipal,
        string name)
    {
        Check.NotNull(name, nameof(name));

        var permission = PermissionDefinitionManager.Get(name);

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
        var context = new PermissionValueCheckContext(permission, claimsPrincipal);
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

    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(string[] names)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, names);
    }

    public async Task<MultiplePermissionGrantResult> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string[] names)
    {
        Check.NotNull(names, nameof(names));

        var multiTenancySide = claimsPrincipal?.GetMultiTenancySide() ?? CurrentTenant.GetMultiTenancySide();

        var result = new MultiplePermissionGrantResult();
        if (!names.Any())
        {
            return result;
        }

        var permissionDefinitions = new List<PermissionDefinition>();
        foreach (var name in names)
        {
            var permission = PermissionDefinitionManager.Get(name);

            result.Result.Add(name, PermissionGrantResult.Undefined);

            if (permission.IsEnabled &&
                await StateCheckerManager.IsEnabledAsync(permission) &&
                permission.MultiTenancySide.HasFlag(multiTenancySide))
            {
                permissionDefinitions.Add(permission);
            }
        }

        foreach (var provider in PermissionValueProviderManager.ValueProviders)
        {
            var permissions = permissionDefinitions
                .Where(x => !x.Providers.Any() || x.Providers.Contains(provider.Name))
                .ToList();

            if (permissions.IsNullOrEmpty())
            {
                break;
            }

            var context = new PermissionValuesCheckContext(
                permissions,
                claimsPrincipal);

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
