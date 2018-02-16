using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class UserPermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => "User";

        public UserPermissionManagementProvider(IPermissionGrantRepository 
            permissionGrantRepository, 
            IGuidGenerator guidGenerator) 
            : base(
                permissionGrantRepository, 
                guidGenerator)
        {
        }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            if (providerName != Name)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await PermissionGrantRepository.FindAsync(name, providerName, providerKey) != null,
                providerKey
            );
        }
    }
}