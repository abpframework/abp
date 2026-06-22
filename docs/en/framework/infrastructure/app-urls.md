```json
//[doc-seo]
{
    "Description": "Configure cross-application URLs in ABP with AppUrlOptions and IAppUrlProvider, including multi-tenant subdomain templates and redirect URL validation."
}
```

# Application URLs

ABP provides the `AppUrlOptions` options class and the `IAppUrlProvider` service to centrally configure and resolve URLs that point to **other applications** in your solution (for example, an MVC/Razor Pages UI, an Auth Server, an HTTP API host, etc.). They are typically used when code in one application needs to build a link that targets another — like the Account module putting a **password reset link** into an email.

* Defines `AppUrlOptions` to register the **root URL** and named relative URLs of each application.
* Provides `IAppUrlProvider` to **resolve** those URLs at runtime, with optional **tenant-aware** placeholder substitution.
* Supports **subdomain-style templates** (e.g. `https://{0}.example.com`) that produce per-tenant URLs without extra code.
* Maintains a `RedirectAllowedUrls` list used by `IAppUrlProvider.IsRedirectAllowedUrlAsync` to validate redirect targets.

> `AppUrlOptions` is defined in the `Volo.Abp.UI.Navigation` package, which comes pre-installed with the [application startup template](../../solution-templates/layered-web-application).

## Configuring Application URLs

`AppUrlOptions` exposes a dictionary of **applications**, each with a `RootUrl` and a set of named `Urls`.

**Example: Set the root URL and a named URL for the MVC application**

```csharp
Configure<AppUrlOptions>(options =>
{
    options.Applications["MVC"].RootUrl = "https://my-app.com";
    options.Applications["MVC"].Urls["MyPage"] = "my-page";
});
```

* `"MVC"` is the **application key**. Some modules (such as Account) register their URLs under a known key — `"MVC"` is the default for the **server-side UI**. You can use any key you want for your own applications.
* `RootUrl` is the **base URL** of that application.
* `Urls[urlName]` is a **relative path** appended to `RootUrl`. The final URL is built as `RootUrl.EnsureEndsWith('/') + Urls[urlName]`, so the relative path should **not** start with a `/`. When `RootUrl` is `null`, the value of `Urls[urlName]` is returned as-is.

The Account module, for example, **pre-registers** its URLs in its application module:

**Example: How the Account module registers the password reset URL**

```csharp
Configure<AppUrlOptions>(options =>
{
    options.Applications["MVC"].Urls[AccountUrlNames.PasswordReset] = "Account/ResetPassword";
});
```

> So configuring `Applications["MVC"].RootUrl` in your own module is usually enough to make password reset and similar Account email links point to the right host.

### Defaults in the application startup template

The ABP **application startup template** wires `Applications["MVC"].RootUrl` to the `App:SelfUrl` setting and seeds `RedirectAllowedUrls` from `App:RedirectAllowedUrls`:

```csharp
Configure<AppUrlOptions>(options =>
{
    options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
    options.RedirectAllowedUrls.AddRange(
        configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
});
```

> This is why Account email links point to your **host URL** out of the box: they reuse `App:SelfUrl`. If that default isn't what you want — for example, in a subdomain-based **multi-tenant** setup — override `Applications["MVC"].RootUrl` with the template you need (see [Multi-Tenant Aware URLs](#multi-tenant-aware-urls)).

## Using `IAppUrlProvider`

[Inject](../fundamentals/dependency-injection.md) the `IAppUrlProvider` service into any class that needs to build a cross-application URL.

**Example: Resolve a root URL and a named URL of the MVC application**

```csharp
public class MyNotificationSender : ITransientDependency
{
    private readonly IAppUrlProvider _appUrlProvider;

    public MyNotificationSender(IAppUrlProvider appUrlProvider)
    {
        _appUrlProvider = appUrlProvider;
    }

    public async Task SendAsync()
    {
        var rootUrl = await _appUrlProvider.GetUrlAsync("MVC");
        var pageUrl = await _appUrlProvider.GetUrlAsync("MVC", "MyPage");
    }
}
```

* `GetUrlAsync(appName)` returns the configured `RootUrl` for the given application.
* `GetUrlAsync(appName, urlName)` returns the **combined URL** described above.
* `GetUrlAsync(...)` throws an `AbpException` when the resolved URL is `null` or empty (e.g. both `RootUrl` and `Urls[urlName]` are unset). Use `GetUrlOrNullAsync(...)` if you'd rather get `null` and decide what to do yourself.
* `NormalizeUrlAsync(url)` applies tenant placeholder substitution to a URL string that you already have. Useful when the URL doesn't come from `AppUrlOptions`.

## Multi-Tenant Aware URLs

If your solution uses **subdomain-based** multi-tenancy (see the [Domain/Subdomain Tenant Resolver](../architecture/multi-tenancy/index.md#domainsubdomain-tenant-resolver)), you'll usually want the **outbound URLs** you generate (email links, redirects) to also be tenant-aware — otherwise the link in a password reset email won't point to the tenant's subdomain.

`AppUrlOptions` supports the following **placeholders** in any URL value. They are substituted by `IAppUrlProvider` based on the **current tenant**:

| Placeholder | Replaced with |
| --- | --- |
| `{0}` | Current tenant **name** |
| `{%{{{ {{tenantName}} }}}%}` | Current tenant **name** |
| `{%{{{ {{tenantId}} }}}%}` | Current tenant **id** |

The `{0}` placeholder uses the **same convention** as `AddDomainTenantResolver("{0}.example.com")`, so a typical subdomain-tenant setup looks like this:

**Example: Tenant-aware Account email links via a subdomain template**

```csharp
Configure<AbpTenantResolveOptions>(options =>
{
    options.AddDomainTenantResolver("{0}.example.com");
});

Configure<AppUrlOptions>(options =>
{
    options.Applications["MVC"].RootUrl = "https://{0}.example.com";
});
```

With this configuration, password reset emails sent to a tenant whose name is `acme` will contain a link starting with `https://acme.example.com/`, matching the tenant's subdomain.

### Host (no tenant) Fallback

When there is **no current tenant** (host-side request), the placeholder **and the dot following it** are removed together:

| Template | Tenant `acme` | Host (no tenant) |
| --- | --- | --- |
| `https://{0}.example.com` | `https://acme.example.com` | `https://example.com` |
| `https://{%{{{ {{tenantId}} }}}%}.example.com` | `https://3a21....example.com` | `https://example.com` |

A single subdomain-style template like the ones above therefore works for **both** tenant and host scenarios without extra configuration.

> If your subdomain is based on the tenant **id** rather than the name, use `https://{%{{{ {{tenantId}} }}}%}.example.com`. The resolver's `{0}` placeholder accepts both name and id when finding a tenant, but `AppUrlOptions` substitutes `{0}` with the tenant **name**; if those two don't match, switch to the explicit `{%{{{ {{tenantId}} }}}%}` form on the `AppUrlOptions` side.

## Redirect Allowed URLs

`AppUrlOptions.RedirectAllowedUrls` is a list of URL entries used by `IAppUrlProvider.IsRedirectAllowedUrlAsync(url)` to decide whether a redirect target is allowed. A URL is allowed when it satisfies **either** of:

* **Prefix match**: the URL string **starts with** a configured entry (case-insensitive).
* **Subdomain match**: the URL and the entry have the **same scheme** and **port**, and the URL's host **ends with** `.{entry-host}`.

**Example: Register allowed redirect URLs (including a wildcard)**

```csharp
Configure<AppUrlOptions>(options =>
{
    options.RedirectAllowedUrls.Add("https://my-app.com");
    options.RedirectAllowedUrls.Add("https://admin.my-app.com");

    options.RedirectAllowedUrls.Add("https://*.my-app.com");
});
```

* A **plain entry** like `https://my-app.com` allows any URL that starts with that prefix, plus any subdomain of `my-app.com`.
* A **wildcard entry** like `https://*.my-app.com` allows any subdomain of `my-app.com`; the `*.` is stripped before the subdomain check.
* Entries also go through **tenant placeholder substitution**, so `https://{0}.my-app.com` is resolved to the current tenant's URL first (e.g. `https://acme.my-app.com`) and then compared. Use the wildcard form when you need to allow *any* tenant subdomain regardless of the current tenant.

## See Also

* [Multi-Tenancy](../architecture/multi-tenancy/index.md)
* [Account Module](../../modules/account.md)
* [Emailing](emailing.md)
