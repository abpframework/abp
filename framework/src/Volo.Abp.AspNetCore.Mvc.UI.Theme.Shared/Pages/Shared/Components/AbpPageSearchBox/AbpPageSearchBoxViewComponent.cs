using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Pages.Shared.Components.AbpPageSearchBox
{
    public class AbpPageSearchBoxViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Shared/Components/AbpPageSearchBox/Default.cshtml");
        }
    }
}