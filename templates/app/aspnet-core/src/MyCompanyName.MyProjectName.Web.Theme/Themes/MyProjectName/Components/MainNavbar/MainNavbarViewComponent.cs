using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName.Web.Theme.Themes.MyProjectName.Components.MainNavbar
{
    public class MainNavbarViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/MyProjectName/Components/MainNavbar/Default.cshtml");
        }
    }
}
