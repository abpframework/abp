using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpLanguagesController_Tests : AspNetCoreMvcTestBase
{
    private const string SwitchUrl = "/Abp/Languages/Switch";

    [Fact]
    public async Task Should_Replace_Route_Culture_In_ReturnUrl_When_Cookie_Is_Set()
    {
        var response = await SendSwitchRequestAsync(
            targetCulture: "zh-Hans",
            returnUrl: "/en/Home/About",
            currentCultureCookie: "en");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/zh-Hans/Home/About");
    }

    [Fact]
    public async Task Should_Replace_Route_Culture_When_Switching_Back()
    {
        var response = await SendSwitchRequestAsync(
            targetCulture: "en",
            returnUrl: "/zh-Hans/About",
            currentCultureCookie: "zh-Hans");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/en/About");
    }

    [Fact]
    public async Task Should_Replace_Region_Culture_In_ReturnUrl()
    {
        var response = await SendSwitchRequestAsync(
            targetCulture: "zh-Hans",
            returnUrl: "/en-US/products",
            currentCultureCookie: "en-US");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/zh-Hans/products");
    }

    [Fact]
    public async Task Should_Not_Replace_When_No_Cookie()
    {
        // No cookie — GetCurrentCultureFromRequestCookie returns null, no route replacement
        var response = await SendSwitchRequestAsync(
            targetCulture: "zh-Hans",
            returnUrl: "/en/Home/About",
            currentCultureCookie: null);

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/en/Home/About");
    }

    [Fact]
    public async Task Should_Redirect_To_Root_When_ReturnUrl_Is_Empty()
    {
        var response = await SendSwitchRequestAsync(
            targetCulture: "zh-Hans",
            returnUrl: "",
            currentCultureCookie: "en");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldStartWith("/");
    }

    [Fact]
    public async Task Should_Not_Replace_Culture_Inside_Longer_Segment_Via_Http()
    {
        // "en" must not corrupt "/enterprise/products"
        var response = await SendSwitchRequestAsync(
            targetCulture: "zh-Hans",
            returnUrl: "/enterprise/products",
            currentCultureCookie: "en");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/enterprise/products");
    }

    [Fact]
    public async Task Should_Replace_Culture_After_Tenant_Segment()
    {
        // Multi-tenant URL: /tenant-a/zh-Hans/About → /tenant-a/en/About
        var response = await SendSwitchRequestAsync(
            targetCulture: "en",
            returnUrl: "/tenant-a/zh-Hans/About",
            currentCultureCookie: "zh-Hans");

        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.Location?.ToString().ShouldBe("/tenant-a/en/About");
    }

    private async Task<HttpResponseMessage> SendSwitchRequestAsync(
        string targetCulture,
        string returnUrl,
        string? currentCultureCookie)
    {
        var url = $"{SwitchUrl}?culture={Uri.EscapeDataString(targetCulture)}" +
                  $"&uiCulture={Uri.EscapeDataString(targetCulture)}" +
                  $"&returnUrl={Uri.EscapeDataString(returnUrl)}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (currentCultureCookie != null)
        {
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(currentCultureCookie, currentCultureCookie));
            request.Headers.Add("Cookie",
                $"{CookieRequestCultureProvider.DefaultCookieName}={Uri.EscapeDataString(cookieValue)}");
        }

        return await Client.SendAsync(request);
    }
}
