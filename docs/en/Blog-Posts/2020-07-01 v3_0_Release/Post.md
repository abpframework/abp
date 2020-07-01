# ABP Framework v3.0 Has Been Released

We are excited to announce that the **ABP Framework** & and the **ABP Commercial** version 3.0 have been released. As different than the regular release lifecycles, which are 2-weeks, this version has taken 4-weeks with **119 [issues](https://github.com/abpframework/abp/issues?q=is%3Aopen+is%3Aissue+milestone%3A3.0)** closed, **89 [pull requests](https://github.com/abpframework/abp/pulls?q=is%3Aopen+is%3Apr+milestone%3A3.0)** merged and **796 commits** done just in the main framework [repository](https://github.com/abpframework/abp). See the [GitHub release notes](https://github.com/abpframework/abp/releases/tag/3.0.0) for a detailed change log.

Since this is a **major version**, it also includes some **breaking changes**. Don't panic, the changes are easy to adapt and will be explained below.

## What's New with the ABP Framework 3.0?

You can see all the changes on the [GitHub release notes](https://github.com/abpframework/abp/releases/tag/2.9.0). This post will only cover the important features/changes.

### Angular 10!

Angular version 10 has just been [released](https://blog.angular.io/version-10-of-angular-now-available-78960babd41) and we've immediately migrated the [startup templates](https://docs.abp.io/en/abp/latest/Startup-Templates/Application) to Angular 10! So, when you [create a new solution](https://abp.io/get-started) with the Angular UI, you will take the advantage of the new Angular.

We've prepared a [migration guide](https://github.com/abpframework/abp/blob/dev/docs/en/UI/Angular/Migration-Guide-v3.md) for the projects created an older version and want to migrate to Angular 10.

### The Oracle Integration Package

We had created [an integration package](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.Oracle.Devart) for the Oracle for EF Core based applications using the Devart's library since the official Oracle EF Core package was not supporting the EF Core 3.1. It now supports as a [beta release](https://www.nuget.org/packages/Oracle.EntityFrameworkCore/3.19.0-beta1). While it is in beta, we've created [the integration package](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.Oracle), so you can use it in your application.

See [the documentation](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Oracle) for details.

### Azure BLOB Storage Provider

We had created a [BLOB storing system](https://docs.abp.io/en/abp/latest/Blob-Storing) in the previous version. This release introduces the Azure BLOB Storage integration. See [the documentation](https://docs.abp.io/en/abp/latest/Blob-Storing-Azure).

### Distributed Cache Bulk Operations & the New Redis Cache Package

The [standard IDistributeCache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) interface of the ASP.NET Core doesn't contain **bulk operations**, like setting multiple items with a single method/server call. ABP Framework introduces new methods those can be used for bulk operations on the ABP's `IDistributedCache<T>` interface:

* GetManyAsync / GetMany
* SetManyAsync / SetMany

Then we needed to implement these new methods for Redis cache and [had to create](https://github.com/abpframework/abp/issues/4483) a Redis integration package which extends the Microsoft's implementation.

These methods are also used by the ABP Framework to cache settings, features and permissions for a user/role/tenant and brings a **significant performance improvement**.

See the [caching document](https://docs.abp.io/en/abp/latest/Caching) for details.

### Embedded Files Manifest Support for the Virtual File System

Virtual File System now supports to use `GenerateEmbeddedFilesManifest` in your projects to add the real file/directory structure of your embedded resources in the compiled file. So, you can now access to the files without any file name restriction (previously, some special chars like `.` in the directory names was a problem in some cases)

See [the documentation](https://docs.abp.io/en/abp/latest/Virtual-File-System) to learn how to take the advantage of new system.

### New Samples

Based on the requests from the community, we've prepared two new sample applications:

* [StoredProcedureDemo](https://github.com/abpframework/abp-samples/tree/master/StoredProcedureDemo) demonstrates how to call stored procedures, views and functions inside a custom repository.
* [OrganizationUnitSample](https://github.com/abpframework/abp-samples/tree/master/OrganizationUnitSample) shows how to use the organization unit system of the [Identity module](https://docs.abp.io/en/abp/latest/Modules/Identity) for your entities.

### Others

See the [GitHub release notes](https://github.com/abpframework/abp/releases/tag/3.0.0) for others updates.

## What's New with the ABP Commercial 3.0?

In addition to all the features coming with the ABP Framework, the ABP Commercial has additional features with this release, as always. This section covers the [ABP Commercial](https://commercial.abp.io/) highlights in the version 2.9.

...

## About the Next Versions

We will continue to release a new minor/feature version in every two weeks. So, the next expected release date is **2020-07-16**, for the version **3.1**.

In the next few versions, we will be focused on the **Blazor UI**, as promised on [the road map](https://docs.abp.io/en/abp/latest/Road-Map). We will continue to improve the documentation, create samples, add other new features and enhancements. Follow the [ABP Framework Twitter account](https://twitter.com/abpframework) for the latest news...

## Bonus: Articles!

Beside developing our products, our team are constantly writing articles/tutorials on various topics. You may want to check the latest articles:

* [What is New in Angular 10?](https://volosoft.com/blog/what-is-new-in-angular-10)
* [Real-Time Messaging In A Distributed Architecture Using ABP, SignalR & RabbitMQ](https://volosoft.com/blog/RealTime-Messaging-Distributed-Architecture-Abp-SingalR-RabbitMQ)
* [How to Use Attribute Directives to Avoid Repetition in Angular Templates](https://volosoft.com/blog/attribute-directives-to-avoid-repetition-in-angular-templates)