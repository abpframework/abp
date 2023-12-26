using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo.Menus;

public class BasicThemeDemoMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            AddMainMenuItems(context);
        }

        return Task.CompletedTask;
    }

    private void AddMainMenuItems(MenuConfigurationContext context)
    {

        var menuItem = new ApplicationMenuItem("Administration.SubMenu1", "Sub menu 1")
            .AddItem(new ApplicationMenuItem("Administration.SubMenu1.11", "Sub menu 1.11")
                .AddItem(new ApplicationMenuItem("Administration.SubMenu1.1111", "Sub menu 1.1111", url: "/submenu1/submenu1_1"))
                .AddItem(new ApplicationMenuItem("Administration.SubMenu1.2222", "Sub menu 1.2222", url: "/submenu1/submenu1_2")))
            .AddItem(new ApplicationMenuItem("Administration.SubMenu1.2", "Sub menu 1.2", url: "/submenu1/submenu1_2"));

        context.Menu.AddItem(menuItem);
    }
}
