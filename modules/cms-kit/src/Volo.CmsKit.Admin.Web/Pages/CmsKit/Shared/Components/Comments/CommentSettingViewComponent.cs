using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Shared.Components.Comments;
[ViewComponent(Name = "CmsCommentSetting")]
[Widget(
    ScriptTypes = new[] { typeof(CommentSettingScriptBundleContributor) },
    // RefreshUrl = "/CmsKitPublicWidgets/Commenting",
    AutoInitialize = true
)]
public class CommentSettingViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Pages/CmsKit/Shared/Components/Comments/Index.cshtml");
    }
}
