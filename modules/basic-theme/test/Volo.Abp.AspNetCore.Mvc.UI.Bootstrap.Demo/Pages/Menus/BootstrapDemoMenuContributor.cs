using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Pages.Menus;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Demo.Menus;

public class BootstrapDemoMenuContributor : IMenuContributor
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
        var menuItem = new ApplicationMenuItem(BootstrapDemoMenus.Components.Root, "Components");

        var items = new List<ApplicationMenuItem>()
            {
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Alerts, "Alerts", url: "/Components/Alerts"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Badges, "Badges", url: "/Components/Badges"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Borders, "Borders", url: "/Components/Borders"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Breadcrumbs, "Breadcrumbs", url: "/Components/Breadcrumbs"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Buttons, "Buttons", url: "/Components/Buttons"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.ButtonGroups, "Button Groups", url: "/Components/ButtonGroups"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Cards, "Cards", url: "/Components/Cards"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Carousel, "Carousel", url: "/Components/Carousel"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Collapse, "Collapse", url: "/Components/Collapse"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.DatePicker, "Date Picker & Date Range Picker", url: "/Components/DatePicker"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Dropdowns, "Dropdowns", url: "/Components/Dropdowns"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.DynamicForms, "Dynamic Forms", url: "/Components/DynamicForms"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.FormElements, "Form Elements", url: "/Components/FormElements"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Grids, "Grids", url: "/Components/Grids"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.ListGroups, "List Groups", url: "/Components/ListGroup"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Modals, "Modals", url: "/Components/Modals"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Navbars, "Navbars", url: "/Components/Navs"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Paginator, "Paginator", url: "/Components/Paginator"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Popovers, "Popovers", url: "/Components/Popovers"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.ProgressBars, "Progress Bars", url: "/Components/ProgressBars"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Tables, "Tables", url: "/Components/Tables"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Tabs, "Tabs", url: "/Components/Tabs"),
                new ApplicationMenuItem(BootstrapDemoMenus.Components.Tooltips, "Tooltips", url: "/Components/Tooltips")
            };

        items.OrderBy(x => x.Name)
             .ToList()
             .ForEach(x => menuItem.AddItem(x));

        context.Menu.AddItem(menuItem);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                name: "Platform",
                displayName: "Platform",
                url: "https://abp.io"
                )
            );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                name: "Community",
                displayName: "Community",
                url: "https://abp.io/community"
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                name: "Documents",
                displayName: "Documents",
                url: "https://abp.io/docs/latest"
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                name: "Github",
                displayName: "Github",
                url: "https://github.com/abpframework/abp",
                icon:"fa fa-github"
            )
        );
    }
}
