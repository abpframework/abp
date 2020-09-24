using System.Threading.Tasks;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.TenantManagement.Blazor.Navigation
{
    public class TenantManagementBlazorMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private async Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var administrationMenu = context.Menu.GetAdministration();

            var l = context.GetLocalizer<AbpTenantManagementResource>();

            var tenantManagementMenuItem = new ApplicationMenuItem(TenantManagementMenuNames.GroupName, l["Menu:TenantManagement"], icon: "fa fa-users");
            administrationMenu.AddItem(tenantManagementMenuItem);

            if (await context.IsGrantedAsync(TenantManagementPermissions.Tenants.Default))
            {
                tenantManagementMenuItem.AddItem(new ApplicationMenuItem(TenantManagementMenuNames.Tenants, l["Tenants"], url: "TenantManagement/Tenants"));
            }
        }
    }
}
