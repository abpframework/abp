using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement
{
    public class TestPermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => "Test";

        public TestPermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IPermissionStore permissionStore,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
            : base(
                permissionGrantRepository,
                permissionStore,
                guidGenerator,
                currentTenant)
        {

        }
    }
}
