# Hangfire Background Job Manager

[Hangfire](https://www.hangfire.io/) is an advanced background job manager. You can integrate Hangfire with the ABP Framework to use it instead of the [default background job manager](Background-Jobs.md). In this way, you can use the same background job API for Hangfire and your code will be independent of Hangfire. If you like, you can directly use Hangfire's API, too.

> See the [background jobs document](Background-Jobs.md) to learn how to use the background job system. This document only shows how to install and configure the Hangfire integration.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundJobs.HangFire
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.BackgroundJobs.HangFire](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.HangFire) NuGet package to your project:

   ````
   Install-Package Volo.Abp.BackgroundJobs.HangFire
   ````

2. Add the `AbpBackgroundJobsHangfireModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsHangfireModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## Configuration

TODO...