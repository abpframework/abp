````json
//[doc-seo]
{
    "Description": "Learn how to use ABP's URL-based localization to embed culture in the URL path, enabling SEO-friendly and shareable localized URLs."
}
````

# URL-Based Localization

ABP supports embedding the current culture directly in the URL path, for example `/tr/products` or `/en/about`. This approach is widely used by documentation sites, e-commerce platforms, and any site that needs SEO-friendly, shareable localized URLs.

By default, ABP detects language from QueryString (`?culture=tr`), Cookie, and `Accept-Language` header. URL path detection is **opt-in** and fully backward-compatible.

## Enabling URL-Based Localization

Configure the `AbpRequestLocalizationOptions` in your [module class](../architecture/modularity/basics.md):

````csharp
Configure<AbpRequestLocalizationOptions>(options =>
{
    options.UseRouteBasedCulture = true;
});
````

That's all you need. The framework automatically handles the rest.

> You also need to ensure that `UseAbpRequestLocalization()` is called **after** `UseRouting()` in your middleware pipeline. See the [Middleware Order](#middleware-order) section below.

## What Happens Automatically

When you set `UseRouteBasedCulture` to `true`, ABP automatically registers the following:

* **`RouteDataRequestCultureProvider`** — Reads `{culture}` from route data (highest priority provider).
* **`{culture}/{controller}/{action}` route** — A conventional route for MVC controllers.
* **`AbpCultureRoutePagesConvention`** — An `IPageRouteModelConvention` that adds `{culture}/...` route selectors to all Razor Pages.
* **`AbpCultureRouteUrlHelperFactory`** — Replaces the default `IUrlHelperFactory` to auto-inject culture into `Url.Page()` and `Url.Action()` calls.
* **`AbpCultureMenuItemUrlProvider`** — Prepends the culture prefix to navigation menu item URLs.

You do not need to configure these individually.

## Middleware Order

URL-based localization requires `UseAbpRequestLocalization()` to be called **after** `UseRouting()`:

````csharp
app.MapAbpStaticAssets();
app.UseRouting();
app.UseAbpRequestLocalization(); // Must be after UseRouting()
app.UseAuthorization();
app.UseConfiguredEndpoints();
````

> If you do not enable `UseRouteBasedCulture`, the middleware order does not matter and your existing application continues to work as before.

## URL Generation

When a request has a `{culture}` route value, all URL generation methods automatically include the culture prefix:

````csharp
// In a Razor Page — culture is auto-injected, no manual parameter needed
@Url.Page("/About")         // Generates: /zh-Hans/About
@Url.Action("About", "Home") // Generates: /zh-Hans/Home/About
````

This works because `AbpCultureRouteUrlHelperFactory` replaces the default `IUrlHelperFactory` and injects the current `{culture}` route value into all URL generation calls.

Menu items registered via `IMenuContributor` also automatically get the culture prefix. No changes are needed in your menu contributors or theme.

## Language Switching

ABP's built-in language switcher (the `/Abp/Languages/Switch` action) automatically replaces the culture segment in the `returnUrl`. The controller reads `CultureInfo.CurrentCulture` to identify the current culture and replaces it with the new one:

| Before switching | After switching to English |
|---|---|
| `/tr/products` | `/en/products` |
| `/tenant-a/zh-Hans/about` | `/tenant-a/en/about` |
| `/home?culture=tr&ui-culture=tr` | `/home?culture=en&ui-culture=en` |
| `/about` (no prefix) | `/about` (unchanged) |

No changes are needed in any theme or language switcher component.

## Blazor Server

Blazor Server uses SignalR (WebSocket), which does not re-run the HTTP middleware pipeline after the initial connection. ABP automatically persists the detected URL culture to a **Cookie** on the first request, so the entire Blazor circuit uses the correct language.

No additional configuration is needed beyond `UseRouteBasedCulture = true` and the correct middleware order.

## Blazor WebAssembly

The server project handles culture detection via routing. The WebAssembly client reads the culture from the server's application configuration API, which already reflects the URL-based culture.

No code changes are required in the WASM project.

## Multi-Tenancy Compatibility

URL-based localization is fully compatible with [multi-tenancy URL routing](../architecture/multi-tenancy/index.md). The culture route is registered as a conventional route `{culture}/{controller}/{action}`. If your application uses tenant routing (e.g., `/{tenant}/...`), the tenant middleware strips the tenant segment before routing, and the culture segment is handled separately.

Language switching also supports tenant-prefixed URLs. For example, `/tenant-a/zh-Hans/About` correctly switches to `/tenant-a/en/About`.

## API Routes

Routes like `/api/products` have no `{culture}` segment, so `RouteDataRequestCultureProvider` returns `null` and falls through to the next provider (Cookie → `Accept-Language` → default). API routes are completely unaffected.

## FAQ

### What happens with an invalid culture code in the URL?

If `/xyz1234/page` is requested and `xyz1234` is not a valid culture, `RequestLocalizationMiddleware` ignores it and falls through to the default culture. No error is thrown.

### Can I mix URL-based and QueryString-based culture detection?

Yes. All providers work together in priority order:

1. `RouteDataRequestCultureProvider` (URL path — highest priority when enabled)
2. `QueryStringRequestCultureProvider`
3. `CookieRequestCultureProvider`
4. `AcceptLanguageHeaderRequestCultureProvider`

### Should I keep both localized and non-localized routes?

Yes. ABP automatically registers both `{culture}/{controller}/{action}` and `{controller}/{action}` routes. The second route handles direct navigation to `/` and any controller action that doesn't have a culture prefix.
