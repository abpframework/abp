# ABP Version 6.0 Migration Guide

This document is a guide for upgrading ABP v5.3 solutions to ABP v6.0. There is a change in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Added IsActive property

`IsActive` property is added to `IUserData`. This property is set to **true** by default. **Cmskit** and **Blog** modules are affected by this change. You need to add new migration to your existing application if you are using any of these modules. Please see [#11417](https://github.com/abpframework/abp/pull/11417) for more info.

## Default behavior change in MultiTenancyMiddlewareErrorPageBuilder

If you have customized the `MultiTenancyMiddlewareErrorPageBuilder` of `AbpMultiTenancyOptions`, the pipeline now returns **true** to stop the pipeline as the default behavior. See [AbpMultiTenancyOptions: Handle inactive and non-existent tenants](https://github.com/abpframework/abp/blob/dev/docs/en/Multi-Tenancy.md#abpmultitenancyoptions-handle-inactive-and-non-existent-tenants) for more info.

## Migrating to LeptonX Lite

LeptonX Lite is now being introduced and you can follow the guides below to migrate your existing applications:

- [Migrating to LeptonX MVC UI](../themes/LeptonXLite/AspNetCore.md)
- [Migrating to LeptonX Angular UI](../themes/LeptonXLite/angular.md)
- [Migrating to LeptonX Blazor UI](../themes/LeptonXLite/blazor.md)

## Migrating to OpenIddict

After the [announcement of plan to replace the IdentityServer](https://github.com/abpframework/abp/issues/11989), we have successfully implemented [Openiddict](https://github.com/openiddict/openiddict-core) as a replacement for IdentityServer4 as OpenID-Provider. You can follow [Migrating Identity Server to OpenIddict guide](./IdentityServer_To_OpenIddict.md).

## See Also

* [Official blog post for the 6.0 release](https://blog.abp.io/abp/ABP.IO-Platform-6.0-RC-Has-Been-Published)
