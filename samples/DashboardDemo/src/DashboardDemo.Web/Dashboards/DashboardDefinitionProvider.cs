using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using DashboardDemo.Pages.widgets.Filters;
using DashboardDemo.Widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace DashboardDemo.Dashboards
{
    public static class DashboardDefinitionProvider
    {
        public static List<DashboardDefinition> GetDefinitions()
        {
            var myDashboard = new DashboardDefinition(
                DashboardNames.MyDashboard,
                LocalizableString.Create<DashboardDemoResource>("MyDashboard")
                )
                .WithWidget(RoleListWidgetViewComponent.WidgetName)
                .WithWidget(UserCountWidgetViewComponent.WidgetName)
                .WithGlobalFilter(DateRangeGlobalFilterViewComponent.GlobalFilterName);

            return new List<DashboardDefinition>
            {
                myDashboard
            };
        }
    }
}
