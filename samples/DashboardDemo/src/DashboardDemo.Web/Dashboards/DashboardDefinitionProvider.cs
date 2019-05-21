using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace DashboardDemo.Dashboards
{
    public class DashboardDefinitionProvider : IDashboardDefinitionProvider
    {
        public List<DashboardDefinition> GetDefinitions()
        {
            var myDashboard = new DashboardDefinition(
                DashboardNames.MyDashboard,
                new LocalizableString(typeof(DashboardDemoResource), "MyDashboard")
            );

            myDashboard.AvailableWidgets.Add(
                new DashboardWidgetConfiguration(WidgetNames.MyWidget)
                );
            myDashboard.AvailableWidgets.Add(
                new DashboardWidgetConfiguration(WidgetNames.DemoStatistics, new WidgetDimensions(8,2))
            );


            var dashboards = new List<DashboardDefinition>
            {
                myDashboard
            };

            return dashboards;
        }
    }
}
