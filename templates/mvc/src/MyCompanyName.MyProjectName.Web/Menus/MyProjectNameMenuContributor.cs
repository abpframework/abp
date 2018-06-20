using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MyCompanyName.MyProjectName.Localization.MyProjectName;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName.Menus
{
    public class MyProjectNameMenuContributor : IMenuContributor
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
            var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<MyProjectNameResource>>();

            context.Menu.Items.Insert(0, new ApplicationMenuItem("MyProjectName.Home", l["Menu:Home"], "/"));
        }
    }
}
