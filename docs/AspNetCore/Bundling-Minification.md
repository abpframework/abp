## ASP.NET Core MVC Bundling & Minification

There are many ways of bundling & minification of client side resources (JavaScript and CSS files). Most common ways are:

* Using the [Bundler & Minifier](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier) Visual Studio extension or the [nuget package](https://www.nuget.org/packages/BuildBundlerMinifier/).
* Using [Gulp](https://gulpjs.com/)/[Grunt](https://gruntjs.com/) task managers and their plugins.

ABP offers a simpler, dynamic, powerful, modular and built-in way.

### Volo.Abp.AspNetCore.Mvc.UI.Bundling Package

> This package is already installed by default with the startup templates. So, most of the time, you don't need to install it manually.

Install the `Volo.Abp.AspNetCore.Mvc.UI.Bundling` nuget package to your project:

````
install-package Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

Then you can add the `AbpAspNetCoreMvcUiBundlingModule` dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
    public class MyWebModule : AbpModule
    {
        //...
    }
}
````

### Razor Bundling Tag Helpers

The simplest way of creating a bundle is to use `abp-script-bundle` or `abp-style-bundle` tag helpers. Example:

````html
<abp-style-bundle name="MyGlobalBundle">
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    <abp-style src="/styles/my-global-style.css" />
</abp-style-bundle>
````

This bundle defines a style bundle with a **unique name**: *MyGlobalBundle*. It's very easy to *understand* how to use it. Let's see how it *works*:

* ABP creates the bundle as **lazy** from the provided files when it's **first requested**. For the subsequent calls, it's returned from the **cache**. That means if you conditionally add files to the bundle, it's executed only once and any change of the condition will not effect the bundle for the next requests.
* ABP adds bundle files **individually** to the page for the `development` environment. It automatically bundles & minifies for other environments (`staging`, `production`...).
* The bundle files may be **physical** files or [**virtual/embedded** files](../Virtual-File-System.md).
* ABP automatically adds **version query string** (like ?_v=67872834243042 - generated from last change date of the related files) to the bundle file URL which prevents browser caching when a bundle changes. The versioning works even if the bundle files are individually added to the page (on the development environment).

#### Importing The Bundling Tag Helpers

> This is already imported by default with the startup templates. So, most of the time, you don't need to add it manually.

In order to use bundle tag helpers, you need to add it into your `_ViewImports.cshtml` file or into your page:

````
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

#### Unnamed Bundles

The `name` is **optional** for the razor bundle tag helpers. If you don't define a name, it's automatically **calculated** based on the used bundle file names (they are **concatenated** and **hashed**). That means you can conditionally add items to the bundle. Example:

````html
<abp-style-bundle>
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    @if (ViewBag.IncludeCustomStyles != false)
    {
        <abp-style src="/styles/my-global-style.css" />
    }
</abp-style-bundle>
````

This will potentially create two different bundles (one incudes the `my-global-style.css` and other does not).

Advantages of **unnamed** bundles:

* Can **conditionally** add items to the bundle. But this may lead to multiple variations of the bundle based on the conditions used.

Advantages of **named** bundles:

* **A little more performant** since it does not need to calculate a dynamic bundle name.
* Other modules can **contribute** to the bundle by its name.

#### Single File

If you need to just add a single file to the page, you can use the `abp-script` or `abp-style` tag without a surrounding `abp-script-bundle` or `abp-style-bundle` tag. Example:

````xml
<abp-script src="/scripts/my-script.js" />
````

The bundle name will be *scripts.my-scripts* for the example above (/ is replaced by .).

### Bundling Options

If you need to use same bundle in **multiple pages** or want to use some more **powerful features**, you can configure bundles **by code** in your [module](../Module-Development-Basics.md) class.

#### Creating A New Bundle

Example usage:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
public class MyWebModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<BundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Add("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/libs/jquery/jquery.js",
                        "/libs/bootstrap/js/bootstrap.js",
                        "/libs/toastr/toastr.min.js",
                        "/scripts/my-global-scripts.css"
                    );
                });                
        });
    }
}
````

> You can use the same name (*MyGlobalBundle* here) for a script & style bundle since they are added to different collections (`ScriptBundles` and `StyleBundles`).

After defining such a bundle, it can be included into a page using the same tag helpers defined above. Example:

````html
<abp-script-bundle name="MyGlobalBundle" />
````

This time, no file defined in the tag helper definition because the bundle files are defined by the code.

#### Configuring An Existing Bundle

ABP supports [modularity](../Module-Development-Basics.md) for bundling too. A module can modify an existing bundle that is created by a depended module. Example:

````C#
[DependsOn(typeof(MyWebModule))]
public class MyWebExtensionModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.Configure<BundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Configure("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/scripts/my-extension-script.js"
                    );
                });                
        });
    }
}
````

### Bundle Contributors

Adding files to an existing bundle seems useful. What if you need to **replace** a file in the bundle or you want to **conditionally** add files? Defining a bundle contributor provides extra power for such cases.

An example bundle contributor that replaces bootstrap.css with a customized version:

````C#
public class MyExtensionGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.ReplaceOne(
            "/libs/bootstrap/css/bootstrap.css",
            "/styles/extensions/bootstrap-customized.css"
        );
    }
}
````

Then you can use this contributor like that:

````C#
services.Configure<BundlingOptions>(options =>
{
    options
        .ScriptBundles
        .Get("MyGlobalBundle")
        .AddContributors(typeof(MyExtensionStyleBundleContributor));
});
````

#### Contributor Dependencies

A bundle contributor can have one or more dependencies to other contributors. Example:

````C#
[DependsOn(typeof(MyDependedBundleContributor))] //Define the dependency
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

When a bundle contributor is added, its dependencies are **automatically and recursively** added. Dependencies added by the **dependency order** by preventing **duplicates**.

Creating contributors and defining dependencies is a way of organizing bundle creation across different modules.

#### Accessing to the IServiceProvider

While it is rarely needed,`BundleConfigurationContext` has a `ServiceProvider` property that you can resolve service dependencies inside the `ConfigureBundle` method.

#### Built-In Package Contributors

Adding a NPM package resources (js, css files) into a bundle is pretty standard. For instance you always add the same `bootstrap.css` file for the bootstrap NPM package.

There are built-in contributors for all [standard NPM packages](Client-Side-Package-Management.md). For instance, if your contributor depends on the bootstrap, you can just declare it instead of adding the bootstrap.css yourself:

````C#
[DependsOn(typeof(BootstrapStyleContributor))] //Define the bootstrap style dependency
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

Using the built-in contributors for standard packages;

* Prevents you **wrongly typing** the resource paths.
* Prevents changing your contributor if the resource **path changes** (the depended contributor will handle it).
* Prevents multiple modules add the same file **multiple times** to the same bundle.

#### Volo.Abp.AspNetCore.Mvc.UI.Packages Package

> This package is already installed by default with the startup templates. So, most of the time, you don't need to install it manually.

Standard package contributors are defined in the `Volo.Abp.AspNetCore.Mvc.UI.Packages` nuget package. To install it into your project:

````
install-package Volo.Abp.AspNetCore.Mvc.UI.Packages
````

Then add the `AbpAspNetCoreMvcUiPackagesModule` module dependency to your own module

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiPackagesModule))]
    public class MyWebModule : AbpModule
    {
        //...
    }
}
````

### Themes

Themes uses the standard package contributors to add library resources to page layouts. Themes may also define some standard/global bundles, so any module can contribute to those standard/global bundles. See the [theming documentation](Theming.md) for more.

### Best Practices & Suggestions

It's suggested to define multiple bundles for an application, each one is used for different purposes.

* **Global bundle**: Global style/script bundles are included to every page in the application. Themes already defines global style & script bundles. Your module can contribute to them.
* **Layout bundles**: This is a specific bundle to an individual layout. Only contains resources shared among all the pages use the layout.
* **Module bundles**: For shared resources among an individual module pages.
* **Page bundles**: Specific bundles created for each page. Use the bundling tag helpers to create the bundle.

Establish a balance between performance, network bandwidth usage and managing too many bundles.

### See Also

* [Client Side Package Management](Client-Side-Package-Management.md)
* [Theming](Theming.md)