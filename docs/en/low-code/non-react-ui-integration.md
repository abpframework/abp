```json
//[doc-seo]
{
    "Description": "Add the Low-Code designer and a companion React runtime to an existing MVC, Razor Pages, Blazor, or Angular ABP application."
}
```

# Use Low-Code from a Non-React Application

> **Target release:** This guide targets the ABP `10.6.x` release line and .NET `10`. Keep the existing solution, Admin Console, Low-Code backend, and companion React packages on the same `10.6.x` patch version.

> **Verification history:** The workflow was first verified on July 22, 2026 with ABP `10.5.0`, .NET `10`, a layered MVC application, a layered Angular application, and a layered Blazor Web App application. Version-specific `10.5.0` observations below are historical regression checkpoints, not package-version instructions for a `10.6.x` solution.

> **Verification boundary:** Anonymous and authorized menu rendering was verified for MVC, Angular, and Blazor. On July 22, 2026, the complete workflow was also reapplied to a fresh layered MVC solution: Studio import, migration and seed, Admin Console filtering, same-host React build, anonymous and authorized menus, OIDC-protected deep-link return, entity and Data Grid creation, runtime record creation, scoped fallback, and **Back to the application** navigation all passed. The standalone Blazor sample used a separate authority, so its authenticated remote-menu call was not used as proof for a production topology; keep the Blazor UI and Low-Code API on the same backend and authority as described below.

The Low-Code backend is independent of the application's main UI framework. The current Low-Code runtime renderer, however, is React. An existing MVC, Razor Pages, Blazor, or Angular application can therefore keep its current UI and open generated Low-Code pages in a companion React application.

This is the verified topology:

| URL | Owner | Purpose |
|-----|-------|---------|
| `/` | Existing MVC, Razor Pages, Blazor, or Angular UI | The application's existing user interface |
| `/admin-console/lowcode-designer` | Admin Console | Design entities, pages, forms, permissions, and actions |
| `/lowcode/` | Companion React application | Render generated Low-Code pages at runtime |
| `/api/*` | Existing backend | Serve both the existing UI and Low-Code runtime |

The backend setup is the same as described in [Add Low-Code to an Existing Solution](add-to-existing-solution.md). This page focuses on the UI-specific work that is required when the existing application is not React.

## How the Application Changes

This integration adds two React-based experiences next to the existing UI. They run from the same backend, but they do not adopt the existing MVC, Razor Pages, Blazor, or Angular layout.

### Admin Console and the Designer

Admin Console is a separate administration application under `/admin-console`. It has its own layout, navigation, theme, frontend assets, and OIDC client. It does not replace the existing application's administration pages when `RedirectRootToAdminConsole` is `false`.

Admin Console discovers the installed modules that expose an Admin Console UI. Consequently, it can display administration interfaces for other supported modules in addition to Low-Code. The Low-Code Designer appears as one of these module interfaces and opens inside Admin Console at `/admin-console/lowcode-designer`.

This has a visible side effect: users may see two administration experiences with different designs. For example, a module may remain accessible from the existing application's **Administration** menu while its Admin Console interface is also available under `/admin-console`. Decide which entry points to expose to each role; hiding a duplicate menu entry does not change its authorization requirements.

Adding Admin Console also adds its routes, static frontend assets, module discovery, configuration endpoints, permissions, and authentication client to the existing host. The host startup and deployment therefore include an additional frontend application even when users continue to start at `/`.

### Low-Code Runtime Pages

Generated Low-Code pages are rendered by a second React application under `/lowcode`. Opening the `Dynamic` menu item performs a full-page navigation out of the existing UI. The runtime has its own layout and navigation, so it does not visually inherit the existing application's shell.

The companion runtime uses the same backend APIs, users, roles, tenants, and generated permissions, but it has a dedicated OIDC client and callback URLs. Its build output becomes part of the backend deployment. A scoped SPA fallback is required for deep links, and the `/lowcode` path must be reserved so it does not conflict with an existing route.

The existing root UI remains unchanged as long as Admin Console root redirection is disabled and the React fallback is limited to `/lowcode/*`.

## Important Boundaries

* `Volo.Abp.LowCode` provides the runtime backend.
* `Volo.Abp.LowCode.Designer.*` provides the designer application services and HTTP APIs.
* `Volo.Abp.AdminConsole` provides the browser shell that hosts the designer.
* `@volo/abp-react-lowcode` provides the runtime renderer.
* Angular, MVC, Razor Pages, and Blazor do not currently render Low-Code page descriptors themselves.

Adding only the backend packages does not make the designer UI appear. A non-React host may also be missing `Volo.Abp.AdminConsole`, so check and add it explicitly.

## Step 1: Add the Shared Low-Code Backend

### Import with ABP Studio

Use ABP Studio's **Import Module** action, select `Volo.Abp.LowCode`, keep **Install this module** checked, and accept Studio's suggested package-to-layer mapping.

Then complete the manual backend work from [Add Low-Code to an Existing Solution](add-to-existing-solution.md):

* create and embed `_Dynamic`
* add the Low-Code initializer
* initialize the runtime from the host and DbMigrator
* configure the design-time DbContext factory
* implement `IDbContextWithDynamicEntities`
* call `ConfigureDynamicEntities()` and `ConfigureLowCode()`
* add the default `DynamicEntity` repository
* map Low-Code custom endpoints
* inspect Studio's migration, create one only when the final model still has pending changes, and apply it

The DbContext configuration order is significant and is the same for MVC, Angular, and Blazor solutions. Keep this common backend step outside the UI-specific conditions:

```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.ConfigureDynamicEntities();

    base.OnModelCreating(builder);

    builder.ConfigureLowCode();
}
```

`ConfigureDynamicEntities()` must run before the base model conventions, while `ConfigureLowCode()` stays after the base call. Also run the Low-Code initializer before design-time DbContext creation. Otherwise, generated migrations can be incomplete or the runtime and design-time EF Core models can diverge.

Studio may create `Added_DynamicEntities_LowCode` and insert both configuration calls. In the historically verified ABP `10.5.0` solution, Studio placed both calls after the base call, so the first call had to be moved manually. Inspect the generated `10.6.x` result instead of assuming the generator still behaves identically. After completing all backend wiring, run `dotnet ef migrations has-pending-model-changes` against the EF Core project with DbMigrator as the startup project. Keep Studio's migration when no changes remain; do not add a duplicate migration.

ABP Studio is the supported path in this guide. The remaining steps assume Studio has already added the Low-Code runtime and designer packages to the appropriate layers.

## Step 2: Add Admin Console for the Designer

Skip this step only when the startup host already references `Volo.Abp.AdminConsole` and depends on `AbpAdminConsoleModule`.

Add the package to the runnable host:

```powershell
abp add-package Volo.Abp.AdminConsole `
  --project "src\Acme.NonReactLowCode.Web\Acme.NonReactLowCode.Web.csproj" `
  --version <AbpVersion>
```

Add the module dependency:

```csharp
using Volo.Abp.AdminConsole;

[DependsOn(typeof(AbpAdminConsoleModule))]
public class NonReactLowCodeWebModule : AbpModule
{
}
```

Configure Admin Console in the startup host. Keep `RedirectRootToAdminConsole` disabled so the existing UI remains at `/`:

```json
{
  "AdminConsole": {
    "IsEnabled": true,
    "RedirectRootToAdminConsole": false,
    "Authority": "https://localhost:44323",
    "ClientId": "NonReactLowCode_AdminConsole",
    "Scope": "openid profile email offline_access NonReactLowCode",
    "LocalizationLanguages": ["en", "tr"],
    "VisibleModules": ["lowCodeDesigner"],
    "ThemeOverrideCssPath": null,
    "InitialTheme": "system",
    "CustomizationPermissionName": null
  }
}
```

Create a dedicated public OpenIddict client. Do not reuse the existing MVC, Angular, Blazor, Swagger, or runtime client.

```json
{
  "OpenIddict": {
    "Applications": {
      "NonReactLowCode_AdminConsole": {
        "ClientId": "NonReactLowCode_AdminConsole",
        "RootUrl": "https://localhost:44323/admin-console"
      }
    }
  }
}
```

Seed the client with these values:

| Setting | Value |
|---------|-------|
| Application type | `Web` |
| Client type | `Public` |
| Grant types | `AuthorizationCode`, `RefreshToken`, `LinkLogin`, `Impersonation` |
| Redirect URI | `https://localhost:44323/admin-console/` |
| Silent renew URI | `https://localhost:44323/admin-console/silent-renew.html` |
| Post-logout URI | `https://localhost:44323/admin-console/` |
| API scope | The existing backend scope, such as `NonReactLowCode` |

Fail startup or data seeding when the configured root URL is empty. Do not silently fall back to a development URL.

Run the DbMigrator again so the client and designer permission definitions are seeded. The administrator role needs:

* `AbpLowCodeDesigner.Default`
* `AbpLowCodeDesigner.Edit`
* `AbpLowCodeDesigner.ScriptTest` when script testing is allowed

These are designer permissions. They are not a replacement for the permissions assigned to generated runtime pages and endpoints.

### Limit Admin Console to Low-Code

`AbpAdminConsoleOptions` does not have a built-in module allowlist. However, the Admin Console React application calls `GET /admin-console/api/modules` before registering its module routes and sidebar entries. Replace `AdminConsoleModuleDiscoveryController` with an application controller to control that response without removing backend services.

The `VisibleModules` value in the earlier configuration example is an application-specific setting consumed by this replacement controller; it is not a built-in Admin Console option. Create `LowCodeAdminConsoleModuleDiscoveryController.cs` in the startup host:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Volo.Abp.AdminConsole.Controllers;
using Volo.Abp.AspNetCore.Controllers;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.NonReactLowCode.Web.Controllers;

[ReplaceControllers(typeof(AdminConsoleModuleDiscoveryController))]
[Route("admin-console/api")]
public class LowCodeAdminConsoleModuleDiscoveryController : AbpController
{
    private static readonly string[] KnownModuleKeys =
    [
        "identity",
        "saas",
        "auditLogging",
        "gdpr",
        "openIddict",
        "aiManagement",
        "lowCodeDesigner",
        "settingManagement",
        "textTemplateManagement"
    ];

    private readonly HashSet<string> _visibleModules;

    public LowCodeAdminConsoleModuleDiscoveryController(IConfiguration configuration)
    {
        var configuredModules = configuration
            .GetSection("AdminConsole:VisibleModules")
            .Get<string[]>();

        if (configuredModules is null || configuredModules.Length == 0)
        {
            throw new InvalidOperationException(
                "AdminConsole:VisibleModules must contain at least one module key."
            );
        }

        var unknownModules = configuredModules
            .Except(KnownModuleKeys, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (unknownModules.Length > 0)
        {
            throw new InvalidOperationException(
                $"Unknown Admin Console module keys: {string.Join(", ", unknownModules)}"
            );
        }

        _visibleModules = configuredModules.ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    [HttpGet("modules")]
    public IActionResult GetAvailableModules()
    {
        return Ok(KnownModuleKeys.ToDictionary(
            key => key,
            key => _visibleModules.Contains(key)
        ));
    }
}
```

`ReplaceControllers` removes the original controller from the MVC application model, so the replacement can keep the same route without causing an ambiguous endpoint. With only `lowCodeDesigner` enabled, Admin Console registers the Low-Code Designer route and omits the Identity, SaaS, Audit Logging, GDPR, OpenIddict, AI Management, Setting Management, and Text Template Management routes. Opening an omitted route directly produces the Admin Console `404` page.

This is global UI composition, not authorization. The omitted modules, application services, HTTP APIs, permissions, and existing MVC, Blazor, or Angular management pages remain installed and operational. Keep permission checks in place and grant designer users only the permissions they require:

1. Create a dedicated role such as `LowCodeDesigner`.
2. Grant `AbpLowCodeDesigner.Default` and `AbpLowCodeDesigner.Edit` to that role.
3. Grant `AbpLowCodeDesigner.ScriptTest` only when those users may execute designer script tests.
4. Do not grant unrelated administration permissions unless the same users need them in the existing UI.

Permissions are shared by the existing UI and Admin Console, and grants are additive. The controller allowlist avoids changing those shared grants merely to shape Admin Console. A user still needs the required Low-Code permission even when `lowCodeDesigner` is returned as `true`.

The Admin Console home page and authenticated account pages are core routes and remain available. In the historically verified ABP `10.5.0` package, the home page's management cards and Quick Navigation links were static and did not consume the module discovery response. Verify that behavior with the installed `10.6.x` patch. If the current package still uses static cards, hidden module links on that page lead to `404`; link users directly to `/admin-console/lowcode-designer`, keep `RedirectRootToAdminConsole` disabled, and treat the Admin Console root as a known limitation. Removing or redesigning that static home content requires a custom Admin Console frontend. Keep `CustomizationPermissionName` unset or `null` to avoid registering the customization route.

The module keys are an Admin Console frontend contract and can change when the package is upgraded. Compare `KnownModuleKeys` with the upgraded `AdminConsoleModuleDiscoveryController` and React route configuration during every ABP upgrade. The explicit validation above makes a missing or unknown allowlist fail the discovery request instead of silently exposing additional modules.

## Step 3: Create a Companion React Runtime

The safest source for the companion application is a temporary solution generated at exactly the same ABP version. In ABP Studio, create a modern React application with Low-Code enabled and use the same tenancy, authentication, theme, and database-provider choices as the existing solution.

Before adapting or copying the frontend, establish whether that generated `10.6.x` seed is clean on its own:

```powershell
cd react
yarn install --frozen-lockfile
yarn lint
yarn test:run
yarn build
```

Use the scripts and package manager declared by the generated application when their names differ. If the untouched seed fails, record and resolve that template baseline separately; otherwise later failures can be misattributed to the `/lowcode` integration. Do not replace its lockfile or switch package managers merely to make the commands pass.

Testing Library packages have peer dependencies. If the test runner or TypeScript cannot resolve `screen`, `waitFor`, or `fireEvent`, inspect `package.json` and the lockfile. Add a compatible `@testing-library/dom` version only when it is missing; the historical `10.5.0` seed required `@testing-library/dom@10.4.1`, while a generated `10.6.x` patch may already declare a compatible version.

Copy the generated `react/` directory into the existing solution, for example as `lowcode-react/`. Keep the generated ABP authentication, Axios, localization, `configureLowCode`, `createDynamicRoutes`, and `useMenuItems` integration.

Do not copy the temporary backend. The companion React application must use the existing application's backend and database.

### Rename and Localize the Companion Application

The copied frontend still contains the temporary solution's identity. Update it before building the application that will be deployed:

* Change the package name in `package.json` without discarding the generated lockfile.
* Replace the HTML `<title>` in `index.html`.
* Update `application.name`, `apis.default.rootNamespace`, client IDs, scopes, and URLs in `dynamic-env.json` and the development environment source.
* Replace the temporary application's `::AppName` and resource-qualified keys in `src/locales/*.json`.
* Add `Menu:LowCodeDesigner` to the localization resource actually loaded by the companion runtime. Adding it only to the MVC, Angular, or Blazor resource does not update a copied React localization snapshot.

Search the copied frontend for the temporary solution and client names. Review every match instead of doing a blind replacement because generated test fixtures can intentionally assert a client ID:

```powershell
rg "TemporaryProjectName|TemporaryProjectName_App" lowcode-react
```

After the build, verify the browser tab title, application name, and menu labels. A raw value such as `Menu:LowCodeDesigner` means the key is absent from the active React localization resource.

### Configure the `/lowcode` Base Path

Set both the bundler and router base paths. Setting only one causes broken assets or broken client-side routes.

In `lowcode-react/vite.config.ts`:

```ts
export default defineConfig(() => ({
  base: '/lowcode/',
  build: {
    outDir: '../src/Acme.NonReactLowCode.Web/wwwroot/lowcode',
    emptyOutDir: true,
  },
  optimizeDeps: {
    exclude: ['@volo/abp-react-lowcode'],
  },
  plugins: [copyDynamicEnv(), react()],
}))
```

In the TanStack router configuration:

```tsx
export const router = createRouter({
  routeTree,
  basepath: '/lowcode',
})
```

Load `dynamic-env.json` relative to the Vite base path:

```ts
const urlsToTry = [
  `${import.meta.env.BASE_URL}dynamic-env.json`,
  '/getEnvConfig',
]
```

The silent renew URI must also include the base path:

```ts
const silentRedirectUri =
  `${window.location.origin}${import.meta.env.BASE_URL}silent-renew.html`
```

### Configure the Runtime URLs

Use the same backend origin, but create a separate runtime client:

```json
{
  "production": "true",
  "application": {
    "baseUrl": "https://localhost:44323/lowcode/",
    "returnUrl": "https://localhost:44323/",
    "name": "NonReactLowCode",
    "logoUrl": ""
  },
  "oAuthConfig": {
    "issuer": "https://localhost:44323/",
    "redirectUri": "https://localhost:44323/lowcode/",
    "requireHttps": "true",
    "clientId": "NonReactLowCode_LowCodeUi",
    "responseType": "code",
    "scope": "offline_access NonReactLowCode",
    "strictDiscoveryDocumentValidation": false,
    "skipIssuerCheck": false
  },
  "apis": {
    "default": {
      "url": "https://localhost:44323",
      "rootNamespace": "Acme.NonReactLowCode"
    }
  }
}
```

The example uses explicit development URLs for clarity. Replace them with deployment-specific configuration. Do not ship `requireHttps=false` or `skipIssuerCheck=true` as a production workaround.

### Add the Runtime OpenIddict Client

Add this explicit entry to the DbMigrator configuration:

```json
{
  "NonReactLowCode_LowCodeUi": {
    "ClientId": "NonReactLowCode_LowCodeUi",
    "RootUrl": "https://localhost:44323/lowcode"
  }
}
```

Seed it as a public web client with authorization code, refresh token, `LinkLogin`, and `Impersonation` grants. Use these exact callback shapes:

```text
https://localhost:44323/lowcode/
https://localhost:44323/lowcode/silent-renew.html
```

Use `https://localhost:44323/lowcode/` as the post-logout URI.

### Serve the React Build from the Existing Host

Build the companion application with its existing package manager:

```powershell
cd lowcode-react
yarn install --frozen-lockfile
yarn lint
yarn test:run
yarn build
```

Map a scoped SPA fallback in the backend host. Do not cache the fallback HTML because it contains the current hashed JavaScript and CSS asset names:

```csharp
app.UseConfiguredEndpoints(endpoints =>
{
    endpoints.UseLowCodeCustomEndpoints();
    endpoints.MapFallbackToFile(
        "/lowcode/{*path:nonfile}",
        "lowcode/index.html",
        new StaticFileOptions
        {
            OnPrepareResponse = context =>
            {
                context.Context.Response.Headers.CacheControl =
                    "no-store, no-cache, must-revalidate";
                context.Context.Response.Headers.Pragma = "no-cache";
                context.Context.Response.Headers.Expires = "0";
            }
        }
    );
});
```

Do not use a global fallback. A global fallback can swallow MVC, Razor Pages, Admin Console, and API routes. Hashed assets can remain cacheable; the no-cache rule applies only to fallback `index.html` responses. Otherwise, a deployment can leave browsers requesting asset hashes that no longer exist and `/lowcode/` can become a blank page.

### Configure ABP Antiforgery for Mutations

The Low-Code React source template configures ABP's antiforgery cookie and header names, but treat this as a mandatory verification checkpoint. A fresh distributed ABP `10.5.0` React Low-Code template generated during this verification omitted both values even though the source template contained them. Inspect the companion runtime's shared Axios instance, keep the values when present, and add them when absent. Do not create a second Axios instance just for this setting.

```ts
export const api = axios.create({
  baseURL: '',
  headers: {
    'X-Requested-With': 'XMLHttpRequest',
    'Content-Type': 'application/json',
  },
  xsrfCookieName: 'XSRF-TOKEN',
  xsrfHeaderName: 'RequestVerificationToken',
})
```

The application-configuration endpoint creates the `XSRF-TOKEN` cookie. Axios then sends its value as `RequestVerificationToken` for same-origin mutation requests. This is required when `/lowcode/` shares an origin and authentication cookie with the MVC host. Without it, reads can work while create, update, delete, import, and other mutation requests fail with `400`; the MVC host may then redirect the XHR to `/Error?httpStatusCode=400`, making the network trace look like a missing API.

Do not fix this by disabling `AbpAntiForgeryOptions.AutoValidate`. Disabling validation weakens every affected browser endpoint in the host. If the runtime is deployed on another origin, validate its bearer-token, CORS, cookie, and antiforgery behavior separately; a browser cannot read an XSRF cookie from an unrelated origin.

Also audit redirects and account links copied from a root-hosted React seed. Absolute client paths such as `window.location.href = '/403'` and `'/account/login'` leave the `/lowcode` base path. Use `import.meta.env.BASE_URL`, the TanStack router, or a validated absolute application URL instead. This applies to forbidden redirects, password-reset completion, login, logout, and any custom navigation added later.

### Protect Runtime Menus and Routes

Do not fetch or render dynamic menu items for an anonymous user:

```tsx
const { isAuthenticated } = useAuth()
const { data: dynamicMenuItems } = useMenuItems({
  enabled: isAuthenticated,
})
```

The returned tree is already filtered by the signed-in user's generated page permissions. Empty groups are removed by the backend. Protect direct dynamic routes separately so hiding the menu is never the authorization boundary:

```tsx
const dynamicEntityRoute = createDynamicRoutes(rootRoute, {
  beforeLoad: authGuard,
})
```

Preserve the requested deep link before starting OIDC. Use `window.location.href`, not TanStack's `location.href`, because the router value does not include the `/lowcode` base path:

```ts
const PENDING_RETURN_URL_KEY = 'abp.oidc.pending-return-url'

export async function authGuard({ location }: GuardContext) {
  const accessToken = await ensureAccessToken()
  const user = await userManager.getUser()

  if (!user || user.expired || !accessToken) {
    const returnUrl =
      typeof window === 'undefined' ? location.href : window.location.href

    sessionStorage.setItem(PENDING_RETURN_URL_KEY, returnUrl)
    await userManager.signinRedirect({ state: { returnUrl } })
    throw new Error('Redirecting to login')
  }
}
```

After `signinCallback`, accept only a same-origin URL whose path starts with `import.meta.env.BASE_URL`. Prefer the OIDC user state and use the consume-once session storage value as a fallback. Remove `code`, `state`, `session_state`, and `iss` from the callback URL before replacing it with the validated return URL. This prevents an external redirect and restores URLs such as `/lowcode/dynamic/orders?view=open`.

Create the router only after callback handling. A static `App` import creates the router too early and can make it capture `/lowcode/?code=...` instead of the restored dynamic route:

```tsx
async function bootstrap() {
  await loadRuntimeConfig()
  await initUserManager()
  await ensureOidcCallbackHandled()

  const { default: App } = await import('./App.tsx')
  createRoot(document.getElementById('root')!).render(<App />)
}
```

The session storage entry contains only a route URL, is scoped to the runtime origin and tab, and must be removed after success or failure. It is not an authorization token. The backend still authorizes the generated page and data endpoint.

### Update Tests for the `/lowcode` Subpath

The generated tests describe a root-hosted application. Update their expectations together with the implementation instead of accepting a production build while leaving the test suite red:

* Route-guard tests should expect the preserved absolute browser URL, including `/lowcode`, rather than only a TanStack path such as `/dashboard`.
* Forbidden and account-navigation tests should expect a base-aware destination such as `/lowcode/403`; avoid assertions that assign directly to JSDOM's unimplemented navigation API.
* If you remove the generated Home or Identity Users UI, remove or rewrite the tests that still expect those screens and menu items.
* Keep OIDC callback tests for a valid same-origin `/lowcode/` URL, rejection of an external or out-of-base URL, consume-once session storage, and restoration of query strings.

Run `yarn lint`, `yarn test:run`, and `yarn build` after these changes. Do not weaken the deep-link, authorization, or antiforgery behavior only to preserve an obsolete root-hosted test expectation.

### Add Back to the Application

Read the explicit `application.returnUrl` value added to `dynamic-env.json` and fail when it is missing:

```ts
export function getHostApplicationUrl(): string {
  const returnUrl = loadedConfig?.application?.returnUrl
  if (!returnUrl) {
    throw new Error('application.returnUrl must be configured.')
  }

  return returnUrl
}
```

Add a normal anchor to the runtime header:

```tsx
<Button variant="ghost" size="sm" asChild>
  <a href={getHostApplicationUrl()}>
    <ArrowLeft className="size-4" />
    <span>{t('AbpUi::BackToTheApplication')}</span>
  </a>
</Button>
```

The anchor intentionally performs a full-page navigation back to the existing application. Like the source application's links into Low-Code, it discards unsaved in-memory state in the current UI.

### Simplify the Companion React Shell

The generated React seed is a complete application shell, so it may contain sample or management UI that is not needed by a dedicated Low-Code runtime. You can remove these parts without changing the shared Low-Code backend.

#### Remove the Users Entry

To remove only **Users** from the sidebar, delete its item and the unused `Users` icon import from `src/lib/routing/route-config.ts`:

```ts
export const routeConfig: RouteConfigItem[] = [
  { path: '/', nameKey: 'Menu:Home', icon: Home, order: 1 },
  // Dynamic items are added from useMenuItems().
]
```

This is a navigation-only change. An authorized user can still open `/lowcode/identity/users` directly while the route remains registered.

If the companion runtime should not contain the React Identity Users UI at all, also remove the following from `src/routes/router.tsx`:

* the `IdentityLayout` and `UsersPage` imports
* `identityLayoutRoute`, `identityIndexRoute`, and `identityUsersRoute`
* the composed `identityRoute`
* `identityRoute` from `rootRoute.addChildren(...)`

The resulting route tree should no longer contain `identityRoute`:

```tsx
const routeTree = rootRoute.addChildren([
  indexRoute,
  forbiddenRoute,
  accountRoute,
  dynamicEntityRoute,
])
```

Remove now-unused imports. You can also delete `src/components/identity` and `src/pages/identity` after confirming that no other routes import them. The scoped SPA fallback still serves `index.html` for `/lowcode/identity/users`, but TanStack Router now renders its not-found component because the client route no longer exists.

Removing this React route does not uninstall the Identity module, revoke `AbpIdentity.Users`, or remove Identity HTTP APIs from the backend. Existing MVC, Angular, Blazor, and Admin Console management interfaces remain unchanged.

#### Customize or Remove Home

Replace the generated sample content in `src/pages/home/HomePage.tsx` with a product-specific landing page, usage instructions, or a simple branded welcome screen. For example:

```tsx
import { useTranslation } from 'react-i18next'
import { Button } from '@/components/ui/button'
import { useAuth } from '@/lib/auth/AuthContext'

export function HomePage() {
  const { t } = useTranslation()
  const { isAuthenticated, navigateToLogin } = useAuth()

  return (
    <section className="rounded-xl border bg-card p-8">
      <p className="text-sm font-medium text-muted-foreground">Low-Code</p>
      <h1 className="mt-2 text-3xl font-semibold">Application workspace</h1>
      <p className="mt-3 max-w-2xl text-muted-foreground">
        Select a page from the navigation menu to work with application data.
      </p>
      {!isAuthenticated && (
        <Button className="mt-6" onClick={navigateToLogin}>
          {t('AbpAccount::Login')}
        </Button>
      )}
    </section>
  )
}
```

The `/lowcode/` route is registered by `indexRoute` in `src/routes/router.tsx`, and its sidebar entry is the `/` item in `route-config.ts`.

To hide Home only from the sidebar, remove the `/` item from `route-config.ts` but keep `indexRoute`. To remove Home completely, also remove the `HomePage` import, the `indexRoute` declaration, and `indexRoute` from `rootRoute.addChildren(...)`. The latter makes `/lowcode/` render the client-side not-found page unless another index route replaces it.

Do not hard-code a generated page as the default unless every intended user has that page permission. The safer approach is to keep a small Home page and render links from the already-filtered `useMenuItems()` response. This avoids advertising or redirecting to pages that the signed-in user cannot open.

The companion Home is independent from `/` in the existing application and from the Admin Console home page. Customizing it changes only `/lowcode/`.

#### Keep an Account Exit

The header `UserMenu` can also be removed, but it normally provides logout and account actions. To replace it with only login and logout controls in `src/components/layout/Header.tsx`:

1. Remove the `UserMenu` import.
2. Add `LogOut` to the `lucide-react` import.
3. Include `logout` in the existing `useAuth()` destructuring.
4. Replace the `UserMenu` branch with the following button:

```tsx
const { isAuthenticated, isLoading, login, logout } = useAuth()

// Inside the existing header actions:
{!isLoading && (
  isAuthenticated ? (
    <Button variant="ghost" size="sm" onClick={() => void logout()}>
      <LogOut className="size-4" />
      <span className="hidden sm:inline">{t('AbpAccount::Logout')}</span>
    </Button>
  ) : (
    <Button size="sm" onClick={() => void login()}>
      <LogIn className="size-4" />
      {t('AbpAccount::Login')}
    </Button>
  )
)}
```

Keep the **Back to the application** link beside these controls. Use the existing `logout()` function rather than clearing browser storage or navigating directly to a logout URL; the auth context revokes the current tokens before completing the OIDC logout. Removing `UserMenu` hides profile and account actions only. It does not remove account routes or backend account endpoints.

#### Link to Designer from the Companion Runtime

An authenticated designer can also open Admin Console directly from the companion sidebar. Gate this entry with the real Designer policy; `AdminConsole.Access` is not a built-in permission and using it leaves the link hidden even for the administrator.

```ts
{
  path: '/low-code-designer',
  nameKey: 'Menu:LowCodeDesigner',
  icon: ShieldCheck,
  order: 6,
  requiredPolicy: 'AbpLowCodeDesigner.Default',
  externalHref: () =>
    `${getBackendOrigin()}/admin-console/lowcode-designer`,
  externalTarget: '_blank',
  externalRel: 'noopener noreferrer',
}
```

Use the backend origin from the loaded runtime configuration rather than hard-coding a development port. Opening a new tab preserves an in-progress runtime page; use `_self` instead if the product deliberately wants a single application surface.

## Step 4: Add UI Navigation

The existing UI does not render the Low-Code pages or Designer. Its menu entries perform full-page navigation to the two React applications. Add `Menu:Dynamic` and `Menu:LowCodeDesigner` to the existing application's localization resource.

{{if UI=="MVC"}}

### MVC or Razor Pages

Add both required URLs to the startup host configuration:

```json
{
  "LowCodeUi": {
    "RuntimeUrl": "https://localhost:44323/lowcode/",
    "DesignerUrl": "https://localhost:44323/admin-console/lowcode-designer"
  }
}
```

Read and validate them in the existing `IMenuContributor`. Make the contributor asynchronous so it can ask the Low-Code UI service whether the current user has at least one visible page:

```csharp
private static async Task ConfigureMainMenuAsync(
    MenuConfigurationContext context)
{
    var l = context.GetLocalizer<MyProjectResource>();
    var configuration = context.ServiceProvider
        .GetRequiredService<IConfiguration>();
    var runtimeUrl = configuration["LowCodeUi:RuntimeUrl"];
    var designerUrl = configuration["LowCodeUi:DesignerUrl"];

    if (string.IsNullOrWhiteSpace(runtimeUrl))
    {
        throw new InvalidOperationException(
            "LowCodeUi:RuntimeUrl must be configured."
        );
    }

    if (string.IsNullOrWhiteSpace(designerUrl))
    {
        throw new InvalidOperationException(
            "LowCodeUi:DesignerUrl must be configured."
        );
    }

    var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
    if (currentUser.IsAuthenticated)
    {
        var lowCodeUi = context.ServiceProvider
            .GetRequiredService<IDynamicEntityUIAppService>();
        var visibleItems = await lowCodeUi.GetMenuItemsAsync();

        if (visibleItems.Count > 0)
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "MyProject.Dynamic",
                    l["Menu:Dynamic"],
                    runtimeUrl,
                    icon: "fa fa-cubes",
                    order: 3,
                    target: "_self"
                ).RequireAuthenticated()
            );
        }
    }

    context.Menu.AddItem(
        new ApplicationMenuItem(
            "MyProject.LowCodeDesigner",
            l["Menu:LowCodeDesigner"],
            designerUrl,
            icon: "fa fa-wand-magic-sparkles",
            order: 4,
            target: "_self"
        ).RequireAuthenticated()
         .RequirePermissions(AbpLowCodeDesignerPermissions.Designer.Default)
    );
}
```

Add `Volo.Abp.Users`, `Volo.Abp.LowCode.UI`, and `Volo.Abp.LowCode.Designer.Permissions` imports as needed. `target: "_self"` deliberately replaces the MVC page with the selected React application. Using a new tab is also possible, but it creates another authenticated browser context for users to manage.

{{else if UI=="NG"}}

### Angular

Add explicit URLs to both development and production environment configuration:

```ts
import { Environment } from '@abp/ng.core'

export type LowCodeEnvironment = Environment & {
  lowCodeUi: {
    runtimeUrl: string
    designerUrl: string
  }
}

export const environment = {
  // Existing ABP environment values...
  lowCodeUi: {
    runtimeUrl: 'https://localhost:44323/lowcode/',
    designerUrl: 'https://localhost:44323/admin-console/lowcode-designer',
  },
} as LowCodeEnvironment
```

Create `low-code-redirect.guard.ts`:

```ts
import { Environment, EnvironmentService } from '@abp/ng.core'
import { inject } from '@angular/core'
import { CanActivateFn } from '@angular/router'

type LowCodeEnvironment = Environment & {
  lowCodeUi?: {
    runtimeUrl?: string
    designerUrl?: string
  }
}

export const lowCodeRedirectGuard: CanActivateFn = route => {
  const target = route.data['lowCodeTarget']
  if (target !== 'runtime' && target !== 'designer') {
    throw new Error('Route data lowCodeTarget must be runtime or designer.')
  }

  const environment = inject(EnvironmentService)
    .getEnvironment() as LowCodeEnvironment
  const targetUrl = target === 'runtime'
    ? environment.lowCodeUi?.runtimeUrl
    : environment.lowCodeUi?.designerUrl

  if (!targetUrl) {
    throw new Error(
      `lowCodeUi.${target === 'runtime' ? 'runtimeUrl' : 'designerUrl'} must be configured.`,
    )
  }

  window.location.assign(targetUrl)
  return false
}
```

Register two componentless routes. Angular `21` requires `children`, a component, a redirect, or a lazy loader even when a guard always leaves the application:

```ts
{
  path: 'dynamic',
  canActivate: [authGuard, lowCodeRedirectGuard],
  data: { lowCodeTarget: 'runtime' },
  children: [],
},
{
  path: 'low-code-designer',
  canActivate: [authGuard, permissionGuard, lowCodeRedirectGuard],
  data: {
    lowCodeTarget: 'designer',
    requiredPolicy: 'AbpLowCodeDesigner.Default',
  },
  children: [],
},
```

Register `Dynamic` as initially invisible. Give Designer its actual permission requirement. Then patch `Dynamic` after the authenticated user's filtered Low-Code menu has loaded:

```ts
const DYNAMIC_MENU_NAME = '::Menu:Dynamic'

routes.add([
  {
    path: '/dynamic',
    name: DYNAMIC_MENU_NAME,
    iconClass: 'fas fa-cubes',
    order: 3,
    layout: eLayoutType.application,
    invisible: true,
  },
  {
    path: '/low-code-designer',
    name: '::Menu:LowCodeDesigner',
    iconClass: 'fas fa-wand-magic-sparkles',
    order: 4,
    layout: eLayoutType.application,
    requiredPolicy: 'AbpLowCodeDesigner.Default',
  },
])

configState.getOne$('currentUser').pipe(
  map(currentUser => currentUser?.isAuthenticated === true),
  distinctUntilChanged(),
  switchMap(isAuthenticated =>
    isAuthenticated
      ? rest.request<void, DynamicMenuItemDefinition[]>(
          {
            method: 'GET',
            url: '/api/low-code/ui/menu-items',
          },
          { apiName: 'Default' },
        ).pipe(catchError(() => of([])))
      : of([]),
  ),
).subscribe(menuItems => {
  routes.patch(DYNAMIC_MENU_NAME, {
    invisible: menuItems.length === 0,
  })
})
```

Inject `ConfigStateService` as `configState` and `RestService` as `rest`; import `catchError`, `distinctUntilChanged`, `map`, `of`, and `switchMap` from RxJS. Define `DynamicMenuItemDefinition` with at least the response fields your code consumes. The request is not sent for anonymous users. An API error hides `Dynamic` rather than exposing an unverified entry, and the menu may appear asynchronously after authentication and configuration loading. The destination React runtime and Admin Console still enforce their own authorization.

When Angular is hosted on another origin, add that exact origin to `App:CorsOrigins` and seed an explicit public OIDC client with the Angular callback and post-logout URLs. Do not treat `localhost` and `127.0.0.1` as interchangeable origins.

{{else if UI=="Blazor" || UI=="BlazorServer" || UI=="BlazorWebApp"}}

### Blazor

Add both URLs to the configuration consumed by the Blazor menu contributor:

```json
{
  "LowCodeUi": {
    "RuntimeUrl": "https://localhost:44323/lowcode/",
    "DesignerUrl": "https://localhost:44323/admin-console/lowcode-designer"
  }
}
```

For Blazor Web App with interactive Auto or WebAssembly rendering, add the same section to both the server `appsettings.json` and the client `wwwroot/appsettings.json`. Otherwise, the menu can work during server rendering and fail after the client becomes interactive.

Inject `IConfiguration` into the existing `IMenuContributor`, validate the URLs, and use the same filtered-menu and Designer permission checks as MVC:

```csharp
private readonly IConfiguration _configuration;

public MyProjectMenuContributor(IConfiguration configuration)
{
    _configuration = configuration;
}

private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
{
    var l = context.GetLocalizer<MyProjectResource>();
    var runtimeUrl = _configuration["LowCodeUi:RuntimeUrl"];
    var designerUrl = _configuration["LowCodeUi:DesignerUrl"];

    if (string.IsNullOrWhiteSpace(runtimeUrl))
    {
        throw new InvalidOperationException("LowCodeUi:RuntimeUrl must be configured.");
    }

    if (string.IsNullOrWhiteSpace(designerUrl))
    {
        throw new InvalidOperationException("LowCodeUi:DesignerUrl must be configured.");
    }

    var currentUser = context.ServiceProvider.GetRequiredService<ICurrentUser>();
    if (currentUser.IsAuthenticated)
    {
        var lowCodeUi = context.ServiceProvider
            .GetRequiredService<IDynamicEntityUIAppService>();
        var visibleItems = await lowCodeUi.GetMenuItemsAsync();

        if (visibleItems.Count > 0)
        {
            context.Menu.AddItem(
                new ApplicationMenuItem(
                    "MyProject.Dynamic",
                    l["Menu:Dynamic"],
                    runtimeUrl,
                    icon: "fa fa-cubes",
                    order: 3,
                    target: "_self"
                ).RequireAuthenticated()
            );
        }
    }

    if (await context.IsGrantedAsync(
        AbpLowCodeDesignerPermissions.Designer.Default))
    {
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "MyProject.LowCodeDesigner",
                l["Menu:LowCodeDesigner"],
                designerUrl,
                icon: "fa fa-wand-magic-sparkles",
                order: 4,
                target: "_self"
            ).RequireAuthenticated()
        );
    }
}
```

The menu contributor needs the Low-Code contracts and HTTP client proxy. If Studio's backend import did not add them to the Blazor UI projects, add `Volo.Abp.LowCode.HttpApi.Client` and `Volo.Abp.LowCode.Designer.Application.Contracts`, then depend on `AbpLowCodeHttpApiClientModule` and `AbpLowCodeDesignerApplicationContractsModule`. Without the contracts module, `context.IsGrantedAsync` can fail because the Designer policy definition is unknown rather than simply returning `false`.

For a separate Blazor client project, configure the Low-Code remote service explicitly in both server and client configuration:

```json
{
  "RemoteServices": {
    "LowCode": {
      "BaseUrl": "https://localhost:44323"
    }
  }
}
```

Use the same backend and authority for the Blazor UI and Low-Code service so the proxy sends the current user's access token and the returned menu is filtered for that user. A separate authority requires an explicit token-exchange or delegated-authentication design and is outside this guide.

The verified Blazor Web App template registers the same contributor in its server and client modules. Keep that existing registration; do not register a second contributor only for Low-Code.

{{end}}

The original unconditional menu examples were visible before login because no authentication or permission rule was attached to them. The revised code has three deliberate layers: anonymous users see neither entry, `Dynamic` appears only when the filtered response contains at least one visible page or group, and Designer requires `AbpLowCodeDesigner.Default`.

`GetMenuItemsAsync()` reads Low-Code page metadata and applies the current user's generated page permissions; it does not query every row from a generated business entity table. MVC and Blazor await this call while composing the menu. Angular starts with the entry hidden and patches it after the response. A failed menu lookup therefore hides the convenience link, but it does not make a permitted page inaccessible by URL.

These checks improve navigation only. Dynamic page APIs enforce their generated runtime permissions, while Designer enforces `AbpLowCodeDesigner.Default` plus any edit or script-test permissions used by the user. Full-page navigation also discards unsaved in-memory UI state in the source MVC, Angular, or Blazor application.

## Deployment Alternatives

### Same Host and Subpath: Recommended

Serving React at `/lowcode` avoids a second origin, minimizes CORS configuration, and lets MVC plus React use the same authority and API origin. It still requires separate OIDC client IDs because their callback paths and lifecycle are different.

### Separate React Origin

Hosting React at a separate origin also works. In that case:

* add the exact React origin to backend CORS configuration
* add its exact redirect, silent renew, and logout URIs to OpenIddict
* set the MVC or Blazor `LowCodeUi:RuntimeUrl` and Angular `lowCodeUi.runtimeUrl` to that origin
* validate cookies, SameSite behavior, reverse-proxy headers, and HTTPS end to end

Do not use wildcard callback URLs.

### Iframe

An iframe is not recommended. Authentication cookies, Content Security Policy, `X-Frame-Options`, focus handling, downloads, and full-page designer/runtime navigation are harder to make reliable. Prefer a normal same-tab navigation.

## Permissions, Tenancy, and Culture

* Designer access is controlled by `AbpLowCodeDesigner.*` permissions.
* Generated pages and endpoints must enforce their generated runtime permissions independently.
* The companion React client must request the existing backend API scope.
* In a multi-tenant solution, test host and tenant login, tenant switching, and logout separately.
* Preserve the application's normal culture and UI-culture handling through the OIDC flow.
* Never hide a menu item as the only authorization check.

## Validation Checklist

Run the backend checks:

```powershell
dotnet build Acme.NonReactLowCode.slnx

dotnet run `
  --project src\Acme.NonReactLowCode.Web\Acme.NonReactLowCode.Web.csproj `
  --no-build -- --check-lowcode-model-files

Push-Location src\Acme.NonReactLowCode.DbMigrator
dotnet run --no-build
Pop-Location
```

Running from the DbMigrator project directory ensures a project-local `appsettings.secrets.json` is discovered. If a commercial module exits with `ABP-LIC-0020`, verify the working directory and secret configuration without printing or moving the license into source-controlled settings.

Build the companion React runtime for every UI choice:

```powershell
cd lowcode-react
yarn install --frozen-lockfile
yarn lint
yarn test:run
yarn build
```

{{if UI=="NG"}}

Build Angular separately:

```powershell
cd ..\angular
yarn build
```

{{else if UI=="MVC" || UI=="Blazor" || UI=="BlazorServer" || UI=="BlazorWebApp"}}

The MVC or Blazor UI is already included in the preceding .NET solution build.

{{end}}

Verify these behaviors in a browser:

1. `/` still opens the existing UI.
2. Anonymous MVC, Angular, and Blazor users see neither `Dynamic` nor `Low-Code Designer`.
3. An authenticated user with no visible generated page does not see `Dynamic`.
4. A user with at least one visible page sees `Dynamic`; a user with `AbpLowCodeDesigner.Default` sees `Low-Code Designer`.
5. `/admin-console/api/modules` returns `true` only for the explicitly allowed modules.
6. `/admin-console/lowcode-designer` opens for an authorized administrator, while an omitted module route shows the Admin Console `404` page.
7. Anonymous `/lowcode/` shows no dynamic menu and does not request `/api/low-code/ui/menu-items`.
8. Opening `/lowcode/dynamic/<page-name>` anonymously starts login and returns to that exact deep link after authentication.
9. Refreshing the dynamic deep link returns the SPA HTML with `Cache-Control: no-store, no-cache, must-revalidate` and does not return `404`.
10. The signed-in runtime shows only the filtered dynamic menu tree and can render the permitted page.
11. **Back to the application** navigates from `/lowcode/` to the configured existing application URL.
12. `Dynamic` and `Low-Code Designer` use full-page navigation to their React applications.
13. A user without a generated page permission cannot open that page or its data API directly.
14. A Designer create/update mutation sends `RequestVerificationToken` and succeeds; a runtime record create/update also succeeds without a `302` or `/Error?httpStatusCode=400` redirect.
15. The companion `Low-Code Designer` link is controlled by `AbpLowCodeDesigner.Default`, not a synthetic Admin Console permission.
16. Admin Console's module discovery response enables only `lowCodeDesigner`; its static home cards may still advertise hidden modules whose direct routes return `404`.

## Troubleshooting

| Symptom | Cause and correction |
|---------|----------------------|
| Designer route is missing | Add `Volo.Abp.AdminConsole`, `AbpAdminConsoleModule`, and the `Volo.Abp.LowCode.Designer.*` layer packages. |
| Designer returns `403` | Re-run data seeding and verify `AbpLowCodeDesigner.Default` for the role. Restart and sign in again after changing grants. |
| Hidden Admin Console modules reappear after an ABP upgrade | Compare the replacement controller's keys with the upgraded discovery controller and React route configuration. Do not add unknown keys without checking their route and permission behavior. |
| Admin Console home links to hidden modules | The built-in home content is static. Link users directly to `/admin-console/lowcode-designer`; use a custom Admin Console frontend if the root page must also be redesigned. |
| Companion React does not show its Designer link for admin | Gate the link with `AbpLowCodeDesigner.Default`. `AdminConsole.Access` is not a built-in permission. |
| `/lowcode/` loads but assets return `404` | Set Vite `base` to `/lowcode/` and rebuild. |
| `/lowcode/` becomes blank after deploying a new build | The browser may have cached fallback HTML that points to deleted hashed assets. Send the documented no-cache headers for fallback `index.html`; keep normal hashed-asset caching. |
| A Low-Code create/update/delete request becomes `302`, then `/Error?httpStatusCode=400` | The endpoint exists, but ABP rejected the mutation because `RequestVerificationToken` was missing. Configure Axios with `XSRF-TOKEN` and `RequestVerificationToken`; do not disable antiforgery globally. |
| React navigation works until browser refresh | Add the scoped `MapFallbackToFile` route. |
| OIDC says the redirect URI is invalid | Seed the trailing-slash callback and `silent-renew.html` callback for the dedicated runtime client. |
| Login from a dynamic deep link returns to `/lowcode/` | Store `window.location.href`, validate it against the runtime origin and base path after callback, and create the router only after callback handling. |
| Menu entries are visible before login | Add the authentication and permission rules shown in Step 4. An external URL alone has no implicit authorization requirement. |
| `Dynamic` is visible although the user has no visible page | Do not add it unconditionally. Use the filtered `/api/low-code/ui/menu-items` response and hide the entry when the root list is empty. |
| Angular fails with `NG04014` for `dynamic` | Add `children: []` or a redirect component to the componentless guard route. |
| Angular API calls fail only from `127.0.0.1` | Use the configured `localhost` origin or add the exact origin to CORS; origins are not interchangeable. |
| Angular `Dynamic` remains hidden after login | Verify the `Default` API URL, access token, menu-items request, and the `RoutesService.patch` name. The fail-closed example intentionally remains hidden on request failure. |
| Blazor menu disappears after the page becomes interactive | In Blazor Web App interactive Auto or WebAssembly mode, add the same `LowCodeUi` section to the client `wwwroot/appsettings.json` as well as the server configuration. |
| Blazor throws `No policy found` for Designer | Add the Designer application contracts package and `AbpLowCodeDesignerApplicationContractsModule` to the UI host/client that composes the menu. |
| Generated React seed fails to type-check test imports | Inspect its Testing Library peer dependencies. Add a compatible `@testing-library/dom` only when missing; the historical `10.5.0` seed required `10.4.1`, while the generated `10.6.x` application may already include it. |
| Companion title or menu still shows the temporary project name or `Menu:LowCodeDesigner` | Rename `index.html`, package/application identity, and the copied `src/locales/*.json` resource. Confirm that the localization key exists in the resource loaded by React, not only in the existing UI's resource. |
| Production build passes but companion tests fail after moving to `/lowcode` | Update root-hosted guard, redirect, OIDC-return, and removed-screen expectations for the subpath. Run lint and tests in addition to the build. |
| DbMigrator exits with `ABP-LIC-0020` although a secrets file exists | Run it from the project directory containing `appsettings.secrets.json`, or use the deployment's supported secret provider. Never commit or print the license value. |

## See Also

* [Add Low-Code to an Existing Solution](add-to-existing-solution.md)
* [React Runtime](react-runtime.md)
* [Designer](designer.md)
