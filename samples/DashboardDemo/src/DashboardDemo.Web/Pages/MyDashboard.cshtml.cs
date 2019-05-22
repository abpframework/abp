using System.Linq;
using DashboardDemo.Dashboards;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.UI.Dashboards;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Pages
{
    public class MyDashboardModel : DashboardDemoPageModelBase
    {
        public DashboardDefinition Dashboard { get; set; }

        public readonly DashboardOptions _dashboardOptions;
        public readonly WidgetOptions _widgetOptions;

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