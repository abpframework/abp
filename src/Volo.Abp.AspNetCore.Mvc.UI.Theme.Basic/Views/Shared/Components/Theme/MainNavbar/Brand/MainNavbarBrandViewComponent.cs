using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic.Views.Shared.Components.Theme.MainNavbar.Brand
{
    public class MainNavbarBrandViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Views/Shared/Components/Theme/MainNavbar/Brand/Default.cshtml");
        }
    }
}
