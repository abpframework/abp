using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Web.Contents;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;

public class ContentPreviewViewComponent : AbpViewComponent
{
    protected ContentParser ContentParser { get; }
    protected IBlogFeatureAppService BlogFeatureAppService { get; }

    public ContentPreviewViewComponent(ContentParser contentParser)
        : this(contentParser, null)
    {
    }

    [ActivatorUtilitiesConstructor]
    public ContentPreviewViewComponent(
        ContentParser contentParser,
        IBlogFeatureAppService blogFeatureAppService)
    {
        ContentParser = contentParser;
        BlogFeatureAppService = blogFeatureAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync(string content)
    {
        // Read from the invocation arguments to keep the overridable InvokeAsync(string) signature.
        var preventXSS = (bool)(ViewComponentContext.Arguments?.GetOrDefault("preventXSS") ?? true);
        var blogId = (Guid?)ViewComponentContext.Arguments?.GetOrDefault("blogId");
        if (blogId.HasValue)
        {
            Check.NotNull(BlogFeatureAppService, nameof(BlogFeatureAppService));
            var feature = await BlogFeatureAppService
                .GetOrDefaultAsync(blogId.Value, BlogConsts.PreventXssFeatureName);
            preventXSS = feature.IsEnabled;
        }

        var fragments = await ContentParser.ParseAsync(content);

        return View("~/Pages/CmsKit/Components/ContentPreview/Default.cshtml", new DefaultContentDto
        {
            ContentFragments = fragments,
            AllowHtmlTags = true,
            PreventXSS = preventXSS
        });
    }
}
