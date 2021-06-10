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
            if (context.Menu.Name == CmsKitMenus.Public)
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
                    foreach (var menuItemDto in mainMenu.Items.Where(x => x.ParentId == null && x.IsActive))
                    {
                        var applicationMenuItem = CreateApplicationMenuItem(menuItemDto);
                        context.Menu.Items.Add(applicationMenuItem);
                        AddChildItems(menuItemDto, mainMenu.Items);
                    }
                }
            }
        }

        private void AddChildItems(MenuItemDto menuItem, List<MenuItemDto> source, ApplicationMenuItem parent = null)
        {
            var applicationMenuItem = CreateApplicationMenuItem(menuItem);
            parent?.Items.Add(applicationMenuItem);

            foreach (var item in source.Where(x => x.ParentId == menuItem.Id && x.IsActive))
            {
                AddChildItems(item, source, applicationMenuItem);
            }
        }

        private ApplicationMenuItem CreateApplicationMenuItem(MenuItemDto menuItem)
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
    }
}