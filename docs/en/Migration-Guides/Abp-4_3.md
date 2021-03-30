# ABP Framework 4.x to 4.3 Migration Guide

## Common

* `app.UseVirtualFiles()` has been marked as **obsolete**. Use `app.UseStaticFiles()` instead. ABP will handle the virtual file system integrated to the static files middleware.

## Blazor UI

Implemented the Blazor Server Side support with this release. It required some packages and namespaces arrangements. **Existing Blazor (WebAssembly) applications should done the changes explained in this section**.

### Namespace Changes

- `AbpBlazorMessageLocalizerHelper` -> moved to Volo.Abp.AspNetCore.Components.Web
- `AbpRouterOptions` -> moved to Volo.Abp.AspNetCore.Components.Web.Theming.Routing
- `AbpToolbarOptions` and `IToolbarContributor` -> moved to Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars
- `IAbpUtilsService` -> moved to Volo.Abp.AspNetCore.Components.Web
- `PageHeader` -> moved to `Volo.Abp.AspNetCore.Components.Web.Theming.Layout`.

In practice, if your application is broken because of the `Volo.Abp.AspNetCore.Components.WebAssembly.*` namespace, please try to switch to `Volo.Abp.AspNetCore.Components.Web.*` namespace.

Remember to change namespaces in the `_Imports.razor` files.

### Package Changes

No change on the framework packages, but **module packages are separated as Web Assembly & Server**;

* Use `Volo.Abp.Identity.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.Identity.Blazor` package. Also, change `AbpIdentityBlazorModule` usage to `AbpIdentityBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.TenantManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.TenantManagement.Blazor` package. Also, change `AbpTenantManagementBlazorModule` usage to `AbpTenantManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.PermissionManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.PermissionManagement.Blazor` package. Also, change `AbpPermissionManagementBlazorModule` usage to `AbpPermissionManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.SettingManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.SettingManagement.Blazor` package. Also, change `AbpSettingManagementBlazorModule` usage to `AbpSettingManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.FeatureManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.FeatureManagement.Blazor` package. Also, change `AbpFeatureManagementBlazorModule` usage to `AbpFeatureManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.

### Other Changes

* `EntityAction.RequiredPermission` has been marked as **obsolete**, because of performance reasons. It is suggested to use the `Visible` property by checking the permission/policy yourself and assigning to a variable. 