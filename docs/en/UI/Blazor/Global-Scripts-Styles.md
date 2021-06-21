# Blazor UI: Managing Global Scripts & Styles

Some modules may require additional styles or scripts that need to be referenced in **index.html** file. It's not easy to find and update these types of references in Blazor apps. ABP offers a simple, powerful, and modular way to manage global style and scripts in Blazor apps.

To update script & style references without worrying about dependencies, ordering, etc in a project, you can use the [bundle command](../../CLI.md#bundle).

You can also add custom styles and scripts and let ABP manage them for you. In your Blazor project, you can create a class implementing `IBundleContributor` interface.

`IBundleContributor` interface contains two methods.

* `AddScripts(...)`
* `AddStyles(...)`

Both methods get `BundleContext` as a parameter. You can add scripts and styles to the `BundleContext` and run [bundle command](../../CLI.md#bundle). Bundle command detects custom styles and scripts with module dependencies and updates `index.html` file.

## Example Usage
```csharp
namespace MyProject.Blazor
{
    public class MyProjectBundleContributor : IBundleContributor
    {
        public void AddScripts(BundleContext context)
        {
            context.Add("site.js");
        }

        public void AddStyles(BundleContext context)
        {
            context.Add("main.css");
            context.Add("custom-styles.css");
        }
    }
}
```

> There is a BundleContributor class implementing `IBundleContributor` interface coming by default with the startup templates. So, most of the time, you don't need to add it manually.

## Bundling And Minification
`abp bundle` command offers bundling and minification support for client-side resources(JavaScript and CSS files). `abp bundle` command reads the `appsettings.json` file inside the Blazor project and bundles the resources according to the configuration. You can find the bundle configurations inside `AbpCli.Bundle` element.

Here are the options that you can control inside the `appsettings.json` file.

`Mode`: Bundling and minification mode. Possible values are
* `BundleAndMinify`: Bundle all the files into a single file and minify the content.
* `Bundle`: Bundle all files into a single file, but not minify.
* `None`: Add files individually, do not bundle.

`Name`: Bundle file name. Default value is `global`.

`Parameters`: You can define additional key/value pair parameters inside this section. `abp bundle` command automatically sends these parameters to the bundle contributors, and you can check these parameters inside the bundle contributor, take some actions according to these values. 

Let's say that you want to exclude some resources from the bundle and control this action using the bundle parameters. You can add a parameter to the bundle section like below.

```json
"AbpCli": {
    "Bundle": {
      "Mode": "BundleAndMinify", /* Options: None, Bundle, BundleAndMinify */
      "Name": "global",
      "Parameters": {
        "ExcludeThemeFromBundle":"true"
      }
    }
  }
```

You can check this parameter and take action like below.

```csharp
public class MyProjectNameBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
    }

    public void AddStyles(BundleContext context)
    {
        var excludeThemeFromBundle = bool.Parse(context.Parameters.GetValueOrDefault("ExcludeThemeFromBundle"));
        context.Add("mytheme.css", excludeFromBundle: excludeThemeFromBundle);
        context.Add("main.css");
    }
}
```

