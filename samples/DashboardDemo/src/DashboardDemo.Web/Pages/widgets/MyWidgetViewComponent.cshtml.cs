using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class MyWidgetViewComponent : ViewComponent
    {
        public const string WidgetName = "MyWidget";
        
        public const string DisplayName = "MyWidgett";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/MyWidgetViewComponent.cshtml", new MyWidgetViewComponent());
        }
    }
}