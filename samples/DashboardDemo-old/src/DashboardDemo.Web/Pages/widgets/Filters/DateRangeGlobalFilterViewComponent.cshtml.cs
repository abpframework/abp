using Microsoft.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets.Filters
{
    [ViewComponent]
    public class DateRangeGlobalFilterViewComponent : ViewComponent
    {
        public const string Name = "DateRangeGlobalFilter";

        public const string DisplayName = "Date Range Filter";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/Filters/DateRangeGlobalFilterViewComponent.cshtml", new DateRangeGlobalFilterViewComponent());
        }
    }
}