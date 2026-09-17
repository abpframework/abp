using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class RoleResourcePermissionValueProvider : ResourcePermissionValueProvider
{
    public const string ProviderName = "R";

    public override string Name => ProviderName;

    public RoleResourcePermissionValueProvider(IResourcePermissionStore resourcePermissionStore)
        : base(resourcePermissionStore)
    {

    }

    public override async Task<PermissionGrantResult> CheckAsync(ResourcePermissionValueCheckContext context)
    {
        var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

        if (roles == null || !roles.Any())
        {
            return PermissionGrantResult.Undefined;
        }

        foreach (var role in roles.Distinct())
        {
            if (await ResourcePermissionStore.IsGrantedAsync(context.Permission.Name, context.ResourceName, context.ResourceKey, Name, role))
            {
                return PermissionGrantResult.Granted;
            }
        }

        return PermissionGrantResult.Undefined;
    }

    public override async Task<MultiplePermissionGrantResult> CheckAsync(ResourcePermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToList();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

        var roles = context.Principal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();
        if (roles == null || !roles.Any())
        {
            return result;
        }

        foreach (var role in roles.Distinct())
        {
            var multipleResult = await ResourcePermissionStore.IsGrantedAsync(permissionNames.ToArray(), context.ResourceName, context.ResourceKey, Name, role);

            foreach (var grantResult in multipleResult.Result.Where(grantResult =>
                result.Result.ContainsKey(grantResult.Key) &&
                result.Result[grantResult.Key] == PermissionGrantResult.Undefined &&
                grantResult.Value != PermissionGrantResult.Undefined))
            {
                result.Result[grantResult.Key] = grantResult.Value;
                permissionNames.RemoveAll(x => x == grantResult.Key);
            }

            if (result.AllGranted || result.AllProhibited)
            {
                break;
            }

            if (permissionNames.IsNullOrEmpty())
            {
                break;
            }
        }

        return result;
    }
}
