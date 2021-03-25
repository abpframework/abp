# ABP Framework 4.3 RC Has Been Published

We are super excited to announce the ABP Framework 4.3 RC (Release Candidate). Here, a list of highlights for this release;

* **CMS Kit** module initial release.
* **Blazor UI server-side** support.
* **Module extensibility** system for the Blazor UI.
* Angular UI **resource owner password** flow comes back.
* **Volo.Abp.EntityFrameworkCore.Oracle** package is now compatible with .NET 5.
* CLI support to easily add the **Basic Theme** into the solution.
* New **IInitLogger** service to write logs before dependency injection phase completed.
* Infrastructure for **multi-lingual entities**.

Beside the new features above, we've done many performance improvements, enhancements and bug fixes on the current features. See the [4.3 milestone](https://github.com/abpframework/abp/milestone/49) on GitHub for all changes made on this version.

This version was a big development journey for us; [150+ issues](https://github.com/abpframework/abp/issues?q=is%3Aopen+is%3Aissue+milestone%3A4.3-preview) resolved, [260+ PRs](https://github.com/abpframework/abp/pulls?q=is%3Aopen+is%3Apr+milestone%3A4.3-preview) merged and 1,600+ commits done only in the [main framework repository](https://github.com/abpframework/abp). **Thanks to the ABP Framework team and all the contributors.**

## The Migration Guide

We normally don't make breaking changes in feature versions. However, this version has some small **breaking changes** mostly related to Blazor UI WebAssembly & Server separation. **Please check the [migration guide](https://docs.abp.io/en/abp/4.3/Migration-Guides/Abp-4_3) before starting with the version 4.3**.

## Get Started With The 4.3 RC

If you want to try the version 4.3 today, follow the steps below;

1) **Upgrade** the ABP CLI to the version `4.3.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 4.3.0-rc.1
````

**or install** if you haven't installed before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 4.3.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/4.3/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

## What's New With The ABP Framework 4.3

### CMS Kit

CMS Kit was a module we were working for the last couple of months. It is usable now and we are releasing the initial version with this release.

### Blazor Server Side

TODO

### Blazor UI Module Extensibility

TODO

### Angular UI Resource Owner Password Flow

TODO

### Volo.Abp.EntityFrameworkCore.Oracle Package

TODO

### Add Basic Theme Into Your Solution

TODO

### IInitLogger

TODO

### Multi-Lingual Entities

TODO

### Other News

* [#7423](https://github.com/abpframework/abp/issues/7423) MongoDB repository base aggregation API.
* [#8163](https://github.com/abpframework/abp/issues/8163) Ignoring files on minification for MVC UI.
* [#7799](https://github.com/abpframework/abp/pull/7799) Added `RequiredPermissionName` to `ApplicationMenuItem` for MVC & Blazor UI to easily show/hide menu items based on user permissions. Also added `RequiredPermissionName` to `ToolbarItem` for the MVC UI for the same purpose. 
* [#7523](https://github.com/abpframework/abp/pull/7523) Add more bundle methods to the distributed cache.

See the [4.3 milestone](https://github.com/abpframework/abp/milestone/49) on GitHub for all changes made on this version.

## Feedback

Please check out the ABP Framework 4.3 RC and [provide feedback](https://github.com/abpframework/abp/issues/new) to help us to release a more stable version. **The planned release date for the [4.3.0 final](https://github.com/abpframework/abp/milestone/50) version is April 15, 2021**.