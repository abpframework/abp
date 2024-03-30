# Quartz Background Worker Manager

[Quartz](https://www.quartz-scheduler.net/) is an advanced background worker manager. You can integrate Quartz with the ABP Framework to use it instead of the [default background worker manager](Background-Workers.md). ABP simply integrates quartz.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundWorkers.Quartz
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.BackgroundWorkers.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundWorkers.Quartz) NuGet package to your project:

   ````
   Install-Package Volo.Abp.BackgroundWorkers.Quartz
   ````

2. Add the `AbpBackgroundWorkersQuartzModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

> Quartz background worker integration provided `QuartzPeriodicBackgroundWorkerAdapter` to adapt `PeriodicBackgroundWorkerBase` and `AsyncPeriodicBackgroundWorkerBase` derived class. So, you can still fllow the [background workers document](Background-Workers.md) to define the background worker.

## Configuration

See [Configuration](Background-Jobs-Quartz#Configuration).

## Create a Background Worker

A background work is a class that derives from the `QuartzBackgroundWorkerBase` base class. for example. A simple worker class is shown below:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

We simply implemented the Execute method to write a log. The background worker is a **singleton by default**. If you want, you can also implement a [dependency interface](Dependency-Injection#DependencyInterfaces) to register it as another life cycle.

> Tips: Add identity to background workers is a best practice,because quartz distinguishes different jobs based on identity.

## Add to BackgroundWorkerManager

Default background workers are **automatically** added to the BackgroundWorkerManager when the application is **initialized**. You can set `AutoRegister` property value to `false`,if you want to add it manually:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        AutoRegister = false;
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

If you want to globally disable auto add worker, you can global disable via `AbpBackgroundWorkerQuartzOptions` options:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundWorkerQuartzOptions>(options =>
        {
            options.IsAutoRegisterEnabled = false;
        });
    }
}
```

## Advanced topics

### Customize ScheduleJob

Assume you have a worker executes every 10 minutes,but because server is unavailable for 30 minutes, 3 executions are missed. You want to execute all missed times after the server is available. You should define your background worker like this:

```csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).WithSimpleSchedule(s=>s.WithIntervalInMinutes(1).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires()).Build();

        ScheduleJob = async scheduler =>
        {
            if (!await scheduler.CheckExists(JobDetail.Key))
            {
                await scheduler.ScheduleJob(JobDetail, Trigger);
            }
        };
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
```

In the example we defined the worker execution interval to be 10 minutes and set `WithMisfireHandlingInstructionIgnoreMisfires`. we customized `ScheduleJob` and add worker to quartz only when the background worker does not exist.

### More

Please see Quartz's [documentation](https://www.quartz-scheduler.net/documentation/index.html) for more information.
