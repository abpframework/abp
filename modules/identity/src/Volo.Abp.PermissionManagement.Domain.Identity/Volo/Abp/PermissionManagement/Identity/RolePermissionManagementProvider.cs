using System;
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

        private readonly IUserRoleFinder _userRoleFinder;

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
            _userRoleFinder = userRoleFinder;
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

            if (providerName == "User")
            {
                var userId = Guid.Parse(providerKey);
                var roleNames = await _userRoleFinder.GetRolesAsync(userId);

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
    }
}