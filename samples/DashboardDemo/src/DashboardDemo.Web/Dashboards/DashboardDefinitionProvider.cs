using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using DashboardDemo.Pages.widgets.Filters;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
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
                .WithWidget(UserCountWidgetViewComponent.Name)
                .WithWidget(RoleListWidgetViewComponent.Name)
                .WithWidget(MonthlyProfitWidgetViewComponent.Name)
                .WithGlobalFilter(DateRangeGlobalFilterViewComponent.Name);

            return new List<DashboardDefinition>
            {
                myDashboard
            };
        }
    }
}
