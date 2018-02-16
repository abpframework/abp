using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

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

        public override async Task<PermissionValueProviderGrantInfo> CheckAsync(PermissionDefinition permission)
        {
            if (CurrentUser.Id == null)
            {
                return PermissionValueProviderGrantInfo.NonGranted;
            }

            return new PermissionValueProviderGrantInfo(
                await PermissionStore.IsGrantedAsync(permission.Name, Name, CurrentUser.Id.Value.ToString()),
                CurrentUser.Id.ToString()
            );
        }
    }
}
