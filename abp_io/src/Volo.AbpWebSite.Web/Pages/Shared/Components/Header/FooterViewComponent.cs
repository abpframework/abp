using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.AbpWebSite.Pages.Shared.Components.Header
{
    public class HeaderViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Shared/Components/Header/Default.cshtml");
        }
    }
}