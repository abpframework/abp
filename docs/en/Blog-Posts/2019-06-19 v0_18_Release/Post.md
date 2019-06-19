# ABP CLI, New Templates & Other Features with the v0.18 Release

ABP v0.18 has been released with [70+ issues](https://github.com/abpframework/abp/milestone/16?closed=1) resolved and [500+ commits](https://github.com/abpframework/abp/compare/0.17.0.0...0.18.0) pushed.

## Web Site Changes

[abp.io](https://abp.io) web site is **completely renewed** to highlight the goals and important features of the ABP framework. Document & blog URLs are also changed:

- `abp.io/documents` moved to [docs.abp.io](https://docs.abp.io).
- `abp.io/blog` moved to [blog.abp.io](https://blog.abp.io).

## ABP CLI

ABP CLI (Command Line Interface) is a new global command line tool to perform some common operations for ABP based solutions. Main functions are;

* **Creating a new application** or module project.
* **Adding a new module** to an application.
* **Updating** all ABP related packages in a solution.

ABP CLI is now the preferred way to create a new project, while you can still download a new project from the [get started](https://abp.io/get-started) page.

### Usage

Install the ABP CLI using a command line window:

````bash
dotnet tool install -g Volo.Abp.Cli
````

Create a new application:

````bash
abp new Acme.BookStore
````

Add a module to an application:

````bash
abp add-module Volo.Blogging
````

Update all ABP related packages in a solution:

````bash
abp update
````

See [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for details.

## New Templates

In this release, we've renewed all startup templates. The main goal is to provide better startup templates based on Domain Driven Design layers those also allow to create tiered solutions (where Web and API layers can be physically separated). It also includes unit & integration test projects separated for different layers.

The image below shows the new startup project for an MVC application.

![mvc-template-solution](mvc-template-solution.png)

See the [startup templates document](https://docs.abp.io/en/abp/latest/Startup-Templates/Index) for details.

## Change Logs

Here are some other features and enhancements coming with this release:

* New data seed system with `IDataSeeder` service & `IDataSeedContributor` interface to allow a modular initial data seed system.
* New [Volo.Abp.Specifications](https://www.nuget.org/packages/Volo.Abp.Specifications) package.
* Improved MemoryDB implementation to serialize/deserialize objects stored in memory, so it provides more realistic infrastructure for mocking database in unit/integration tests.
* Added multi-language support for the docs module. Used it for the [ABP documentation](https://docs.abp.io).

See the [GitHub Release Notes](https://github.com/abpframework/abp/releases/tag/0.18.0) for all features, enhancements & bugfixes in this release.

## Road Map

One thing related to the ABP v1.0 release is .NET Core / ASP.NET Core 3.0 release. According to the [.NET Core road map](https://github.com/dotnet/core/blob/master/roadmap.md), 3.0 release has been scheduled for September 2019.

ASP.NET Core comes with big changes and features. As a big breaking change, it will [only run on .NET Core](https://github.com/aspnet/Announcements/issues/324) (dropping .net standard support), so it will not work with full .net framework anymore.

We had declared to release v1.0 in 2019 Q2. The main works we should do for v1.0 are;

* Fill the gaps in current features.
* Refactor & improve the current APIs.
* Fix known bugs.
* Complete the documentation & tutorials.

In addition to the work we should do, we are also considering to wait ASP.NET Core 3.0 release. Because, if we release ABP v1.0 before ASP.NET Core 3.0, we will have to release ABP v2.0 again in a short time and drop v1.0 support. So, we are considering to publish ABP v1.0 RC with ASP.NET Core 3.0 RC and align the final release date with Microsoft.

## Want to Contribute?

Thanks to the community for their support for ABP development. It is very appreciated. If you also want to contribute, see [this guide](https://github.com/abpframework/abp/blob/master/docs/en/Contribution/Index.md) as the beginning.