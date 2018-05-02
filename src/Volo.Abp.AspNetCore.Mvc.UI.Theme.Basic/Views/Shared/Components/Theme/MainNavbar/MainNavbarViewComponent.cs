using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar
{
    public class MainNavbarViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/Components/Theme/MainNavbar/Default.cshtml");
        }
    }
}
