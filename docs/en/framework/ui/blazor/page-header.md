```json
//[doc-params]
{
    "BlazorUI": ["Blazorise", "MudBlazor"]
}
```

```json
//[doc-seo]
{
    "Description": "Learn how to use the Blazor `PageHeader` component to customize page titles, breadcrumbs, and toolbars for your ABP Framework applications."
}
```

# Blazor UI: Page Header

You can use the `PageHeader` component to set the page title, the breadcrumb items and the toolbar items for a page. Before using the `PageHeader` component, you need to add a using statement for the namespace:

{{if BlazorUI == "Blazorise"}}

```razor
@using Volo.Abp.AspNetCore.Components.Web.Theming.Layout
```

{{end}}

{{if BlazorUI == "MudBlazor"}}

```razor
@using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Layout
```

{{end}}

Once you add the `PageHeader` component to your page, you can control the related values using the parameters. 

## Page Title

 You can use the `Title` parameter to control the page header.

```csharp
<PageHeader Title="Book List">
</PageHeader>
```

## Breadcrumb

> **The [Basic Theme](basic-theme.md) currently doesn't implement the breadcrumbs.**
> 
> The [LeptonX Lite Theme](../../../ui-themes/lepton-x-lite/blazor.md) supports breadcrumbs.

Breadcrumbs can be added using the `BreadcrumbItems` property.

**Example: Add Language Management to the breadcrumb items.**

{{if BlazorUI == "Blazorise"}}

Create a collection of `Volo.Abp.BlazoriseUI.BreadcrumbItem` objects and set the collection to the `BreadcrumbItems` parameter.

```csharp
public partial class Index
{
    protected List<BreadcrumbItem> BreadcrumbItems { get; } = new();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbItem("Language Management"));
    }
}
```

{{end}}

{{if BlazorUI == "MudBlazor"}}

Create a collection of `MudBlazor.BreadcrumbItem` objects and set the collection to the `BreadcrumbItems` parameter. The MudBlazor `BreadcrumbItem` constructor takes `(string text, string href, bool disabled = false, string icon = null)`.

```csharp
using MudBlazor;

public partial class Index
{
    protected List<BreadcrumbItem> BreadcrumbItems { get; } = new();

    protected override void OnInitialized()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(
            text: "Language Management",
            href: null,
            disabled: true));
    }
}
```

{{end}}

Navigate back to the razor page.

```csharp
<PageHeader BreadcrumbItems="@BreadcrumbItems" /> 
```

The theme then renders the breadcrumb. An example render result can be:

![breadcrumbs-example](../../../images/breadcrumbs-example.png)

* The Home icon is rendered by default. Set `BreadcrumbShowHome` to `false` to hide it.
* Breadcrumb items will be activated based on current navigation. Set `BreadcrumbShowCurrent` to `false` to disable it.

You can add as many items as you need.

{{if BlazorUI == "Blazorise"}}

The `Volo.Abp.BlazoriseUI.BreadcrumbItem` constructor gets three parameters:

* `text`: The text to show for the breadcrumb item.
* `url` (optional): A URL to navigate to, if the user clicks to the breadcrumb item.
* `icon` (optional): An icon class (like `fas fa-user-tie` for Font-Awesome) to show with the `text`.

{{end}}

{{if BlazorUI == "MudBlazor"}}

The `MudBlazor.BreadcrumbItem` constructor takes:

* `text`: The text to show for the breadcrumb item.
* `href`: A URL to navigate to (use `null` for the current page).
* `disabled` (optional): When `true`, the item is rendered as the current/non-clickable item.
* `icon` (optional): A Material icon (e.g. `Icons.Material.Filled.Language`).

{{end}}

## Page Toolbar

Page toolbar can be set using the `Toolbar` property.

**Example: Add  a "New Item" toolbar item to the page toolbar.**

Create a `PageToolbar` object and define toolbar items using the `AddButton` extension method.

{{if BlazorUI == "Blazorise"}}

```csharp
public partial class Index
{
    protected PageToolbar Toolbar { get; } = new();
    
    protected override void OnInitialized()
    {
        Toolbar.AddButton("New Item", () =>
        {
            //Write your click action here
            return Task.CompletedTask;
        }, icon:IconName.Add);
    }
}
```

{{end}}

{{if BlazorUI == "MudBlazor"}}

```csharp
public partial class Index
{
    protected PageToolbar Toolbar { get; } = new();

    protected override void OnInitialized()
    {
        Toolbar.AddButton("New Item", () =>
        {
            //Write your click action here
            return Task.CompletedTask;
        }, icon: MudBlazor.Icons.Material.Filled.Add);
    }
}
```

{{end}}

Navigate back to the razor page and set the `Toolbar` parameter.

```csharp
<PageHeader Toolbar="@Toolbar"  /> 
```

An example render result can be:

![breadcrumbs-example](../../../images/page-header-toolbar-blazor.png)

---

## Options
Rendering can be enabled or disabled for each section of PageHeader via using `PageHeaderOptions`.

```csharp
    Configure<PageHeaderOptions>(options => 
    {
        options.RenderPageTitle = false;
        options.RenderBreadcrumbs = false;
        options.RenderToolbar = false;
    });
```

*All values are **true** by default. If the PageHeaderOptions isn't configured, each section will be rendered.*