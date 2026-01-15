# Migration Plan: Blazorise to MudBlazor in ABP Framework

## Executive Summary

This document outlines a phased approach to **replace** Blazorise with MudBlazor as the UI component library in ABP Framework. The migration will be gradual, allowing users time to migrate their existing applications before Blazorise packages are deprecated and eventually removed.

**Key Strategy:** No abstraction layer - MudBlazor packages will directly replace Blazorise packages over time.

---

## Current Architecture Analysis

### Existing Blazorise Integration

The ABP Framework currently uses Blazorise (v1.8.8) with the following structure:

**Core Packages:**
- `Volo.Abp.BlazoriseUI` - Core UI module with service implementations
- `Volo.Abp.AspNetCore.Components.Web.Theming` - Theming infrastructure (currently depends on BlazoriseUI - **this needs to change**)

**Hosting-Specific Packages:**
- `Volo.Abp.AspNetCore.Components.Server.Theming`
- `Volo.Abp.AspNetCore.Components.WebAssembly.Theming`
- `Volo.Abp.AspNetCore.Components.MauiBlazor.Theming`

**Feature Module Packages (using Blazorise components):**
- `Volo.Abp.Identity.Blazor`
- `Volo.Abp.TenantManagement.Blazor`
- `Volo.Abp.PermissionManagement.Blazor`
- `Volo.Abp.FeatureManagement.Blazor`
- `Volo.Abp.SettingManagement.Blazor`
- `Volo.Abp.Account.Blazor`

### Existing Service Abstractions

ABP already has UI-agnostic service interfaces in `Volo.Abp.AspNetCore.Components`:
- `IUiMessageService` - Message dialogs (Info, Success, Warn, Error, Confirm)
- `IUiNotificationService` - Toast notifications
- `IUiPageProgressService` - Page progress indicators

These interfaces will be reused - only the implementations change from Blazorise to MudBlazor.

### Current Problem: Theming Package Dependency

The `Volo.Abp.AspNetCore.Components.Web.Theming` package currently depends on `Volo.Abp.BlazoriseUI`, but analysis shows this dependency is **not necessary**. The theming package uses:

1. `BreadcrumbItem` class - A simple POCO that should be UI-agnostic
2. `PageHeader.razor` - Uses Blazorise components directly (should be moved)
3. `PageToolbarExtensions.AddButton` - Uses Blazorise `ToolbarButton` (should be moved)

**Solution:** Refactor theming into UI-agnostic base + UI-specific implementations.

---

## Migration Timeline Overview

| Phase | Description | Blazorise Status |
|-------|-------------|------------------|
| Phase 0 | Refactor theming architecture | Fully supported |
| Phase 1 | Create MudBlazor core packages | Fully supported |
| Phase 2 | Create MudBlazor theming | Fully supported |
| Phase 3 | Create MudBlazor CRUD components | Fully supported |
| Phase 4 | Create MudBlazor feature modules | Fully supported |
| Phase 5 | Create MudBlazor themes | Fully supported |
| Phase 6 | Update templates (MudBlazor default) | **Deprecated** |
| Phase 7 | Documentation & migration guides | **Deprecated** |
| Phase 8 | Remove Blazorise packages | **Removed** |

---

## Phase 0: Refactor Theming Architecture (Pre-requisite)

**Objective:** Make `Volo.Abp.AspNetCore.Components.Web.Theming` UI-agnostic by removing its dependency on `Volo.Abp.BlazoriseUI`.

### 0.1 Move UI-Agnostic Classes

Move `BreadcrumbItem` from `Volo.Abp.BlazoriseUI` to `Volo.Abp.AspNetCore.Components.Web`:

```
framework/src/Volo.Abp.AspNetCore.Components.Web/
└── Volo/Abp/AspNetCore/Components/Web/
    └── Theming/
        └── BreadcrumbItem.cs    # Moved from BlazoriseUI
```

**BreadcrumbItem.cs** (UI-agnostic):
```csharp
namespace Volo.Abp.AspNetCore.Components.Web.Theming;

public class BreadcrumbItem
{
    public string Text { get; set; }
    public object? Icon { get; set; }
    public string? Url { get; set; }

    public BreadcrumbItem(string text, string? url = null, object? icon = null)
    {
        Text = text;
        Url = url;
        Icon = icon;
    }
}
```

### 0.2 Refactor Theming Package Structure

Split the theming infrastructure:

```
BEFORE (Current):
─────────────────
Volo.Abp.AspNetCore.Components.Web.Theming
├── Depends on: Volo.Abp.BlazoriseUI ❌ (Problem!)
├── PageHeader.razor (Blazorise components)
├── PageToolbarExtensions.cs (uses ToolbarButton)
└── Theming infrastructure (UI-agnostic)

AFTER (Refactored):
───────────────────
Volo.Abp.AspNetCore.Components.Web.Theming (UI-Agnostic)
├── Depends on: Volo.Abp.AspNetCore.Components.Web ✓
├── Depends on: Volo.Abp.UI.Navigation ✓
├── NO dependency on BlazoriseUI or MudBlazorUI ✓
├── Theming/
│   ├── ITheme.cs
│   ├── IThemeManager.cs
│   ├── IThemeSelector.cs
│   ├── ThemeDictionary.cs
│   └── ...
├── PageToolbars/
│   ├── IPageToolbarManager.cs
│   ├── PageToolbar.cs
│   ├── PageToolbarItem.cs
│   └── ... (no UI-specific extensions)
├── Toolbars/
│   ├── IToolbarManager.cs
│   ├── Toolbar.cs
│   └── ...
├── Routing/
│   └── AbpRouterOptions.cs
└── Layout/
    ├── PageLayout.cs (uses BreadcrumbItem from Components.Web)
    └── StandardLayouts.cs

Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise (Blazorise-Specific)
├── Depends on: Volo.Abp.AspNetCore.Components.Web.Theming ✓
├── Depends on: Volo.Abp.BlazoriseUI ✓
├── AbpAspNetCoreComponentsWebThemingBlazoriseModule.cs
├── Components/
│   └── PageHeader.razor (Blazorise implementation)
└── PageToolbars/
    └── BlazorisePageToolbarExtensions.cs (AddButton with ToolbarButton)

Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor (MudBlazor-Specific)
├── Depends on: Volo.Abp.AspNetCore.Components.Web.Theming ✓
├── Depends on: Volo.Abp.MudBlazorUI ✓
├── AbpAspNetCoreComponentsWebThemingMudBlazorModule.cs
├── Components/
│   └── PageHeader.razor (MudBlazor implementation)
└── PageToolbars/
    └── MudBlazorPageToolbarExtensions.cs (AddButton with MudButton)
```

### 0.3 Updated Package Dependencies

**`Volo.Abp.AspNetCore.Components.Web.Theming.csproj`** (UI-Agnostic):
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- NO BlazoriseUI dependency! -->
    <ProjectReference Include="..\Volo.Abp.AspNetCore.Components.Web\Volo.Abp.AspNetCore.Components.Web.csproj" />
    <ProjectReference Include="..\Volo.Abp.UI.Navigation\Volo.Abp.UI.Navigation.csproj" />
  </ItemGroup>
</Project>
```

**`Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise.csproj`**:
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Volo.Abp.AspNetCore.Components.Web.Theming\Volo.Abp.AspNetCore.Components.Web.Theming.csproj" />
    <ProjectReference Include="..\Volo.Abp.BlazoriseUI\Volo.Abp.BlazoriseUI.csproj" />
  </ItemGroup>
</Project>
```

**`Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.csproj`**:
```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\Volo.Abp.AspNetCore.Components.Web.Theming\Volo.Abp.AspNetCore.Components.Web.Theming.csproj" />
    <ProjectReference Include="..\Volo.Abp.MudBlazorUI\Volo.Abp.MudBlazorUI.csproj" />
  </ItemGroup>
</Project>
```

### 0.4 Module Updates

**`AbpAspNetCoreComponentsWebThemingModule.cs`** (UI-Agnostic):
```csharp
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.AspNetCore.Components.Web.Theming;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),  // NOT BlazoriseUI!
    typeof(AbpUiNavigationModule)
)]
public class AbpAspNetCoreComponentsWebThemingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDynamicLayoutComponentOptions>(options =>
        {
            options.Components.Add(typeof(AbpAuthenticationState), null);
        });
    }
}
```

**`AbpAspNetCoreComponentsWebThemingBlazoriseModule.cs`**:
```csharp
using Volo.Abp.BlazoriseUI;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpBlazoriseUIModule)
)]
public class AbpAspNetCoreComponentsWebThemingBlazoriseModule : AbpModule
{
}
```

**`AbpAspNetCoreComponentsWebThemingMudBlazorModule.cs`**:
```csharp
using Volo.Abp.MudBlazorUI;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpMudBlazorUIModule)
)]
public class AbpAspNetCoreComponentsWebThemingMudBlazorModule : AbpModule
{
}
```

### 0.5 Hosting-Specific Theming Packages

Apply the same pattern to hosting-specific packages:

```
BEFORE:
├── Volo.Abp.AspNetCore.Components.Server.Theming (depends on Blazorise.Bootstrap5)
├── Volo.Abp.AspNetCore.Components.WebAssembly.Theming (depends on Blazorise.Bootstrap5)
└── Volo.Abp.AspNetCore.Components.MauiBlazor.Theming (depends on Blazorise.Bootstrap5)

AFTER:
├── Volo.Abp.AspNetCore.Components.Server.Theming (UI-agnostic base)
├── Volo.Abp.AspNetCore.Components.Server.Theming.Blazorise (Blazorise-specific)
├── Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor (MudBlazor-specific)
│
├── Volo.Abp.AspNetCore.Components.WebAssembly.Theming (UI-agnostic base)
├── Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Blazorise (Blazorise-specific)
├── Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor (MudBlazor-specific)
│
├── Volo.Abp.AspNetCore.Components.MauiBlazor.Theming (UI-agnostic base)
├── Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Blazorise (Blazorise-specific)
└── Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor (MudBlazor-specific)
```

---

## Phase 1: MudBlazor Core Package

**Objective:** Create the core MudBlazor UI package that implements ABP's UI service interfaces.

### 1.1 Create `Volo.Abp.MudBlazorUI`

```
framework/src/Volo.Abp.MudBlazorUI/
├── Volo.Abp.MudBlazorUI.csproj
├── AbpMudBlazorUIModule.cs
├── MudBlazorUiMessageService.cs
├── MudBlazorUiNotificationService.cs
├── MudBlazorUiPageProgressService.cs
├── AbpMudCrudPageBase.cs
├── Components/
│   ├── AbpMudExtensibleDataGrid.razor
│   ├── AbpMudExtensibleDataGrid.razor.cs
│   ├── MudDataGridEntityActionsColumn.razor
│   ├── MudEntityAction.razor
│   ├── MudEntityActions.razor
│   ├── UiMessageAlert.razor
│   ├── UiNotificationAlert.razor
│   ├── UiPageProgress.razor
│   ├── MudSubmitButton.razor
│   ├── MudToolbarButton.razor
│   ├── MudPageAlert.razor
│   └── ObjectExtending/
│       ├── MudExtensionProperties.razor
│       ├── MudTextExtensionProperty.razor
│       ├── MudTextAreaExtensionProperty.razor
│       ├── MudCheckExtensionProperty.razor
│       ├── MudSelectExtensionProperty.razor
│       ├── MudDateTimeExtensionProperty.razor
│       ├── MudTimeExtensionProperty.razor
│       └── MudLookupExtensionProperty.razor
├── MudBlazorUiObjectExtensionPropertyInfoExtensions.cs
├── MudBlazorExtensionPropertyPolicyChecker.cs
└── wwwroot/
    └── volo.abp.mudblazorui.css
```

### 1.2 Project File: `Volo.Abp.MudBlazorUI.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Razor">

  <Import Project="..\..\configureawait.props" />
  <Import Project="..\..\common.props" />

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="MudBlazor" Version="8.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Volo.Abp.AspNetCore.Components.Web\Volo.Abp.AspNetCore.Components.Web.csproj" />
    <ProjectReference Include="..\Volo.Abp.Ddd.Application.Contracts\Volo.Abp.Ddd.Application.Contracts.csproj" />
    <ProjectReference Include="..\Volo.Abp.Authorization\Volo.Abp.Authorization.csproj" />
    <ProjectReference Include="..\Volo.Abp.GlobalFeatures\Volo.Abp.GlobalFeatures.csproj" />
    <ProjectReference Include="..\Volo.Abp.Features\Volo.Abp.Features.csproj" />
  </ItemGroup>

</Project>
```

### 1.3 Module Class: `AbpMudBlazorUIModule.cs`

```csharp
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.Authorization;
using Volo.Abp.Ddd.Application.Contracts;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Modularity;

namespace Volo.Abp.MudBlazorUI;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpGlobalFeaturesModule),
    typeof(AbpFeaturesModule)
)]
public class AbpMudBlazorUIModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
        });
    }
}
```

### 1.4 Component Mapping (Blazorise → MudBlazor)

| Blazorise Component | MudBlazor Equivalent | Notes |
|---------------------|----------------------|-------|
| `<DataGrid>` | `<MudDataGrid>` | Different API for columns |
| `<Modal>` | `<MudDialog>` | Uses `IDialogService` |
| `<Button>` | `<MudButton>` | Similar API |
| `<TextEdit>` | `<MudTextField>` | Different binding syntax |
| `<Select>` | `<MudSelect>` | Similar API |
| `<Check>` | `<MudCheckBox>` | Similar API |
| `<Snackbar>` | `<MudSnackbar>` | Uses `ISnackbar` service |
| `<Card>` | `<MudCard>` | Similar structure |
| `<Tabs>` | `<MudTabs>` | Different panel syntax |
| `<Tooltip>` | `<MudTooltip>` | Similar API |
| `<Field>/<FieldLabel>` | `<MudField>` | MudBlazor uses labels differently |
| `<Validation>` | `<MudForm>` | Different validation approach |
| `<Addons>` | `<MudInputAdornment>` | For input decorations |
| `<Icon>` | `<MudIcon>` | Uses `Icons.*` constants |
| `<Row>/<Column>` | `<MudGrid>/<MudItem>` | Different grid system |

---

## Phase 2: MudBlazor Theming Implementation

**Objective:** Create MudBlazor-specific theming packages using the refactored architecture.

### 2.1 Create `Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor`

```
framework/src/Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor/
├── Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.csproj
├── AbpAspNetCoreComponentsWebThemingMudBlazorModule.cs
├── Components/
│   └── PageHeader.razor (MudBlazor implementation)
├── PageToolbars/
│   └── MudBlazorPageToolbarExtensions.cs
└── Themes/
    ├── AbpMudTheme.cs
    └── AbpMudThemeProvider.razor
```

### 2.2 Create Hosting-Specific MudBlazor Theming Packages

**Server:**
```
framework/src/Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor/
├── Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.csproj
├── AbpAspNetCoreComponentsServerThemingMudBlazorModule.cs
└── Bundling/
    └── BlazorServerMudBlazorStyleContributor.cs
```

**WebAssembly:**
```
framework/src/Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor/
├── Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor.csproj
├── AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule.cs
└── Bundling/
    └── BlazorWebAssemblyMudBlazorStyleContributor.cs
```

**MAUI Blazor:**
```
framework/src/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor/
├── Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor.csproj
├── AbpAspNetCoreComponentsMauiBlazorThemingMudBlazorModule.cs
└── Bundling/
    └── MauiBlazorMudBlazorStyleContributor.cs
```

### 2.3 MudBlazor CSS Requirements

```html
<!-- Required in _Host.cshtml or index.html -->
<link href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap" rel="stylesheet" />
<link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
<script src="_content/MudBlazor/MudBlazor.min.js"></script>
```

---

## Phase 3: MudBlazor CRUD Page Base

**Objective:** Create MudBlazor version of `AbpCrudPageBase` with full CRUD functionality.

### 3.1 `AbpMudCrudPageBase.cs`

Located in `Volo.Abp.MudBlazorUI`:

```csharp
public abstract class AbpMudCrudPageBase<TAppService, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    : AbpComponentBase
    where TAppService : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    where TEntityDto : IEntityDto<TKey>
    where TGetListInput : new()
    where TCreateInput : class, new()
    where TUpdateInput : class, new()
{
    [Inject] protected TAppService AppService { get; set; }
    [Inject] protected IDialogService DialogService { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }

    protected IReadOnlyList<TEntityDto> Entities { get; set; } = Array.Empty<TEntityDto>();
    protected int TotalCount { get; set; }
    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 10;
    protected TGetListInput GetListInput { get; set; } = new();
    protected TCreateInput NewEntity { get; set; } = new();
    protected TKey EditingEntityId { get; set; }
    protected TUpdateInput EditingEntity { get; set; } = new();

    // Permission properties
    protected bool HasCreatePermission { get; set; }
    protected bool HasUpdatePermission { get; set; }
    protected bool HasDeletePermission { get; set; }

    // Abstract methods for permissions
    protected abstract string CreatePolicyName { get; }
    protected abstract string UpdatePolicyName { get; }
    protected abstract string DeletePolicyName { get; }

    protected override async Task OnInitializedAsync()
    {
        await SetPermissionsAsync();
        await GetEntitiesAsync();
    }

    protected virtual async Task SetPermissionsAsync()
    {
        HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
        HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
    }

    protected virtual async Task GetEntitiesAsync()
    {
        var result = await AppService.GetListAsync(GetListInput);
        Entities = result.Items;
        TotalCount = (int)result.TotalCount;
    }

    protected virtual async Task CreateEntityAsync()
    {
        await AppService.CreateAsync(NewEntity);
        Snackbar.Add(L["SuccessfullyCreated"], Severity.Success);
        await GetEntitiesAsync();
    }

    protected virtual async Task UpdateEntityAsync()
    {
        await AppService.UpdateAsync(EditingEntityId, EditingEntity);
        Snackbar.Add(L["SuccessfullyUpdated"], Severity.Success);
        await GetEntitiesAsync();
    }

    protected virtual async Task DeleteEntityAsync(TEntityDto entity)
    {
        var confirmed = await DialogService.ShowMessageBox(
            L["AreYouSure"],
            L["AreYouSureToDelete"],
            yesText: L["Yes"],
            cancelText: L["Cancel"]);

        if (confirmed == true)
        {
            await AppService.DeleteAsync(entity.Id);
            Snackbar.Add(L["SuccessfullyDeleted"], Severity.Success);
            await GetEntitiesAsync();
        }
    }

    // ... additional CRUD methods
}
```

---

## Phase 4: Feature Module MudBlazor Packages

**Objective:** Create MudBlazor versions of all ABP feature module UI packages.

### 4.1 Package Structure for Each Module

For each existing `.Blazor` module, create a corresponding MudBlazor package:

```
modules/identity/src/
├── Volo.Abp.Identity.Blazor/                         # Existing (Blazorise) - DEPRECATED
├── Volo.Abp.Identity.Blazor.MudBlazor/               # New (MudBlazor)
│   ├── Volo.Abp.Identity.Blazor.MudBlazor.csproj
│   ├── AbpIdentityBlazorMudBlazorModule.cs
│   ├── Pages/Identity/
│   │   ├── UserManagement.razor
│   │   ├── UserManagement.razor.cs
│   │   ├── RoleManagement.razor
│   │   └── RoleManagement.razor.cs
│   └── _Imports.razor
├── Volo.Abp.Identity.Blazor.MudBlazor.Server/
│   ├── Volo.Abp.Identity.Blazor.MudBlazor.Server.csproj
│   └── AbpIdentityBlazorMudBlazorServerModule.cs
└── Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly/
    ├── Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly.csproj
    └── AbpIdentityBlazorMudBlazorWebAssemblyModule.cs
```

### 4.2 Complete Module List

| Current Package (Blazorise) | New Package (MudBlazor) |
|----------------------------|-------------------------|
| `Volo.Abp.Identity.Blazor` | `Volo.Abp.Identity.Blazor.MudBlazor` |
| `Volo.Abp.Identity.Blazor.Server` | `Volo.Abp.Identity.Blazor.MudBlazor.Server` |
| `Volo.Abp.Identity.Blazor.WebAssembly` | `Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly` |
| `Volo.Abp.TenantManagement.Blazor` | `Volo.Abp.TenantManagement.Blazor.MudBlazor` |
| `Volo.Abp.TenantManagement.Blazor.Server` | `Volo.Abp.TenantManagement.Blazor.MudBlazor.Server` |
| `Volo.Abp.TenantManagement.Blazor.WebAssembly` | `Volo.Abp.TenantManagement.Blazor.MudBlazor.WebAssembly` |
| `Volo.Abp.PermissionManagement.Blazor` | `Volo.Abp.PermissionManagement.Blazor.MudBlazor` |
| `Volo.Abp.PermissionManagement.Blazor.Server` | `Volo.Abp.PermissionManagement.Blazor.MudBlazor.Server` |
| `Volo.Abp.PermissionManagement.Blazor.WebAssembly` | `Volo.Abp.PermissionManagement.Blazor.MudBlazor.WebAssembly` |
| `Volo.Abp.FeatureManagement.Blazor` | `Volo.Abp.FeatureManagement.Blazor.MudBlazor` |
| `Volo.Abp.FeatureManagement.Blazor.Server` | `Volo.Abp.FeatureManagement.Blazor.MudBlazor.Server` |
| `Volo.Abp.FeatureManagement.Blazor.WebAssembly` | `Volo.Abp.FeatureManagement.Blazor.MudBlazor.WebAssembly` |
| `Volo.Abp.SettingManagement.Blazor` | `Volo.Abp.SettingManagement.Blazor.MudBlazor` |
| `Volo.Abp.SettingManagement.Blazor.Server` | `Volo.Abp.SettingManagement.Blazor.MudBlazor.Server` |
| `Volo.Abp.SettingManagement.Blazor.WebAssembly` | `Volo.Abp.SettingManagement.Blazor.MudBlazor.WebAssembly` |
| `Volo.Abp.Account.Blazor` | `Volo.Abp.Account.Blazor.MudBlazor` |

### 4.3 Example: UserManagement.razor (MudBlazor)

```razor
@page "/identity/users"
@attribute [Authorize(IdentityPermissions.Users.Default)]
@using Microsoft.AspNetCore.Authorization
@using Volo.Abp.Identity.Localization
@inherits AbpMudCrudPageBase<IIdentityUserAppService, IdentityUserDto, Guid, GetIdentityUsersInput, IdentityUserCreateDto, IdentityUserUpdateDto>

<MudCard>
    <MudCardHeader>
        <CardHeaderContent>
            <MudText Typo="Typo.h5">@L["Users"]</MudText>
        </CardHeaderContent>
        <CardHeaderActions>
            @if (HasCreatePermission)
            {
                <MudButton Variant="Variant.Filled"
                           Color="Color.Primary"
                           OnClick="@OpenCreateDialog">
                    @L["NewUser"]
                </MudButton>
            }
        </CardHeaderActions>
    </MudCardHeader>
    <MudCardContent>
        <MudTextField @bind-Value="GetListInput.Filter"
                      Label="@L["Search"]"
                      Adornment="Adornment.Start"
                      AdornmentIcon="@Icons.Material.Filled.Search"
                      OnKeyUp="@OnSearchKeyUp" />

        <MudDataGrid Items="@Entities"
                     T="IdentityUserDto"
                     SortMode="SortMode.Single"
                     Filterable="false"
                     Dense="true">
            <Columns>
                <PropertyColumn Property="x => x.UserName" Title="@L["UserName"]" />
                <PropertyColumn Property="x => x.Email" Title="@L["Email"]" />
                <PropertyColumn Property="x => x.PhoneNumber" Title="@L["PhoneNumber"]" />
                <TemplateColumn Title="@L["Actions"]">
                    <CellTemplate>
                        <MudMenu Icon="@Icons.Material.Filled.MoreVert">
                            @if (HasUpdatePermission)
                            {
                                <MudMenuItem OnClick="@(() => OpenEditDialog(context.Item))">
                                    @L["Edit"]
                                </MudMenuItem>
                            }
                            @if (HasDeletePermission)
                            {
                                <MudMenuItem OnClick="@(() => DeleteEntityAsync(context.Item))">
                                    @L["Delete"]
                                </MudMenuItem>
                            }
                        </MudMenu>
                    </CellTemplate>
                </TemplateColumn>
            </Columns>
            <PagerContent>
                <MudDataGridPager T="IdentityUserDto" />
            </PagerContent>
        </MudDataGrid>
    </MudCardContent>
</MudCard>
```

---

## Phase 5: MudBlazor Basic Theme

**Objective:** Create MudBlazor-based theme to replace Blazorise BasicTheme.

### 5.1 Theme Package Structure

```
modules/basic-theme/src/
├── Volo.Abp.AspNetCore.Components.Web.BasicTheme/              # Existing (Blazorise) - DEPRECATED
├── Volo.Abp.AspNetCore.Components.Server.BasicTheme/           # DEPRECATED
├── Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme/      # DEPRECATED
│
├── Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme/     # New (MudBlazor)
│   ├── Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme.csproj
│   ├── AbpAspNetCoreComponentsWebMudBlazorBasicThemeModule.cs
│   ├── Themes/Basic/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   ├── LoginDisplay.razor
│   │   └── Branding.razor
│   └── wwwroot/
│       └── themes/basic/
│           └── main.css
├── Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme/
│   ├── Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme.csproj
│   └── AbpAspNetCoreComponentsServerMudBlazorBasicThemeModule.cs
└── Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme/
    ├── Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme.csproj
    └── AbpAspNetCoreComponentsWebAssemblyMudBlazorBasicThemeModule.cs
```

### 5.2 Example: MainLayout.razor (MudBlazor)

```razor
@inherits LayoutComponentBase

<MudThemeProvider />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />

<MudLayout>
    <MudAppBar Elevation="1">
        <MudIconButton Icon="@Icons.Material.Filled.Menu"
                       Color="Color.Inherit"
                       Edge="Edge.Start"
                       OnClick="@ToggleDrawer" />
        <Branding />
        <MudSpacer />
        <LoginDisplay />
    </MudAppBar>

    <MudDrawer @bind-Open="@_drawerOpen"
               Elevation="1"
               Variant="@DrawerVariant.Mini">
        <NavMenu />
    </MudDrawer>

    <MudMainContent>
        <MudContainer MaxWidth="MaxWidth.False" Class="pa-4">
            <UiPageProgress />
            <UiMessageAlert />
            <UiNotificationAlert />
            @Body
        </MudContainer>
    </MudMainContent>
</MudLayout>

@code {
    private bool _drawerOpen = true;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }
}
```

---

## Phase 6: Update Application Templates

**Objective:** Make MudBlazor the default for new projects, with Blazorise as deprecated option.

### 6.1 CLI Changes

```bash
# New default (MudBlazor)
abp new MyApp -t app --ui blazor

# Legacy option (deprecated, shows warning)
abp new MyApp -t app --ui blazor --blazor-ui-framework blazorise
```

### 6.2 Template Updates

Update all Blazor templates to use MudBlazor packages:

```xml
<!-- Default package references in templates -->
<ItemGroup>
  <PackageReference Include="Volo.Abp.MudBlazorUI" Version="$(AbpVersion)" />
  <PackageReference Include="Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor" Version="$(AbpVersion)" />
  <PackageReference Include="Volo.Abp.Identity.Blazor.MudBlazor" Version="$(AbpVersion)" />
  <PackageReference Include="Volo.Abp.TenantManagement.Blazor.MudBlazor" Version="$(AbpVersion)" />
  <!-- etc. -->
</ItemGroup>
```

### 6.3 Deprecation Warning

When `--blazor-ui-framework blazorise` is used:

```
WARNING: Blazorise UI framework is deprecated and will be removed in ABP v12.0.
Please use MudBlazor (default) for new projects.
See migration guide: https://docs.abp.io/en/abp/latest/UI/Blazor/Migration-Blazorise-To-MudBlazor
```

---

## Phase 7: Documentation & Migration Guides

**Objective:** Comprehensive documentation for migration and MudBlazor usage.

### 7.1 Documentation Structure

```
docs/en/
├── UI/
│   └── Blazor/
│       ├── Overall.md                              # Updated - MudBlazor focus
│       ├── Getting-Started.md                      # MudBlazor setup
│       ├── Components.md                           # MudBlazor components
│       ├── Theming.md                              # MudBlazor theming
│       ├── CRUD-Pages.md                           # AbpMudCrudPageBase
│       ├── Migration-From-Blazorise.md             # Migration guide
│       └── Blazorise/                              # Legacy docs (deprecated)
│           └── Index.md
```

### 7.2 Migration Guide Contents

1. **Component Migration Reference**
   - Complete mapping of Blazorise → MudBlazor components
   - Code examples for each component type

2. **Service Migration**
   - `IDialogService` usage (replaces Modal pattern)
   - `ISnackbar` usage (replaces Snackbar pattern)

3. **Validation Migration**
   - Blazorise `<Validation>` → MudBlazor `<MudForm>`
   - Data annotation handling differences

4. **CSS/Styling Migration**
   - Blazorise CSS classes → MudBlazor classes
   - Theme customization differences

5. **Step-by-Step Migration**
   - Update package references
   - Update module dependencies
   - Update component markup
   - Update code-behind files
   - Test and verify

---

## Phase 8: Remove Blazorise Packages

**Objective:** Complete removal of Blazorise from ABP Framework.

### 8.1 Packages to Remove

**Framework Packages:**
- `Volo.Abp.BlazoriseUI`
- `Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise`
- `Volo.Abp.AspNetCore.Components.Server.Theming.Blazorise`
- `Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Blazorise`
- `Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Blazorise`

**Module Packages:**
- `Volo.Abp.Identity.Blazor`
- `Volo.Abp.Identity.Blazor.Server`
- `Volo.Abp.Identity.Blazor.WebAssembly`
- `Volo.Abp.TenantManagement.Blazor`
- `Volo.Abp.TenantManagement.Blazor.Server`
- `Volo.Abp.TenantManagement.Blazor.WebAssembly`
- `Volo.Abp.PermissionManagement.Blazor`
- `Volo.Abp.PermissionManagement.Blazor.Server`
- `Volo.Abp.PermissionManagement.Blazor.WebAssembly`
- `Volo.Abp.FeatureManagement.Blazor`
- `Volo.Abp.FeatureManagement.Blazor.Server`
- `Volo.Abp.FeatureManagement.Blazor.WebAssembly`
- `Volo.Abp.SettingManagement.Blazor`
- `Volo.Abp.SettingManagement.Blazor.Server`
- `Volo.Abp.SettingManagement.Blazor.WebAssembly`
- `Volo.Abp.Account.Blazor`

**Theme Packages:**
- `Volo.Abp.AspNetCore.Components.Web.BasicTheme`
- `Volo.Abp.AspNetCore.Components.Server.BasicTheme`
- `Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme`

### 8.2 Removal Timeline

| ABP Version | Action |
|-------------|--------|
| v10.x | Phase 0: Refactor theming architecture |
| v10.x | Phases 1-5: MudBlazor packages introduced, Blazorise fully supported |
| v11.0 | Phase 6-7: MudBlazor default in templates, Blazorise deprecated |
| v12.0 | Phase 8: Blazorise packages removed |

---

## New Packages Summary

### Framework Packages (framework/src/)

| Package | Description |
|---------|-------------|
| `Volo.Abp.MudBlazorUI` | Core MudBlazor UI module (replaces BlazoriseUI) |
| `Volo.Abp.AspNetCore.Components.Web.Theming` | UI-agnostic theming base (refactored) |
| `Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise` | Blazorise theming (new, split from Web.Theming) |
| `Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor` | MudBlazor theming |
| `Volo.Abp.AspNetCore.Components.Server.Theming` | UI-agnostic server theming (refactored) |
| `Volo.Abp.AspNetCore.Components.Server.Theming.Blazorise` | Blazorise server theming |
| `Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor` | MudBlazor server theming |
| `Volo.Abp.AspNetCore.Components.WebAssembly.Theming` | UI-agnostic WASM theming (refactored) |
| `Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Blazorise` | Blazorise WASM theming |
| `Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor` | MudBlazor WASM theming |
| `Volo.Abp.AspNetCore.Components.MauiBlazor.Theming` | UI-agnostic MAUI theming (refactored) |
| `Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Blazorise` | Blazorise MAUI theming |
| `Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor` | MudBlazor MAUI theming |

### Module Packages (modules/)

| Package | Description |
|---------|-------------|
| `Volo.Abp.Identity.Blazor.MudBlazor` | Identity management UI |
| `Volo.Abp.Identity.Blazor.MudBlazor.Server` | Identity Server hosting |
| `Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly` | Identity WASM hosting |
| `Volo.Abp.TenantManagement.Blazor.MudBlazor` | Tenant management UI |
| `Volo.Abp.TenantManagement.Blazor.MudBlazor.Server` | Tenant Server hosting |
| `Volo.Abp.TenantManagement.Blazor.MudBlazor.WebAssembly` | Tenant WASM hosting |
| `Volo.Abp.PermissionManagement.Blazor.MudBlazor` | Permission management UI |
| `Volo.Abp.PermissionManagement.Blazor.MudBlazor.Server` | Permission Server hosting |
| `Volo.Abp.PermissionManagement.Blazor.MudBlazor.WebAssembly` | Permission WASM hosting |
| `Volo.Abp.FeatureManagement.Blazor.MudBlazor` | Feature management UI |
| `Volo.Abp.FeatureManagement.Blazor.MudBlazor.Server` | Feature Server hosting |
| `Volo.Abp.FeatureManagement.Blazor.MudBlazor.WebAssembly` | Feature WASM hosting |
| `Volo.Abp.SettingManagement.Blazor.MudBlazor` | Setting management UI |
| `Volo.Abp.SettingManagement.Blazor.MudBlazor.Server` | Setting Server hosting |
| `Volo.Abp.SettingManagement.Blazor.MudBlazor.WebAssembly` | Setting WASM hosting |
| `Volo.Abp.Account.Blazor.MudBlazor` | Account UI |

### Theme Packages (modules/basic-theme/)

| Package | Description |
|---------|-------------|
| `Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme` | Basic theme for MudBlazor |
| `Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme` | Server hosting |
| `Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme` | WASM hosting |

---

## Package Dependency Diagram (Refactored)

```
                    ┌─────────────────────────────────────┐
                    │  Volo.Abp.AspNetCore.Components.Web │
                    │  (UI-Agnostic)                      │
                    │  - IUiMessageService                │
                    │  - IUiNotificationService           │
                    │  - IUiPageProgressService           │
                    │  - BreadcrumbItem (moved here)      │
                    └─────────────────┬───────────────────┘
                                      │
                    ┌─────────────────┴───────────────────┐
                    │                                     │
                    ▼                                     ▼
         ┌─────────────────────┐             ┌─────────────────────┐
         │ Volo.Abp.BlazoriseUI│             │ Volo.Abp.MudBlazorUI│
         │ (Blazorise Impl)    │             │ (MudBlazor Impl)    │
         └──────────┬──────────┘             └──────────┬──────────┘
                    │                                   │
                    │    ┌──────────────────────────┐   │
                    │    │ ...Web.Theming           │   │
                    │    │ (UI-Agnostic Base)       │   │
                    │    │ - IThemeManager          │   │
                    │    │ - PageToolbars           │   │
                    │    │ - Toolbars               │   │
                    │    └────────────┬─────────────┘   │
                    │                 │                 │
                    ▼                 │                 ▼
         ┌─────────────────────┐      │      ┌─────────────────────┐
         │ ...Web.Theming      │◄─────┴─────►│ ...Web.Theming      │
         │    .Blazorise       │             │    .MudBlazor       │
         │ (Blazorise Theme)   │             │ (MudBlazor Theme)   │
         └──────────┬──────────┘             └──────────┬──────────┘
                    │                                   │
                    ▼                                   ▼
         ┌─────────────────────┐             ┌─────────────────────┐
         │ Identity.Blazor     │             │ Identity.Blazor     │
         │ (Blazorise)         │             │   .MudBlazor        │
         │ DEPRECATED          │             │                     │
         └─────────────────────┘             └─────────────────────┘
```

---

## Success Criteria

1. Phase 0 complete: Theming architecture refactored to be UI-agnostic
2. All MudBlazor packages created and functional
3. Feature parity with existing Blazorise implementations
4. Templates default to MudBlazor
5. Comprehensive migration documentation
6. Blazorise packages deprecated with clear timeline
7. Blazorise packages removed in final phase

---

## Risk Assessment

| Risk | Mitigation |
|------|------------|
| Breaking changes during Phase 0 refactoring | Careful API preservation, deprecation notices |
| User migration difficulty | Detailed migration guide, code examples |
| MudBlazor breaking changes | Pin to specific versions, test thoroughly |
| Missing MudBlazor features | Document gaps, provide workarounds |
| Performance differences | Benchmark and optimize |
| Community resistance | Clear communication, sufficient deprecation period |

---

## Conclusion

This migration plan provides a clear path from Blazorise to MudBlazor as the sole UI component library in ABP Framework. The key improvement is **Phase 0**, which refactors the theming architecture to be UI-agnostic, enabling clean separation between:

- `Volo.Abp.AspNetCore.Components.Web.Theming` - UI-agnostic (interfaces, options, base classes)
- `Volo.Abp.AspNetCore.Components.Web.Theming.Blazorise` - Blazorise-specific implementations
- `Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor` - MudBlazor-specific implementations

This architecture allows both UI frameworks to coexist during the transition period while sharing common theming infrastructure.
