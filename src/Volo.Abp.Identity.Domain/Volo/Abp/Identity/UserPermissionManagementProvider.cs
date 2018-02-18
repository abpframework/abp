using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class UserPermissionManagementProvider : PermissionManagementProvider
    {
        public const string ProviderName = "User"; //TODO: Share a common string with UserPermissionValueProvider (same is true for the role)

        public override string Name => ProviderName;

        public UserPermissionManagementProvider(IPermissionGrantRepository 
            permissionGrantRepository, 
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant) 
            : base(
                permissionGrantRepository,
                guidGenerator,
                currentTenant)
        {

        }
    }
}