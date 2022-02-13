# Page Layout
PageLayout is used to define page-specific elements across application. 


## Title
Title is used to render page title in the PageHeader.

```csharp
@inject PageLayout PageLayout

@{
    PageLayout.Title = "My Page Title";
}
```

## BreadCrumbs
BreadCrumbItems are used to render breadcrumbs in the PageHeader.
```csharp
@inject PageLayout PageLayout

@{
    PageLayout.BreadcrumbItems.Add(new BlazoriseUI.BreadcrumbItem("My Page", "/my-page")); 
}
```

## Toolbar
ToolbarItems are used to render action toolbar items in the PageHeader.

Check out [Page Toolbar](https://docs.abp.io/en/abp/latest/UI/Blazor/Page-Header#page-toolbar)

```csharp
@inject PageLayout PageLayout
@{
    PageLayout.ToolbarItems.Add(new PageToolbars.PageToolbarItem(typeof(MyButtonComponent)));
}
```