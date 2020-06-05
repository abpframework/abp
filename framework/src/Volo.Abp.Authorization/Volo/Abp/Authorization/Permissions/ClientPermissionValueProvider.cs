using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class ClientPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "C";

        public override string Name => ProviderName;

        public ClientPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {

        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var clientId = context.Principal?.FindFirst(AbpClaimTypes.ClientId)?.Value;

            if (clientId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, clientId) 
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }
}
