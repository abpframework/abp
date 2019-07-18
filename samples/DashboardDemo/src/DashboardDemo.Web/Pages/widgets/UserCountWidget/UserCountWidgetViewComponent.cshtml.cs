using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Pages.widgets
{
    public class UserCountWidgetViewComponent : AbpViewComponent
    {
        public const string Name = "UserCountWidget";
        
        public const string DisplayName = "UserCountWidgett";

        public IViewComponentResult Invoke()
        {
            return View("/Pages/widgets/UserCountWidget/UserCountWidgetViewComponent.cshtml", new UserCountWidgetViewComponent());
        }
    }
}