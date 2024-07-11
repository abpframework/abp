# ABP 4.x to 4.3 Migration Guide

This version comes with some changes in the startup template, mostly related to Blazor UI. This document explains the breaking changes. However, **it is suggested to [compare the startup templates manually](upgrading-startup-template.md) to see all the changes** and apply to your solution.

## Open-Source (Framework)

If you are using one of the open-source startup templates, then you can check the following sections to apply the related breaking changes:

### `UseVirtualFiles` Has Been Marked as `Obsolete`

* `app.UseVirtualFiles()` has been marked as **obsolete**. Use `app.UseStaticFiles()` instead. ABP will handle the virtual file system integrated to the static files middleware.

### Blazor UI

Implemented the Blazor Server Side support with this release. It required some packages and namespaces arrangements. **Existing Blazor (WebAssembly) applications should done the changes explained in this section**.

#### Namespace Changes

- `AbpBlazorMessageLocalizerHelper` -> moved to Volo.Abp.AspNetCore.Components.Web
- `AbpRouterOptions` -> moved to Volo.Abp.AspNetCore.Components.Web.Theming.Routing
- `AbpToolbarOptions` and `IToolbarContributor` -> moved to Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars
- `IAbpUtilsService` -> moved to Volo.Abp.AspNetCore.Components.Web
- `PageHeader` -> moved to `Volo.Abp.AspNetCore.Components.Web.Theming.Layout`.

In practice, if your application is broken because of the `Volo.Abp.AspNetCore.Components.WebAssembly.*` namespace, please try to switch to `Volo.Abp.AspNetCore.Components.Web.*` namespace.

Remember to change namespaces in the `_Imports.razor` files.

#### Package Changes

No change on the framework packages, but **module packages are separated as Web Assembly & Server**;

* Use `Volo.Abp.Identity.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.Identity.Blazor` package. Also, change `AbpIdentityBlazorModule` usage to `AbpIdentityBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.TenantManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.TenantManagement.Blazor` package. Also, change `AbpTenantManagementBlazorModule` usage to `AbpTenantManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.PermissionManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.PermissionManagement.Blazor` package. Also, change `AbpPermissionManagementBlazorModule` usage to `AbpPermissionManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.SettingManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.SettingManagement.Blazor` package. Also, change `AbpSettingManagementBlazorModule` usage to `AbpSettingManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* Use `Volo.Abp.FeatureManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.FeatureManagement.Blazor` package. Also, change `AbpFeatureManagementBlazorModule` usage to `AbpFeatureManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.

#### Other Changes

* `EntityAction.RequiredPermission` has been marked as **obsolete**, because of performance reasons. It is suggested to use the `Visible` property by checking the permission/policy yourself and assigning to a variable.

#### Resource Reference Changes

Open `<YourProjectName>BundleContributor.cs` and replace `context.Add("main.css");` to `context.Add("main.css", true);`

Then run `abp bundle` command in the `blazor` folder to update resource references.

## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### Identity Pro Module

With this release, we've published new packages for the [Identity.Pro](../../modules/identity-pro.md) module. These packages extend the open source Identity module and will provide flexibility to us for adding unique features to the Identity Pro module.

Existing applications should replace some usages as described below.

#### Package Changes

* Replace `Volo.Abp.Identity.Domain` with the new `Volo.Abp.Identity.Pro.Domain` package and also replace `AbpIdentityDomainModule` usage with `AbpIdentityProDomainModule` in your module's `[DependsOn]` attribute.
* Replace `Volo.Abp.Identity.Domain.Shared` with the new `Volo.Abp.Identity.Pro.Domain.Shared` package and also replace `AbpIdentityDomainSharedModule` usage with `AbpIdentityProDomainSharedModule` in your module's `[DependsOn]` attribute.
* If you are using EF Core, Replace `Volo.Abp.Identity.EntityFrameworkCore` with the new `Volo.Abp.Identity.Pro.EntityFrameworkCore` package and also replace `AbpIdentityEntityFrameworkCoreModule` usage with `AbpIdentityProEntityFrameworkCoreModule` in your module's `[DependsOn]` attribute.
* If you are using MongoDB, Replace `Volo.Abp.Identity.MongoDB` with the new `Volo.Abp.Identity.Pro.MongoDB` package and also replace `AbpIdentityMongoDbModule` usage with `AbpIdentityProMongoDbModule` in your module's `[DependsOn]` attribute.

#### Other Changes

* Find `modelBuilder.ConfigureIdentity()` and change by `modelBuilder.ConfigureIdentityPro()` in your EF Core project.
* If you've used `IdentityDbContext`, use `IdentityProDbContext` instead.

### Blazor UI

Blazor server support comes with this release and we've created separate WebAssembly and Server packages for the modules.

So, if you are using Blazor WebAssembly UI, please make the following changes in your Blazor project;

* **Identity Module**: Use `Volo.Abp.Identity.Pro.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.Identity.Pro.Blazor` package. Also, change `AbpIdentityProBlazorModule` usage to `AbpIdentityProBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **Audit Logging Module**: Use `Volo.Abp.AuditLogging.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.AuditLogging.Blazor` package. Also, change `AbpAuditLoggingBlazorModule` usage to `AbpAuditLoggingBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **Text Template Management Module**: Use `Volo.Abp.TextTemplateManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.TextTemplateManagement.Blazor` package. Also, change `TextTemplateManagementBlazorModule` usage to `TextTemplateManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **Identity Server Module**: Use `Volo.Abp.IdentityServer.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.IdentityServer.Blazor` package. Also, change `AbpIdentityServerBlazorModule` usage to `AbpIdentityServerBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **Language Management Module**: Use `Volo.Abp.LanguageManagement.Blazor.WebAssembly` NuGet package instead of `Volo.Abp.LanguageManagement.Blazor` package. Also, change `LanguageManagementBlazorModule` usage to `LanguageManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **SaaS Module (host)**: Use `Volo.Saas.Host.Blazor.WebAssembly` NuGet package instead of `Volo.Saas.Host.Blazor` package. Also, change `SaasHostBlazorModule` usage to `SaasHostBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **SaaS Module (tenant)**: Use `Volo.Saas.Tenant.Blazor.WebAssembly` NuGet package instead of `Volo.Saas.Tenant.Blazor` package. Also, change `SaasTenantBlazorModule` usage to `SaasTenantBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.
* **File Management Module**: Use `Volo.FileManagement.Blazor.WebAssembly` NuGet package instead of `Volo.FileManagement.Blazor` package. Also, change `FileManagementBlazorModule` usage to `FileManagementBlazorWebAssemblyModule` in the `[DependsOn]` attribute on your module class.

#### Resource Reference Changes

Open `<YourProjectName>BundleContributor.cs` and replace `context.Add("main.css");` to `context.Add("main.css", true);`

Open `appsettings.json` and add the following:

```json
"AbpCli": {
    "Bundle": {
      "Mode": "BundleAndMinify", /* Options: None, Bundle, BundleAndMinify */
      "Name": "global",
      "Parameters": {
        "LeptonTheme.Style": "Style6", /* Options: Style1, Style2... Style6 */
        "LeptonTheme.ChangeStyleDynamically": "true"
      }
    }
  }
```

Then run `abp bundle` command in the `blazor` folder to update resource references.

### Angular UI

* **AccountConfigModule**: `AccountConfigModule` has been deprecated. It will be removed in v5.0. Import `AccountAdminConfigModule` instead as shown below:

```js
// app.module.ts

import { AccountAdminConfigModule } from '@volo/abp.ng.account/admin/config';

@NgModule({
  //...
  imports: [
    //...
    AccountAdminConfigModule.forRoot(),
    //...
  ]
})
export class AppModule {}
```

* **CommercialUiConfigModule**: `CommercialUiConfigModule` has been created to make `CommercialUIModule` lazy-loadable. The module will also provide some benefits for the configuration. Import it to the `app.module.ts` as shown below:

```js
import { CommercialUiConfigModule } from '@volo/abp.commercial.ng.ui/config';

@NgModule({
  //...
  imports: [
    //...
    CommercialUiConfigModule.forRoot(),
    //...other abp config modules
  ]
})
export class AppModule {}
```