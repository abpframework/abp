using System;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class RolePermissionManagementProvider : PermissionManagementProvider
    {
        public const string ProviderName = "Role";

        public override string Name => ProviderName;

        private readonly IIdentityUserRepository _identityUserRepository;

        public RolePermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator,
            IIdentityUserRepository identityUserRepository)
            : base(
                permissionGrantRepository,
                guidGenerator)
        {
            _identityUserRepository = identityUserRepository;
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
                var roleNames = await _identityUserRepository.GetRoleNamesAsync(userId);

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