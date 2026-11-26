using AngleSharp.Html.Parser;
using Shouldly;
using Volo.Blogging.Pages.Blogs;
using Xunit;

namespace Volo.Blogging;

public class BloggingPageHelper_Tests
{
    private readonly BloggingPageHelper _helper = new();

    [Theory]
    [InlineData("Hello **team**\n\n<script>alert(1)</script>")]
    [InlineData("Hello **team**\n\n<img src=x onerror=alert(1)>")]
    [InlineData("Hello **team**\n\n[click](javascript:alert(1))")]
    [InlineData("Hello **team**\n\n<javascript:alert(1)>")]
    public void Should_Sanitize_Post_Content(string markdown)
    {
        var html = _helper.RenderMarkdownToHtmlAsString(markdown);
        var document = new HtmlParser().ParseDocument(html);

        html.ShouldContain("<strong>team</strong>");
        document.QuerySelectorAll("script, [onerror], [onclick]").ShouldBeEmpty();
        foreach (var link in document.QuerySelectorAll("[href]"))
        {
            link.GetAttribute("href").ShouldNotStartWith("javascript:");
        }
    }

    [Fact]
    public void Should_Preserve_Harmless_Html_And_Markdown()
    {
        var html = _helper.RenderMarkdownToHtmlAsString("Hello **team**\n\n<div class=\"text-info\">Note<br>More</div>\n\n[Docs](https://example.com)");

        html.ShouldContain("<strong>team</strong>");
        html.ShouldContain("<div class=\"text-info\">");
        html.ShouldContain("<br>");
        html.ShouldContain("href=\"https://example.com\"");
    }

    [Fact]
    public void Should_Not_Inject_An_Unterminated_Tag_Through_The_Excerpt()
    {
        var excerpt = _helper.GetShortContent("Intro text\n\n<details open ontoggle=alert(1) x=\"");
        var document = new HtmlParser().ParseDocument("<p>" + excerpt + "</p><a href=\"/next\">Next</a>");

        document.QuerySelectorAll("[ontoggle], script").ShouldBeEmpty();
        document.QuerySelector("a[href='/next']").ShouldNotBeNull();
    }
}
