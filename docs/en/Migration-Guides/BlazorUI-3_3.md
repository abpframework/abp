# Migration Guide for the Blazor UI from the v3.2 to the v3.3

## Startup Template Changes

* Remove `Volo.Abp.Account.Blazor` NuGet package from your `.Blazor.csproj` and add `Volo.Abp.TenantManagement.Blazor` NuGet package.
* Remove the ``typeof(AbpAccountBlazorModule)`` from the dependency list of *YourProjectBlazorModule* class and add the `typeof(AbpTenantManagementBlazorModule)`.
* Add `@using Volo.Abp.BlazoriseUI` and `@using Volo.Abp.BlazoriseUI.Components` into the `_Imports.razor` file.
* Remove the `div` with `id="blazor-error-ui"` (with its contents) from the `wwwroot/index.html ` file, since the ABP Framework now shows error messages as a better message box.
* Update the`AddOidcAuthentication` options in your *YourProjectBlazorModule* class as described in the issue [#5913](https://github.com/abpframework/abp/issues/5913).

## BlazoriseCrudPageBase to AbpCrudPageBase

Renamed `BlazoriseCrudPageBase` to `AbpCrudPageBase`. Just update the usages. It also has some changes, you may need to update method calls/usages manually.

