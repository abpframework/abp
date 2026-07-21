using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class RouteBasedCultureNavigationHelper_Tests
{
    private static readonly IEnumerable<LanguageInfo> AllLanguages = new[]
    {
        new LanguageInfo("en"),
        new LanguageInfo("tr"),
        new LanguageInfo("zh-Hans"),
    };

    private readonly RouteBasedCultureNavigationHelper _helper = new();

    [Fact]
    public async Task Should_Replace_Culture_In_Simple_Path()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr/home");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("en"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/en/home");
    }

    [Fact]
    public async Task Should_Replace_Culture_When_No_Path_After_Culture()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("en"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/en");
    }

    [Fact]
    public async Task Should_Replace_Culture_When_Query_String_Follows_Culture_Directly()
    {
        // Regression: "tr?x=1" was being treated as a single segment "tr?x=1"
        // instead of culture="tr" + suffix="?x=1".
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr?x=1");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("en"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/en?x=1");
    }

    [Fact]
    public async Task Should_Replace_Culture_When_Fragment_Follows_Culture_Directly()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr#section");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("en"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/en#section");
    }

    [Fact]
    public async Task Should_Replace_Culture_Preserving_Path_Query_And_Fragment()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr/about?ref=main#top");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("zh-Hans"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/zh-Hans/about?ref=main#top");
    }

    [Fact]
    public async Task Should_Prepend_Culture_When_No_Existing_Culture_Prefix()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/identity/users");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("zh-Hans"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/zh-Hans/identity/users");
    }

    [Fact]
    public async Task Should_Prepend_Culture_When_At_Root()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("tr"), AllLanguages);
        nav.LastNavigatedUri.ShouldBe("https://example.com/tr/");
    }

    [Fact]
    public async Task Should_Not_Navigate_When_Target_Culture_Matches_Current()
    {
        var nav = new TestNavigationManager("https://example.com/", "https://example.com/tr/home");
        await _helper.NavigateToNewCultureAsync(nav, new LanguageInfo("tr"), AllLanguages);
        // Already on /tr/home — no navigation should occur
        nav.LastNavigatedUri.ShouldBeNull();
    }

    private sealed class TestNavigationManager : NavigationManager
    {
        public string? LastNavigatedUri { get; private set; }

        public TestNavigationManager(string baseUri, string uri)
        {
            Initialize(baseUri, uri);
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            LastNavigatedUri = uri;
        }
    }
}
