using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MyCompanyName.MyProjectName.Web.Theme.Themes.MyProjectName.Components.Brand
{
    public class MainNavbarBrandViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Themes/MyProjectName/Components/Brand/Default.cshtml");
        }
    }
}
