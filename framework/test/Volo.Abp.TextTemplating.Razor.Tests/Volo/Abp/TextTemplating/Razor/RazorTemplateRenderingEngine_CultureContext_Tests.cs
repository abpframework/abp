using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.TextTemplating.Razor.SampleTemplates;
using Xunit;

namespace Volo.Abp.TextTemplating.Razor;

public class RazorTemplateRenderingEngine_CultureContext_Tests : AbpTextTemplatingTestBase<RazorTextTemplatingTestModule>
{
    private readonly ITemplateRenderer _templateRenderer;

    public RazorTemplateRenderingEngine_CultureContext_Tests()
    {
        _templateRenderer = GetRequiredService<ITemplateRenderer>();
    }

    [Theory]
    [InlineData("en", "<html lang=\"en\" dir=\"ltr\">")]
    [InlineData("fr", "<html lang=\"fr\" dir=\"ltr\">")]
    [InlineData("ar", "<html lang=\"ar\" dir=\"rtl\">")]
    public async Task Should_Render_Culture_And_Text_Direction_Of_The_Rendering_Culture(string cultureName, string expected)
    {
        (await _templateRenderer.RenderAsync(
            RazorTestTemplates.CultureContext,
            cultureName: cultureName
        )).Trim().ShouldBe(expected);
    }

    [Fact]
    public async Task Should_Not_Leak_The_Culture_Of_A_Rendering_Into_The_Next_One()
    {
        var globalContext = new Dictionary<string, object>();

        (await _templateRenderer.RenderAsync(
            RazorTestTemplates.CultureContext,
            cultureName: "en",
            globalContext: globalContext
        )).Trim().ShouldBe("<html lang=\"en\" dir=\"ltr\">");

        (await _templateRenderer.RenderAsync(
            RazorTestTemplates.CultureContext,
            cultureName: "ar",
            globalContext: globalContext
        )).Trim().ShouldBe("<html lang=\"ar\" dir=\"rtl\">");

        globalContext.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Keep_The_Comparer_Of_The_Context_Of_The_Caller()
    {
        var globalContext = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            ["ABP_CULTURE"] = "custom"
        };

        (await _templateRenderer.RenderAsync(
            RazorTestTemplates.CultureContext,
            cultureName: "ar",
            globalContext: globalContext
        )).Trim().ShouldBe("<html lang=\"custom\" dir=\"rtl\">");
    }
}
