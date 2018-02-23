using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Permissions;
using Volo.Abp.Session;

namespace Volo.Abp.Identity
{
    public class UserPermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => UserPermissionValueProvider.ProviderName;

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