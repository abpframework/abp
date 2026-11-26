using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;
using Xunit;

namespace Volo.CmsKit.Web.Contents;

public class ContentPreview_Http_Tests : AbpWebApplicationFactoryIntegratedTest<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseContentRoot(AppContext.BaseDirectory);
        builder.ConfigureServices(services => services.AddControllers()
            .AddApplicationPart(typeof(LegacyContentPreviewTestController).Assembly));
    }

    [Fact]
    public async Task Should_Render_The_Legacy_Single_Argument_Invocation()
    {
        using var response = await Client.GetAsync("/tests/legacy-content-preview");
        var html = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK, html);
        html.ShouldContain("<strong>content</strong>");
    }

    [Fact]
    public async Task Should_Require_Authentication_For_Preview()
    {
        var response = await Client.PostAsync("/CmsKitCommonWidgets/ContentPreview", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Content"] = "<script>alert(document.domain)</script>"
        }));

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Should_Apply_Preview_Sanitization_Through_The_Controller(bool? preventXSS)
    {
        Client.DefaultRequestHeaders.Add("X-Test-User", "author");
        var values = new Dictionary<string, string>
        {
            ["Content"] = "Hello **team**\n\n<script>alert(document.domain)</script>"
        };
        if (preventXSS.HasValue)
        {
            values["PreventXSS"] = preventXSS.Value.ToString();
        }

        var response = await Client.PostAsync("/CmsKitCommonWidgets/ContentPreview", new FormUrlEncodedContent(values));
        var html = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK, html);
        html.ShouldContain("<strong>team</strong>");
        if (preventXSS ?? true)
        {
            html.ShouldNotContain("<script>alert(document.domain)</script>");
        }
        else
        {
            html.ShouldContain("<script>alert(document.domain)</script>");
        }
    }

    [Theory]
    [InlineData("1ef78ddd-58be-4dd0-b919-10053abeaf89", false, true)]
    [InlineData("bda7328d-c395-4b40-ab50-5f6199ac034d", true, false)]
    public async Task Should_Use_The_Blogs_Sanitization_Setting(string blogId, bool requestedPreventXSS, bool expectedPreventXSS)
    {
        Client.DefaultRequestHeaders.Add("X-Test-User", "author");
        var response = await Client.PostAsync("/CmsKitCommonWidgets/ContentPreview", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["Content"] = "Hello **team**\n\n<script>alert(document.domain)</script>",
            ["BlogId"] = blogId,
            ["PreventXSS"] = requestedPreventXSS.ToString()
        }));
        var html = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK, html);
        html.ShouldContain("<strong>team</strong>");
        html.Contains("<script>alert(document.domain)</script>").ShouldBe(!expectedPreventXSS);
    }
}

public class LegacyContentPreviewTestController : AbpController
{
    [HttpGet("/tests/legacy-content-preview")]
    public IActionResult Get()
    {
        return ViewComponent(typeof(ContentPreviewViewComponent), new { content = "Legacy **content**" });
    }
}
