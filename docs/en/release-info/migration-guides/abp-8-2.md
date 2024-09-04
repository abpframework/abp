# ABP Version 8.2 Migration Guide

This document is a guide for upgrading ABP v8.x solutions to ABP v8.2. There are some changes in this version that may affect your applications, please read it carefully and apply the necessary changes to your application.

## Updated target frameworks to net8.0 for template projects

`TargetFrameworks` of the following template projects are upgraded to **net8.0**:

* `*.Application.Contracts`
* `*.Domain.Shared`
* `*.Domain`
* `*.MongoDB`
* `*.HttpApi.Client`
* `*.Host.Shared` (for module template)

Before this version, all of the projects above were targeting multiple frameworks (**netstandard2.0**, **netstandard2.1** and **net8.0**), with this version, we started to only target **net8.0** for these template projects. Note that, all other shared libraries still target multiple frameworks.

> This change should not affect your pre-existing solutions and you don't need to make any changes in your application. See the PR for more info: https://github.com/abpframework/abp/pull/19565
## Upgraded AutoMapper to 13.0.1

In this version, **AutoMapper** library version upgraded to 13.0.1. See [the release notes of AutoMapper v13.0.1](https://github.com/AutoMapper/AutoMapper/releases/tag/v13.0.1) for more information.

## Added default padding to `.tab-content` class for Basic Theme

In this version, default padding (padding-top: 1.5rem and padding-bottom: 1.5rem) has been added to the tab-content for the Basic Theme. See [#19475](https://github.com/abpframework/abp/pull/19475) for more information.

## Moved members page directory for Blogging Module

With this version on, ABP Framework allows you to use single blog mode, without needing to define a blog and a prefix. With this change, the following namespace changes were done:
* `Volo.Blogging.Pages.Blog` -> `Volo.Blogging.Pages.Blogs`
* `Volo.Blogging.Pages.Members` -> `Volo.Blogging.Pages.Blogs.Members` (members folder)

> If you haven't overridden the pages above, then you don't need to make any additional changes. See [#19418](https://github.com/abpframework/abp/pull/19418) for more information.
## Removed `FlagIcon` property from the `ILanguageInfo`

The `FlagIcon` property has been removed from the `ILanguageInfo` interface since we removed the flag icon library in the earlier versions from all of our themes and none of them using it now.

If the flag icon has been specified while defining the localization languages, then it should be removed:

```diff
        Configure<AbpLocalizationOptions>(options =>
        {
-           options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi", "in"));
+          options.Languages.Add(new LanguageInfo("hi", "hi", "Hindi"));
             //...
        }
```

## Blazor Full-Stack Web UI

In this version, ABP Framework provides a new UI option called **Blazor Full-Stack WebApp**. We have already created an introduction/migration guide for you to check it: [Migrating to Blazor Web App](abp-8-2-blazor-web-app.md)

> Please read the documentation carefully if you are considering migrating your existing **Blazor** project to **Blazor WebApp**.

## Angular UI

In this version, the Angular UI has been updated to use the Angular version 17.3.0 and Nx version to 19.0.0. See the PR for more information: [#19915](https://github.com/abpframework/abp/pull/19915/)

## Session Management Infrastructure

The **Session Management** feature allows you to prevent concurrent login and manage user sessions.

In this version, a new entity called `IdentitySession` has been added to the framework and you should create a new migration and apply it to your database.