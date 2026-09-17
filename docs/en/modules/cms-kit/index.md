```json
//[doc-seo]
{
    "Description": "Explore the CMS Kit Module, offering essential tools for building dynamic websites with page management, blogging, and tagging features."
}
```

# CMS Kit Module

This module provides CMS (Content Management System) capabilities for your application. It provides **core building blocks** and fully working **sub-systems** to create your own website with CMS features enabled, or use the building blocks in your web sites with any purpose.

> You can see the live demo at [cms-kit-demo.abpdemo.com](https://cms-kit-demo.abpdemo.com/).

> CMS Kit provides MVC / Razor Pages packages for both the administration and public websites. The `@abp/ng.cms-kit` package provides an Angular administration UI. There is no official Blazor package; a Blazor Server application can host the MVC / Razor Pages UI because it runs on ASP.NET Core.

The following features are currently available:

* Provides a [**page**](./pages.md) management system to manage dynamic pages with dynamic URLs.
* Provides a [**blogging**](./blogging.md) system to create publish blog posts with multiple blog support.
* Provides a [**tagging**](./tags.md) system to tag any kind of resource, like a blog post.
* Provides a [**comment**](./comments.md) system to add comments feature to any kind of resource, like blog post or a product review page.
* Provides a [**reaction**](./reactions.md) system to add reactions (smileys) feature to any kind of resource, like a blog post or a comment.
* Provides a [**rating**](./ratings.md) system to add rating feature to any kind of resource.
* Provides a [**menu**](./menus.md) system to manage public menus dynamically.
* Provides a [**global resources**](./global-resources.md) system to add global styles and scripts dynamically.
* Provides a [**Dynamic Widget**](./dynamic-widget.md) system to create dynamic widgets for page and blog posts.
* Provides a [**Marked Item**](./marked-items.md) system to mark any kind of resource, like a blog post or a product, as a favorite, starred, flagged, or bookmarked.

> You can click on the any feature links above to understand and learn how to use it.

CMS Kit uses two feature layers:

* [Global Features](../../framework/infrastructure/global-features.md) select the CMS Kit subsystems included in the application model. When using Entity Framework Core, changing these features requires a new migration because disabled entities are excluded from the EF Core model.
* The [Feature System](../../framework/infrastructure/features.md) can enable or disable the corresponding subsystem at runtime for a tenant or another feature value provider. Runtime feature changes do not change the database model.

Most subsystems can be selected independently. The built-in MVC blog pages are currently registered together with the Pages global feature, so enable both `Blogs` and `Pages` when you use the built-in public blog UI.

## Pre Requirements

-  This module depends on [BlobStoring](../../framework/infrastructure/blob-storing) module for keeping media content.
> Make sure `BlobStoring` module is installed and at least one provider is configured properly. For more information, check the [documentation](../../framework/infrastructure/blob-storing).

- CMS Kit uses [distributed cache](../../framework/fundamentals/caching.md) for responding faster. 
> Using a distributed cache, such as [Redis](../../framework/fundamentals/redis-cache.md), is highly recommended for data consistency in distributed/clustered deployments.

## Media Storage and Entity Types

When the Media global feature is enabled, CMS Kit registers media definitions for blog posts and pages. To upload media for another entity type, register a `MediaDescriptorDefinition` and specify the permissions that can create and delete its media:

```csharp
Configure<CmsKitMediaOptions>(options =>
{
    options.EntityTypes.Add(
        new MediaDescriptorDefinition(
            "Product",
            createPolicies: new[] { "Products.Update" },
            deletePolicies: new[] { "Products.Update" }));
});
```

The administration service grants an operation when the current user has any policy in the corresponding list. An empty list grants no access. Media files are downloaded from the anonymous `GET /api/cms-kit/media/{id}` endpoint, so this facility is for public media; use a separately authorized BLOB endpoint for private files.

## Identity Integration for User Lookup

CMS Kit uses `ICmsUserLookupService` when it needs user information for features such as comments, ratings, blog post management and user synchronization.

If the CMS Kit and Identity modules run in the same application, no extra configuration is typically needed.

If the Identity module runs in another application or service (for example, in a tiered or distributed solution), CMS Kit may need to call the Identity integration endpoints remotely to create or update `CmsUser` records.

In that case, the calling application should:

* Depend on `Volo.Abp.Identity.HttpApi.Client`.
* Add `AbpIdentityHttpApiClientModule` and an IdentityModel module (`AbpHttpClientIdentityModelWebModule` for web applications or `AbpHttpClientIdentityModelModule` for non-web hosts).
* Configure `RemoteServices:AbpIdentity` to point to the application that hosts the Identity module.
* Configure `IdentityClients` for client credentials flow so the lookup can use server-to-server authentication.
* Expose integration services on the application that hosts the Identity module.

Example module dependency for a web application:

```csharp
[DependsOn(
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelWebModule)
)]
public class MyWebModule : AbpModule
{
}
```

Example `appsettings.json` configuration:

```json
"RemoteServices": {
  "AbpIdentity": {
    "BaseUrl": "https://localhost:44388/",
    "UseCurrentAccessToken": false
  }
},
"IdentityClients": {
  "Default": {
    "GrantType": "client_credentials",
    "ClientId": "MyProject_Web",
    "ClientSecret": "your-client-secret",
    "Authority": "https://localhost:44322/",
    "Scope": "your-internal-api-scope"
  }
}
```

See the [Identity module's External User Lookup Service section](../identity.md#external-user-lookup-service) and the [Synchronous Interservice Communication guide](../../guides/synchronous-interservice-communication.md) for the complete setup.

## How to Install

[ABP CLI](../../cli) allows installing a module to a solution using the `add-module` command. You can install the CMS Kit module in a command-line terminal with the following command:

```bash
abp add-module Volo.CmsKit --skip-db-migrations
```

> By default, Cms-Kit is disabled by `GlobalFeature`. Because of that the initial migration will be empty. So you can skip the migration by adding `--skip-db-migrations` to command when installing if you are using Entity Framework Core. After enabling Cms-Kit global feture, please add new migration.

After the installation process, open the `GlobalFeatureConfigurator` class in the `Domain.Shared` project of your solution and place the following code into the `Configure` method to enable all the features in the CMS Kit module.

```csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.EnableAll();
});
```

Instead of enabling all, you may prefer to enable the features one by one. The following example enables only the [tags](./tags.md) and [comments](./comments.md) features:

````csharp
GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
{
    cmsKit.Tags.Enable();
    cmsKit.Comments.Enable();
});
````

> If you are using Entity Framework Core, do not forget to add a new migration and update your database.

## The Packages

This module follows the [module development best practices guide](../../framework/architecture/best-practices) and consists of several NuGet and NPM packages. See the guide if you want to understand the packages and relations between them.

CMS kit packages are designed for various usage scenarios. If you check the [CMS kit packages](https://www.nuget.org/packages?q=Volo.CmsKit), you will see that some packages have `Admin` and `Public` suffixes. The reason is that the module has two application layers, considering they might be used in different type of applications. These application layers uses a single domain layer:

 - `Volo.CmsKit.Admin.*` packages contain the functionalities required by admin (back office) applications.
 - `Volo.CmsKit.Public.*` packages contain the functionalities used in public websites where users read blog posts or leave comments.
 - `Volo.CmsKit.*` (without Admin/Public suffix) packages are called as unified packages. Unified packages are shortcuts for adding Admin & Public packages (of the related layer) separately. If you have a single application for administration and public web site, you can use these packages.

### Angular Administration UI

The `@abp/ng.cms-kit` package contains the Angular administration components, routes, configuration providers and generated proxies. Register the administration menu configuration in the application configuration and lazy-load the administration routes:

```typescript
import { ApplicationConfig } from '@angular/core';
import { Routes } from '@angular/router';
import { provideCmsKitAdminConfig } from '@abp/ng.cms-kit/admin/config';

export const appConfig: ApplicationConfig = {
  providers: [provideCmsKitAdminConfig()],
};

export const routes: Routes = [
  {
    path: 'cms',
    loadChildren: () => import('@abp/ng.cms-kit/admin').then(m => m.createRoutes()),
  },
];
```

The Angular administration routes include comments, tags, pages, blogs, blog posts, menus and global resources. The built-in public page and blog UI documented in this guide uses the MVC / Razor Pages packages.

`createRoutes` accepts a `CmsKitAdminConfigOptions` object for Angular UI extensions. It supports entity-action, entity-property, toolbar-action, create-form-property and edit-form-property contributors. Key the contributor dictionaries with `eCmsKitAdminComponents`; the supported screens cover comment lists/details, tags, pages and page forms, blogs, blog posts and blog post forms, and menus. Each contributor type exposes only the keys supported by that screen.

## Integrating Public and Admin Packages in a Unified Application

If you are using a single application for both admin and public web site, it's important to configure the global layout settings appropriately. By default, the layout is set for a **Public Website**, which is suitable for public-facing pages. However, when your application serves both admin and public pages, you should explicitly set the global layout for all CMS Kit pages.

To do this, add a `_ViewStart.cshtml` file to your web project at `/Pages/Public/CmsKit/_ViewStart.cshtml` and configure the layout as shown below:

```html
@using Volo.Abp.AspNetCore.Mvc.UI.Theming
@inject IThemeManager ThemeManager
@{
    // default: GetPublicLayout()
    Layout = ThemeManager.CurrentTheme.GetApplicationLayout(); 
}
```

> The `_ViewStart.cshtml` file is used to set the layout for all pages in the `CmsKit` folder.

## Internals

### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `AbpCmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

### Connection string

This module uses `CmsKit` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string.

See the [connection strings](../../framework/fundamentals/connection-strings.md) documentation for details.

## Entity Extensions

[Module entity extension](../../framework/architecture/modularity/extending/module-entity-extensions.md) system is a **high-level** extension system that allows you to **define new properties** for existing entities of the dependent modules. It automatically **adds properties to the entity**, **database**, **HTTP API, and user interface** in a single point.

To extend entities of the CMS Kit module, open your `YourProjectNameModuleExtensionConfigurator` class inside of your `DomainShared` project and change the `ConfigureExtraProperties` method like shown below.

```csharp
public static void ConfigureExtraProperties()
{
    OneTimeRunner.Run(() =>
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureCmsKit(cmsKit =>
            {
                cmsKit.ConfigureBlog(plan => // extend the Blog entity
                {
                    plan.AddOrUpdateProperty<string>( //property type: string
                      "BlogDescription", //property name
                      property => {
                        //validation rules
                        property.Attributes.Add(new RequiredAttribute()); //adds required attribute to the defined property

                        //...other configurations for this property
                      }
                    );
                });
              
                cmsKit.ConfigureBlogPost(blogPost => // extend the BlogPost entity
                    {
                        blogPost.AddOrUpdateProperty<string>( //property type: string
                        "BlogPostDescription", //property name
                        property => {
                            //validation rules
                            property.Attributes.Add(new RequiredAttribute()); //adds required attribute to the defined property
                            property.Attributes.Add(
                            new StringLengthAttribute(MyConsts.MaximumDescriptionLength) {
                                MinimumLength = MyConsts.MinimumDescriptionLength
                            }
                            );

                            //...other configurations for this property
                        }
                        );
                });  
            });
    });
}
```
 
* `ConfigureCmsKit(...)` method is used to configure the entities of the CMS Kit module.

* CMS Kit provides configuration methods for `Blog`, `BlogPost`, `BlogFeature`, `MediaDescriptor`, `Page`, `Tag`, `Comment`, `MenuItem`, `CmsUser` and `GlobalResource`.

* The built-in MVC create and update forms consume extensions for blogs, blog posts, menu items, pages and tags. The Angular administration UI consumes object extensions and contributor callbacks for its comments, tags, pages, blogs, blog posts and menu screens.

* You can also set some validation rules for the property that you defined. In the above sample, `RequiredAttribute` and `StringLengthAttribute` were added for the property named **"BlogPostDescription"**. 

* Each helper exposes the module entity extension configuration for that type. Persistence and DTO propagation depend on the mappings registered by the installed CMS Kit packages. Automatic form and table rendering is available only on the MVC and Angular screens listed above. For other entities or custom screens, read the extra property from the DTO and render it explicitly.
