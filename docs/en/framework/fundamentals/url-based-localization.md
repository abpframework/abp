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
* **`AbpCultureMenuItemUrlProvider`** — Prepends the culture prefix to navigation menu item URLs (MVC / Blazor Server).
* **`AbpWasmCultureMenuItemUrlProvider`** — Prepends the culture prefix to menu item URLs in Blazor WebAssembly (reads the `UseRouteBasedCulture` flag from `/api/abp/application-configuration`).

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

ABP's built-in language switcher (the `/Abp/Languages/Switch` action) automatically replaces the culture segment in the `returnUrl`. The controller reads the culture from the request cookie to identify the current page culture and replaces it with the new one:

| Before switching | After switching to English |
|---|---|
| `/tr/products` | `/en/products` |
| `/tenant-a/zh-Hans/about` | `/tenant-a/en/about` |
| `/home?culture=tr&ui-culture=tr` | `/home?culture=en&ui-culture=en` |
| `/about` (no prefix) | `/about` (unchanged) |

No changes are needed in any theme or language switcher component.

## MVC / Razor Pages

MVC and Razor Pages have the most complete support. Everything works automatically when `UseRouteBasedCulture = true`:

| Feature | How it works |
|---|---|
| **Route registration** | `AbpCultureRoutePagesConvention` adds `{culture}/...` route selectors to every Razor Page. MVC controllers get a `{culture}/{controller}/{action}` conventional route. |
| **URL generation** | `AbpCultureRouteUrlHelperFactory` wraps `IUrlHelperFactory` to auto-inject the `{culture}` route value into `Url.Page()`, `Url.Action()`, and tag helpers. |
| **Menu URLs** | `AbpCultureMenuItemUrlProvider` prepends the culture prefix to all local menu item URLs. |
| **Language switching** | The `/Abp/Languages/Switch` action replaces the culture segment in the return URL and sets the cookie. |

**No code changes are needed in your pages or controllers.** The framework handles everything.

### Example middleware pipeline

````csharp
app.UseRouting();
app.UseAbpRequestLocalization();
app.UseAuthorization();
app.UseConfiguredEndpoints();
````

## Blazor Server

Blazor Server uses SignalR (WebSocket) for the interactive circuit. The HTTP middleware pipeline only runs on the **initial page load** — subsequent interactions happen over the WebSocket connection. ABP handles this by persisting the detected URL culture to a **Cookie** on the first request, so the entire Blazor circuit uses the correct language.

### What works automatically

| Feature | How it works |
|---|---|
| **Culture detection** | `RouteDataRequestCultureProvider` reads `{culture}` from the URL on the initial HTTP request (SSR). |
| **Cookie persistence** | The middleware automatically saves the detected culture to `.AspNetCore.Culture` cookie, which persists across the WebSocket connection. |
| **Menu URLs** | `AbpCultureMenuItemUrlProvider` prepends the culture prefix. In the interactive circuit (where `HttpContext` is null — no active HTTP request), it falls back to `CultureInfo.CurrentCulture`. |
| **Language switching** | The built-in `LanguageSwitch` component navigates to `/Abp/Languages/Switch` with `forceLoad: true`, triggering a full HTTP reload. The culture segment in the return URL is automatically replaced. |

### What requires manual changes

**Blazor component routes**: ASP.NET Core does not provide an `IPageRouteModelConvention` equivalent for Blazor components. You must manually add the `{culture}` route to each page:

````razor
@page "/"
@page "/{culture}"

@code {
    [Parameter]
    public string? Culture { get; set; }
}
````

````razor
@page "/About"
@page "/{culture}/About"

@code {
    [Parameter]
    public string? Culture { get; set; }
}
````

> This applies to your own application pages. ABP built-in module pages (Identity, Tenant Management, Settings, Account, etc.) already include `@page "/{culture}/..."` routes out of the box — you do not need to add them manually.

### Example module configuration

````csharp
Configure<AbpRequestLocalizationOptions>(options =>
{
    options.UseRouteBasedCulture = true;
});
````

### Example middleware pipeline

````csharp
app.UseRouting();
app.UseAbpRequestLocalization();
app.UseAntiforgery();
app.UseConfiguredEndpoints(builder =>
{
    builder.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
});
````

## Blazor WebAssembly (WebApp)

Blazor WebAssembly (WASM) runs in the browser. On the **first page load**, the server renders the page via SSR, and the culture is detected from the URL. After WASM downloads, subsequent renders run in the browser. The WASM app fetches `/api/abp/application-configuration` from the server to get the current culture, so the culture stays consistent.

### What works automatically

| Feature | How it works |
|---|---|
| **SSR culture detection** | Same as Blazor Server — `RouteDataRequestCultureProvider` reads `{culture}` on the initial HTTP request. |
| **Cookie persistence** | The cookie is set during SSR, ensuring the WASM client inherits the correct culture. |
| **Menu URLs** | `AbpWasmCultureMenuItemUrlProvider` prepends the culture prefix to menu items. The `UseRouteBasedCulture` flag is read from `/api/abp/application-configuration`. |
| **Language switching** | The built-in `LanguageSwitch` component replaces the culture segment in the current URL and navigates with `forceLoad: true`, triggering a full page reload with the new culture. |
| **Login link** | The `LoginDisplay` component automatically prepends the culture prefix to the login URL when route-based culture is active. |

### What requires manual changes

Same as Blazor Server — you must manually add `@page "/{culture}/..."` routes to your Blazor pages.

### Example module configuration

````csharp
// Server project
Configure<AbpRequestLocalizationOptions>(options =>
{
    options.UseRouteBasedCulture = true;
});

// WASM client project — no special UseRouteBasedCulture configuration needed.
// The WASM client reads the flag from the server via /api/abp/application-configuration.
````

### Example middleware pipeline

````csharp
app.UseRouting();
app.UseAbpRequestLocalization();
app.UseAntiforgery();
app.UseConfiguredEndpoints(builder =>
{
    builder.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode()
        .AddInteractiveWebAssemblyRenderMode()
        .AddAdditionalAssemblies(clientAssembly);
});
````

## Multi-Tenancy Compatibility

URL-based localization is fully compatible with [multi-tenancy URL routing](../architecture/multi-tenancy/index.md). The culture route is registered as a conventional route `{culture}/{controller}/{action}`. If your application uses tenant routing (e.g., `/{tenant}/...`), the tenant middleware strips the tenant segment before routing, and the culture segment is handled separately.

Language switching also supports tenant-prefixed URLs. For example, `/tenant-a/zh-Hans/About` correctly switches to `/tenant-a/en/About`.

## API Routes

Routes like `/api/products` have no `{culture}` segment, so `RouteDataRequestCultureProvider` returns `null` and falls through to the next provider (Cookie → `Accept-Language` → default). API routes are completely unaffected.

## Culture Detection Priority

All request culture providers work together in priority order. When `UseRouteBasedCulture` is enabled, the route-based provider is added as the highest priority:

1. `RouteDataRequestCultureProvider` (URL path — highest priority when enabled)
2. `QueryStringRequestCultureProvider`
3. `CookieRequestCultureProvider`
4. `AcceptLanguageHeaderRequestCultureProvider`

If a URL contains an invalid culture code (e.g. `/xyz1234/page`), `RequestLocalizationMiddleware` ignores it and falls through to the next provider. No error is thrown.

## Route Registration

ABP automatically registers both `{culture}/{controller}/{action}` and `{controller}/{action}` routes. The non-prefixed route handles direct navigation to `/` and URLs without a culture segment.

> ABP built-in module pages (Identity, Tenant Management, Settings, Account, etc.) already include `@page "/{culture}/..."` route variants out of the box. You only need to add these routes to your own application pages.
