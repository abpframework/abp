# Widgets

ABP provides a model and infrastructure to create **reusable widgets**. It relies on [ASP.NET Core's ViewComponent](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components) system, so any view component can be used as a widget. Widget system is especially useful when you want to;

* Define widgets in reusable **[modules](../Module-Development-Basics.md)**.
* Have **scripts & styles** for your widget.
* Create **[dashboards](Dashboards.md)** with widgets used in (widgets you've built yourself or defined by modules you are using).
* Co-operate widgets with **[authorization](../Authorization.md)** and **[bundling](Bundling-Minification.md)** systems.

## Basic Widget Definition

### Create a View Component

As the first step, create a regular ASP.NET Core View Component:

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

**Default.cshtml**:

```xml
<div class="my-simple-widget">
    <h2>My Simple Widget</h2>
    <p>This is a simple widget!</p>
</div>
```

### Define the Widget

Second step is to define a widget using the `WidgetOptions` (in the `ConfigureServices` method of your Web module):

````csharp
Configure<WidgetOptions>(options =>
{
    options.Widgets.Add(
        new WidgetDefinition(
            "MySimpleWidget", //Unique Widget name
            typeof(MySimpleWidgetViewComponent) //Type of the ViewComponent
    	)
    );
});
````

Widget name should be unique in the application.

*TODO: This is in development and will probably change!*

### Render the Widget

Whenever you want to render a widget, you can inject the `IWidgetRenderer` and use the `RenderAsync` method with the unique widget name.

````xml
@inject IWidgetRenderer WidgetRenderer

@await WidgetRenderer.RenderAsync(Component, "MySimpleWidget")
````

You could do the same with standard `Component.InvokeAsync` method. The main difference is that `IWidgetRenderer` uses the widget name defined before. The essential benefit of the `WidgetRenderer` comes when your widget has additional resources, like script and style files.

## Style & Script Dependencies

There are some challenges when your widget has script and style files;

* Any page uses the widget should also include the **its script & styles** files into the page.
* The page should also care about **depended libraries/files** of the widget.

ABP solves all these issues when you properly relate the resources with the widget. You don't care about dependencies of the widget.