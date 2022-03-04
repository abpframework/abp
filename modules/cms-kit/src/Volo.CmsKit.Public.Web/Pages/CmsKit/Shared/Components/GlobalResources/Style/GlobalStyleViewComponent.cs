using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.GlobalResources.Style;

public class GlobalStyleViewComponent : AbpViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View("~/Pages/CmsKit/Shared/Components/GlobalResources/Style/Default.cshtml");
    }
}