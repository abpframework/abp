# CMS Kit Module

This module provides CMS (Content Management System) capabilities for your application. It provides **core building blocks** and fully working **sub-systems** to create your own website with CMS features enabled, or use the building blocks in your web sites with any purpose.

> You can see the live demo at [cms-kit-demo.abpdemo.com](https://cms-kit-demo.abpdemo.com/).

> **This module currently available only for the MVC / Razor Pages UI**. While there is no official Blazor package, it can also work in a Blazor Server UI since a Blazor Server UI is actually a hybrid application that runs in an ASP.NET Core MVC / Razor Pages application.

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

All features are individually usable. If you disable a feature, it completely disappears from your application, even from the database tables, with the help of the [Global Features](../../framework/infrastructure/global-features.md) system.

## Pre Requirements

-  This module depends on [BlobStoring](../../framework/infrastructure/blob-storing) module for keeping media content.
> Make sure `BlobStoring` module is installed and at least one provider is configured properly. For more information, check the [documentation](../../framework/infrastructure/blob-storing).

- CMS Kit uses [distributed cache](../../framework/fundamentals/caching.md) for responding faster. 
> Using a distributed cache, such as [Redis](../../framework/fundamentals/redis-cache.md), is highly recommended for data consistency in distributed/clustered deployments.

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

## Internals

### Table / collection prefix & schema

All tables/collections use the `Cms` prefix by default. Set static properties on the `CmsKitDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

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

* `cmsKit.ConfigureBlog(...)` is used to configure the **Blog** entity of the CMS Kit module. You can add or update your extra properties on the **Blog** entity. 

* `cmsKit.ConfigureBlogPost(...)` is used to configure the **BlogPost** entity of the CMS Kit module. You can add or update your extra properties of the **BlogPost** entity.

* You can also set some validation rules for the property that you defined. In the above sample, `RequiredAttribute` and `StringLengthAttribute` were added for the property named **"BlogPostDescription"**. 

* When you define the new property, it will automatically add to **Entity**, **HTTP API**, and **UI** for you. 
  * Once you define a property, it appears in the create and update forms of the related entity. 
  * New properties also appear in the datatable of the related page.

