using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets.Filters
{
    [ViewComponent]
    public class RefreshGlobalFilterViewComponent : ViewComponent
    {
        public const string GlobalFilterName = "RefreshGlobalFilter";

        public const string DisplayName = "Refresh Filter";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/Filters/RefreshGlobalFilterViewComponent.cshtml", new RefreshGlobalFilterViewComponent());
        }
    }
}