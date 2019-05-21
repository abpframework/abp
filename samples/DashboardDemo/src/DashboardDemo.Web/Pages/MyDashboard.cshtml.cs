using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardDemo.Dashboards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Pages
{
    public class MyDashboardModel : DashboardDemoPageModelBase
    {
        public DashboardDefinition Dashboard { get; set; }

        private readonly DashboardOptions _dashboardOptions;
        private readonly WidgetOptions _widgetOptions;

        public MyDashboardModel(IOptions<DashboardOptions> dashboardOptions, IOptions<WidgetOptions> widgetOptions)
        {
            _dashboardOptions = dashboardOptions.Value;
            _widgetOptions = widgetOptions.Value;
        }

        public void OnGet()
        {
            Dashboard = _dashboardOptions.Dashboards.Single(d => d.Name.Equals(DashboardNames.MyDashboard));
        }

        public WidgetDefinition GetWidget(string name)
        {
            return _widgetOptions.Widgets.Single(d => d.Name.Equals(name));
        }
    }
}