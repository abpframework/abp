using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class LanguageSwitchRouteCultureReplacement_Tests
{
    private readonly AbpAspNetCoreMvcQueryStringCultureReplacement _replacement = new();

    [Fact]
    public async Task Should_Replace_Route_Prefix()
    {
        var context = CreateContext("tr", "en", "/tr/products");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en/products");
    }

    [Fact]
    public async Task Should_Replace_Region_Culture()
    {
        var context = CreateContext("en-US", "zh-Hans", "/en-US/about");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/zh-Hans/about");
    }

    [Fact]
    public async Task Should_Replace_Culture_Only_Url()
    {
        var context = CreateContext("tr", "en", "/tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en");
    }

    [Fact]
    public async Task Should_Replace_Culture_With_Query_String()
    {
        var context = CreateContext("tr", "en", "/tr?returnUrl=/home");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en?returnUrl=/home");
    }

    [Fact]
    public async Task Should_Replace_Culture_After_Tenant()
    {
        var context = CreateContext("zh-Hans", "en", "/tenant-a/zh-Hans/About");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/tenant-a/en/About");
    }

    [Fact]
    public async Task Should_Replace_Culture_Only_After_Tenant()
    {
        var context = CreateContext("tr", "en", "/tenant-a/tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/tenant-a/en");
    }

    [Fact]
    public async Task Should_Replace_Via_RouteData_When_No_CurrentCulture()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.RouteValues["culture"] = "tr";
        var context = new QueryStringCultureReplacementContext(
            httpContext, new RequestCulture("en"), "/tr/products");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en/products");
    }

    [Fact]
    public async Task Should_Not_Replace_When_No_Culture_Source()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), "/volosoft/products");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/volosoft/products");
    }

    [Fact]
    public async Task Should_Not_Replace_Culture_Inside_Longer_Segment()
    {
        // "en" must not match inside "enterprise"
        var context = CreateContext("en", "tr", "/enterprise/products");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/enterprise/products");
    }

    [Fact]
    public async Task Should_Not_Replace_Culture_When_Culture_Is_Segment_Prefix()
    {
        // "fr" appears at the start of "fr-zone" but is not a complete segment
        var context = CreateContext("fr", "en", "/fr-zone/about");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/fr-zone/about");
    }

    [Fact]
    public async Task Should_Replace_Culture_Before_Fragment()
    {
        var context = CreateContext("en", "tr", "/en#section");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/tr#section");
    }

    [Fact]
    public async Task Should_Replace_Culture_Before_Fragment_With_Path()
    {
        var context = CreateContext("en", "tr", "/en/about#top");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/tr/about#top");
    }

    [Fact]
    public async Task Should_Replace_Query_String_Culture()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en", "en"), "/home?culture=tr&ui-culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/home?culture=en&ui-culture=en");
    }

    [Fact]
    public async Task Should_Replace_Both_Route_And_Query_String()
    {
        var context = CreateContext("tr", "en", "/tr/home?culture=tr&ui-culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en/home?culture=en&ui-culture=en");
    }

    [Fact]
    public async Task Should_Handle_Null_ReturnUrl()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), null!);
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Handle_Empty_ReturnUrl()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), "");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("");
    }

    [Fact]
    public async Task Should_Not_Replace_When_CurrentCulture_Not_In_ReturnUrl()
    {
        // currentCulture is "fr" but returnUrl has no "/fr" segment
        var context = CreateContext("fr", "en", "/about");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/about");
    }

    [Fact]
    public async Task Should_Handle_Same_Culture_Switch()
    {
        // Switching to the same culture — no change
        var context = CreateContext("en", "en", "/en/about");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en/about");
    }

    [Fact]
    public async Task Should_Replace_Case_Insensitive()
    {
        var context = CreateContext("zh-hans", "en", "/zh-Hans/about");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/en/about");
    }

    [Fact]
    public async Task Should_Handle_Whitespace_ReturnUrl()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), "   ");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("   ");
    }

    [Fact]
    public async Task Should_Prefer_CurrentCulture_Over_RouteData()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.RouteValues["culture"] = "fr";
        var context = new QueryStringCultureReplacementContext(
            httpContext, new RequestCulture("en"), "/tr/about", currentCulture: "tr");
        await _replacement.ReplaceAsync(context);
        // Should use "tr" from CurrentCulture, not "fr" from RouteData
        context.ReturnUrl.ShouldBe("/en/about");
    }

    [Fact]
    public async Task Should_Only_Replace_Query_String_When_No_Route_Culture()
    {
        // No currentCulture, no RouteData — only query string replacement
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en", "en"), "/?culture=tr&ui-culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/?culture=en&ui-culture=en");
    }

    [Fact]
    public async Task Should_Replace_Culture_When_Only_Culture_Param_Present()
    {
        // culture= and ui-culture= are now handled independently
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), "/?culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/?culture=en");
    }

    [Fact]
    public async Task Should_Replace_UiCulture_When_Only_UiCulture_Param_Present()
    {
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(), new RequestCulture("en"), "/?ui-culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/?ui-culture=en");
    }

    [Fact]
    public async Task Should_Support_Numeric_Region_Culture_Tag()
    {
        // es-419 (Latin America Spanish) contains a digit — previously the regex
        // [A-Za-z-]+ would not match it, leaving the query string unreplaced.
        var context = new QueryStringCultureReplacementContext(
            new DefaultHttpContext(),
            new RequestCulture("es-419", "es-419"),
            "/home?culture=tr&ui-culture=tr");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/home?culture=es-419&ui-culture=es-419");
    }

    [Fact]
    public async Task Should_Replace_Only_First_Culture_Occurrence_In_Path()
    {
        // /en/products/en/details — the second "/en" is part of the path content,
        // not a culture prefix, and must not be replaced.
        var context = CreateContext("en", "tr", "/en/products/en/details");
        await _replacement.ReplaceAsync(context);
        context.ReturnUrl.ShouldBe("/tr/products/en/details");
    }

    private static QueryStringCultureReplacementContext CreateContext(
        string currentCulture, string targetCulture, string returnUrl)
    {
        return new QueryStringCultureReplacementContext(
            new DefaultHttpContext(),
            new RequestCulture(targetCulture),
            returnUrl,
            currentCulture);
    }
}
