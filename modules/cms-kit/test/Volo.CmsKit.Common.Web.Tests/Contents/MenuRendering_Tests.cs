using System;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Xunit;

namespace Volo.CmsKit.Web.Contents;

public class MenuRendering_Tests : AbpWebApplicationFactoryIntegratedTest<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
    }

    [Theory]
    [InlineData("root", "guide\" onmouseover=\"alert(1)")]
    [InlineData("dropdown", "guide\" onmouseover=\"alert(1)")]
    [InlineData("user", "guide\" onmouseover=\"alert(1)")]
    [InlineData("root", "guide onmouseover=alert(1)//")]
    [InlineData("dropdown", "guide onmouseover=alert(1)//")]
    [InlineData("user", "guide onmouseover=alert(1)//")]
    [InlineData("root", "guide")]
    [InlineData("dropdown", "guide")]
    [InlineData("user", "guide")]
    public async Task Should_Render_Element_Id_As_One_Encoded_Attribute(string kind, string elementId)
    {
        var response = await Client.GetAsync("/test-menu/" + kind + "?elementId=" + Uri.EscapeDataString(elementId));
        var html = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK, html);
        var document = await new HtmlParser().ParseDocumentAsync(html);
        document.QuerySelectorAll("[onmouseover]").ShouldBeEmpty();
        document.GetElementById(elementId).ShouldNotBeNull();
        document.QuerySelector("a[href='/guide']").TextContent.Trim().ShouldBe("Guide");
    }
}
