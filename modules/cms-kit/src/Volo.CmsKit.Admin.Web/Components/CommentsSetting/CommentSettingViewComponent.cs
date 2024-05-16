using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.CmsKit.Admin.Web.Components.MySettingGroup;

public class CommentSettingViewComponent: AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Components/CommentsSetting/Index.cshtml");
    }
}
