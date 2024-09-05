# ABP Version 8.3 Migration Guide

This document is a guide for upgrading ABP v8.x solutions to ABP v8.3. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Open-Source (Framework)

### Added `AdminUserName` parameter to `IIdentityDataSeeder.SeedAsync` method

Prior to this version, `IIdentityDataSeeder.SeedAsync` was used to seed the initial user data with the `admin` as the username, to be able to easily test all applications with all permissions granted.

In this version on, we allow you to set the default **adminUserName**. So, you can set the default user name to any name you want or leave it as it is and let the username be `admin` as before.

For example, in the `MyProjectNameDbMigrationService.SeedDataAsync` method (under the domain project's **Data** folder), you can pass the username as below:

```diff
    private async Task SeedDataAsync(Tenant? tenant = null)
    {
        Logger.LogInformation($"Executing {(tenant == null ? "host" : tenant.Name + " tenant")} database seed...");

        await _dataSeeder.SeedAsync(new DataSeedContext(tenant?.Id)
            .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
            .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue)
+           .WithProperty(IdentityDataSeedContributor.AdminUserNamePropertyName, "your-admin-user-name")
        );
    }
```

Normally, you don't need to make any changes in your code related to this change. However, if you used the `IIdentityDataSeeder.SeedAsync` method in your application, now also gets additional (and optional) `adminUserName` parameter that you can pass the value on.

See the PR for more info: https://github.com/abpframework/abp/pull/20131/

### Docs Module: Improvements & Single Project Mode Support

In this version, we made some improvements in the [Docs Module](../../modules/docs.md) and introduced single project mode, which allows you to use a single project name for all of your documents. 

While implementing this new feature, we made some small changes to the existing classes. For example, the constructor methods of the following classes have been changed, and need to be updated in your code, if you use them:

* `TreeTagHelper.cs`: https://github.com/abpframework/abp/pull/19419/files#diff-e0fb91c1b564d61dedf9dfc60d08e7e0af57b433797fcb3bd664f3fd4768ea0d

* `MarkdownDocumentToHtmlConverter.cs`: https://github.com/abpframework/abp/pull/19419/files#diff-948e25b2d8851576888f8053b5f2c9416e23576aff95a88ef8ec8ca2841b3622

See the PR for more info: https://github.com/abpframework/abp/pull/19419

### Updated `datatables.net` to 2.0.2

In this version, the `datatables.net` library version was updated from v1.11.4 to v2.0.2. We made all related changes at the framework level and updated the dependency and its resource mapping configurations. Also, we made it backward compatible so it should not directly affect your application.

However, notice this package is used by the `@abp/aspnetcore.mvc.ui.theme.shared` package, which is used by the official themes. Therefore, if you used any methods from the **datatables.net** library it might be already deprecated or removed. 

See the PR for more info: https://github.com/abpframework/abp/pull/19340

### Angular UI

We wish to formally inform you that ABP version 8.3 has been upgraded to incorporate Angular version 18. Consequently, we recommend migrating your application to [Angular v18](https://angular.dev/update-guide) to ensure compatibility.

## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### Added RegEx Support to CMS Kit's (PRO) URL Forwarding System

ABP provides a [URL Forwarding System](../../modules/cms-kit-pro/url-forwarding.md), which allows you to create URLs that redirect to other pages or external websites. Prior to this version, the URL Forwarding System was only working for static URLs and didn't support RegEx. In this version, we introduced the RegEx support for the URL Forwarding System and now you can pass regular expressions for not static URLs.

Since a new property has been added to the `CmsShortenedUrl` entity, you should create a new migration and apply it to your database.

> After creating a new migration and applying it to your database, typically you don't need to make any changes, however, if you have overridden a page/method or class related to the URL Forwarding System, you might need to update it accordingly. For this purpose, you can get the source code of the [CMS Kit Module's](../../modules/cms-kit-pro/index.md) and update the related parts in your application.