# SEO-Friendly Localized URLs in ABP with a Single Line of Configuration

ABP has always supported language switching via the `?culture=en` query string and the culture cookie. That works fine for most applications — but it has a limitation that shows up quickly once SEO or link-sharing matters.

Consider a book-store app where users browse in their language:

- A Spanish user shares a product link. The recipient opens it in English because the cookie on *their* machine says `en`.
- Search engines crawl the same URL in every language, making it impossible to create separate sitemaps per locale.
- A user bookmarks `/Books/Detail?id=42&culture=es`. After a cookie reset, the `?culture=` parameter is missing from the bookmark — the page loads in the wrong language.

Embedding the culture in the URL path — `/es/books`, `/zh-Hans/about` — solves all three. Each language has its own stable URL, readable by humans and index-friendly for search engines.

ABP supports this out of the box. You opt in with a single configuration property, and the framework takes care of routing, URL generation, menu links, and language switching automatically.

## Enabling URL-Based Localization

In your ABP module class, add:

```csharp
Configure<AbpRequestLocalizationOptions>(options =>
{
    options.UseRouteBasedCulture = true;
});
```

That is the only change you need to make. The ABP application template already uses the correct middleware order — `UseAbpRequestLocalization()` comes after `UseRouting()` — so the default pipeline works as-is:

```csharp
app.UseRouting();
app.UseAbpRequestLocalization(); // already in this position by default
app.UseAuthorization();
app.UseConfiguredEndpoints();
```

> This order matters because culture detection from route data is only possible after routing has assigned route values. If you have manually rearranged your middleware pipeline, make sure this order is preserved.

## What ABP Registers Automatically

Setting `UseRouteBasedCulture = true` triggers a cascade of automatic registrations. It is worth knowing what they are, because understanding them helps when you need to troubleshoot or extend the feature.

**Route registration.** ABP inserts a `{culture}/{controller}/{action}/{id?}` conventional route *before* the default route. A matching route constraint (`^[a-zA-Z]{2,8}(-[a-zA-Z0-9]{1,8})*$`) ensures only valid IETF BCP 47 language tags are accepted, so a URL like `/enterprise/products` is not mistaken for a culture-prefixed route.

**Razor Pages convention.** `AbpCultureRoutePagesConvention` adds a `{culture}/...` selector to every Razor Page route model at startup. This is what makes `/zh-Hans/Books` match the `Books/Index.cshtml` page.

**URL helper factory.** ABP replaces `IUrlHelperFactory` with `AbpCultureRouteUrlHelperFactory`. When the current request has a `{culture}` route value, every call to `Url.Page()` or `Url.Action()` automatically receives the culture as an explicit route value — no code changes needed in your views or pages.

**Menu URL provider.** `AbpCultureMenuItemUrlProvider` implements the new `IMenuItemUrlProvider` extension point. When the menu is built for a request, all local menu item URLs get the culture prefix prepended automatically. Themes and menu contributors remain completely untouched.

## URL Generation Just Works

In a Razor Page or view running under a culture-prefixed URL (say, `/zh-Hans/Books`), you do not need to pass a `culture` parameter anywhere:

```cshtml
<!-- In /zh-Hans/Books/Index.cshtml -->
@Url.Page("/Books/Detail", new { id = book.Id })
@* Generates: /zh-Hans/Books/Detail?id=42 *@

@Url.Action("About", "Home")
@* Generates: /zh-Hans/Home/About *@
```

`AbpCultureRouteUrlHelperFactory` injects the ambient culture value into every URL generation call. If you explicitly pass a different `culture` value, that takes precedence — so cross-language links are also straightforward:

```cshtml
@Url.Page("/Books/Index", new { culture = "tr" })
@* Generates: /tr/Books *@
```

## Language Switching

The built-in ABP language switcher (`/Abp/Languages/Switch`) already works with route-based culture. When a user switches language, the controller reads the current culture from the **request cookie** and rewrites the `{culture}` segment in the `returnUrl`.

| Current URL | Switch to | Redirect to |
|---|---|---|
| `/tr/books` | `en` | `/en/books` |
| `/zh-Hans/about` | `en` | `/en/about` |
| `/tenant-a/zh-Hans/about` | `en` | `/tenant-a/en/about` |
| `/books?culture=tr&ui-culture=tr` | `en` | `/books?culture=en&ui-culture=en` |

Reading from the request cookie (rather than `CultureInfo.CurrentCulture`) is important: the switch URL itself carries `?culture=zh-Hans` as a query parameter, which the ASP.NET Core `QueryStringRequestCultureProvider` would otherwise interpret first, overwriting the "current" culture before the controller runs.

No theme changes, no language switcher changes — the existing UI component just works.

## Multi-Tenancy

URL-based localization is fully compatible with ABP's multi-tenant routing. The culture route is handled as a separate routing layer from the tenant. Language switching explicitly supports tenant-prefixed URLs, so `/tenant-a/zh-Hans/About → /tenant-a/en/About` works without any additional configuration.

> For details on combining tenant routing with culture routing, see the [Multi-Tenancy](https://docs.abp.io/en/abp/latest/Multi-Tenancy) documentation.

## Summary

To add SEO-friendly localized URL paths to your ABP application:

1. Set `options.UseRouteBasedCulture = true` in your module.
2. Ensure `UseAbpRequestLocalization()` comes after `UseRouting()` in the pipeline.

ABP automatically registers the culture route, adds `{culture}/...` selectors to all Razor Pages, rewrites URLs generated by `Url.Page()` and `Url.Action()`, updates navigation menus, and handles language switching — all without any changes to themes, menu contributors, or views.

A runnable sample demonstrating this feature is available at [abp-samples/UrlBasedLocalization](https://github.com/abpframework/abp-samples/tree/master/UrlBasedLocalization). It includes English, Turkish, French, and Simplified Chinese, and is the quickest way to see the feature in action before integrating it into your own project.

## References

- [URL-Based Localization — ABP Documentation](https://docs.abp.io/en/abp/latest/URL-Based-Localization)
- [Localization — ABP Documentation](https://docs.abp.io/en/abp/latest/Localization)
- [abp-samples/UrlBasedLocalization — GitHub](https://github.com/abpframework/abp-samples/tree/master/UrlBasedLocalization)
- [Request Localization in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/select-language-culture)
- [IETF BCP 47 Language Tags](https://www.rfc-editor.org/info/bcp47)
