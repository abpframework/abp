using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.OpenIddict
{
    public class ClientPermissionManagementProvider : PermissionManagementProvider
    {
        public override string Name => ClientPermissionValueProvider.ProviderName;

        public ClientPermissionManagementProvider(
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
            : base(
                permissionGrantRepository,
                guidGenerator,
                currentTenant)
        {

        }

        public override Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
        {
            using (CurrentTenant.Change(null))
            {
                return base.CheckAsync(name, providerName, providerKey);
            }
        }

        protected override Task GrantAsync(string name, string providerKey)
        {
            using (CurrentTenant.Change(null))
            {
                return base.GrantAsync(name, providerKey);
            }
        }

        protected override Task RevokeAsync(string name, string providerKey)
        {
            using (CurrentTenant.Change(null))
            {
                return base.RevokeAsync(name, providerKey);
            }
        }

        public override Task SetAsync(string name, string providerKey, bool isGranted)
        {
            using (CurrentTenant.Change(null))
            {
                return base.SetAsync(name, providerKey, isGranted);
            }
        }
    }
}
