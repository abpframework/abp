using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo
{
    public class BasicThemeDemoMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if(context.Menu.Name == StandardMenus.Main)
            {
                AddMainMenuItems(context);
            }

            return Task.CompletedTask;
        }

        private void AddMainMenuItems(MenuConfigurationContext context)
        {
            context.Menu.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components", "Components")
                    .AddItem(
                        new ApplicationMenuItem("BasicThemeDemo.Components.Buttons", "Buttons", url: "/Components/Buttons")
                    )
            );
        }
    }
}
