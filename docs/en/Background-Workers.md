# Background Workers

## Introduction

Background workers are different than [background jobs](Background-Jobs.md). They are simple independent threads in the application running in the background. Generally, they run periodically to perform some tasks. Examples;

* A background worker can run periodically to delete old logs.
* A background worker can run periodically to determine inactive users and send emails to get users to return to your application.


### Create a Background Worker

A background worker is a `Singleton Depency` class that derives from the `AsyncPeriodicBackgroundWorkerBase` or `PeriodicBackgroundWorkerBase`.

Assume that we want to make a user passive, if he did not login to the application in last 30 days. See the code:

````csharp
public class PassiveUserCheckerWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PassiveUserCheckerWorker> _logger;

    public PassiveUserCheckerWorker(
                AbpTimer timer,
                IServiceScopeFactory serviceScopeFactory,
                IUserRepository userRepository,
                ILogger<GithubDataCollectingWorker> logger
            ) : base(timer, serviceScopeFactory)
    {
        _userRepository = userRepository;
        _logger = logger;
        Timer.Period = 5_000; //5 seconds (good for tests)
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        _logger.LogInformation($"{nameof(PassiveUserCheckerWorker)} started to work.");
        
        // UserRepository sets statuses of inactive users as a passive.
        await _userRepository.UpdateInactiveUserStatusesAsync();

        _logger.LogInformation($"{nameof(PassiveUserCheckerWorker)} finished it's work.");
    }
}
````

* If your background worker derive from `PeriodicBackgroundWorkerBase`, you should implement the `DoWork` method to perform your periodic working code.
* If you directly implement IBackgroundWorker, you will override/implement the `StartAsync` and `StopAsync` methods.

### Register Background Worker

After creating a background worker, add it to the IBackgroundWorkerManager. The most common place is the `OnApplicationInitialization` method of your module:

````csharp
public class MyModule : AbpModule
{
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IBackgroundWorkerManager>()
                .Add(
                    context.ServiceProvider.GetRequiredService<PassiveUserCheckerWorker>()
                );
        }
}
````

While we generally add workers in OnApplicationInitialization, there are no restrictions on that. You can inject IBackgroundWorkerManager anywhere and add workers at runtime. IBackgroundWorkerManager will stop and release all registered workers when your application is being shut down.

## Making Your Application Always Run

Background jobs and workers only work if your application is running. If you host the background job execution in your web application (this is the default behavior), you should ensure that your web application is configured to always be running. Otherwise, background jobs only work while your application is in use.

## See Also
* [Background Jobs](Background-Jobs.md)