using System.Threading.Tasks;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Authorization.Permissions
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        protected ICurrentUser CurrentUser { get; }

        public const string ProviderName = "User";

        public override string Name => ProviderName;

        public UserPermissionValueProvider(ICurrentUser currentUser, IPermissionStore permissionStore)
            : base(permissionStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var userId = CurrentUser.Id?.ToString();

            if (userId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }
}
