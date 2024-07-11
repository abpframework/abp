# Microservice Solution: Distributed Locking

````json
//[doc-nav]
{
  "Next": {
    "Name": "Distributed cache in the Microservice solution",
    "Path": "solution-templates/microservice/distributed-cache"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Distributed locking is a common pattern in distributed systems to ensure that only one instance of a service can execute a specific operation at a time. This document explains how the distributed locking mechanism works in the microservice solution template. You can learn more in the [Distributed Locking](../../framework/infrastructure/distributed-locking.md) document.

## Distributed Locking in Microservice Solutions

The microservice solution template uses [Redis](https://redis.io/) to implement the distributed locking mechanism. You can configure the Redis connection in the `appsettings.json` file of the related projects. The following is an example configuration:

```json
  "Redis": {
    "Configuration": "localhost:6379"
  }
```

Additionally, you can configure the distributed lock key prefix in the `ConfigureServices` method. This approach helps prevent key conflicts if you use the same Redis server for multiple applications. Here is an example configuration:

```csharp
Configure<AbpDistributedLockOptions>(options =>
{
    options.KeyPrefix = "MyApp1";
});
```

The distributed locking mechanism is often needed in background workers. You can inject the `IAbpDistributedLock` interface into your background worker class and use it to acquire the lock. Here is an example:

```csharp
public class MyService : ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;

    public MyService(IAbpDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
    }
    
    public async Task MyMethodAsync()
    {
        await using (var handle = await _distributedLock.TryAcquireAsync("MyLockName"))
        {
            if (handle != null)
            {
                // your code that access the shared resource
            }
        }   
    }
}
```