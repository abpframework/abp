# ABP Version 9.0 Migration Guide

This document is a guide for upgrading ABP v8.x solutions to ABP v9.0. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

> ABP upgraded to .NET 9.0, so you need to move your solutions to .NET 9.0 if you want to use the ABP 9.0. You can check the [Migrate from ASP.NET Core 8.0 to 9.0](https://learn.microsoft.com/en-us/aspnet/core/migration/80-90) documentation.

## Open-Source (Framework)



## PRO

> Please check the **Open-Source (Framework)** section before reading this section. The listed topics might affect your application and you might need to take care of them.

If you are a paid-license owner and using the ABP's paid version, then please follow the following sections to get informed about the breaking changes and apply the necessary ones:

### ABP Suite: Better Naming For Multiple Navigation Properties

> **Note:** As a developer, you don't need to make any changes in your solution regarding this change. We just wanted to highlight this change to let you know.

Prior to this version, when you defined multiple (same) navigation properties to same entity, then ABP Suite was renaming them with a duplicate number.

Consider the following scenerio for an example. If you have a book with an author and coauthor, prior to this version ABP Suite was creating a DTO class as below:

```csharp
public class BookWithNavigationPropertiesDto
{
    public BookDto Book { get; set; }

    public AuthorDto Author { get; set; }

    public AuthorDto Author1 { get; set; }
}
```

Notice, the since the book entity has two same navigation properties, ABP Suite renamed them with a duplicate number. In this version on, ABP Suite will ask you to define a propertyName for the navigationProperties and you'll be able to specify a meaningful name such as:

```csharp
public class BookWithNavigationPropertiesDto
{
    public BookDto Book { get; set; }

    public AuthorDto Author { get; set; }

    public AuthorDto Author1 { get; set; }
}
```

### CMS Kit Pro: Feedback Feature Improvements

In this version, we revised the **CMS Kit's Feedback Feature** and as a result, we made the following changes:

* `CmsKitProSettingGroupViewComponent` doesn't inject any service anymore and the design of the component has been updated. (no need for a change in your code, if you did not override the component)
* `Default.cshtml` and `Default.js` files (under the **Pages/Public/Shared/Components/PageFeedbacks/** directory) have been updated. (no need for a change in your code, if you did not override the files)
* `FeedbackUserId` property has been added to the `PageFeedback` entity, and that means you should create a new migration and apply it to your database.
* `PageFeedbackManager.CreateAsync` method now expecting an additional parameter: ***Guid feedbackUserId***. (no need for a change in your code, if you did not override or use this file)
* `PageFeedbackPublicAppService` has been updated due to saving the feedback user id. (no need for a change in your code, if you did not override or use this file)