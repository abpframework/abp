# Hangfire Background Worker Manager

[Hangfire](https://www.hangfire.io/) is an advanced background jobs and worker manager. You can integrate Hangfire with the ABP Framework to use it instead of the [default background worker manager](Background-Workers.md).

The major advantage is that you can use the same server farm to manage your Background Jobs and Workers, as well as leverage the advanced scheduling that is available from Hangfire for [Recurring Jobs](https://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html?highlight=recurring), aka Background Workers.

## Installation

It is suggested to use the [ABP CLI](CLI.md) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundWorkers.Hangfire
````

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.BackgroundWorkers.Hangfire](https://www.nuget.org/packages/Volo.Abp.BackgroundWorkers.Hangfire) NuGet package to your project:

   ````
   Install-Package Volo.Abp.BackgroundWorkers.Hangfire
   ````

2. Add the `AbpBackgroundWorkersHangfireModule` to the dependency list of your module:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersHangfireModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

> Hangfire background worker integration provides an adapter `HangfirePeriodicBackgroundWorkerAdapter` to automatically load any `PeriodicBackgroundWorkerBase` and `AsyncPeriodicBackgroundWorkerBase` derived classes as `IHangfireBackgroundWorker` instances. This allows you to still to easily switch over to use Hangfire as the background manager even you have existing background workers that are based on the [default background workers implementation](Background-Workers.md).

## Create a Background Worker

`HangfireBackgroundWorkerBase` is an easy way to create a background worker.

```` csharp
public class MyLogWorker : HangfireBackgroundWorkerBase
{
    public MyLogWorker()
    {
        RecurringJobId = nameof(MyLogWorker);
        CronExpression = Cron.Daily();
    }

    public override Task DoWorkAsync()
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

* **RecurringJobId** Is an optional parameter, see [Hangfire document](https://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html)
* **CronExpression** Is a CRON expression, see [CRON expression](https://en.wikipedia.org/wiki/Cron#CRON_expression)

> You can directly implement the `IHangfireBackgroundWorker`, but `HangfireBackgroundWorkerBase` provides some useful properties like Logger.

### UnitOfWork

For use with `UnitOfWorkAttribute`, you need to define an interface for worker:

```csharp
public interface IMyLogWorker : IHangfireBackgroundWorker
{
}

[ExposeServices(typeof(IMyLogWorker))]
public class MyLogWorker : HangfireBackgroundWorkerBase, IMyLogWorker
{
    public MyLogWorker()
    {
        RecurringJobId = nameof(MyLogWorker);
        CronExpression = Cron.Daily();
    }

    [UnitOfWork]
    public override Task DoWorkAsync()
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
```

## Register BackgroundWorkerManager

After creating a background worker class, you should add it to the `IBackgroundWorkerManager`. The most common place is the `OnApplicationInitialization` method of your module class:

```` csharp
[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class MyModule : AbpModule
{
    public override void OnApplicationInitialization(
        ApplicationInitializationContext context)
    {
        context.AddBackgroundWorker<MyLogWorker>();

        //If the interface is defined
        //context.AddBackgroundWorker<IMyLogWorker>(); 
    }
}
````

`context.AddBackgroundWorker(...)` is a shortcut extension method for the expression below:

```` csharp
context.ServiceProvider
    .GetRequiredService<IBackgroundWorkerManager>()
    .Add(
        context
            .ServiceProvider
            .GetRequiredService<MyLogWorker>()
    );
````

So, it resolves the given background worker and adds to the `IBackgroundWorkerManager`.

While we generally add workers in OnApplicationInitialization, there are no restrictions on that. You can inject IBackgroundWorkerManager anywhere and add workers at runtime. Background worker manager will stop and release all the registered workers when your application is being shut down.