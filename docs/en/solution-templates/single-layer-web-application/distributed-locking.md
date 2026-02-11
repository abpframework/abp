```json
//[doc-seo]
{
    "Description": "Learn how to implement distributed locking in single-layer solutions with ABP Framework, ensuring synchronized access to shared resources."
}
```

# Single Layer Solution: Distributed Locking

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Background Workers",
    "Path": "solution-templates/single-layer-web-application/background-workers"
  },
  "Next": {
    "Name": "Multi-Tenancy",
    "Path": "solution-templates/single-layer-web-application/multi-tenancy"
  }
}
```

Distributed locking is a mechanism that allows multiple instances of an application to coordinate and synchronize access to shared resources. It is useful for scenarios where multiple instances of an application need to ensure that only one instance can access a resource at a time. You can learn more in the [Distributed Locking](../../framework/infrastructure/distributed-locking.md) document.

## Distributed Locking in Single Layer Solutions

The single-layer solution template does not include distributed lock package by default. You can add the [Volo.Abp.DistributedLock](https://www.nuget.org/packages/Volo.Abp.DistributedLocking) package to your project to use distributed locking. This package provides a distributed lock mechanism that works with Redis. You can inject the `IAbpDistributedLock` service to acquire and release. Here is an example of using distributed locking in your application:

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
