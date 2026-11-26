using Microsoft.AspNetCore.Mvc;
using Volo.Abp.UI.Navigation;

namespace Volo.CmsKit.Web.Contents;

[Route("test-menu")]
public class MenuRenderingController : Controller
{
    [HttpGet("{kind}")]
    public IActionResult Render(string kind, string elementId)
    {
        var item = new ApplicationMenuItem("Guide", "Guide", url: "/guide") { ElementId = elementId };
        if (kind == "dropdown")
        {
            return PartialView("~/Themes/Basic/Components/Menu/_MenuItem.cshtml", item);
        }

        var menu = new ApplicationMenu("Main").AddItem(item);
        return PartialView(kind == "user"
            ? "~/Themes/Basic/Components/Toolbar/UserMenu/Default.cshtml"
            : "~/Themes/Basic/Components/Menu/Default.cshtml", menu);
    }
}
