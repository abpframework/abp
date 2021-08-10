using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.Identity
{
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
            if (providerName == Name)
            {
                return new PermissionValueProviderGrantInfo(
                    await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                    providerKey
                );
            }

            if (providerName == UserPermissionValueProvider.ProviderName)
            {
                var userId = Guid.Parse(providerKey);
                var roleNames = await UserRoleFinder.GetRolesAsync(userId);

                foreach (var roleName in roleNames)
                {
                    var permissionGrant = await PermissionGrantRepository.FindAsync(name, Name, roleName);
                    if (permissionGrant != null)
                    {
                        return new PermissionValueProviderGrantInfo(true, roleName);
                    }
                }
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }

        public override async Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
        {
            var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
            List<PermissionGrant> permissionGrants = null;

            if (providerName == Name)
            {
                 permissionGrants = await PermissionGrantRepository.GetListAsync(names, providerName, providerKey);

            }

            if (providerName == UserPermissionValueProvider.ProviderName)
            {
                var userId = Guid.Parse(providerKey);
                var roleNames = await UserRoleFinder.GetRolesAsync(userId);

                foreach (var roleName in roleNames)
                {
                    permissionGrants = await PermissionGrantRepository.GetListAsync(names, Name, roleName);
                }
            }

            if (permissionGrants == null)
            {
                return multiplePermissionValueProviderGrantInfo;
            }

            foreach (var permissionName in names)
            {
                if (permissionGrants.Any(x => x.Name == permissionName))
                {
                    multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(true, providerKey);
                }
            }

            return multiplePermissionValueProviderGrantInfo;
        }
    }
}
