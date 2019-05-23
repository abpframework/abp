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
                    UserCountWidgetViewComponent.WidgetName,
                    typeof(UserCountWidgetViewComponent),
                    LocalizableString.Create<DashboardDemoResource>(UserCountWidgetViewComponent.DisplayName),
                    new WidgetDimensions(4,2)
                ),
                new WidgetDefinition(
                    RoleListWidgetViewComponent.WidgetName,
                    typeof(RoleListWidgetViewComponent),
                    LocalizableString.Create<DashboardDemoResource>(RoleListWidgetViewComponent.DisplayName),
                    new WidgetDimensions(6,4)
                )
            };
        }
    }
}
