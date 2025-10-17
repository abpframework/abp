```json
//[doc-seo]
{
    "Description": "Learn how to manage global scripts and styles in Blazor apps using ABP, simplifying references and dependency management effortlessly."
}
```

# Blazor UI: Managing Global Scripts & Styles

You can add your JavaScript and CSS files from your modules or applications to the Blazor global assets system. All the JavaScript and CSS files will be added to the `global.js` and `global.css` files. You can access these files via the following URL in a Blazor WASM project:

-  https://localhost/global.js
-  https://localhost/global.css

## Add JavaScript and CSS to the global assets system in the module

Your module project solution will have two related Blazor projects:

* `MyModule.Blazor`：This project includes the JavaScript/CSS files required for your Blazor components. The `MyApp.Blazor.Client (Blazor WASM)` project will reference this project.
* `MyModule.Blazor.WebAssembly.Bundling`：This project is used to add your JavaScript/CSS files to the Blazor global resources. The `MyModule.Blazor (ASP.NET Core)` project will reference this project.

You need to define JavaScript and CSS contributor classes in the `MyModule.Blazor.WebAssembly.Bundling` project to add the files to the global assets system.

> Please use `BlazorWebAssemblyStandardBundles.Scripts.Global` and `BlazorWebAssemblyStandardBundles.Styles.Global` for the bundle name.

```cs
public class MyModuleBundleScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("_content/MyModule.Blazor/libs/myscript.js");
    }
}
```

```cs
public class MyModuleBundleStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("_content/MyModule.Blazor/libs/mystyle.css");
    }
}
```

```cs
[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingBundlingModule)
)]
public class MyBlazorWebAssemblyBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            // Add script bundle
            options.ScriptBundles.Get(BlazorWebAssemblyStandardBundles.Scripts.Global)
                .AddContributors(typeof(MyModuleBundleScriptContributor));

            // Add style bundle
            options.StyleBundles.Get(BlazorWebAssemblyStandardBundles.Styles.Global)
                .AddContributors(typeof(MyModuleBundleStyleContributor));
        });
    }
}
```

## Add JavaScript and CSS to the global assets system in the application

This is similar to the module. You need to define JavaScript and CSS contributor classes in the `MyApp.Blazor.Client` project to add the files to the global assets system.

## AbpBundlingGlobalAssetsOptions

You can configure the JavaScript and CSS file names in the `GlobalAssets` property of the `AbpBundlingOptions` class. The default values are `global.js` and `global.css`.

## Reference

- [ASP.NET Core MVC Bundling & Minification](../mvc-razor-pages/bundling-minification#bundle-contributorsg)
- [ABP Global Assets - New way to bundle JavaScript/CSS files in Blazor WebAssembly app](https://github.com/abpframework/abp/blob/dev/docs/en/Community-Articles/2024-11-25-Global-Assets/POST.md)
