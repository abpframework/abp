﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;
using Volo.Blogging.Localization;

namespace Volo.Blogging
{
    public class BloggingMenuContributor : IMenuContributor
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
            var l = context.GetLocalizer<BloggingResource>();

            if (await context.IsGrantedAsync(BloggingPermissions.Blogs.Management))
            {
                var managementRootMenuItem = new ApplicationMenuItem("BlogManagement", l["Menu:BlogManagement"]);

                //TODO: Using the same permission. Reconsider.
                if (await context.IsGrantedAsync(BloggingPermissions.Blogs.Management))
                {
                    managementRootMenuItem.AddItem(new ApplicationMenuItem("BlogManagement.Blogs", l["Menu:Blogs"], "~/Admin/Blogs"));
                }

                context.Menu.AddItem(managementRootMenuItem);
            }
        }
    }
}
