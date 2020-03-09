# Quartz Background Worker Manager

[Quartz](https://www.quartz-scheduler.net/) is an advanced background worker manager. You can integrate Quartz with the ABP Framework to use it instead of the [default background worker manager](Background-Worker.md). ABP simply integrates quartz.

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

### Configuration

See [Configuration](Background-Jobs-Quartz#Configuration).

### Create a Background Worker

A background work is a class that derives from the `QuartzBackgroundWorkerBase` base class. for example. A simple worker class is shown below:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().Build();
        Trigger = TriggerBuilder.Create().StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

We simply implemented the Execute method to write a log. The background worker is a **singleton by default**. If you want, you can also implement a [dependency interface](Dependency-Injection#DependencyInterfaces) to register it as another life cycle.

### More

Please see Quartz's [documentation](https://www.quartz-scheduler.net/documentation/index.html) for more information.
