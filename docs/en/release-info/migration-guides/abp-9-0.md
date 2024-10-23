# ABP Version 9.0 Migration Guide

This document is a guide for upgrading ABP v8.x solutions to ABP v9.0. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

> ABP upgraded to .NET 9.0, so you need to move your solutions to .NET 9.0 if you want to use the ABP 9.0. You can check the [Migrate from ASP.NET Core 8.0 to 9.0](https://learn.microsoft.com/en-us/aspnet/core/migration/80-90) documentation.

## Open-Source (Framework)

### Upgraded to .NET 9.0

We've upgraded ABP to .NET 9.0, so you need to move your solutions to .NET 9.0 if you want to use ABP 9.0. You can check Microsoft’s [Migrate from ASP.NET Core 8.0 to 9.0](https://learn.microsoft.com/en-us/aspnet/core/migration/80-90) documentation, to see how to update an existing ASP.NET Core 8.0 project to ASP.NET Core 9.0.

After updating your solution to .NET 9.0, you should apply the following steps:

* Change the `app.UseStaticFiles()` to `app.MapAbpStaticAssets()` in the module classes of your host applications.
    * Some JavaScript/CSS/Images files exist in the Virtual File System, but ASP NET Core 9's `MapStaticAssets` can't handle them. This is why we created the **MapAbpStaticAssets**.
* Upgrade the Microsoft packages (also other packages) for .NET 9.0. You can check the [Directory.Packages.props](https://github.com/abpframework/abp/blob/rel-9.0/Directory.Packages.props) file for package versions and update the necessary ones.

### Made the IdentitySession Entity Extensible & Updated the MaxIpAddressesLength

In this version, we made the `IdentitySession` entity extensible and as a result of that, you should be aware of the changes explained below:

* `IdentitySession` entity inherits from **AggregateRoot** instead of **BasicAggregateRoot**. Here is the PR for the related change: [#20771](https://github.com/abpframework/abp/pull/20771)
* `IdentitySession` entity's **MaxIpAddress** property now allows up to 2048 characters. You can check the related PR, [here](https://github.com/abpframework/abp/pull/20819).

You should create a new migration and apply it to your database for the changes explained above.

### Removed Auditing Properties From the `OpenIddictAuthorization` and `OpenIddictToken` Entities

In this version, we removed the auditing properties from the `OpenIddictAuthorization` and `OpenIddictToken` entities. Now, these entities are inherited from `AggregateRoot` instead of `FullAuditedAggregateRoot`.

> See the PR, if you need further information: https://github.com/abpframework/abp/pull/20671

Since the auditing properties are removed, you should create a new migration and apply it to your database.

### Updated Method Signature of `AbpCrudPageBase`

The method signature of [framework/src/Volo.Abp.BlazoriseUI/AbpCrudPageBase.cs](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.BlazoriseUI/AbpCrudPageBase.cs) has been changed as follows:

```diff
- IEnumerable<TableColumn> GetExtensionTableColumns(string moduleName, string entityType) 
+ Task<List<TableColumn>> GetExtensionTableColumnsAsync(string moduleName, string entityType)
```

### Removed React Native Mobile Option From Open Source Templates

In this version, we removed the **React Native** mobile option from the open source templates due to maintaining reasons. We updated the related documents and the ABP CLI (both old & new CLI) for this change, and with v9.0, you will not be able to create a free template with react-native as the mobile option.

> **Note:** Pro templates still provide the **React Native** as the mobile option and we will continue supporting it.

If you want to access the open-source React-Native template, you can visit the **abp-archive** repository: https://github.com/abpframework/abp-archive

## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### ABP Suite: Better Naming For Multiple Navigation Properties

> **Note:** As a developer, you don't need to make any changes in your solution regarding this change. We just wanted to highlight this change and let you know.

Prior to this version, when you defined multiple (same) navigation properties to same entity, then ABP Suite was renaming them with a duplicate number.

Consider the following scenario for an example: If you have a book with an author and coauthor, prior to this version ABP Suite was creating a DTO class as below:

```csharp
public class BookWithNavigationPropertiesDto
{
    public BookDto Book { get; set; }

    public AuthorDto Author { get; set; }

    public AuthorDto Author1 { get; set; }
}
```

Notice, that since the book entity has two same navigation properties, ABP Suite renamed them with a duplicate number. In this version, ABP Suite will ask you to define a propertyName for the **navigation properties** and you'll be able to specify a meaningful name such as:

```csharp
public class BookWithNavigationPropertiesDto
{
    public BookDto Book { get; set; }

    public AuthorDto Author { get; set; }

    //used the specified property name
    public AuthorDto CoAuthor { get; set; }
}
```

### CMS Kit Pro: Feedback Feature Improvements

In this version, we revised the **CMS Kit's Feedback Feature** and as a result, we made the following changes:

* `CmsKitProSettingGroupViewComponent` doesn't inject any service anymore and the design of the component has been updated. (no need for a change in your code, if you did not override the component)
* `Default.cshtml` and `Default.js` files (under the **Pages/Public/Shared/Components/PageFeedbacks/** directory) have been updated. (no need for a change in your code, if you did not override the files)
* `FeedbackUserId` property has been added to the `PageFeedback` entity, and that means you should create a new migration and apply it to your database.
* `PageFeedbackManager.CreateAsync` method now expecting an additional parameter: ***Guid feedbackUserId***. (no need for a change in your code, if you did not override or use this file)
* `PageFeedbackPublicAppService` has been updated due to saving the feedback user id. (no need for a change in your code, if you did not override or use this file)
