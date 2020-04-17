﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.TenantManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.TenantManagement.Web.Navigation
{
    public class AbpTenantManagementWebMainMenuContributor : IMenuContributor
    {
        public virtual async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != StandardMenus.Main)
            {
                return;
            }

            var administrationMenu = context.Menu.GetAdministration();

            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<AbpTenantManagementResource>>();

            var tenantManagementMenuItem = new ApplicationMenuItem(TenantManagementMenuNames.GroupName, l["Menu:TenantManagement"], icon: "fa fa-users");
            administrationMenu.AddItem(tenantManagementMenuItem);

            if (await authorizationService.IsGrantedAsync(TenantManagementPermissions.Tenants.Default))
            {
                tenantManagementMenuItem.AddItem(new ApplicationMenuItem(TenantManagementMenuNames.Tenants, l["Tenants"], url: "/TenantManagement/Tenants"));
            }
        }
    }
}
