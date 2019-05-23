using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards.Components.Dashboard
{
    public class DashboardViewModel
    {
        public DashboardDefinition Dashboard { get; set; }

        public WidgetOptions WidgetOptions { get; set; }

        public DashboardViewModel(DashboardDefinition dashboard, WidgetOptions widgetOptions)
        {
            Dashboard = dashboard;
            WidgetOptions = widgetOptions;
        }

        public WidgetDefinition GetWidget(string name)
        {
            return WidgetOptions.Widgets.Single(d => d.Name.Equals(name));
        }
    }
}
