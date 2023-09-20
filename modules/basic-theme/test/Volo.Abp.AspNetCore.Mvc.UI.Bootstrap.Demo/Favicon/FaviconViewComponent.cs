using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.Favicon;

public class FaviconViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/Favicon/Default.cshtml");
    }
}