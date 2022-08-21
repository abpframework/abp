# Background Workers

## Introduction

Background workers are simple independent threads in the application running in the background. Generally, they run periodically to perform some tasks. Examples;

* A background worker can run periodically to **delete old logs**.
* A background worker can run periodically to **determine inactive users** and **send emails** to get users to return to your application.


## Create a Background Worker

A background worker should directly or indirectly implement the `IBackgroundWorker` interface.

> A background worker is inherently [singleton](Dependency-Injection.md). So, only a single instance of your worker class is instantiated and run.

### BackgroundWorkerBase

`BackgroundWorkerBase` is an easy way to create a background worker.

````csharp
public class MyWorker : BackgroundWorkerBase
{
    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        //...
    }

    public override Task StopAsync(CancellationToken cancellationToken = default)
    {
        //...
    }
}
````

Start your worker in the `StartAsync` (which is called when the application begins) and stop in the `StopAsync` (which is called when the application shuts down).

> You can directly implement the `IBackgroundWorker`, but `BackgroundWorkerBase` provides some useful properties like `Logger`.

### AsyncPeriodicBackgroundWorkerBase

Assume that we want to make a user passive, if the user has not logged in to the application in last 30 days. `AsyncPeriodicBackgroundWorkerBase` class simplifies to create periodic workers, so we will use it for the example below:

````csharp
public class PassiveUserCheckerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PassiveUserCheckerWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory
        ) : base(
            timer, 
            serviceScopeFactory)
    {
        Timer.Period = 600000; //10 minutes
    }

    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Starting: Setting status of inactive users...");

        //Resolve dependencies
        var userRepository = workerContext
            .ServiceProvider
            .GetRequiredService<IUserRepository>();

        //Do the work
        await userRepository.UpdateInactiveUserStatusesAsync();

        Logger.LogInformation("Completed: Setting status of inactive users...");
    }
}
````

* `AsyncPeriodicBackgroundWorkerBase` uses the `AbpTimer` (a thread-safe timer) object to determine **the period**. We can set its `Period` property in the constructor.
* It required to implement the `DoWorkAsync` method to **execute** the periodic work.
* It is a good practice to **resolve dependencies** from the `PeriodicBackgroundWorkerContext` instead of constructor injection. Because `AsyncPeriodicBackgroundWorkerBase` uses a `IServiceScope` that is **disposed** when your work finishes.
* `AsyncPeriodicBackgroundWorkerBase` **catches and logs exceptions** thrown by the `DoWorkAsync` method.


## Register Background Worker

After creating a background worker class, you should add it to the `IBackgroundWorkerManager`. The most common place is the `OnApplicationInitializationAsync` method of your module class:

````csharp
[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class MyModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<PassiveUserCheckerWorker>();
    }
}
````

`context.AddBackgroundWorkerAsync(...)` is a shortcut extension method for the expression below:

````csharp
await context.ServiceProvider
    .GetRequiredService<IBackgroundWorkerManager>()
    .AddAsync(
        context
            .ServiceProvider
            .GetRequiredService<PassiveUserCheckerWorker>()
    );
````

So, it resolves the given background worker and adds to the `IBackgroundWorkerManager`.

While we generally add workers in `OnApplicationInitializationAsync`, there are no restrictions on that. You can inject `IBackgroundWorkerManager` anywhere and add workers at runtime. Background worker manager will stop and release all the registered workers when your application is being shut down.

## Options

`AbpBackgroundWorkerOptions` class is used to [set options](Options.md) for the background workers. Currently, there is only one option:

* `IsEnabled` (default: true): Used to **enable/disable** the background worker system for your application.

> See the [Options](Options.md) document to learn how to set options.

## Making Your Application Always Run

Background workers only work if your application is running. If you host the background job execution in your web application (this is the default behavior), you should ensure that your web application is configured to always be running. Otherwise, background jobs only work while your application is in use.

## Running On a Cluster

Be careful if you run multiple instances of your application simultaneously in a clustered environment. In that case, every application runs the same worker which may create conflicts if your workers are running on the same resources (processing the same data, for example).

If that's a problem for your workers, you have the following options:

* Implement your background workers so that they work in a clustered environment without any problem. Using the [distributed lock](Distributed-Locking.md) to ensure concurrency control is a way of doing that. A background worker in an application instance may handle a distributed lock, so the workers in other application instances will wait for the lock. In this way, only one worker does the actual work, while others wait in idle. If you implement this, your workers run safely without caring about how the application is deployed.
* Stop the background workers (set `AbpBackgroundWorkerOptions.IsEnabled` to `false`) in all application instances except one of them, so only the single instance runs the workers.
* Stop the background workers (set `AbpBackgroundWorkerOptions.IsEnabled` to `false`) in all application instances and create a dedicated application (maybe a console application running in its own container or a Windows Service running in the background) to execute all the background tasks. This can be a good option if your background workers consume high system resources (CPU, RAM or Disk), so you can deploy that background application to a dedicated server and your background tasks don't affect your application's performance.

## Integrations

Background worker system is extensible and you can change the default background worker manager with your own implementation or on of the pre-built integrations.

See pre-built worker manager alternatives:

* [Quartz Background Worker Manager](Background-Workers-Quartz.md) 
* [Hangfire Background Worker Manager](Background-Workers-Hangfire.md) 

## See Also

* [Background Jobs](Background-Jobs.md)
