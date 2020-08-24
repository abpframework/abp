using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Rating
{
    [ViewComponent(Name = "CmsRating")]
    [Widget(
        StyleTypes = new[] {typeof(RatingStyleBundleContributor)},
        ScriptTypes = new[] {typeof(RatingScriptBundleContributor)},
        RefreshUrl = "/CmsKitPublicWidgets/Rating"
    )]
    public class RatingViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/CmsKit/Shared/Components/Rating/Default.cshtml");
        }
    }
}