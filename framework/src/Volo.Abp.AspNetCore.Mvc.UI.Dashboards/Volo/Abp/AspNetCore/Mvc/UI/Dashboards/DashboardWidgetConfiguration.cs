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

        [CanBeNull]
        public WidgetLocation Location { get; set; }

        public DashboardWidgetConfiguration(
            [NotNull] string widgetName,
            [CanBeNull] WidgetDimensions dimensions = null,
            [CanBeNull] WidgetLocation location = null
            )
        {
            WidgetName = Check.NotNullOrWhiteSpace(widgetName, nameof(widgetName));
            Dimensions = dimensions;
            Location = location;
        }
    }
}