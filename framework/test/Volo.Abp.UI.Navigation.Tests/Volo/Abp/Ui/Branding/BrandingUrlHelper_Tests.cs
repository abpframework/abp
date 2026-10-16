using Shouldly;
using Xunit;

namespace Volo.Abp.Ui.Branding;

public class BrandingUrlHelper_Tests
{
    [Theory]
    [InlineData("//cdn.example.com/logo.svg")]
    [InlineData("https://cdn.example.com/logo.svg")]
    [InlineData("data:image/svg+xml;base64,PHN2Zy8+")]
    [InlineData("blob:1234")]
    [InlineData(" https://cdn.example.com/logo.svg ")]
    [InlineData("http://[")]
    public void Should_Detect_External_Urls(string url)
    {
        BrandingUrlHelper.IsExternalUrl(url).ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("logo.svg")]
    [InlineData("/logo.svg")]
    [InlineData("~/logo.svg")]
    [InlineData("/images/a:b.svg")]
    [InlineData("1st:logo.svg")]
    public void Should_Not_Detect_Other_Urls_As_External(string? url)
    {
        BrandingUrlHelper.IsExternalUrl(url).ShouldBeFalse();
    }

    [Theory]
    [InlineData("~/images/logo.svg", "images/logo.svg")]
    [InlineData("/images/logo.svg", "images/logo.svg")]
    [InlineData("images/logo.svg", "images/logo.svg")]
    public void Should_Remove_The_Application_Relative_Prefix(string url, string expected)
    {
        BrandingUrlHelper.RemoveApplicationRelativePrefix(url).ShouldBe(expected);
    }

    [Fact]
    public void Should_Escape_The_Characters_That_End_A_Css_Url_Or_A_Style_Element()
    {
        BrandingUrlHelper.EscapeCssValue("logo.svg\\')\r\n\f</style>")
            .ShouldBe("logo.svg\\\\\\')%3C/style>");
    }
}
