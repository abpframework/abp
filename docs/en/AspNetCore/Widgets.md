# Widgets

ABP provides a model and infrastructure to create **reusable widgets**. Widget system is an extension to [ASP.NET Core's ViewComponents](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components). Widgets are especially useful when you want to;

* Define widgets in reusable **[modules](../Module-Development-Basics.md)**.
* Have **scripts & styles** dependencies for your widget.
* Create **[dashboards](Dashboards.md)** with widgets used inside.
* Co-operate widgets with **[authorization](../Authorization.md)** and **[bundling](Bundling-Minification.md)** systems.

## Basic Widget Definition

### Create a View Component

As the first step, create a new regular ASP.NET Core View Component:

![widget-basic-files](../images/widget-basic-files.png)

**MySimpleWidgetViewComponent.cs**:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

Inheriting from `AbpViewComponent` is not required. You could inherit from ASP.NET Core's standard `ViewComponent`. `AbpViewComponent` only defines some base useful properties.

**Default.cshtml**:

```xml
<div class="my-simple-widget">
    <h2>My Simple Widget</h2>
    <p>This is a simple widget!</p>
</div>
```

### Define the Widget

Add a `Widget` attribute to the `MySimpleWidgetViewComponent` class to mark this view component as a widget:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

## Rendering a Widget

Rendering a widget is pretty standard. Use the `Component.InvokeAsync` method in a razor view/page as you do for any view component. Examples:

````xml
@await Component.InvokeAsync("MySimpleWidget")
@await Component.InvokeAsync(typeof(MySimpleWidgetViewComponent))
````

First approach uses the widget name while second approach uses the view component type.

## Widget Name

Default name of the view components are calculated based on the name of the view component type. If your view component type is `MySimpleWidgetViewComponent` then the widget name will be `MySimpleWidget` (removes `ViewComponent` postfix). This is how ASP.NET Core calculates a view component's name.

To customize widget's name, just use the standard `ViewComponent` attribute of ASP.NET Core:

```csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget]
    [ViewComponent(Name = "MyCustomNamedWidget")]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Pages/Components/MySimpleWidget/Default.cshtml");
        }
    }
}
```

ABP will respect to the custom name by handling the widget.

> If the view component name and the folder name of the view component don't match, you may need to manually write the view path as done in this example.

### Display Name

You can also define a human-readable, localizable display name for the widget. This display name then can be used on the UI when needed. Display name is optional and can be defined using properties of the  `Widget` attribute:

````csharp
using DashboardDemo.Localization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        DisplayName = "MySimpleWidgetDisplayName", //Localization key
        DisplayNameResource = typeof(DashboardDemoResource) //localization resource
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

See [the localization document](../Localization.md) to learn about localization resources and keys.

## Style & Script Dependencies

There are some challenges when your widget has script and style files;

* Any page uses the widget should also include the **its script & styles** files into the page.
* The page should also care about **depended libraries/files** of the widget.

ABP solves these issues when you properly relate the resources with the widget. You don't care about dependencies of the widget while using it.

### Defining as Simple File Paths

The example widget below adds a style and a script file:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        StyleSrcs = new[] { "/Pages/Components/MySimpleWidget/Default.css" },
        ScriptSrcs = new[] { "/Pages/Components/MySimpleWidget/Default.js" }
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

ABP takes account these dependencies and properly adds to the view/page when you use the widget. Style/script files can be **physical or virtual**. It is completely integrated to the [Virtual File System](../Virtual-File-System.md).

### Defining Bundle Contributors

All resources for used widgets in a page are added as a **bundle** (bundled & minified in production if you don't configure otherwise). In addition to adding a simple file, you can take full power of the bundle contributors.

The sample code below does the same with the code above, but defines and uses bundle contributors:

````csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(
        StyleTypes = new []{ typeof(MySimpleWidgetStyleBundleContributor) },
        ScriptTypes = new[]{ typeof(MySimpleWidgetScriptBundleContributor) }
        )]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

    public class MySimpleWidgetStyleBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files
              .AddIfNotContains("/Pages/Components/MySimpleWidget/Default.css");
        }
    }

    public class MySimpleWidgetScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files
              .AddIfNotContains("/Pages/Components/MySimpleWidget/Default.js");
        }
    }
}

````

Bundle contribution system is very powerful. If your widget uses a JavaScript library to render a chart, then you can declare it as a dependency, so the JavaScript library is automatically added to the page if it wasn't added before. In this way, the page using your widget doesn't care about the dependencies.

See the [bundling & minification](Bundling-Minification.md) documentation for more information about that system.

## Authorization

Some widgets may need to be available only for authenticated or authorized users. In this case, use the following properties of the `Widget` attribute:

* `RequiresAuthentication` (`bool`): Set to true to make this widget usable only for authentication users (user have logged in to the application).
* `RequiredPolicies` (`List<string>`): A list of policy names to authorize the user. See [the authorization document](../Authorization.md) for more info about policies.

Example:

````csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;

namespace DashboardDemo.Web.Pages.Components.MySimpleWidget
{
    [Widget(RequiredPolicies = new[] { "MyPolicyName" })]
    public class MySimpleWidgetViewComponent : AbpViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
````

## Widget Options

As alternative to the `Widget` attribute, you can use the `WidgetOptions` to configure widgets:

```csharp
Configure<WidgetOptions>(options =>
{
    options.Widgets.Add<MySimpleWidgetViewComponent>();
});
```

Write this into the `ConfigureServices` method of your [module](../Module-Development-Basics.md). All the configuration done with the `Widhet` attribute is also possible with the `WidgetOptions`. Example configuration that adds a style for the widget:

````csharp
Configure<WidgetOptions>(options =>
{
    options.Widgets
        .Add<MySimpleWidgetViewComponent>()
        .WithStyles("/Pages/Components/MySimpleWidget/Default.css");
});
````

> Tip: `WidgetOptions` can also be used to get an existing widget and change its configuration. This is especially useful if you want to modify the configuration of a widget inside a module used by your application. Use `options.Widgets.Find` to get an existing `WidgetDefinition`.