using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class RouteBasedCultureUrlHelper_Tests
{
    private readonly ICachedApplicationConfigurationClient _configClient;
    private readonly RouteBasedCultureUrlHelper _helper;
    private readonly ApplicationConfigurationDto _config;

    public RouteBasedCultureUrlHelper_Tests()
    {
        _config = new ApplicationConfigurationDto
        {
            Localization = new ApplicationLocalizationConfigurationDto
            {
                UseRouteBasedCulture = true,
                Languages = new List<LanguageInfo>
                {
                    new LanguageInfo("en"),
                    new LanguageInfo("zh-Hans"),
                    new LanguageInfo("tr"),
                    new LanguageInfo("es-419"),
                }
            }
        };

        _configClient = Substitute.For<ICachedApplicationConfigurationClient>();
        _configClient.GetAsync().Returns(_config);

        _helper = new RouteBasedCultureUrlHelper(_configClient);
    }

    [Theory]
    [InlineData("https://auth-server.example.com/connect/authorize")]
    [InlineData("http://example.com/login")]
    public async Task Should_Not_Modify_Absolute_Urls(string url)
    {
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync(url);
        result.ShouldBe(url);
    }

    [Fact]
    public async Task Should_Not_Modify_Protocol_Relative_Url()
    {
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("//cdn.example.com/asset.js");
        result.ShouldBe("//cdn.example.com/asset.js");
    }

    [Fact]
    public async Task Should_Prepend_Culture_To_Root_Relative_Url()
    {
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("/account/manage-profile");
        result.ShouldBe("/zh-Hans/account/manage-profile");
    }

    [Fact]
    public async Task Should_Prepend_Culture_To_Tilde_Slash_Url()
    {
        using var _ = CultureScope("tr");
        var result = await _helper.PrependCulturePrefixAsync("~/account/manage-profile");
        result.ShouldBe("~/tr/account/manage-profile");
    }

    [Fact]
    public async Task Should_Prepend_Culture_To_Bare_Relative_Url()
    {
        // Default auth URLs like "authentication/login" have no leading slash.
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("authentication/login");
        result.ShouldBe("zh-Hans/authentication/login");
    }

    [Fact]
    public async Task Should_Not_Modify_Url_When_Feature_Disabled()
    {
        _config.Localization.UseRouteBasedCulture = false;
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("/home");
        result.ShouldBe("/home");
    }

    [Fact]
    public async Task Should_Not_Modify_Url_When_Culture_Not_In_Language_List()
    {
        using var _ = CultureScope("fr");
        var result = await _helper.PrependCulturePrefixAsync("/home");
        result.ShouldBe("/home");
    }

    [Fact]
    public async Task Should_Return_Empty_String_Unchanged()
    {
        var result = await _helper.PrependCulturePrefixAsync(string.Empty);
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public async Task Should_Support_Numeric_Region_Culture_Tag()
    {
        using var _ = CultureScope("es-419");
        var result = await _helper.PrependCulturePrefixAsync("/home");
        result.ShouldBe("/es-419/home");
    }

    [Fact]
    public async Task Should_Be_Idempotent_On_Root_Relative_Url()
    {
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("/zh-Hans/account/manage-profile");
        result.ShouldBe("/zh-Hans/account/manage-profile");
    }

    [Fact]
    public async Task Should_Be_Idempotent_On_Tilde_Slash_Url()
    {
        using var _ = CultureScope("tr");
        var result = await _helper.PrependCulturePrefixAsync("~/tr/account/manage-profile");
        result.ShouldBe("~/tr/account/manage-profile");
    }

    [Fact]
    public async Task Should_Be_Idempotent_On_Bare_Relative_Url()
    {
        using var _ = CultureScope("zh-Hans");
        var result = await _helper.PrependCulturePrefixAsync("zh-Hans/authentication/login");
        result.ShouldBe("zh-Hans/authentication/login");
    }

    private static IDisposable CultureScope(string cultureName)
    {
        var previous = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = new CultureInfo(cultureName);
        return new DelegateDisposable(() => CultureInfo.CurrentCulture = previous);
    }

    private sealed class DelegateDisposable : IDisposable
    {
        private readonly System.Action _onDispose;
        public DelegateDisposable(System.Action onDispose) => _onDispose = onDispose;
        public void Dispose() => _onDispose();
    }
}
