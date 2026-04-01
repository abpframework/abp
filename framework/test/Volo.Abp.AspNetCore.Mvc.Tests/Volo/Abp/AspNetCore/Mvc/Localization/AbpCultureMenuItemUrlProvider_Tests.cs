using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Shouldly;
using MsOptions = Microsoft.Extensions.Options.Options;
using Volo.Abp.Localization;
using Volo.Abp.UI.Navigation;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Localization;

public class AbpCultureMenuItemUrlProvider_Tests
{
    [Fact]
    public async Task Should_Not_Modify_Urls_When_RouteBasedCulture_Is_Disabled()
    {
        var provider = CreateProvider(useRouteBasedCulture: false, cultureName: "zh-Hans");
        var menu = CreateMenuWithItems("/home", "/about");

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        menu.Items[0].Url.ShouldBe("/home");
        menu.Items[1].Url.ShouldBe("/about");
    }

    [Fact]
    public async Task Should_Prepend_Culture_Prefix_When_Route_Has_Culture()
    {
        var provider = CreateProvider(useRouteBasedCulture: true, cultureName: "zh-Hans");
        var menu = CreateMenuWithItems("/home", "/about");

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        menu.Items[0].Url.ShouldBe("/zh-Hans/home");
        menu.Items[1].Url.ShouldBe("/zh-Hans/about");
    }

    [Fact]
    public async Task Should_Not_Add_Prefix_For_Mvc_Request_Without_Culture()
    {
        // MVC request to /about (no culture, no HasRouteCulture cookie).
        var httpContext = new DefaultHttpContext();
        httpContext.Request.RouteValues["controller"] = "Home";
        httpContext.Request.RouteValues["action"] = "About";
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
        var localizationOptions = MsOptions.Create(
            new AbpRequestLocalizationOptions { UseRouteBasedCulture = true });
        var abpLocOptions = new AbpLocalizationOptions();
        abpLocOptions.Languages.Add(new LanguageInfo("en"));
        abpLocOptions.Languages.Add(new LanguageInfo("zh-Hans"));
        var provider = new AbpCultureMenuItemUrlProvider(
            httpContextAccessor, localizationOptions, MsOptions.Create(abpLocOptions), new MenuItemCulturePrefixHelper());

        var menu = CreateMenuWithItems("/home", "/about");

        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("zh-Hans");
            await provider.HandleAsync(new MenuItemUrlProviderContext(menu));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }

        menu.Items[0].Url.ShouldBe("/home");
        menu.Items[1].Url.ShouldBe("/about");
    }

    [Fact]
    public async Task Should_Fallback_To_CurrentCulture_In_Blazor_Circuit()
    {
        // Blazor Server interactive circuit: HttpContext exists (SignalR) but has
        // no route culture. Cookie was set during SSR indicating route culture was used.
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Cookie"] = $"{AbpRequestCultureCookieHelper.HasRouteCultureCookieName}=1";
        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
        var localizationOptions = MsOptions.Create(
            new AbpRequestLocalizationOptions { UseRouteBasedCulture = true });
        var abpLocOptions = new AbpLocalizationOptions();
        abpLocOptions.Languages.Add(new LanguageInfo("en"));
        abpLocOptions.Languages.Add(new LanguageInfo("zh-Hans"));
        var provider = new AbpCultureMenuItemUrlProvider(
            httpContextAccessor, localizationOptions, MsOptions.Create(abpLocOptions), new MenuItemCulturePrefixHelper());

        var menu = CreateMenuWithItems("/home", "/about");

        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("zh-Hans");
            await provider.HandleAsync(new MenuItemUrlProviderContext(menu));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }

        menu.Items[0].Url.ShouldBe("/zh-Hans/home");
        menu.Items[1].Url.ShouldBe("/zh-Hans/about");
    }

    [Fact]
    public async Task Should_Use_CurrentCulture_Fallback_When_No_HttpContext()
    {
        // Simulates Blazor interactive circuit: no HttpContext, but CurrentCulture is set.
        // CurrentCulture (not CurrentUICulture) is used because {culture} route segments
        // represent the culture, not the UI culture.
        var provider = CreateProviderWithoutHttpContext(
            useRouteBasedCulture: true,
            knownLanguages: new[] { "en", "zh-Hans", "tr" });

        var menu = CreateMenuWithItems("/home", "/about");

        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("zh-Hans");
            await provider.HandleAsync(new MenuItemUrlProviderContext(menu));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }

        menu.Items[0].Url.ShouldBe("/zh-Hans/home");
        menu.Items[1].Url.ShouldBe("/zh-Hans/about");
    }

    [Fact]
    public async Task Should_Not_Modify_Urls_When_No_HttpContext_And_Unknown_Culture()
    {
        // Blazor interactive circuit with a culture that is not in the known languages list
        var provider = CreateProviderWithoutHttpContext(
            useRouteBasedCulture: true,
            knownLanguages: new[] { "en", "tr" });

        var menu = CreateMenuWithItems("/home", "/about");

        var previousCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("fr");
            await provider.HandleAsync(new MenuItemUrlProviderContext(menu));
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }

        menu.Items[0].Url.ShouldBe("/home");
        menu.Items[1].Url.ShouldBe("/about");
    }

    [Fact]
    public async Task Should_Prepend_Prefix_Recursively_For_Nested_Items()
    {
        var provider = CreateProvider(useRouteBasedCulture: true, cultureName: "tr");

        var menu = new ApplicationMenu("TestMenu");
        var parent = new ApplicationMenuItem("Parent", "Parent", url: "/parent");
        var child = new ApplicationMenuItem("Child", "Child", url: "/child");
        var grandChild = new ApplicationMenuItem("GrandChild", "GrandChild", url: "/grandchild");
        child.AddItem(grandChild);
        parent.AddItem(child);
        menu.AddItem(parent);

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        parent.Url.ShouldBe("/tr/parent");
        child.Url.ShouldBe("/tr/child");
        grandChild.Url.ShouldBe("/tr/grandchild");
    }

    [Fact]
    public async Task Should_Handle_Tilde_Slash_Urls()
    {
        // ~/identity/users is the pattern used by ABP module menu contributors (e.g. Identity)
        var provider = CreateProvider(useRouteBasedCulture: true, cultureName: "zh-Hans");

        var menu = new ApplicationMenu("TestMenu");
        menu.AddItem(new ApplicationMenuItem("Users", "Users", url: "~/identity/users"));
        menu.AddItem(new ApplicationMenuItem("Roles", "Roles", url: "~/identity/roles"));

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        // ~/identity/users → ~/zh-Hans/identity/users
        // Blazor theme strips "~/" via TrimStart('/', '~') → "zh-Hans/identity/users"
        // With <base href="/"> resolves to /zh-Hans/identity/users
        menu.Items[0].Url.ShouldBe("~/zh-Hans/identity/users");
        menu.Items[1].Url.ShouldBe("~/zh-Hans/identity/roles");
    }

    [Fact]
    public async Task Should_Not_Modify_External_Urls()
    {
        var provider = CreateProvider(useRouteBasedCulture: true, cultureName: "zh-Hans");

        var menu = new ApplicationMenu("TestMenu");
        menu.AddItem(new ApplicationMenuItem("External", "External", url: "https://example.com/page"));
        menu.AddItem(new ApplicationMenuItem("Relative", "Relative", url: "page"));
        menu.AddItem(new ApplicationMenuItem("Local", "Local", url: "/local"));

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        // External and relative URLs should not be modified
        menu.Items[0].Url.ShouldBe("https://example.com/page");
        menu.Items[1].Url.ShouldBe("page");
        // Local URL should be prefixed
        menu.Items[2].Url.ShouldBe("/zh-Hans/local");
    }

    [Fact]
    public async Task Should_Not_Throw_When_Url_Is_Null()
    {
        var provider = CreateProvider(useRouteBasedCulture: true, cultureName: "tr");

        var menu = new ApplicationMenu("TestMenu");
        menu.AddItem(new ApplicationMenuItem("NoUrl", "No URL", url: null));
        menu.AddItem(new ApplicationMenuItem("WithUrl", "With URL", url: "/page"));

        await provider.HandleAsync(new MenuItemUrlProviderContext(menu));

        // Null URL should remain null
        menu.Items[0].Url.ShouldBeNull();
        // Normal URL should be prefixed
        menu.Items[1].Url.ShouldBe("/tr/page");
    }

    private static AbpCultureMenuItemUrlProvider CreateProvider(
        bool useRouteBasedCulture,
        string? cultureName)
    {
        var httpContext = new DefaultHttpContext();
        if (cultureName != null)
        {
            httpContext.Request.RouteValues["culture"] = cultureName;
        }

        var httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };
        var localizationOptions = MsOptions.Create(
            new AbpRequestLocalizationOptions { UseRouteBasedCulture = useRouteBasedCulture });
        var abpLocalizationOptions = MsOptions.Create(new AbpLocalizationOptions());

        return new AbpCultureMenuItemUrlProvider(
            httpContextAccessor, localizationOptions, abpLocalizationOptions, new MenuItemCulturePrefixHelper());
    }

    private static AbpCultureMenuItemUrlProvider CreateProviderWithoutHttpContext(
        bool useRouteBasedCulture,
        string[] knownLanguages)
    {
        var httpContextAccessor = new HttpContextAccessor { HttpContext = null };
        var localizationOptions = MsOptions.Create(
            new AbpRequestLocalizationOptions { UseRouteBasedCulture = useRouteBasedCulture });
        var abpLocOptions = new AbpLocalizationOptions();
        foreach (var lang in knownLanguages)
        {
            abpLocOptions.Languages.Add(new LanguageInfo(lang));
        }

        return new AbpCultureMenuItemUrlProvider(
            httpContextAccessor, localizationOptions, MsOptions.Create(abpLocOptions), new MenuItemCulturePrefixHelper());
    }

    private static ApplicationMenu CreateMenuWithItems(params string[] urls)
    {
        var menu = new ApplicationMenu("TestMenu");
        for (var i = 0; i < urls.Length; i++)
        {
            menu.AddItem(new ApplicationMenuItem($"Item{i}", $"Item {i}", url: urls[i]));
        }
        return menu;
    }

}
