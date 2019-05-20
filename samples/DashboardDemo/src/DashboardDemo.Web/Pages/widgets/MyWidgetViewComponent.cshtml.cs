using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class MyWidgetViewComponentModel : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/MyWidgetViewComponent.cshtml", new MyWidgetViewComponentModel());
        }
    }
}