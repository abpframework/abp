using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.Identity;

public class RolePermissionManagementProvider : PermissionManagementProvider
{
    public override string Name => RolePermissionValueProvider.ProviderName;

    protected IUserRoleFinder UserRoleFinder { get; }

    public RolePermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IUserRoleFinder userRoleFinder)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
        UserRoleFinder = userRoleFinder;
    }

    public override async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        var multipleGrantInfo = await CheckAsync(new[] { name }, providerName, providerKey);

        return multipleGrantInfo.Result.Values.First();
    }

    public override async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
        var permissionGrants = new List<PermissionGrant>();

        if (providerName == Name)
        {
            permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, providerName, providerKey));

        }

        if (providerName == UserPermissionValueProvider.ProviderName)
        {
            var userId = Guid.Parse(providerKey);
            var roleNames = await UserRoleFinder.GetRolesAsync(userId);

            foreach (var roleName in roleNames)
            {
                permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, Name, roleName));
            }
        }

        permissionGrants = permissionGrants.Distinct().ToList();
        if (!permissionGrants.Any())
        {
            return multiplePermissionValueProviderGrantInfo;
        }

        foreach (var permissionName in names)
        {
            var permissionGrant = permissionGrants.FirstOrDefault(x => x.Name == permissionName);
            if (permissionGrant != null)
            {
                multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(true, permissionGrant.ProviderKey);
            }
        }

        return multiplePermissionValueProviderGrantInfo;
    }
}
