using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Public.Menus;

namespace Volo.CmsKit.Public.Web.Menus
{
    public class CmsKitPublicMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            if (GlobalFeatureManager.Instance.IsEnabled<MenuFeature>())
            {
                var menuAppService = context.ServiceProvider.GetRequiredService<IMenuPublicAppService>();

                var mainMenu = await menuAppService.GetMainMenuAsync();

                if (mainMenu != null)
                {
                    // TODO: Consider about not to remove existing static menus.
                    // context.Menu.Items.Clear();
                    
                    foreach (var menuItemDto in mainMenu.Items.Where(x => x.ParentId == null))
                    {
                        var applicationMenuItem = CreateApplicationMenu(menuItemDto);
                        context.Menu.Items.Add(applicationMenuItem);
                        AddChildItems(applicationMenuItem, menuItemDto, mainMenu.Items);
                    }
                }
            }
        }

        private ApplicationMenuItem CreateApplicationMenu(MenuItemDto menuItem)
        {
            return new ApplicationMenuItem(
                menuItem.DisplayName,
                menuItem.DisplayName,
                menuItem.Url,
                menuItem.Icon,
                menuItem.Order,
                customData: null,
                menuItem.Target,
                menuItem.ElementId,
                menuItem.CssClass,
                menuItem.RequiredPermissionName
            );
        }

        private void AddChildItems(ApplicationMenuItem parent, MenuItemDto menuItem, List<MenuItemDto> source)
        {
            var applicationMenuItem = CreateApplicationMenu(menuItem);
            parent.Items.Add(applicationMenuItem);

            foreach (var item in source.Where(x => x.ParentId == menuItem.Id))
            {
                AddChildItems(applicationMenuItem, item, source);
            }
        }
    }
}