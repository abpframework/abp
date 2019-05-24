using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards.Components.Dashboard
{
    public class DashboardViewComponent : AbpViewComponent
    {
        private readonly DashboardOptions _dashboardOptions;
        private readonly WidgetOptions _widgetOptions;

        public DashboardViewComponent(IOptions<DashboardOptions> dashboardOptions, IOptions<WidgetOptions> widgetOptions)
        {
            _dashboardOptions = dashboardOptions.Value;
            _widgetOptions = widgetOptions.Value;
        }

        public IViewComponentResult Invoke(string dashboardName)
        {
            var dashboard = _dashboardOptions.Dashboards.Single(d => d.Name.Equals(dashboardName));

            var model = new DashboardViewModel(dashboard, _widgetOptions.Widgets);

            return View("~/Volo/Abp/AspNetCore/Mvc/UI/Dashboards/Components/Dashboard/Default.cshtml", model);
        }
    }
}