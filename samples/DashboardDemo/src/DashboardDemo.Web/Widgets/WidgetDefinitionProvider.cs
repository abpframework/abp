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
                    MyWidgetViewComponent.WidgetName,
                    typeof(MyWidgetViewComponent),
                    LocalizableString.Create<DashboardDemoResource>(MyWidgetViewComponent.DisplayName)
                ),
                new WidgetDefinition(
                    DemoStatisticsViewComponent.WidgetName,
                    typeof(DemoStatisticsViewComponent),
                    LocalizableString.Create<DashboardDemoResource>(DemoStatisticsViewComponent.DisplayName)
                )
            };
        }
    }
}
