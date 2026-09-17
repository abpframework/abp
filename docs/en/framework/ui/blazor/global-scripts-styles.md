```json
//[doc-seo]
{
    "Description": "Learn how to manage global scripts and styles in Blazor apps using ABP, simplifying references and dependency management effortlessly."
}
```

# Blazor UI: Managing Global Scripts & Styles

You can add your JavaScript and CSS files from your modules or applications to the Blazor global assets system. By default, all the JavaScript and CSS files are added to the `global.js` and `global.css` files. You can access these files via the following URL in a Blazor WASM project:

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

The `GlobalAssets` property of `AbpBundlingOptions` is shared by the MVC, Blazor WebAssembly and MAUI Blazor bundling integrations. It has the following properties:

* `Enabled`: Enables global asset generation. The Blazor WebAssembly and MAUI Blazor theming modules enable it when they configure their global bundles.
* `GlobalStyleBundleName`: The style bundle used to generate the global CSS asset.
* `GlobalScriptBundleName`: The script bundle used to generate the global JavaScript asset.
* `CssFileName`: The generated CSS file name. The default is `global.css`.
* `JavaScriptFileName`: The generated JavaScript file name. The default is `global.js`.

```csharp
Configure<AbpBundlingOptions>(options =>
{
    options.GlobalAssets.Enabled = true;
    options.GlobalAssets.GlobalStyleBundleName =
        BlazorWebAssemblyStandardBundles.Styles.Global;
    options.GlobalAssets.GlobalScriptBundleName =
        BlazorWebAssemblyStandardBundles.Scripts.Global;
    options.GlobalAssets.CssFileName = "app-global.css";
    options.GlobalAssets.JavaScriptFileName = "app-global.js";
});
```

When you change `CssFileName` or `JavaScriptFileName`, update the host page references to the same names. For a standalone Blazor WebAssembly host, replace the default references in `App.razor`:

```html
<link href="app-global.css" rel="stylesheet" />
<script src="app-global.js"></script>
```

For a Blazor Web App, replace `global.css` and `global.js` in the `GlobalStyles` and `GlobalScripts` lists in `Components/App.razor`. Changing only `AbpBundlingOptions` generates the files under the new names but does not rewrite these host references.

## Reference

- [ASP.NET Core MVC Bundling & Minification](../mvc-razor-pages/bundling-minification.md#bundle-contributors)
- [ABP Global Assets - New way to bundle JavaScript/CSS files in Blazor WebAssembly app](https://github.com/abpframework/abp/blob/dev/docs/en/Community-Articles/2024-11-25-Global-Assets/POST.md)
