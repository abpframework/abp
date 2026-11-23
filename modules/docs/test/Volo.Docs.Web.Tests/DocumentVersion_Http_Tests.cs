using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;
using Xunit;

namespace Volo.Docs;

public class DocumentVersion_Http_Tests : AbpWebApplicationFactoryIntegratedTest<DocsWebTestProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
    }

    [Theory]
    [InlineData("1.0.0")]
    [InlineData("1.0.0-rc.1")]
    [InlineData("1.0.0+build.5")]
    public async Task Should_Render_Normal_Unlisted_Versions(string version)
    {
        var response = await Client.GetAsync("/documents/en/product/" + Uri.EscapeDataString(version) + "/Installation");
        var html = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK, html);
        html.ShouldContain("Run the installer for version " + version);
        html.ShouldContain("https://downloads.example.com/product/" + version + "/");
    }

    [Theory]
    [InlineData("1.0.0<img src=x onerror=alert(document.domain)>")]
    [InlineData("[x](javascript:alert(document.domain))")]
    [InlineData("1.0.0\" onmouseover=alert(1) x=\"")]
    [InlineData("1.0.0\n")]
    public async Task Should_Redirect_Unsafe_Unlisted_Versions(string version)
    {
        var response = await Client.GetAsync("/documents/en/product/" + Uri.EscapeDataString(version) + "/Installation");

        response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        response.Headers.Location!.OriginalString.ShouldContain("/latest/");
    }
}
