using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Branding;

public class NavigationManagerBrandingExtensions_Tests
{
    [Theory]
    [InlineData("logo.svg")]
    [InlineData("/logo.svg")]
    [InlineData("~/logo.svg")]
    public void Should_Treat_All_Local_Formats_As_Application_Relative(string url)
    {
        CreateNavigationManager("https://localhost/").ResolveBrandingUrl(url)
            .ShouldBe("/logo.svg");

        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl(url)
            .ShouldBe("/myapp/logo.svg");
    }

    // Keep in sync with the same test of the mvc themes.
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
    public void Should_Resolve_The_Same_As_The_Mvc_Themes(string url, string expected)
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl(url)
            .ShouldBe(expected);
    }

    [Theory]
    [InlineData("images/../logo.svg", "/myapp/logo.svg")]
    [InlineData("images/../../logo.svg", "/logo.svg")]
    public void Should_Resolve_A_Dot_Segment(string url, string expected)
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl(url).ShouldBe(expected);
    }

    [Fact]
    public void Should_Not_Resolve_A_Url_That_Points_To_Another_Host()
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl("/http://cdn.example.com/logo.svg")
            .ShouldBe("/http://cdn.example.com/logo.svg");
    }

    [Fact]
    public void Should_Escape_A_Form_Feed_In_Css()
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingCssUrl("data:image/svg+xml,\u000C<svg/>")
            .ShouldBe("data:image/svg+xml,%3Csvg/>");
    }

    [Fact]
    public void Should_Keep_Query_And_Fragment()
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl("~/images/logo.svg?v=42")
            .ShouldBe("/myapp/images/logo.svg?v=42");

        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl("images/logo.svg#brand")
            .ShouldBe("/myapp/images/logo.svg#brand");
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
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl(url)
            .ShouldBe(url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Should_Return_Null_When_Url_Is_Empty(string? url)
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingUrl(url)
            .ShouldBeNull();
    }

    [Fact]
    public void Should_Escape_Css_Breaking_Characters()
    {
        CreateNavigationManager("https://localhost/myapp/")
            .ResolveBrandingCssUrl("data:image/svg+xml,<svg/>')</style><script>alert(1)</script>")
            .ShouldBe("data:image/svg+xml,%3Csvg/>\\')%3C/style>%3Cscript>alert(1)%3C/script>");
    }

    [Fact]
    public void Should_Escape_Backslashes_And_Drop_Line_Breaks()
    {
        CreateNavigationManager("https://localhost/myapp/")
            .ResolveBrandingCssUrl("data:image/svg+xml,a\\b\r\nc")
            .ShouldBe("data:image/svg+xml,a\\\\bc");
    }

    [Fact]
    public void Should_Resolve_And_Escape_Application_Relative_Urls()
    {
        CreateNavigationManager("https://localhost/myapp/")
            .ResolveBrandingCssUrl("~/images/logo.svg")
            .ShouldBe("/myapp/images/logo.svg");
    }

    [Fact]
    public void Should_Return_Null_From_Css_Overload_When_Url_Is_Empty()
    {
        CreateNavigationManager("https://localhost/myapp/").ResolveBrandingCssUrl("   ").ShouldBeNull();
    }

    private static NavigationManager CreateNavigationManager(string baseUri)
    {
        return new TestNavigationManager(baseUri);
    }

    private class TestNavigationManager : NavigationManager
    {
        public TestNavigationManager(string baseUri)
        {
            Initialize(baseUri, baseUri);
        }
    }
}
