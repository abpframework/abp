using JetBrains.Annotations;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards
{
    public class DashboardWidgetConfiguration
    {
        [NotNull]
        public string WidgetName { get; }

        [CanBeNull]
        public WidgetDimensions Dimensions { get; set; }

        public DashboardWidgetConfiguration(
            [NotNull] string widgetName,
            [CanBeNull] WidgetDimensions dimensions
            )
        {
            WidgetName = Check.NotNullOrWhiteSpace(widgetName, nameof(widgetName));
            Dimensions = dimensions;
        }
    }
}