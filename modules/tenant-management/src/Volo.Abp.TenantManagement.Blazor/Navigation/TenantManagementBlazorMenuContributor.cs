using System.Threading.Tasks;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.TenantManagement.Blazor.Navigation
{
    public class TenantManagementBlazorMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var administrationMenu = context.Menu.GetAdministration();

            var l = context.GetLocalizer<AbpTenantManagementResource>();

            var tenantManagementMenuItem = new ApplicationMenuItem(TenantManagementMenuNames.GroupName, l["Menu:TenantManagement"], icon: "fa fa-users");
            administrationMenu.AddItem(tenantManagementMenuItem);

            if (await context.IsGrantedAsync(TenantManagementPermissions.Tenants.Default))
            {
                tenantManagementMenuItem.AddItem(new ApplicationMenuItem(TenantManagementMenuNames.Tenants, l["Tenants"], url: "tenant-management/tenants"));
            }
        }
    }
}
