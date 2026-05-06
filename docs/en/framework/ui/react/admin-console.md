```json
//[doc-seo]
{
    "Description": "Learn how the ABP Admin Console works with React UI applications and how it is hosted under /admin-console."
}
```

# Admin Console

The **ABP Admin Console** is the React-based administration UI for ABP applications. It provides management pages for ABP modules and is available in React UI solutions created with ABP Studio v3.0+ or `abp new --modern --ui-framework react`.

The Admin Console is delivered as the `Volo.Abp.AdminConsole` NuGet package for layered and single-layer solutions. In microservice solutions, the template also includes a standalone `apps/react-admin-console/` React app.

## What It Provides

The Admin Console contains administration pages for the ABP modules included in the host application. Module pages are activated based on the backend services available in the host, so a solution only shows pages for modules it actually has.

The built-in module areas include:

| Module | Notes |
| --- | --- |
| Identity Pro | User, role, claim, and organization unit management when Identity services are available. |
| Account Pro | Account management pages and account-related flows. |
| OpenIddict | Application and scope management when OpenIddict services are available. |
| Audit Logging UI | Optional. Visible when Audit Logging services are available. |
| AI Management | Optional. Visible when AI Management services are available. |
| Text Template Management | Optional. Visible when Text Template Management services are available. |

Other module pages, such as Setting Management, SaaS, GDPR, or customization pages, can also be available depending on the solution and installed modules.

## Hosting Model

The Admin Console is served under:

```text
/admin-console/*
```

API endpoints used by the Admin Console are served under:

```text
/admin-console/api/*
```

The `Volo.Abp.AdminConsole` package embeds the built React SPA under `wwwroot/admin-console/` and registers it with ABP's Virtual File System. `AdminConsoleSpaMiddleware` then serves static assets and falls back to `index.html` for client-side routes.

The middleware deliberately lets `/admin-console/api/*` requests pass through to MVC controllers.

## Layered and Single-Layer Templates

For layered and single-layer modern templates:

- The developer-owned React app is in the `react/` folder.
- The Admin Console UI is embedded in the backend through the `Volo.Abp.AdminConsole` NuGet package.
- There is no separate `react-admin-console/` source folder in the generated solution.
- The backend host serves Admin Console pages under `/admin-console/*`.

Example URL:

```text
https://localhost:44300/admin-console/
```

The main React app links to the Admin Console through `getAdminConsoleUrl()`.

## Microservice Template

For the microservice modern template:

- The main React app is in `apps/react/`.
- The Admin Console app is in `apps/react-admin-console/`.
- Both are served through the Web Gateway.
- The Admin Console has its own OpenIddict client, normally `<ProjectName>_AdminConsole`.

The main React app uses `adminConsoleUrl` from `dynamic-env.json` to open the Admin Console origin and `/admin-console` base path.

## Module Discovery

The Admin Console calls:

```text
GET /admin-console/api/modules
```

The backend checks for module application service contracts and returns which module areas are available. The discovery keys include:

| Key | Backend service check |
| --- | --- |
| `identity` | `Volo.Abp.Identity.IIdentityUserAppService` |
| `saas` | `Volo.Saas.Host.ITenantAppService` |
| `auditLogging` | `Volo.Abp.AuditLogging.IAuditLogsAppService` |
| `gdpr` | `Volo.Abp.Gdpr.IGdprRequestAppService` |
| `openIddict` | `Volo.Abp.OpenIddict.Applications.IApplicationAppService` |
| `textTemplateManagement` | `Volo.Abp.TextTemplateManagement.TextTemplates.ITemplateDefinitionAppService` |
| `aiManagement` | AI Management service contracts, with a legacy AI engine fallback. |

`settingManagement` is always returned as available by the discovery endpoint, while access to pages is still controlled by permissions.

## Configuration Endpoint

The Admin Console also uses:

```text
GET /admin-console/api/config
```

This endpoint provides Admin Console runtime settings such as authority, client ID, scopes, application name, customization options, and localization language configuration.

Host applications can configure Admin Console options from the `AdminConsole` configuration section.

## Permissions

Admin Console routes still require permissions. For example:

- Identity pages use `AbpIdentity.*` permissions.
- OpenIddict pages use `OpenIddictPro.Application` and `OpenIddictPro.Scope`.
- Audit Logging uses `AuditLogging.AuditLogs`.
- Text Template Management uses `TextTemplateManagement.*`.
- AI Management uses `AIManagement.*`.

The main React app's Admin Console menu item only requires authentication. The Admin Console performs detailed permission checks for its own pages.

## Customization

The developer-owned React app is intended for application-specific pages. The Admin Console is an ABP-managed administration surface and should normally be updated by updating ABP packages.

For layered and single-layer hosts, the package supports host-side options such as application name, localization languages, and theme override CSS path. For larger UI changes, prefer building your own pages in the main React app or extending the backend modules through supported ABP extension points.

## See Also

- [React UI](./index.md)
- [Environment Variables](./environment-variables.md)
- [Permission Management](./permission-management.md)
