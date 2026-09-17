using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.Comments;

[Widget]
[ViewComponent(Name = "CmsKitCommentConfiguration")]
public class CmsKitCommentConfigurationViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/Pages/CmsKit/Components/Comments/CmsKitCommentConfiguration.cshtml", new CmsKitCommentConfigurationViewModel());
    }
}