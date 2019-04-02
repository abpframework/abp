using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using BaseManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace BaseManagement
{
    public class BaseManagementMenuContributor : IMenuContributor
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
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<BaseManagementResource>>();


            var rootMenuItem = new ApplicationMenuItem("BaseManagement", l["Menu:BaseManagement"]);

            if (await authorizationService.IsGrantedAsync(BaseManagementPermissions.BaseTypes.Default))
            {
                rootMenuItem.AddItem(new ApplicationMenuItem("Products", l["Menu:Products"], "/BaseManagement/BaseTypes"));
            }

            context.Menu.AddItem(rootMenuItem);
        }
    }
}