using System;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Web.Pages.Shared.Components.CountersWidget
{
    [Route("Widgets")]
    public class CountersWidgetController : AbpController
    {
        [HttpGet]
        [Route("Counters")]
        public IActionResult Counters(DateTime startDate, DateTime endDate)
        {
            return ViewComponent("CountersWidget", new {startDate, endDate});
        }
    }
}
