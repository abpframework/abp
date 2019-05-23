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
        public DashboardOptions DashboardOptions { get; set; }
        public WidgetOptions WidgetOptions { get; set; }

        public DashboardViewModel(string dashboardName, DashboardOptions dashboardOptions, WidgetOptions widgetOptions)
        {
            Dashboard = dashboardOptions.Dashboards.Single(d => d.Name.Equals(dashboardName));
            DashboardOptions = dashboardOptions;
            WidgetOptions = widgetOptions;
        }

        public WidgetDefinition GetWidget(string name)
        {
            return WidgetOptions.Widgets.Single(d => d.Name.Equals(name));
        }
    }
}
