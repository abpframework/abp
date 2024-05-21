using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Admin.Web.Components.MySettingGroup;

[Widget(
     ScriptFiles = new[] { "/Components/CommentsSetting/index.js" }
 )]
[ViewComponent(Name = "CmsKitCommentOptions")]
public class CommentSettingViewComponent: AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/CommentsSetting/Index.cshtml");
    }
}
