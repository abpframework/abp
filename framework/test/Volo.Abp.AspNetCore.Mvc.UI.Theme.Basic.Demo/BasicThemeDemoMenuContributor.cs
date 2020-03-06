using System.Collections.Generic;
using System.Linq;
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

            var items = new List<ApplicationMenuItem>()
            {
                new ApplicationMenuItem("BasicThemeDemo.Components.Alerts", "Alerts", url: "/Components/Alerts"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Badges", "Badges", url: "/Components/Badges"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Borders", "Borders", url: "/Components/Borders"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Breadcrumbs", "Breadcrumbs", url: "/Components/Breadcrumbs"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Buttons", "Buttons", url: "/Components/Buttons"),
                new ApplicationMenuItem("BasicThemeDemo.Components.ButtonGroups", "ButtonGroups", url: "/Components/ButtonGroups"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Cards", "Cards", url: "/Components/Cards"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Carousel", "Carousel", url: "/Components/Carousel"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Collapse", "Collapse", url: "/Components/Collapse"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Dropdowns", "Dropdowns", url: "/Components/Dropdowns"),
                new ApplicationMenuItem("BasicThemeDemo.Components.DynamicForms", "DynamicForms", url: "/Components/DynamicForms"),
                new ApplicationMenuItem("BasicThemeDemo.Components.FormElements", "FormElements", url: "/Components/FormElements"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Grids", "Grids", url: "/Components/Grids"),
                new ApplicationMenuItem("BasicThemeDemo.Components.ListGroups", "List Groups", url: "/Components/ListGroups"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Modals", "Modals", url: "/Components/Modals"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Navs", "Navs", url: "/Components/Navs"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Navbars", "Navbars", url: "/Components/Navbars"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Paginator", "Paginator", url: "/Components/Paginator"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Popovers", "Popovers", url: "/Components/Popovers"),
                new ApplicationMenuItem("BasicThemeDemo.Components.ProgressBars", "Progress Bars", url: "/Components/ProgressBars"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Tables", "Tables", url: "/Components/Tables"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Tabs", "Tabs", url: "/Components/Tabs"),
                new ApplicationMenuItem("BasicThemeDemo.Components.Tooltips", "Tooltips", url: "/Components/Tooltips")
            };
            
            items.OrderBy(x => x.Name)
                 .ToList()
                 .ForEach(x => menuItem.AddItem(x));

            context.Menu.AddItem(menuItem);
        }
    }
}
