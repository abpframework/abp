using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo.Menus
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
            var menuItem = new ApplicationMenuItem(BasicThemeDemoMenus.Components.Root, "Components");

            var items = new List<ApplicationMenuItem>()
            {
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Alerts, "Alerts", url: "/Components/Alerts"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Badges, "Badges", url: "/Components/Badges"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Borders, "Borders", url: "/Components/Borders"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Breadcrumbs, "Breadcrumbs", url: "/Components/Breadcrumbs"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Buttons, "Buttons", url: "/Components/Buttons"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.ButtonGroups, "Button Groups", url: "/Components/ButtonGroups"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Cards, "Cards", url: "/Components/Cards"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Carousel, "Carousel", url: "/Components/Carousel"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Collapse, "Collapse", url: "/Components/Collapse"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Dropdowns, "Dropdowns", url: "/Components/Dropdowns"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.DynamicForms, "Dynamic Forms", url: "/Components/DynamicForms"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.FormElements, "Form Elements", url: "/Components/FormElements"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Grids, "Grids", url: "/Components/Grids"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.ListGroups, "List Groups", url: "/Components/ListGroups"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Modals, "Modals", url: "/Components/Modals"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Navs, "Navs", url: "/Components/Navs"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Navbars, "Navbars", url: "/Components/Navbars"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Paginator, "Paginator", url: "/Components/Paginator"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Popovers, "Popovers", url: "/Components/Popovers"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.ProgressBars, "Progress Bars", url: "/Components/ProgressBars"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Tables, "Tables", url: "/Components/Tables"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Tabs, "Tabs", url: "/Components/Tabs"),
                new ApplicationMenuItem(BasicThemeDemoMenus.Components.Tooltips, "Tooltips", url: "/Components/Tooltips")
            };
            
            items.OrderBy(x => x.Name)
                 .ToList()
                 .ForEach(x => menuItem.AddItem(x));

            context.Menu.AddItem(menuItem);
        }
    }
}
