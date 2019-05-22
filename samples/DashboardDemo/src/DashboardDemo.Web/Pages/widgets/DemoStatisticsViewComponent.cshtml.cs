using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class DemoStatisticsViewComponent : ViewComponent
    {
        public const string WidgetName = "DemoStatistics";

        public const string DisplayName = "DemoStatisticss";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/DemoStatisticsViewComponent.cshtml", new DemoStatisticsViewComponent());
        }
    }
}