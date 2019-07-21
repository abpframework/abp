using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.CountersWidget
{
    [Widget(
        StyleFiles = new[] { "/Pages/Components/CountersWidget/Default.css" },
        ScriptFiles = new[] { "/Pages/Components/CountersWidget/Default.js" }
        )]
    public class CountersWidgetViewComponent : AbpViewComponent
    {
        private readonly IDashboardAppService _dashboardAppService;

        public CountersWidgetViewComponent(IDashboardAppService dashboardAppService)
        {
            _dashboardAppService = dashboardAppService;
        }

        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate)
        {
            var result = await _dashboardAppService.GetCountersWidgetAsync(
                new CountersWidgetInputDto
                {
                    StartDate = startDate,
                    EndDate = endDate
                }
            );

            return View(result);
        }
    }
}
