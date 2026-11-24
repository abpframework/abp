using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.Data;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;
using Volo.CmsKit.Web.Renderers;
using Xunit;

namespace Volo.CmsKit.Web.Contents;

public class ContentPreview_Tests
{
    [Fact]
    public async Task Should_Allow_Trusted_Page_Previews_To_Opt_Out_Of_Sanitization()
    {
        var parser = new ContentParser(Options.Create(new CmsKitContentWidgetOptions()));
        var component = new ContentPreviewViewComponent(parser)
        {
            ViewComponentContext = new ViewComponentContext
            {
                Arguments = new Dictionary<string, object> { { "preventXSS", false } }
            }
        };
        const string content = "<script>alert(document.domain)</script>";

        var result = (ViewViewComponentResult)await component.InvokeAsync(content);
        var model = result.ViewData.Model.ShouldBeOfType<DefaultContentDto>();
        var renderer = new MarkdownToHtmlRenderer(new MarkdownPipelineBuilder().Build());
        var html = await renderer.RenderAsync(content, model.AllowHtmlTags, model.PreventXSS);

        model.PreventXSS.ShouldBeFalse();
        html.ShouldContain(content);
    }

    [Fact]
    public async Task Should_Sanitize_Blog_Content_By_Default()
    {
        var parser = new ContentParser(Options.Create(new CmsKitContentWidgetOptions()));
        var component = new LegacyPreviewComponent(parser);
        const string content = "Hello **team**\n\n<script>alert(document.domain)</script>\n\n<img src=x onerror=alert(document.domain)>";

        var result = (ViewViewComponentResult)await component.InvokeAsync(content);
        var model = result.ViewData.Model.ShouldBeOfType<DefaultContentDto>();
        var renderer = new MarkdownToHtmlRenderer(new MarkdownPipelineBuilder().Build());
        var html = await renderer.RenderAsync(
            model.ContentFragments[0].GetProperty<string>(ContentConsts.Content), model.AllowHtmlTags, model.PreventXSS);

        model.PreventXSS.ShouldBeTrue();
        component.WasCalled.ShouldBeTrue();
        html.ShouldContain("<strong>team</strong>");
        html.ShouldNotContain("<script");
        html.ShouldNotContain("onerror");
    }

    private sealed class LegacyPreviewComponent : ContentPreviewViewComponent
    {
        public bool WasCalled { get; private set; }

        public LegacyPreviewComponent(ContentParser parser) : base(parser)
        {
        }

        public override Task<IViewComponentResult> InvokeAsync(string content)
        {
            WasCalled = true;
            return base.InvokeAsync(content);
        }
    }
}
