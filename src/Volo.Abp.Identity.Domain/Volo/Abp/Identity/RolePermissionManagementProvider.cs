using System;
using System.Threading.Tasks;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class RolePermissionManagementProvider : IPermissionManagementProvider
    {
        public string Name => "Role";

        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public RolePermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository, 
            IIdentityUserRepository identityUserRepository)
        {
            _permissionGrantRepository = permissionGrantRepository;
            _identityUserRepository = identityUserRepository;
        }

        public async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName == Name)
            {
                return new PermissionValueProviderGrantInfo(
                    await _permissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                    providerKey
                );
            }

            if (providerName == "User")
            {
                var userId = Guid.Parse(providerKey);

                var roleNames = await _identityUserRepository.GetRoleNamesAsync(userId);

                foreach (var roleName in roleNames)
                {
                    var pg = await _permissionGrantRepository.FindAsync(name, providerName, roleName);
                    if (pg != null)
                    {
                        return new PermissionValueProviderGrantInfo(true, roleName);
                    }
                }
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}