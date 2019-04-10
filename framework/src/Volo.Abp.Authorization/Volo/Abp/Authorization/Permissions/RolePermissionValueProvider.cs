using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;

namespace Volo.Abp.Authorization.Permissions
{
    public class RolePermissionValueProvider : PermissionValueProvider
    {
        protected ICurrentUser CurrentUser { get; }

        public const string ProviderName = "Role";

        public override string Name => ProviderName;

        public RolePermissionValueProvider(ICurrentUser currentUser, IPermissionStore permissionStore)
            : base(permissionStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var roles = CurrentUser.Roles;

            if (roles == null || !roles.Any())
            {
                return PermissionGrantResult.Undefined;
            }

            foreach (var role in roles)
            {
                if (await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, role))
                {
                    return PermissionGrantResult.Granted;
                }
            }

            return PermissionGrantResult.Undefined;
        }
    }
}