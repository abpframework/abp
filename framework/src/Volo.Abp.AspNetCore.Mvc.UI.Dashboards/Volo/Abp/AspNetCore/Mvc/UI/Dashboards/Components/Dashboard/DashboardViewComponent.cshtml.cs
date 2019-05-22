using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.Abp.AspNetCore.Mvc.UI.Dashboards.Components.Dashboard
{
    public class DashboardViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string dashboardName, DashboardOptions dashboardOptions, WidgetOptions widgetOptions)
        {
            var model = new DashboardViewModel(dashboardName, dashboardOptions, widgetOptions);
            return View("~/Volo/Abp/AspNetCore/Mvc/UI/Dashboards/Components/Dashboard/Default.cshtml", model);
        }
    }
}