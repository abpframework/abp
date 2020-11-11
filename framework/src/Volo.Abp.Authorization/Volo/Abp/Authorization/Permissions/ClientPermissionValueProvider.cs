using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class ClientPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "C";

        public override string Name => ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public ClientPermissionValueProvider(IPermissionStore permissionStore, ICurrentTenant currentTenant)
            : base(permissionStore)
        {
            CurrentTenant = currentTenant;
        }

        public async override Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;

            if (clientId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            using (CurrentTenant.Change(null))
            {
                return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, clientId)
                    ? PermissionGrantResult.Granted
                    : PermissionGrantResult.Undefined;
            }
        }
    }
}
