# ABP Framework 4.0 RC Has Been Published based on .NET 5.0!

Today, we have released the [ABP Framework](https://abp.io/) (and the [ABP Commercial](https://commercial.abp.io/)) `4.0.0-rc.1` that is based on the .NET 5.0. This blog post introduces the new features and important changes in the new version.

> **The planned release date for the [4.0.0 final](https://github.com/abpframework/abp/milestone/45) version is November 26, 2020**.

## Get Started with the 4.0 RC.1

If you want to try the version `4.0.0-rc.1` today, follow the steps below;

1) **Upgrade** the ABP CLI to the version `4.0.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 4.0.0-rc.1
````

**or install** if you haven't installed before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 4.0.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/3.3/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

## Migrating From 3.x to 4.0

The version 4.0 comes with some major changes including the migration from .NET Core 3.1 to .NET 5.0.

We've prepared a detailed [migration document](https://docs.abp.io/en/abp/4.0/Migration-Guides/Abp-4_0) to explain all the changes and the actions you need to take while upgrading your existing solutions.

## What's new with the ABP Framework 4.0

### The Blazor UI

The Blazor UI is now stable and officially supported.

#### Breaking Changes on the Blazor UI

TODO

## What's new with the ABP Commercial 3.3

TODO

## About the Next Release

The next feature version, `4.1.0`, will mostly focus on completing the missing documents, fixing bugs, performance optimizations and improving the Blazor UI features. The planned preview release date for the version `4.1.0` is December 10 and the final (stable) version release date is December 24.

Follow the [GitHub milestones](https://github.com/abpframework/abp/milestones) for all the planned ABP Framework version release dates.

## Feedback

Please check out the ABP Framework 4.0.0 RC and [provide feedback](https://github.com/abpframework/abp/issues/new) to help us to release a more stable version. **The planned release date for the [4.0.0 final](https://github.com/abpframework/abp/milestone/45) version is November 26**.
