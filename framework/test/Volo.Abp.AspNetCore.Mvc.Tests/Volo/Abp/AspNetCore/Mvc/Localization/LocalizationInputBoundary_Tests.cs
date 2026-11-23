using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Controllers;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class LocalizationInputBoundary_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddControllers().ConfigureApplicationPartManager(manager =>
            manager.FeatureProviders.Add(new OriginalLocalizationControllerFeatureProvider()));
        services.PostConfigure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Clear();
            options.Languages.Add(new LanguageInfo("en", "en"));
            options.Languages.Add(new LanguageInfo("tr", "tr"));
            options.Languages.Add(new LanguageInfo("qaa-Latn-QQ", "qaa-Latn-QQ"));
        });
    }

    [Theory]
    [InlineData("api")]
    [InlineData("script")]
    [InlineData("switch")]
    public async Task Explicit_Culture_Inputs_Should_Reject_Unconfigured_Names_Without_Caching(string entry)
    {
        var name = "en-x-" + Guid.NewGuid().ToString("N")[..8];
        var url = entry switch
        {
            "api" => "/api/abp/application-localization?cultureName=" + name,
            "script" => "/Abp/ApplicationLocalizationScript?cultureName=" + name,
            _ => "/Abp/Languages/Switch?culture=" + name + "&uiCulture=en"
        };
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Requested-With", "XMLHttpRequest");
        using var response = await Client.SendAsync(request);

        AssertCultureWasNotCached(name);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("EN", "en")]
    [InlineData("TR", "tr")]
    [InlineData("QAA-LATN-QQ", "qaa-Latn-QQ")]
    public async Task Configured_Names_Should_Be_Resolved_To_Their_Canonical_Name(string input, string expected)
    {
        var result = await GetResponseAsObjectAsync<ApplicationLocalizationDto>(
            "/api/abp/application-localization?cultureName=" + input);
        result.CurrentCulture.Name.ShouldBe(expected);
        using var response = await Client.GetAsync("/Abp/Languages/Switch?culture=" + input + "&uiCulture=" + input);
        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.GetValues("Set-Cookie").ShouldContain(value => Uri.UnescapeDataString(value).Contains("c=" + expected));
    }

    [Theory]
    [InlineData("query")]
    [InlineData("cookie")]
    [InlineData("header")]
    public async Task Ordinary_Requests_Should_Not_Cache_Unknown_Culture_Names(string source)
    {
        for (var i = 0; i < 4; i++)
        {
            var name = "en-x-" + Guid.NewGuid().ToString("N")[..8];
            var url = "/api/LocalizationTestController";
            if (source == "query")
            {
                url += "?culture=" + name;
            }
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (source == "cookie")
            {
                request.Headers.Add("Cookie", ".AspNetCore.Culture=c=" + name + "|uic=" + name);
            }
            if (source == "header")
            {
                request.Headers.Add("Accept-Language", name + ",tr;q=0.8");
            }
            using var response = await Client.SendAsync(request);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            AssertCultureWasNotCached(name);
            if (source == "header")
            {
                (await response.Content.ReadAsStringAsync()).ShouldBe("tr:tr");
            }
        }
    }

    [Theory]
    [InlineData("en-US", "en:en")]
    [InlineData("tr-TR", "tr:tr")]
    [InlineData("QAA-LATN-QQ", "qaa-Latn-QQ:qaa-Latn-QQ")]
    public async Task Configured_Cultures_And_Standard_Parent_Fallback_Should_Work(string input, string expected)
    {
        (await GetResponseAsStringAsync("/api/LocalizationTestController?culture=" + input)).ShouldBe(expected);
    }

    [Fact]
    public async Task Query_Culture_Should_Keep_Its_Priority_And_Set_The_Culture_Cookie()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/LocalizationTestController?culture=TR");
        request.Headers.Add("Accept-Language", "en");
        using var response = await Client.SendAsync(request);
        (await response.Content.ReadAsStringAsync()).ShouldBe("tr:tr");
        response.Headers.GetValues("Set-Cookie").ShouldContain(value => Uri.UnescapeDataString(value).Contains("c=tr"));
    }

    private static void AssertCultureWasNotCached(string name)
    {
        foreach (var type in new[] { typeof(CultureInfo), typeof(CultureInfo).Assembly.GetType("System.Globalization.CultureData")! })
        {
            var field = type.GetField(type == typeof(CultureInfo) ? "s_cachedCulturesByName" : "s_cachedCultures",
                BindingFlags.NonPublic | BindingFlags.Static);
            field.ShouldNotBeNull();
            if (field.GetValue(null) is IDictionary cache)
            {
                lock (cache)
                {
                    cache.Keys.Cast<string>().ShouldNotContain(key => key.StartsWith(name, StringComparison.OrdinalIgnoreCase));
                }
            }
        }
    }
}

public class OriginalLocalizationControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(System.Collections.Generic.IEnumerable<Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPart> parts, ControllerFeature feature)
    {
        feature.Controllers.Remove(typeof(ReplaceBuiltInController).GetTypeInfo());
    }
}
