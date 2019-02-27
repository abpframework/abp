using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.MultiTenancy
{
    public class TenantPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "Tenant";

        public override string Name => ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public TenantPermissionValueProvider(
            IPermissionStore permissionStore,
            ICurrentTenant currentTenant)
            : base(permissionStore)
        {
            CurrentTenant = currentTenant;
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            if (!context.Permission.IsFeature)
            {
                return PermissionGrantResult.Undefined;
            }

            var tenantId = context.Principal?.FindFirst(AbpClaimTypes.TenantId)?.Value;

            if (tenantId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            using (CurrentTenant.Change(null))
            {
                return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, tenantId)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined;
            }
        }
    }
}
