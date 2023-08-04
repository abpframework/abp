using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Identity.Permissions;

public class UserRolePermissionValueProvider : PermissionValueProvider
{
    public const string ProviderName = "UR";

    public override string Name => ProviderName;

    protected IUserRoleFinder UserRoleFinder { get; }

    public UserRolePermissionValueProvider(IPermissionStore permissionStore, IUserRoleFinder userRoleFinder)
        : base(permissionStore)
    {
        UserRoleFinder = userRoleFinder;
    }

    public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
    {
        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

        if (userId == null)
        {
            return PermissionGrantResult.Undefined;
        }

        if (!Guid.TryParse(userId, out var userGuidId))
        {
            return PermissionGrantResult.Undefined;
        }

        var userRoles = await UserRoleFinder.GetRoleNamesAsync(userGuidId);

        foreach (var role in userRoles.Distinct())
        {
            if (await PermissionStore.IsGrantedAsync(context.Permission.Name, RolePermissionValueProvider.ProviderName, role))
            {
                return PermissionGrantResult.Granted;
            }
        }

        return PermissionGrantResult.Undefined;
    }

    public async override Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
    {
        var permissionNames = context.Permissions.Select(x => x.Name).Distinct().ToList();
        Check.NotNullOrEmpty(permissionNames, nameof(permissionNames));

        var result = new MultiplePermissionGrantResult(permissionNames.ToArray());

        var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

        if (userId == null)
        {
            return result;
        }

        if (!Guid.TryParse(userId, out var userGuidId))
        {
            return result;
        }

        var userRoles = await UserRoleFinder.GetRoleNamesAsync(userGuidId);

        foreach (var role in userRoles.Distinct())
        {
            var multipleResult = await PermissionStore.IsGrantedAsync(permissionNames.ToArray(), RolePermissionValueProvider.ProviderName, role);

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
