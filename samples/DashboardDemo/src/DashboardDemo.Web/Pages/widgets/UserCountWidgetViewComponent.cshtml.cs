using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    public class UserCountWidgetViewComponent : AbpViewComponent
    {
        public const string WidgetName = "UserCountWidget";
        
        public const string DisplayName = "UserCountWidgett";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/UserCountWidgetViewComponent.cshtml", new UserCountWidgetViewComponent());
        }
    }
}