```json
//[doc-seo]
{
    "Description": "Learn how to implement background workers in your application for efficient, long-running tasks that enhance performance and reliability."
}
```

# Single Layer Solution: Background Workers

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Background Jobs",
    "Path": "solution-templates/single-layer-web-application/background-jobs"
  },
  "Next": {
    "Name": "Distributed Locking",
    "Path": "solution-templates/single-layer-web-application/distributed-locking"
  }
}
```

Background workers are long-running processes that run in the background of your application. They are useful for tasks that are not time-sensitive, such as processing data, sending notifications, or monitoring system health. Background workers are typically started when the application starts and run continuously until the application stops. You can learn more about background workers in the [Background Workers](../../framework/infrastructure/background-workers/index.md) document.

Basically, you can create scheduled workers for a specific time interval based on your requirements, such as checking the status of inactive users and changing their status to passive if they have not logged in to the application in the last 30 days.

```csharp
public class PassiveUserCheckerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PassiveUserCheckerWorker(
            AbpAsyncTimer timer,
            IServiceScopeFactory serviceScopeFactory) : base(
            timer, 
            serviceScopeFactory)
    {
        Timer.Period = 600000; //10 minutes
    }

    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Starting: Setting status of inactive users...");

        // Resolve dependencies
        var userRepository = workerContext
            .ServiceProvider
            .GetRequiredService<IUserRepository>();

        // Do the work
        await userRepository.UpdateInactiveUserStatusesAsync();

        Logger.LogInformation("Completed: Setting status of inactive users...");
    }
}
```

After creating a worker, you should also register it in the application. You can register your worker in the `OnApplicationInitializationAsync` method of your module class:

```csharp
public class BookstoreModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<PassiveUserCheckerWorker>();
    }
}
```

> When scaling out your application in a distributed system, it's crucial to consider that the same background workers might run on multiple instances of the same service. This requires careful management of potential side effects. For example, if you're processing messages from a queue, you need to ensure that each message is processed only once. To prevent multiple instances from handling the same message, you can use [distributed locking](../../framework/infrastructure/distributed-locking.md).
