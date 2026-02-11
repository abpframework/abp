using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Web.Contents;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;

public class ContentPreviewViewComponent : AbpViewComponent
{
    protected ContentParser ContentParser { get; }

    public ContentPreviewViewComponent(ContentParser contentParser)
    {
        ContentParser = contentParser;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string content)
    {
        var fragments = await ContentParser.ParseAsync(content);

        return View("~/Pages/CmsKit/Components/ContentPreview/Default.cshtml", new DefaultContentDto
        {
            ContentFragments = fragments,
            AllowHtmlTags = true,
            PreventXSS = false
        });
    }
}
