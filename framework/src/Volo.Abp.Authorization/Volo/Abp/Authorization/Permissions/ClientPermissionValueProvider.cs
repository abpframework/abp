using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using System.Linq;
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

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
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

        public override async Task<MultiplePermissionGrantResult> CheckAsync(PermissionValuesCheckContext context)
        {
            var permissionNames = context.Permissions.Select(x => x.Name).ToArray();

            var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;
            if (clientId == null)
            {
                return new MultiplePermissionGrantResult(permissionNames);;
            }

            using (CurrentTenant.Change(null))
            {
                return await PermissionStore.IsGrantedAsync(permissionNames, Name, clientId);
            }
        }
    }
}
