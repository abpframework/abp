# ABP Platform 5.0 Beta 1 Has Been Released

Today, we are excited to release the [ABP Framework](https://abp.io/) and the [ABP Commercial](https://commercial.abp.io/) version **5.0 Beta 1**. This blog post introduces the new features and important changes in this new version.

> **The planned release date for the [5.0.0 Release Candidate](https://github.com/abpframework/abp/milestone/51) version is November, 2021**.

## Get Started with the 5.0 Beta 1

If you want to try the version 5.0.0 today, follow the steps below;

1) **Upgrade** the ABP CLI to the version `5.0.0-beta.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 5.0.0-beta.1
````

**or install** if you haven't installed before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 5.0.0-beta.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

### Migration Notes & Breaking Changes

This is a major version and there are some breaking changes and upgrade steps. Please see the [migration document](https://docs.abp.io/en/abp/5.0/Migration-Guides/Abp-5_0) for all the details.

Here, a list of important breaking changes in this version:

* Upgraded to .NET 6.0-rc.1, so you need to move your solution to .NET 6.0 if you want to use the ABP 5.0.
* `IRepository` doesn't inherit from `IQueryable` anymore. It was already made obsolete in 4.2.
* Removed NGXS and states from the Angular UI.
* Removed gulp dependency from the MVC / Razor Pages UI in favor of `abp install-libs` command of ABP CLI.

You can see all [the closed issues and pull request](https://github.com/abpframework/abp/releases/tag/5.0.0-beta.1) on GitHub.

## What's new with Beta 1?

In this section, I will introduce some major features released with beta 1.

### Static (Generated) Client Proxies for C# and JavaScript

TODO
