using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class LocalizationOptionsCompatibility_Tests : AspNetCoreMvcTestBase
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddControllers().ConfigureApplicationPartManager(manager =>
            manager.FeatureProviders.Add(new OriginalLocalizationControllerFeatureProvider()));
        services.PostConfigure<AbpLocalizationOptions>(options => options.Languages.Clear());
        services.Configure<AbpRequestLocalizationOptions>(options => options.RequestLocalizationOptionConfigurators.Add((_, localization) =>
        {
            localization.SupportedCultures = new[] { new CultureInfo("en-GB"), new CultureInfo("tr") };
            localization.SupportedUICultures = new[] { new CultureInfo("en"), new CultureInfo("zh-Hant") };
            localization.DefaultRequestCulture = new RequestCulture("en-GB", "en");
            localization.ApplyCurrentCultureToResponseHeaders = true;
            localization.RequestCultureProviders.Insert(0, new TestMappingRequestCultureProvider());
            return Task.CompletedTask;
        }));
    }

    [Fact]
    public async Task Empty_Language_List_Should_Use_The_Configured_Request_Localization_Options()
    {
        var result = await GetResponseAsObjectAsync<ApplicationLocalizationDto>(
            "/api/abp/application-localization?cultureName=EN-GB");
        result.CurrentCulture.Name.ShouldBe("en-GB");
        using var response = await Client.GetAsync("/Abp/Languages/Switch?culture=EN-GB&uiCulture=ZH-HANT");
        response.StatusCode.ShouldBe(HttpStatusCode.Found);
        response.Headers.GetValues("Set-Cookie").ShouldContain(value =>
            Uri.UnescapeDataString(value).Contains("c=en-GB|uic=zh-Hant"));
    }

    [Fact]
    public async Task Formatting_And_UI_Cultures_Should_Remain_Independent_With_Parent_Fallback()
    {
        using var response = await Client.GetAsync("/api/LocalizationTestController?culture=tr-TR&ui-culture=zh-TW");
        (await response.Content.ReadAsStringAsync()).ShouldBe("tr:zh-Hant");
        response.Content.Headers.ContentLanguage.ShouldContain("zh-Hant");
    }

    [Fact]
    public async Task Parent_Fallback_Can_Be_Disabled()
    {
        var options = await ServiceProvider.GetRequiredService<IAbpRequestLocalizationOptionsProvider>().GetLocalizationOptionsAsync();
        options.FallBackToParentCultures = false;
        options.FallBackToParentUICultures = false;

        (await GetResponseAsStringAsync("/api/LocalizationTestController?culture=tr-TR&ui-culture=zh-TW"))
            .ShouldBe("en-GB:en");
    }

    [Fact]
    public async Task Mapping_Provider_Should_Keep_The_Original_Provider_List_And_Identity()
    {
        using var response = await Client.GetAsync("/api/LocalizationTestController?culture=tr&ui-culture=zh-TW&map=true");
        (await response.Content.ReadAsStringAsync()).ShouldBe("tr:zh-Hant");
        response.Headers.Contains("Set-Cookie").ShouldBeFalse();
        var options = await ServiceProvider.GetRequiredService<IAbpRequestLocalizationOptionsProvider>().GetLocalizationOptionsAsync();
        options.RequestCultureProviders[0].ShouldBeOfType<TestMappingRequestCultureProvider>();
        options.RequestCultureProviders.ShouldContain(provider => provider is QueryStringRequestCultureProvider);
    }

    [Fact]
    public async Task A_Null_Provider_List_Should_Still_Use_The_Default_Culture()
    {
        var options = await ServiceProvider.GetRequiredService<IAbpRequestLocalizationOptionsProvider>().GetLocalizationOptionsAsync();
        options.RequestCultureProviders = null;
        (await GetResponseAsStringAsync("/api/LocalizationTestController?culture=tr")).ShouldBe("en-GB:en");
    }

    [Fact]
    public async Task Language_Changes_Should_Refresh_The_Accepted_Cultures()
    {
        using (var response = await Client.GetAsync("/api/abp/application-localization?cultureName=fr"))
        {
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        var languageOptions = ServiceProvider.GetRequiredService<IOptions<AbpLocalizationOptions>>().Value;
        languageOptions.Languages.Add(new LanguageInfo("fr", "fr"));
        var requestOptions = ServiceProvider.GetRequiredService<IOptions<AbpRequestLocalizationOptions>>().Value;
        requestOptions.RequestLocalizationOptionConfigurators.Clear();
        await ServiceProvider.GetRequiredService<DefaultAbpRequestLocalizationOptionsProvider>().HandleEventAsync(new LanguageChangedEto());

        var result = await GetResponseAsObjectAsync<ApplicationLocalizationDto>("/api/abp/application-localization?cultureName=FR");
        result.CurrentCulture.Name.ShouldBe("fr");
    }
}

public class TestMappingRequestCultureProvider : RequestCultureProvider
{
    public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext context)
    {
        if (context.Request.Query["map"] != "true")
        {
            return null;
        }

        var options = await context.RequestServices.GetRequiredService<IAbpRequestLocalizationOptionsProvider>().GetLocalizationOptionsAsync();
        foreach (var provider in options.RequestCultureProviders)
        {
            if (ReferenceEquals(provider, this))
            {
                continue;
            }

            var result = await provider.DetermineProviderCultureResult(context);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
