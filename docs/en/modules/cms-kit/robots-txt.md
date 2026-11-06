```json
//[doc-seo]
{
    "Description": "Learn how to manage a multi-tenant robots.txt dynamically with the CMS Kit, with a host default and per-tenant overrides."
}
```

# CMS Kit: Robots.txt

CMS Kit Robots.txt system allows to manage the content of the `/robots.txt` file of your site dynamically. The host defines the default content for all tenants and each tenant can override it.

## Enabling the Robots.txt Feature

By default, CMS Kit features are disabled. Therefore, you need to enable the features you want, before starting to use it. You can use the [Global Feature](../../framework/infrastructure/global-features.md) system to enable/disable CMS Kit features on development time. Alternatively, you can use the ABP's [Feature System](../../framework/infrastructure/features.md) to disable a CMS Kit feature on runtime.

> Check the ["How to Install" section of the CMS Kit Module documentation](index.md#how-to-install) to see how to enable/disable CMS Kit features on development time.

````csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.RobotsTxt.Enable();
});
````

> The Robots.txt feature adds a new database table. Add a new migration and update your database after enabling it.

The runtime feature is `CmsKit.RobotsTxtEnable` and the permission to manage the content is `CmsKit.RobotsTxt`.

## The User Interface

### Menu items

CMS Kit module admin side adds the following items to the main menu, under the *CMS* menu item:

* **Robots.txt**: Robots.txt management page.

`CmsKitAdminMenus` class has the constants for the menu item names.

### Robots.txt Page

Robots.txt page is used to edit the robots.txt content of the current tenant (or the host). It shows where the content comes from:

* **Custom**: The current tenant (or the host) has its own content.
* **Inherited from host**: The current tenant has no content and uses the host's content.
* **Default**: Neither the tenant nor the host has content. Each application that serves the `/robots.txt` endpoint uses its own default content (see below).

The *Reset* button deletes the custom content, so the inherited or the default content is used again.

## Serving the robots.txt

The `/robots.txt` endpoint is not a part of the UI. It is mapped by the `CmsKitPublicHttpApiModule` (the `Volo.CmsKit.Public.HttpApi` package), so it works with any UI framework. The application that serves your public website domain should depend on this module (directly, or through your application's `HttpApi` module). It returns the first available content in the following order:

1. The content of the current tenant.
2. The content of the host.
3. The static `robots.txt` file in the `wwwroot` folder of the application, if it exists.
4. The `CmsKitRobotsTxtOptions.DefaultContent` value, `User-agent: *` and `Allow: /` by default.

The endpoint is mapped with the highest matching order, so it takes priority over a static `wwwroot/robots.txt` file that is served by the `MapAbpStaticAssets()` endpoints. The static file becomes the default content. If the `CmsKit.RobotsTxtEnable` feature is disabled for the current tenant, the endpoint returns the static file (or `404 Not Found` if there is no static file). If the global feature is disabled, the endpoint is not mapped at all.

> Applications that serve static files with the `app.UseStaticFiles()` middleware before the routing middleware serve the static file directly. Remove the static `wwwroot/robots.txt` file in that case.

> Search engine crawlers don't send a tenant header, so the tenant should be resolved from the domain name to serve a different robots.txt per tenant. See the [domain tenant resolver](../../framework/architecture/multi-tenancy/index.md#domainsubdomain-tenant-resolver).

### Applications with multiple hosts

The HTTP API layer is usually loaded by more than one application of a solution, for example a public website, an admin portal and an API host. All of them map the `/robots.txt` endpoint by default. Disable it in the applications that should not serve the managed content (an admin portal can then serve its own static `robots.txt` with `Disallow: /`):

````csharp
Configure<CmsKitRobotsTxtOptions>(options =>
{
    options.IsEndpointEnabled = false;
});
````

If your public website is a single-page application (like Angular or React) that is served as static files, there is no ASP.NET Core application on the website domain. In that case, forward the `/robots.txt` requests of the website domain to the API host in your web server or reverse proxy configuration, and resolve the tenant from the domain name.

### Options

`CmsKitRobotsTxtOptions` can be configured in the `ConfigureServices` method of your module:

````csharp
Configure<CmsKitRobotsTxtOptions>(options =>
{
    options.DefaultContent = "User-agent: *\nDisallow: /";
});
````

* `IsEndpointEnabled`: Maps the `/robots.txt` endpoint in this application. Default: `true`.
* `DefaultContent`: The content served when there is no stored content and no static file. Default: `User-agent: *\nAllow: /`.
* `StaticFileName`: The file name in the web root folder that is used as the default content, if exists. Default: `robots.txt`.

The maximum length of the content is defined by `RobotsTxtConsts.MaxContentLength`, which is 500 KiB by default (the maximum size Google parses for a robots.txt file).

# Internals

## Domain Layer

#### Aggregates

This module follows the [Entity Best Practices & Conventions](../../framework/architecture/best-practices/entities.md) guide.

- `RobotsTxt` (aggregate root): Stores the robots.txt content of a tenant (or the host).

#### Repositories

This module follows the [Repository Best Practices & Conventions](../../framework/architecture/best-practices/repositories.md) guide.

Following custom repositories are defined for this feature:

- `IRobotsTxtRepository`

#### Domain services

This module follows the [Domain Services Best Practices & Conventions](../../framework/architecture/best-practices/domain-services.md) guide.

##### Robots.txt Manager

`RobotsTxtManager` is used to perform operations for the `RobotsTxt` aggregate root. It resolves the host fallback and caches the content of each tenant in the distributed cache. The cache is invalidated by a local entity event when the content is created, updated or deleted.

### Application layer

#### Application services

- `RobotsTxtAdminAppService` (implements `IRobotsTxtAdminAppService`): Implements the management operations of the robots.txt system.
- `RobotsTxtPublicAppService` (implements `IRobotsTxtPublicAppService`): Implements the public use cases of the robots.txt system.

#### Database

#### Entity Framework Core

##### Tables

- CmsRobotsTxts

#### MongoDB

##### Collections

- CmsRobotsTxts
