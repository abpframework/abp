using System.Collections.Generic;
using DashboardDemo.Localization.DashboardDemo;
using DashboardDemo.Pages.widgets;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.Localization;

namespace DashboardDemo.Widgets
{
    public static class WidgetDefinitionProvider
    {
        public static List<WidgetDefinition> GetDefinitions()
        {
            return new List<WidgetDefinition>
            {
                new WidgetDefinition(
                    WidgetNames.MyWidget,
                    typeof(MyWidgetViewComponentModel),
                    LocalizableString.Create<DashboardDemoResource>("MyWidgett")
                ),
                new WidgetDefinition(
                    WidgetNames.DemoStatistics,
                    typeof(DemoStatisticsViewComponentModel),
                    LocalizableString.Create<DashboardDemoResource>("DemoStatistics")
                )
            };
        }
    }
}
