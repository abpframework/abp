using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Docs.Localization;

namespace Volo.Docs.Admin
{
    public class DocsMenuContributor : IMenuContributor
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
            var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<DocsResource>>();

            //if (await authorizationService.IsGrantedAsync(DocsAdminPermissions.GroupName))
            {
                var rootMenuItem = new ApplicationMenuItem("DocumentManagement", l["Menu:DocumentManagement"], "/Docs/Admin");

                if (await authorizationService.IsGrantedAsync(DocsAdminPermissions.Projects.Default))
                {
                    rootMenuItem.AddItem(new ApplicationMenuItem("ProjectManagement", l["Menu:ProjectManagement"], "/Docs/Admin/Projects"));
                }

                context.Menu.AddItem(rootMenuItem);
            }
        }
    }
}
