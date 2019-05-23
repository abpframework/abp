using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class RoleListWidgetViewComponent : ViewComponent
    {
        public const string WidgetName = "RoleListWidget";

        public const string DisplayName = "RoleListWidgets";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/RoleListWidgetViewComponent.cshtml", new RoleListWidgetViewComponent());
        }
    }
}