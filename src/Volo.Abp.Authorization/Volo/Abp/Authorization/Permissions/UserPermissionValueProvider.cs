using System.Threading.Tasks;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization.Permissions
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "User";

        public override string Name => ProviderName;

        public UserPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {

        }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionValueCheckContext context)
        {
            var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

            if (userId == null)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId))
            {
                return new PermissionValueProviderGrantInfo(true, userId);
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}
