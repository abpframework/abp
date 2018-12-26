using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.AbpWebSite.Pages.Shared.Components.StandardMeta
{
    public class StandardMetaViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Shared/Components/StandardMeta/Default.cshtml");
        }
    }
}