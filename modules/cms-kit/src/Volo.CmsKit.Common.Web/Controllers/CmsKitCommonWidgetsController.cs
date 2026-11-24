using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;

namespace Volo.CmsKit.Web.Controllers;

[Authorize]
public class CmsKitCommonWidgetsController : AbpController
{
    [HttpPost]
    public IActionResult ContentPreview(ContentPreviewDto dto)
    {
        return ViewComponent(typeof(ContentPreviewViewComponent), new { content = dto.Content, preventXSS = dto.PreventXSS, blogId = dto.BlogId });
    }
}
