using Volo.Abp.PermissionManagement.Blazor.Components;

namespace Volo.Abp.Identity.Blazor.Pages.Identity
{
    public partial class RoleManagement
    {
        private const string PermissionProviderName = "R";

        private PermissionManagementModal PermissionManagementModal;

        public RoleManagement()
        {
            ObjectMapperContext = typeof(AbpIdentityBlazorModule);
        }
    }
}
