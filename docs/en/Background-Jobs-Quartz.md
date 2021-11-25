# Quartz Background Job Manager

[Quartz](https://www.quartz-scheduler.net/) is an advanced background job manager. You can integrate Quartz with the ABP Framework to use it instead of the [default background job manager](Background-Jobs.md). In this way, you can use the same background job API for Quartz and your code will be independent of Quartz. If you like, you can directly use Quartz's API, too.

> See the [background jobs document](Background-Jobs.md) to learn how to use the background job system. This document only shows how to install and configure the Quartz integration.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundJobs.Quartz
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.BackgroundJobs.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.Quartz) NuGet package to your project:

   ````
   Install-Package Volo.Abp.BackgroundJobs.Quartz
   ````

2. Add the `AbpBackgroundJobsQuartzModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## Configuration

Quartz is a very configurable library,and the ABP framework provides `AbpQuartzOptions` for this. You can use the `PreConfigure` method in your module class to pre-configure this option. ABP will use it when initializing the Quartz module. For example:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Properties = new NameValueCollection
            {
                ["quartz.jobStore.dataSource"] = "BackgroundJobsDemoApp",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.serializer.type"] = "json",
                ["quartz.dataSource.BackgroundJobsDemoApp.connectionString"] = configuration.GetConnectionString("Quartz"),
                ["quartz.dataSource.BackgroundJobsDemoApp.provider"] = "SqlServer",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
            };
        });
    }
}
````

Starting from ABP 3.1 version, we have added `Configurator` to `AbpQuartzOptions` to configure Quartz. For example:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Configurator = configure =>
            {
                configure.UsePersistentStore(storeOptions =>
                {
                    storeOptions.UseProperties = true;
                    storeOptions.UseJsonSerializer();
                    storeOptions.UseSqlServer(configuration.GetConnectionString("Quartz"));
                    storeOptions.UseClustering(c =>
                    {
                        c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                        c.CheckinInterval = TimeSpan.FromSeconds(10);
                    });
                });
            };
        });
    }
}
````

> You can choose the way you favorite to configure Quaratz.

Quartz stores job and scheduling information **in memory by default**. In the example, we use the pre-configuration of [options pattern](Options.md) to change it to the database. For more configuration of Quartz, please refer to the Quartz's [documentation](https://www.quartz-scheduler.net/).

## Exception handling

### Default exception handling strategy

When an exception occurs in the background job,ABP provide the **default handling strategy** retrying once every 3 seconds, up to 3 times. You can change the retry count and retry interval via `AbpBackgroundJobQuartzOptions` options:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryCount = 1;
            options.RetryIntervalMillisecond = 1000;
        });
    }
}
```

### Customize exception handling strategy

You can customize the exception handling strategy via `AbpBackgroundJobQuartzOptions` options:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryStrategy = async (retryIndex, executionContext, exception) =>
            {
                // customize exception handling
            };
        });
    }
}
```