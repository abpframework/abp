using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class DemoStatisticsViewComponentModel : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/DemoStatisticsViewComponent.cshtml", new DemoStatisticsViewComponentModel());
        }
    }
}