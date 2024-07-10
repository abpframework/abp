# ABP Version 7.3 Migration Guide

This document is a guide for upgrading ABP v7.2 solutions to ABP v7.3. There are a few changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Open-Source (Framework)

If you are using one of the open-source startup templates, then you can check the following sections to apply the related breaking changes:

### OpenIddict - Refactoring of `ClaimsPrincipal`

There are some changes that you might need to fix in your code. You can see the following list of the required changes:

* `AbpOpenIddictClaimDestinationsManager` was renamed as `AbpOpenIddictClaimsPrincipalManager`. 
* Use `AbpOpenIddictClaimsPrincipalManager.HandleAsync` instead of  `AbpOpenIddictClaimDestinationsManager.SetAsync`, which is removed.
* `AbpDefaultOpenIddictClaimDestinationsProvider` was renamed as `AbpDefaultOpenIddictClaimsPrincipalHandler`.
* `IAbpOpenIddictClaimDestinationsProvider` was renamed as `IAbpOpenIddictClaimsPrincipalHandler`.
* Use `IAbpOpenIddictClaimsPrincipalHandler.HandleAsync` instead of  `IAbpOpenIddictClaimDestinationsProvider.SetAsync`, which is removed.
* `AbpOpenIddictClaimDestinationsOptions` was renamed as `AbpOpenIddictClaimsPrincipalOptions`.

Please check [this PR](https://github.com/abpframework/abp/pull/16537) if you encounter any problems related to OpenIddict Module.

### Nonce attribute support for Content Security Policy (CSP)

ABP supports adding unique value to nonce attribute for script tags which can be used by Content Security Policy to determine whether or not a given fetch will be allowed to proceed for a given element. In other words, it provides a mechanism to execute only correct script tags with the correct nonce value. 

> See the [Security Headers](../../framework/ui/mvc-razor-pages/security-headers.md) documentation for more information.

This feature comes with a small restriction. If you use any C# code used inside the script tag, it may cause errors (Because a new `NonceScriptTagHelper` has been added, and it replaces script tags in the HTML contents). 

For example, `<script @string.Empty></script>` will no longer work. However, you can use the C# code for an attribute of script tag, for example, `<script src="@string.Empty"></script>` is completely valid and won't cause any problem.

> Note: You should not use any C# code used inside the script tag, even if you don't use this feature. Because it might cause errors.

### Angular UI
We would like  to inform you that ABP version 7.3 uses Angular version 16. Please migrate your applications to Angular 16. [Update angular](https://update.angular.io/)

## PRO

There is not a single breaking-change that affects the pro modules, nevertheless, please check the **Open-Source (Framework)** section above to ensure, there is not a change that you need to make in your application.