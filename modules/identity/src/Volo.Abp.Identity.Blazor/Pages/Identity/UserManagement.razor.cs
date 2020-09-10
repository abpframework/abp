using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class UserManagement
    {
        private const string PermissionProviderName = "U";

        private PermissionManagementModal PermissionManagementModal;

        public UserManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
        }
    }
}
