using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar.Tools
{
    public class MainNavbarToolsViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/Components/Theme/MainNavbar/Tools/Default.cshtml");
        }
    }
}
