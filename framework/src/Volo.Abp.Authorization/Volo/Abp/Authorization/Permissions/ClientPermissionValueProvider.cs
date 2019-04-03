using System.Threading.Tasks;
using Volo.Abp.Clients;

namespace Volo.Abp.Authorization.Permissions
{
    public class ClientPermissionValueProvider : PermissionValueProvider
    {
        protected ICurrentClient CurrentClient { get; }

        public const string ProviderName = "Client";

        public override string Name => ProviderName;

        public ClientPermissionValueProvider(ICurrentClient currentClient, IPermissionStore permissionStore)
            : base(permissionStore)
        {
            CurrentClient = currentClient;
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var clientId = CurrentClient.Id;

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
