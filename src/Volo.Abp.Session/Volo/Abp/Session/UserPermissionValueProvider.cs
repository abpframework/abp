using System.Threading.Tasks;
using Volo.Abp.Permissions;

namespace Volo.Abp.Session
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "User";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserPermissionValueProvider(IPermissionStore permissionStore, ICurrentUser currentUser)
            : base(permissionStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<bool> IsGrantedAsync(PermissionDefinition permission)
        {
            if (CurrentUser.Id == null)
            {
                return false;
            }

            return await PermissionStore.IsGrantedAsync(permission.Name, Name, CurrentUser.Id.Value.ToString());
        }
    }
}
