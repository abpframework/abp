using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.TextTemplating.Scriban;

public class ScribanTemplateRenderingEngine_CultureContext_Tests : AbpTextTemplatingTestBase<ScribanTextTemplatingTestModule>
{
    private readonly ITemplateRenderer _templateRenderer;

    public ScribanTemplateRenderingEngine_CultureContext_Tests()
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
            ScribanTestTemplateDefinitionProvider.CultureContext,
            cultureName: cultureName
        )).ShouldBe(expected);
    }

    [Fact]
    public async Task Should_Keep_The_Values_Passed_By_The_Caller()
    {
        (await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.CultureContext,
            cultureName: "ar",
            globalContext: new Dictionary<string, object>
            {
                [TemplateRenderingEngineBase.CultureContextKey] = "custom"
            }
        )).ShouldBe("<html lang=\"custom\" dir=\"rtl\">");
    }

    [Fact]
    public async Task Should_Not_Leak_The_Culture_Of_A_Rendering_Into_The_Next_One()
    {
        var globalContext = new Dictionary<string, object>();

        (await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.CultureContext,
            cultureName: "en",
            globalContext: globalContext
        )).ShouldBe("<html lang=\"en\" dir=\"ltr\">");

        (await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.CultureContext,
            cultureName: "ar",
            globalContext: globalContext
        )).ShouldBe("<html lang=\"ar\" dir=\"rtl\">");

        globalContext.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Not_Write_The_Layout_Content_Into_The_Context_Of_The_Caller()
    {
        var globalContext = new Dictionary<string, object>();

        await _templateRenderer.RenderAsync(
            TestTemplates.ForgotPasswordEmail,
            model: new { link = "http://abp.io" },
            cultureName: "en",
            globalContext: globalContext
        );

        globalContext.ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Keep_The_Comparer_Of_The_Context_Of_The_Caller()
    {
        // ABP_CULTURE already covers the key here, so nothing is added; losing the comparer on the copy
        // would add a second, lowercase entry and render "ar". Scriban itself looks names up case
        // sensitively, hence the empty culture in the output.
        var globalContext = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
        {
            ["ABP_CULTURE"] = "custom"
        };

        (await _templateRenderer.RenderAsync(
            ScribanTestTemplateDefinitionProvider.CultureContext,
            cultureName: "ar",
            globalContext: globalContext
        )).ShouldBe("<html lang=\"\" dir=\"rtl\">");
    }

    [Fact]
    public async Task Should_Fall_Back_To_English_For_The_Invariant_Culture()
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            (await _templateRenderer.RenderAsync(
                ScribanTestTemplateDefinitionProvider.CultureContext
            )).ShouldBe("<html lang=\"en\" dir=\"ltr\">");
        }
    }
}
