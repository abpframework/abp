# Upgrade Your Existing Projects to .NET 8 & ABP 8.0

A new .NET version was released on November 14, 2023 and ABP 8.0 RC.1 shipped based on .NET 8.0 just after Microsoft's .NET 8.0 release. Therefore, it's a good time to see what we need to do to upgrade our existing projects to .NET 8.0. 

Despite all the related dependency upgrades and changes made on the ABP Framework and ABP Commercial sides, we still need to make some changes. Let's see the required actions that need to be taken in the following sections.

## Installing the .NET 8.0 SDK

To get started with ASP.NET Core in .NET 8.0, you need to install the .NET 8 SDK. You can install it at [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). 

After installing the SDK & Runtime, you can upgrade your existing ASP.NET Core application to .NET 8.0.

## Updating the Target Framework

First, you need to update all your `*.csproj` files to support .NET 8. Find and replace all your `TargetFramework` definitions in the `*.csproj` files to support .NET 8.0:

```xml
<TargetFramework>net8.0</TargetFramework>
```

> This and all other changes mentioned in this article have already been done in the ABP Framework and ABP Commercial side, so you would not get any problems related to that.

## Updating Microsoft Package Versions

You are probably using some Microsoft packages in your solution, so you need to update them to the latest .NET 8.0 version. Therefore, update all `Microsoft.AspNetCore.*` and `Microsoft.Extensions.*` packages' references to `8.0.0`.

## Checking the Breaking Changes in .NET 8.0

As I have mentioned earlier in this article, on the ABP Framework & ABP Commercial sides all the related code changes have been made, so you would not get any error related to breaking changes introduced with .NET 8.0. However, you still need to check the [Breaking Changes in .NET 8.0 documentation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/8.0), because the breaking changes listed in this documentation still might affect you. Therefore, read them accordingly and make the related changes in your application, if needed.

## Update Your Global Dotnet CLI Tools (optional)

You can update the global dotnet tools to the latest version by running the `dotnet tool update` command. For example, if you are using EF Core, you can update your `dotnet-ef` CLI tool with the following command:

```bash
dotnet tool update dotnet-ef --global
```

## Installing/Restoring the Workloads (required for Blazor WASM & MAUI apps)

The `dotnet workload restore` command installs the workloads needed for a project or a solution. This command analyzes a project or solution to determine which workloads are needed and if you have a .NET MAUI or Blazor-WASM project, you can update your workloads by running the following command in a terminal:

```bash
dotnet workload restore
```

## Docker Image Updates

If you are using Docker to automate the deployment of applications, you also need to update your images. 

For example, you can update the ASP.NET Core image as follows:

```diff
- FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
+ FROM mcr.microsoft.com/dotnet/aspnet:9.0
```

You can check the related images from Docker Hub and update them accordingly:

* [https://hub.docker.com/_/microsoft-dotnet-aspnet/](https://hub.docker.com/_/microsoft-dotnet-aspnet/)
* [https://hub.docker.com/_/microsoft-dotnet-sdk/](https://hub.docker.com/_/microsoft-dotnet-sdk/)
* [https://hub.docker.com/_/microsoft-dotnet-runtime/](https://hub.docker.com/_/microsoft-dotnet-runtime/)

## Upgrading Your Existing Projects to ABP 8.0

Updating your application to ABP 8.0 is pretty straight-forward. You first need to upgrade the ABP CLI to version `8.0.0` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 8.0.0
````

**or install** it if you haven't before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 8.0.0
````

Then, you can use the `abp update` command to update all the ABP related NuGet and NPM packages in your solution:

```bash
abp update --version 8.0.0
```

Also, if you are using ABP Commercial, you can update the ABP Suite version with the following command:

```bash
abp suite update --version 8.0.0
```

After that, you need to check the migration guide documents, listed below:

* [ABP Framework 7.x to 8.0 Migration Guide](https://docs.abp.io/en/abp/8.0/Migration-Guides/Abp-8_0)
* [ABP Commercial 7.x to 8.0 Migration Guide](https://docs.abp.io/en/commercial/8.0/migration-guides/v8_0)

> Check these documents carefully and make the related changes in your solution to prevent errors.

## Final Words

That's it! These were all the related steps that need to be taken to upgrade your application to .NET 8 and ABP 8.0. Now, you can enjoy the .NET 8 & ABP 8.0 and benefit from the performance improvements and new features.

Happy Coding 🤗
