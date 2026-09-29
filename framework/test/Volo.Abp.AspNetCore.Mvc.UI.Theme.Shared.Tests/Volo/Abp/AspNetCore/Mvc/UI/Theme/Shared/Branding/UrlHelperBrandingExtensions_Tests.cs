using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Branding;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Branding;

public class UrlHelperBrandingExtensions_Tests
{
    private readonly IUrlHelper _urlHelper;

    public UrlHelperBrandingExtensions_Tests()
    {
        var httpContext = new DefaultHttpContext {
            Request = { PathBase = "/myapp" }
        };

        _urlHelper = new UrlHelper(new ActionContext(httpContext, new RouteData(), new ActionDescriptor()));
    }

    [Theory]
    [InlineData("logo.svg")]
    [InlineData("/logo.svg")]
    [InlineData("~/logo.svg")]
    public void Should_Treat_All_Local_Formats_As_Application_Relative(string url)
    {
        _urlHelper.ResolveBrandingUrl(url).ShouldBe("/myapp/logo.svg");
    }

    // Keep in sync with the same test of the blazor themes.
    [Theory]
    [InlineData("logo.svg", "/myapp/logo.svg")]
    [InlineData("/logo.svg", "/myapp/logo.svg")]
    [InlineData("~/logo.svg", "/myapp/logo.svg")]
    [InlineData("images/logo.svg?v=42", "/myapp/images/logo.svg?v=42")]
    [InlineData("images/logo.svg#brand", "/myapp/images/logo.svg#brand")]
    [InlineData("https://cdn.example.com/logo.svg", "https://cdn.example.com/logo.svg")]
    [InlineData("//cdn.example.com/logo.svg", "//cdn.example.com/logo.svg")]
    [InlineData("data:image/svg+xml;base64,PHN2Zy8+", "data:image/svg+xml;base64,PHN2Zy8+")]
    [InlineData("http://[", "http://[")]
    [InlineData("https://", "https://")]
    [InlineData("http://a b", "http://a b")]
    [InlineData("  ~/logo.svg  ", "/myapp/logo.svg")]
    [InlineData(" https://cdn.example.com/logo.svg ", "https://cdn.example.com/logo.svg")]
    [InlineData("/http://cdn.example.com/logo.svg", "/http://cdn.example.com/logo.svg")]
    [InlineData("~/http://cdn.example.com/logo.svg", "~/http://cdn.example.com/logo.svg")]
    public void Should_Resolve_The_Same_As_The_Blazor_Themes(string url, string expected)
    {
        _urlHelper.ResolveBrandingUrl(url).ShouldBe(expected);
    }

    [Fact]
    public void Should_Keep_A_Dot_Segment_Of_An_Application_Relative_Url()
    {
        _urlHelper.ResolveBrandingUrl("images/../logo.svg").ShouldBe("/myapp/images/../logo.svg");
    }

    [Fact]
    public void Should_Escape_A_Form_Feed_In_Css()
    {
        _urlHelper.ResolveBrandingCssUrl("data:image/svg+xml,\u000C<svg/>")
            .ShouldBe("data:image/svg+xml,%3Csvg/>");
    }

    [Fact]
    public void Should_Keep_Query_And_Fragment()
    {
        _urlHelper.ResolveBrandingUrl("~/images/logo.svg?v=42").ShouldBe("/myapp/images/logo.svg?v=42");
        _urlHelper.ResolveBrandingUrl("images/logo.svg#brand").ShouldBe("/myapp/images/logo.svg#brand");
    }

    [Theory]
    [InlineData("http://cdn.example.com/logo.svg")]
    [InlineData("https://cdn.example.com/logo.svg")]
    [InlineData("//cdn.example.com/logo.svg")]
    [InlineData("data:image/svg+xml;base64,PHN2Zy8+")]
    [InlineData("http://[")]
    [InlineData("https://")]
    [InlineData("http://a b")]
    public void Should_Return_External_Urls_As_They_Are(string url)
    {
        _urlHelper.ResolveBrandingUrl(url).ShouldBe(url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Return_Null_When_Url_Is_Empty(string? url)
    {
        _urlHelper.ResolveBrandingUrl(url).ShouldBeNull();
    }

    [Fact]
    public void Should_Escape_Css_Breaking_Characters()
    {
        _urlHelper.ResolveBrandingCssUrl("data:image/svg+xml,<svg/>')</style>")
            .ShouldBe("data:image/svg+xml,%3Csvg/>\\')%3C/style>");
    }

    [Fact]
    public void Should_Resolve_And_Escape_Application_Relative_Urls()
    {
        _urlHelper.ResolveBrandingCssUrl("~/images/logo.svg").ShouldBe("/myapp/images/logo.svg");
    }
}
