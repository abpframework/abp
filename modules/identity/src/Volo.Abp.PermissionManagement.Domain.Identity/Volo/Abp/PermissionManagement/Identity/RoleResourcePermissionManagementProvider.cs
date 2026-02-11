using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.Identity;

public class RoleResourcePermissionManagementProvider : ResourcePermissionManagementProvider
{
    public override string Name => RoleResourcePermissionValueProvider.ProviderName;

    protected IUserRoleFinder UserRoleFinder { get; }

    public RoleResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository resourcepPrmissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IUserRoleFinder userRoleFinder)
        : base(
            resourcepPrmissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
        UserRoleFinder = userRoleFinder;
    }

    public override async Task<ResourcePermissionValueProviderGrantInfo> CheckAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var multipleGrantInfo = await CheckAsync(new[] { name }, resourceName, resourceKey, providerName, providerKey);

        return multipleGrantInfo.Result.Values.First();
    }

    public override async Task<MultipleResourcePermissionValueProviderGrantInfo> CheckAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        using (ResourcePermissionGrantRepository.DisableTracking())
        {
            var multiplePermissionValueProviderGrantInfo = new MultipleResourcePermissionValueProviderGrantInfo(names);
            var resourcePermissionGrants = new List<ResourcePermissionGrant>();

            if (providerName == Name)
            {
                resourcePermissionGrants.AddRange(await ResourcePermissionGrantRepository.GetListAsync(names, resourceName, resourceKey, providerName, providerKey));
            }

            if (providerName == UserResourcePermissionValueProvider.ProviderName && Guid.TryParse(providerKey, out var userId))
            {
                var roleNames = await UserRoleFinder.GetRoleNamesAsync(userId);

                foreach (var roleName in roleNames)
                {
                    resourcePermissionGrants.AddRange(await ResourcePermissionGrantRepository.GetListAsync(names, resourceName, resourceKey, Name, roleName));
                }
            }

            resourcePermissionGrants = resourcePermissionGrants.Distinct().ToList();
            if (!resourcePermissionGrants.Any())
            {
                return multiplePermissionValueProviderGrantInfo;
            }

            foreach (var permissionName in names)
            {
                var resourcePermissionGrant = resourcePermissionGrants.FirstOrDefault(x => x.Name == permissionName);
                if (resourcePermissionGrant != null)
                {
                    multiplePermissionValueProviderGrantInfo.Result[permissionName] = new ResourcePermissionValueProviderGrantInfo(true, resourcePermissionGrant.ProviderKey);
                }
            }

            return multiplePermissionValueProviderGrantInfo;
        }
    }
}
