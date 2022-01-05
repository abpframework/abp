using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.TenantManagement.Web.Navigation;

public class AbpTenantManagementWebMainMenuContributor : IMenuContributor
{
    public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name != StandardMenus.Main)
        {
            return Task.CompletedTask;
        }

        var administrationMenu = context.Menu.GetAdministration();

        var l = context.GetLocalizer<AbpTenantManagementResource>();

        var tenantManagementMenuItem = new ApplicationMenuItem(TenantManagementMenuNames.GroupName, l["Menu:TenantManagement"], icon: "fa fa-users");
        administrationMenu.AddItem(tenantManagementMenuItem);

        tenantManagementMenuItem.AddItem(new ApplicationMenuItem(TenantManagementMenuNames.Tenants, l["Tenants"], url: "~/TenantManagement/Tenants").RequirePermissions(TenantManagementPermissions.Tenants.Default));

        return Task.CompletedTask;
    }
}
