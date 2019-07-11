using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    public class MonthlyProfitWidgetViewComponent : AbpViewComponent
    {
        public const string WidgetName = "MonthlyProfitWidget";
        
        public const string DisplayName = "Monthly Profit Widget";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/MonthlyProfitWidget/MonthlyProfitWidgetViewComponent.cshtml", new MonthlyProfitWidgetViewComponent());
        }
    }
}