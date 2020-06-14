﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using ProductManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace ProductManagement
{
    public class ProductManagementMenuContributor : IMenuContributor
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
            var l = context.GetLocalizer<ProductManagementResource>();

            var rootMenuItem = new ApplicationMenuItem("ProductManagement", l["Menu:ProductManagement"]);

            if (await context.IsGrantedAsync(ProductManagementPermissions.Products.Default))
            {
                rootMenuItem.AddItem(new ApplicationMenuItem("Products", l["Menu:Products"], "/ProductManagement/Products"));
            }

            context.Menu.AddItem(rootMenuItem);
        }
    }
}
