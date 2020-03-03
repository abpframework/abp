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
            var menuItem = new ApplicationMenuItem("BasicThemeDemo.Components", "Components");
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Alerts", "Alerts", url: "/Components/Alerts")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Badges", "Badges", url: "/Components/Badges")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Borders", "Borders", url: "/Components/Borders")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Breadcrumbs", "Breadcrumbs", url: "/Components/Breadcrumbs")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Buttons", "Buttons", url: "/Components/Buttons")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Cards", "Cards", url: "/Components/Cards")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Collapse", "Collapse", url: "/Components/Collapse")
            );
            //menuItem.AddItem(
            //    new ApplicationMenuItem("BasicThemeDemo.Components.Dropdowns", "Dropdowns", url: "/Components/Dropdowns")
            //);
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Grids", "Grids", url: "/Components/Grids")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.ListGroups", "List Groups", url: "/Components/ListGroups")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Modals", "Modals", url: "/Components/Modals")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Navs", "Navs", url: "/Components/Navs")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Popovers", "Popovers", url: "/Components/Popovers")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.ProgressBars", "Progress Bars", url: "/Components/ProgressBars")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Tables", "Tables", url: "/Components/Tables")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Tabs", "Tabs", url: "/Components/Tabs")
            );
            menuItem.AddItem(
                new ApplicationMenuItem("BasicThemeDemo.Components.Tooltips", "Tooltips", url: "/Components/Tooltips")
            );


            context.Menu.AddItem(menuItem);
        }
    }
}
