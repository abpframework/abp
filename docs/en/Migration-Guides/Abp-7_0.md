# ABP Version 7.0 Migration Guide

This document is a guide for upgrading ABP v6.x solutions to ABP v7.0. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

> ABP Framework upgraded to .NET 7.0, so you need to move your solutions to .NET 7.0 if you want to use the ABP 7.0. You can check the [Migrate from ASP.NET Core 6.0 to 7.0](https://learn.microsoft.com/en-us/aspnet/core/migration/60-70?view=aspnetcore-7.0) documentation.

## `FormTenantResolveContributor` Removed from the `AbpTenantResolveOptions`

`FormTenantResolveContributor` has been removed from the `AbpTenantResolveOptions`. Thus, if you need to get tenant info from `HTTP Request From`, please add a custom `TenantResolveContributor` to implement it.

## `IHybridServiceScopeFactory` Removed 

`IHybridServiceScopeFactory` has been removed. Please use the `IServiceScopeFactory` instead.

## Hybrid JSON was removed.

Since [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) library supports more custom features in NET 7, ABP no longer need the hybrid Json feature.

### Previous Behavior

There is a `Volo.Abp.Json` package which contains the `AbpJsonModule` module.
`Serialization/deserialization` features of [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) and [Newtonsoft](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm) are implemented in this module.

We use [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview) first,  More custom cases can be handled with [Newtonsoft](https://www.newtonsoft.com/json/help/html/SerializingJSON.htm) by configuring `UnsupportedTypes` of `AbpSystemTextJsonSerializerOptions`.

### New Behavior

We created `Volo.Abp.Json.SystemTextJson` and `Volo.Abp.Json.Newtonsoft` as separate packages, which means you can only use one of them in your project. The default is to use `SystemTextJson`. If you want `Newtonsoft`, please also use `Volo.Abp.AspNetCore.Mvc.NewtonsoftJson` in your web project.

* Volo.Abp.Json.Abstractions 
* Volo.Abp.Json.Newtonsoft
* Volo.Abp.Json.SystemTextJson
* Volo.Abp.Json (Depends on `Volo.Abp.Json.SystemTextJson` by default to prevent breaking)
* Volo.Abp.AspNetCore.Mvc.NewtonsoftJson

The `AbpJsonOptions` now has only two properties, which are

* `InputDateTimeFormats(List<string>)`: Formats of input JSON date, Empty string means default format. You can provide multiple formats to parse the date.
* `OutputDateTimeFormat(string)`: Format of output json date, Null or empty string means default format.

Please remove all `UnsupportedTypes` add custom `Modifiers` to control serialization/deserialization behavior.

Check the docs to see the more info: https://github.com/abpframework/abp/blob/dev/docs/en/JSON.md#configuration

Check the docs to see how to customize a JSON contract: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts

## "Manage Host Features" Moved to the Settings Page

"Manage Host Features" button has been moved from Tenants page to Settings page.

See https://github.com/abpframework/abp/pull/13359 for more info.

## Removed the `setter` from the Auditing Interfaces 

`AuditedEntity` and other base entity classes will continue to have public setters. If you want to make them private, don't derive from these base classes, but implement the interfaces yourself.

See https://github.com/abpframework/abp/issues/12229#issuecomment-1191384798 for more info.

## Added Abp prefix to DbProperties Classes

Please update the database migration and related connection string names.

## `EntityCreatingEventData`, `EntityUpdatingEventData`, `EntityDeletingEventData` and `EntityChangingEventData` has been removed. 

They are deprecated don't use them anymore.

## LayoutHookInfo.cs, LayoutHookViewModel.cs, LayoutHooks.cs, AbpLayoutHookOptions.cs classes have been moved under the Volo.Abp.Ui.LayoutHooks namespace.

See https://github.com/abpframework/abp/pull/13903 for more info.

## Removed `abp.auth.policies`

`abp.auth.polices` has been removed, use `abp.auth.grantedPolicies` instead.

## Static C# Proxy Generation

The `abp generate-proxy -t csharp ..` command will generate all the `classes/enums/other types` in the client side (including application service interfaces) behalf of you.

If you have reference to the target contracts package, then you can pass a parameter `--without-contracts (shortcut: -c)`.

See https://github.com/abpframework/abp/issues/13613#issue-1333088953 for more info.

## Dynamic Permissions

* `IPermissionDefinitionManager` methods are converted to asynchronous, and renamed (added Async postfix).
* Removed `MultiTenancySides` from permission groups.
* Inherit `MultiTenancySides` enum from byte (default was int).
* Needs to add migration for new entities in the Permission Management module.

See https://github.com/abpframework/abp/pull/13644 for more info.

## External Localization Infrastructure

* Introduced `LocalizationResourceBase` that is base for localization resources. `LocalizationResource` inherits from it for typed (static) localization resources (like before). Also introduced `NonTypedLocalizationResource` that inherits from `LocalizationResourceBase` for dynamic/external localization resources. We are using `LocalizationResourceBase` for most of the places where we were using `LocalizationResource` before and that can be a breaking change for some applications.
*  All layouts in all MVC UI themes should add this line just before the **ApplicationConfigurationString** line:

```html
<script src="~/Abp/ApplicationLocalizationScript?cultureName=@CultureInfo.CurrentUICulture.Name"></script>
```

We've already done this for our themes. 

See https://github.com/abpframework/abp/pull/13845 for more info.

## Replaced `BlogPostPublicDto` with `BlogPostCommonDto`

- In the CMS Kit Module, `BlogPostPublicDto` has been moved to `Volo.CmsKit.Common.Application.Contracts` from `Volo.CmsKit.Public.Application.Contracts` and renamed to `BlogPostCommonDto`.

- See the [PR#13499](https://github.com/abpframework/abp/pull/13499) for more information.

> You can ignore this if you don't use CMS Kit Module.

## Devart.Data.Oracle.EFCore

The `Devart.Data.Oracle.EFCore` package do not yet support EF Core 7.0, If you use `AbpEntityFrameworkCoreOracleDevartModule(Volo.Abp.EntityFrameworkCore.Oracle.Devart)` may not work as expected, We will release new packages as soon as they are updated.

See https://github.com/abpframework/abp/issues/14412 for more info.
# Changes on Angular Apps
##  Added a new package `@abp/ng.oauth`
OAuth Functionality moved to a seperate package named `@abp/ng.oauth`, so ABP users should add the `@abp/ng.oauth` packages on app.module.ts.
Add the new npm package to your app.
```
yarn add @abp/ng.oauth
// or npm i ---save @abp/ng.oauth
```

```typescript
// app.module.ts
import { AbpOAuthModule } from "@abp/ng.oauth";
// ...
@NgModule({
 // ...
    imports: [
    AbpOAuthModule.forRoot(), // <-- Add This
   // ...
  ],
 // ...
})
export class AppModule {}

```
## Lepton X Google-Font
If you are using LeptonX that has google fonts, the fonts were built-in the Lepton file. It's been moved to a seperate file. So the ABP user should add font-bundle in angular.json. ( under the 'yourProjectName' > 'architect' > 'build' > 'options' >'styles' )

// for  LeptonX Lite
```json
 {
    input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/font-bundle.rtl.css',
    inject: false,
    bundleName: 'font-bundle.rtl',
  },
  {
    input: 'node_modules/@volo/ngx-lepton-x.lite/assets/css/font-bundle.css',
    inject: false,
    bundleName: 'font-bundle',
  },
```

// for LeptonX
```json
 {
    input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/font-bundle.css',
    inject: false,
    bundleName: 'font-bundle',
  },
  {
    input: 'node_modules/@volosoft/ngx-lepton-x/assets/css/font-bundle.rtl.css',
    inject: false,
    bundleName: 'font-bundle.rtl',
  },
```

## Updated Side Menu Layout

In side menu layout, eThemeLeptonXComponents.Navbar has been changed to eThemeLeptonXComponents.Toolbar, and 
eThemeLeptonXComponents.Sidebar to eThemeLeptonXComponents.Navbar.

And also added new replaceable component like Logo Component, Language Component etc.

If you are using replaceable component system you can check [documentation](https://docs.abp.io/en/commercial/latest/themes/lepton-x/angular#customization).


## ng-zorro-antd-tree.css 

ng-zorro-antd-tree.css file should be in angular.json if the user uses AbpTree component or Abp-commercial. The ABP User should add this style definition on angular.json. ( under the 'yourProjectName' > 'architect' > 'build' > 'options' >'styles' ) 

{ "input": "node_modules/ng-zorro-antd/tree/style/index.min.css", "inject": false, "bundleName": "ng-zorro-antd-tree" },

