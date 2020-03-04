# Background Workers

## Introduction

Background workers are used to execute some tasks in the background periodically.

### Create a Background Worker

A background worker is a class that derives from the `AsyncPeriodicBackgroundWorkerBase` or `PeriodicBackgroundWorkerBase` class. Both base classes are derived from `ISingletonDependency`.

### Status Checker
This example is used to simple check remote application status. Just suppose that, we want to check and store some web applications are running or not? 

````csharp
public class AppStatusService : ITransientDependency
{
    .
    .
    public void CheckAppStatus()
    {
        var ping = new System.Net.NetworkInformation.Ping();

        var result = ping.Send("www.github.com");

        // save the result
    }
    .
    .
}
````

Then create a background worker class that derived from the `PeriodicBackgroundWorkerBase`:

````csharp
.
.
using Volo.Abp.BackgroundWorkers;

namespace Volo.Www.Application
{
    public class AppStatusCheckingWorker : PeriodicBackgroundWorkerBase
    {
        private readonly IAppStatusService _appStatusService;

        public AppStatusCheckingWorker(
            AbpTimer timer,
            IServiceScopeFactory scopeFactory, 
            IAppStatusService appStatusService)
            : base(timer, scopeFactory)
        {
            _appStatusService = appStatusService;
            Timer.Period = 10_000; // 10 secs
        }

        protected override void DoWork(PeriodicBackgroundWorkerContext workerContext)
        {
            _appStatusService.CheckAppStatus();
        }
    }
}
````

This worker will call DoWorkAsync() method every 10 seconds while the application is running.

### Configuration

Add your BackgroundWorker at `OnApplicationInitialization` in your [module class](Module-Development-Basics.md). The example below initialize the background worker to your module:

````csharp
using Volo.Abp.BackgroundWorkers;

public class MyModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .Add(
                    context.ServiceProvider.GetRequiredService<AppStatusCheckingWorker>()
                );
        }
}
````