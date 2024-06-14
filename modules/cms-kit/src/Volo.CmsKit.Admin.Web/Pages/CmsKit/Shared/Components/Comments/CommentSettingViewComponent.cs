using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Shared.Components.Comments;

[Widget(
    ScriptTypes = new[] { typeof(CommentSettingScriptBundleContributor) },
    AutoInitialize = true
)]
[ViewComponent(Name = "CmsCommentSetting")]
public class CommentSettingViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Pages/CmsKit/Shared/Components/Comments/Default.cshtml");
    }
}
