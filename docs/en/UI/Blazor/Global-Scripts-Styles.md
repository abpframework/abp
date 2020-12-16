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

> Bundle command adds style and script references individually. Bundling and minification support will be added to incoming releases.