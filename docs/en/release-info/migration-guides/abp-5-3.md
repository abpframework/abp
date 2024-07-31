# ABP Version 5.3 Migration Guide

This document is a guide for upgrading ABP v5.2 solutions to ABP v5.3. There is a change in this version that may effect your applications, please read it carefully and apply the necessary changes to your application.

## Open-Source (Framework)

If you are using one of the open-source startup templates, then you can check the following sections to apply the related breaking changes:

### AutoMapper Upgraded to v11.0.1

AutoMapper library upgraded to **v11.0.1** in this version. So, you need to change your project's target SDK that use the **AutoMapper** library (typically your `*.Application` project). You can change it from `netstandard2.0` to `netstandard2.1` or `net6` if needed. Please see [#12189](https://github.com/abpframework/abp/pull/12189) for more info.

## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### Saas - Payment Module Dependency Improvements

* **Saas Module** no longer depends on the **Payment Module**. Projects that depend on only the **Saas module** but use payment/subscription features should also add `Volo.Payment.*` module packages.

* Solutions that include **Saas Module** and **Payment Module** together should configure `AbpSaasPaymentOptions` while migrating to v5.3 like below:

```csharp
Configure<AbpSaasPaymentOptions>(options =>
{
    options.IsPaymentSupported = true;
});