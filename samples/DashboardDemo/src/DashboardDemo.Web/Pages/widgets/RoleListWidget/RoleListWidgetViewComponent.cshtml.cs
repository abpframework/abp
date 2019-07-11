using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    [ViewComponent]
    public class RoleListWidgetViewComponent : ViewComponent
    {
        public const string Name = "RoleListWidget";

        public const string DisplayName = "RoleListWidgets";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/RoleListWidget/RoleListWidgetViewComponent.cshtml", new RoleListWidgetViewComponent());
        }
    }
}