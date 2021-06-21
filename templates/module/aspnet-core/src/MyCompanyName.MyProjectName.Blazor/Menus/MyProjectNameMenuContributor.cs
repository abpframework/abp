using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace MyCompanyName.MyProjectName.Blazor.Menus
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

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            //Add main menu items.
            context.Menu.AddItem(new ApplicationMenuItem(MyProjectNameMenus.Prefix, displayName: "MyProjectName", "/MyProjectName", icon: "fa fa-globe"));
            
            return Task.CompletedTask;
        }
    }
}