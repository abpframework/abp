# Upgrading the ABP Framework

This document explains how to upgrade your existing solution when a new ABP Framework version is published.

## ABP UPDATE Command

ABP Framework & module ecosystem consist of hundreds of NuGet and NPM packages. It would be tedious to manually update all these packages to upgrade your application.

[ABP CLI](CLI.md) provides a handy command to update all the ABP related NuGet and NPM packages in your solution with a single command:

````bash
abp update
````

Run this command in the terminal while you are in the root folder of your solution.

> If your solution has the Angular UI, you probably have `aspnet-core` and `angular` folders in the solution. Run this command in the parent folder of these two folders.

### Database Migrations

> Warning: Be careful if you are migrating your database since you may have data loss in some cases. Carefully check the generated migration code before executing it. It is suggested to take a backup of your current database.

When you upgrade to a new version, it is good to check if there is a database schema change and upgrade your database schema if your database provider is **Entity Framework Core**;

* Use `Add-Migration "Upgraded_To_Abp_4_1"` or a similar command in the Package Manager Console (PMC) to create a new migration (Set the `EntityFrameworkCore` as the Default project in the PMC and `.DbMigrator` as the Startup Project in the Solution Explorer, in the Visual Studio).
* Run the `.DbMigrator` application to upgrade the database and seed the initial data.

If `Add-Migration` generates an empty migration, you can use `Remove-Migration` to delete it before executing the `.DbMigrator`.

## The Blog Posts & Guides

Whenever you upgrade your solution, it is strongly suggested to check the [ABP BLOG](https://blog.abp.io/) to learn the new features and changes coming with the new version. We regularly publish posts and write these kind of changes.

### Migration Guides

We prepare migration guides if the new version brings breaking changes for existing applications. See the [Migration Guides](Migration-Guides/Index.md) page for all the guides.

### Upgrading the Startup Template

Sometimes we introduce new features/changes that requires to **make changes in the startup template**. We already implement the changes in the startup template for new applications. However, in some cases you need to manually make some minor changes in your existing solution.

It is not practical to document the necessary changes line by line. In this case, we suggest you to create an example solution, one with your existing version and one with the new version and compare them using a diff tool. You can [see this guide](Migration-Guides/Upgrading-Startup-Template.md) to learn how you can do it using WinMerge application.

## Semantic Versioning & Breaking Changes

We are working hard to keep the semantic versioning rules, so you don't get breaking changes for minor (feature) versions like 3.1, 3.2, 3.3...

However, there are some cases we may introduce breaking changes in feature versions too;

* ABP has many integration packages and sometimes the integrated libraries/frameworks releases major versions and makes breaking changes. In such cases, we carefully check these changes and decide to upgrade the integration package or not. If the impact of the change is relatively small, we update the integration package and explain the change in the release blog post. In such a case, if you've used this integration package, you should follow the instructions explained in the blog post. If the change may break many applications and not easy to fix, we decide to wait this upgrade until the next major ABP Framework release.
* Sometimes we have to make breaking change to fix a major bug or usage problem. In this case, we think that developer already can't properly use that feature, so no problem to fix it with a breaking change. In such cases, the feature will generally be a rarely used feature. Again, we try to keep the impact minimum.

## Preview Releases & Nightly Builds

Preview releases and nightly builds can help you to try new features and adapt your solution earlier than a new stable release.

* [Preview releases](Previews.md) are typically published ~2 weeks before a minor (feature) version (our minor version development cycle is about ~4 weeks).
* [Nightly builds](Nightly-Builds.md) are published in every night (except weekends) from the development branch. That means you can try the previous day's development.

Refer to the their documents to learn details about these kind of releases.