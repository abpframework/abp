# Microservice Solution: Background Workers

````json
//[doc-nav]
{
  "Next": {
    "Name": "Distributed locking in the Microservice solution",
    "Path": "solution-templates/microservice/distributed-locking"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Background workers are long-running processes that perform tasks in the background. They are used in microservice systems to handle tasks that run continuously or periodically, such as data processing, notifications, and more. The ABP Framework includes a system for creating and running these workers in your application. You can learn more in the [Background Workers](../../framework/infrastructure/background-workers/index.md) document.

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

After creating a worker, we should also register it in the related microservice or project. You can register your worker in the `OnApplicationInitializationAsync` method of your module class.

```csharp
public class AdministrationServiceModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<PassiveUserCheckerWorker>();
    }
}
```

> An important point in distributed systems is that the same background workers could be running in multiple instances of the same service. So, you should be careful about the side effects of the workers. For example, if you are processing a message from a queue, you should ensure that the message is processed only once. You can use [distributed locking](../../framework/infrastructure/distributed-locking.md) to prevent multiple instances from processing the same message. You can find more information about distributed locking in microservice environments in the [distributed locking](distributed-locking.md) document.