using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Permissions;

namespace Volo.Abp.Session
{
    public class RolePermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "Role";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public RolePermissionValueProvider(IPermissionStore permissionStore, ICurrentUser currentUser)
            : base(permissionStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionDefinition permission)
        {
            if (CurrentUser.Id == null || !CurrentUser.Roles.Any())
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            foreach (var role in CurrentUser.Roles)
            {
                if (await PermissionStore.IsGrantedAsync(permission.Name, Name, role))
                {
                    return new PermissionValueProviderGrantInfo(true, role);
                }
            }

            return PermissionValueProviderGrantInfo.NonGranted;
        }
    }
}