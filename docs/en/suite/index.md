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

> **Important:** ABP Suite does not generate React UI pages or client-side React code. Its CRUD UI generation is designed for the established Suite-supported UI stacks, such as MVC, Blazor and Angular. The new [React UI](../framework/ui/react/index.md) belongs to ABP's modern template system and is designed for an AI-first, AI-oriented development flow with a modern frontend stack such as React, TypeScript, Vite, TanStack Router, TanStack Query, shadcn/ui, Zod and React Hook Form. In React UI solutions, ABP Suite can still be useful for the backend and application layers where applicable, but the React UI side is expected to be developed in the source-owned React application. ABP license holders can use [ABP Studio AI Agent](https://abp.io/studio/ai-agent) with predefined AI credits to generate React pages and evolve the UI more easily.

It's a .NET Core Global tool that can be installed from the command line. If you are using [ABP Studio](../studio/index.md), you don't even need to install it because it should already be installed, when you first installed the [ABP Studio](../studio/index.md).

By using the ABP Suite, you can generate CRUD pages from the database to the front-end and directly get a kickstart for your application. ABP Suite is actively developed and new features are being added version by version according to the roadmap and your feedback.

## Blazor UI library

When the target solution uses any Blazor UI framework (Blazor, BlazorServer, BlazorWebApp or MauiBlazor), ABP Suite detects the underlying Blazor component library by scanning the Blazor project `.csproj` for known package references:

* A reference to `Volo.Abp.MudBlazorUI`, `Volo.Abp.AspNetCore.Components.*.Theming.MudBlazor` or `*.Blazor.MudBlazor*` selects the **MudBlazor** templates.
* Otherwise ABP Suite falls back to the **Blazorise** templates that have always shipped with Suite.

The detected value is shown in the *solution info* tooltip of the CRUD Page Generator screen and is reused for two things:

1. Template routing — `Frontend.Blazor.*` resource names are resolved to `Frontend.Blazor.MudBlazor.*` for MudBlazor solutions, so the generated `.razor` / `.razor.cs` files use `MudCard`, `MudDataGrid`, `MudDialog`, `MudForm` and the corresponding Mud input controls instead of the Blazorise `Card`, `DataGrid`, `Modal`, `Validations` markup.
2. Template Management UI — the "Manage Templates" screen and the "outdated templates" check only list the variant that matches the current solution, so the Blazorise and MudBlazor template trees never appear side-by-side.

Customized templates are stored under `.suite/customized-templates/` keyed by the full resource name, so MudBlazor customizations (`Frontend.Blazor.MudBlazor.*`) are physically isolated from Blazorise customizations (`Frontend.Blazor.*`) and never clash.
