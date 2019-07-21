using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.CountersWidget
{
    [Widget(
        StyleFiles = new[] { "/Pages/Components/CountersWidget/Default.css" }
        )]
    public class CountersWidgetViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(DateTime startDate, DateTime endDate)
        {
            /* Instead of hard-coded / calculated values, get counts from a database or a service!
             */

            var dayFactor = (int)Math.Round(endDate.Subtract(startDate).TotalDays + 1);
            return View(new CountersWidgetViewModel
            {
                NewUsers = dayFactor * 86,
                ActiveUsers = dayFactor * 58,
                TotalIncome = dayFactor * 749.53,
                TotalProfit = dayFactor * 239.45,
            });
        }
    }
}
