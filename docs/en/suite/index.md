```json
//[doc-seo]
{
    "Description": "Discover ABP Suite, a powerful .NET Core tool for quickly building web pages and generating CRUD applications with ease."
}
```

# ABP Suite

````json
//[doc-nav]
{
  "Next": {
    "Name": "How to install ABP Suite?",
    "Path": "suite/how-to-install"
  }
}
````

> You must have an [ABP Team or a higher license](https://abp.io/pricing) to use the ABP Suite.

ABP Suite is a complementary tool to the ABP Platform. ABP Suite allows you to build web pages in a matter of minutes. 

> **React UI:** ABP Suite also generates React CRUD pages for modern React UI solutions (`app/react`), in addition to the MVC, Blazor and Angular UI stacks. The generator is template-based and does not use AI. It covers the standard CRUD scenarios — navigation-property lookups, many-to-many navigation collections, master-detail child grids, file upload, enums, filtering, Excel export and bulk delete — and registers each page in the React app's route configuration and menu.

It's a .NET Core Global tool that can be installed from the command line. If you are using [ABP Studio](../studio/index.md), you don't even need to install it because it should already be installed, when you first installed the [ABP Studio](../studio/index.md).

By using the ABP Suite, you can generate CRUD pages from the database to the front-end and directly get a kickstart for your application. ABP Suite is actively developed and new features are being added version by version according to the roadmap and your feedback.

## Blazor UI library

When the target solution uses any Blazor UI framework (Blazor, BlazorServer, BlazorWebApp or MAUIBlazor), ABP Suite detects the underlying Blazor component library by scanning the Blazor project `.csproj` for known package references:

* A reference to `Volo.Abp.MudBlazorUI`, `Volo.Abp.AspNetCore.Components.*.Theming.MudBlazor` or `*.Blazor.MudBlazor*` selects the **MudBlazor** templates.
* Otherwise ABP Suite falls back to the **Blazorise** templates that have always shipped with Suite.

The detected value is shown in the *solution info* tooltip of the CRUD Page Generator screen and is reused for two things:

1. Template routing — `Frontend.Blazor.*` resource names are resolved to `Frontend.Blazor.MudBlazor.*` for MudBlazor solutions, so the generated `.razor` / `.razor.cs` files use `MudCard`, `MudDataGrid`, `MudDialog`, `MudForm` and the corresponding Mud input controls instead of the Blazorise `Card`, `DataGrid`, `Modal`, `Validations` markup.
2. Template Management UI — the "Manage Templates" screen and the "outdated templates" check only list the variant that matches the current solution, so the Blazorise and MudBlazor template trees never appear side-by-side.

Customized templates are stored under `.suite/customized-templates/` keyed by the full resource name, so MudBlazor customizations (`Frontend.Blazor.MudBlazor.*`) are physically isolated from Blazorise customizations (`Frontend.Blazor.*`) and never clash.
