```json
//[doc-seo]
{
    "Description": "Learn how to implement distributed locking in layered applications to synchronize access to shared resources effectively."
}
```

# Layered Solution: Distributed Locking

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Background Workers",
    "Path": "solution-templates/layered-web-application/background-workers"
  },
  "Next": {
    "Name": "Multi-Tenancy",
    "Path": "solution-templates/layered-web-application/multi-tenancy"
  }
}
```

Distributed locking is a mechanism that enables multiple instances of an application to coordinate and synchronize access to shared resources. It is particularly useful in scenarios where multiple instances need to ensure that only one instance can access a resource at a time. For more information, refer to the [Distributed Locking](../../framework/infrastructure/distributed-locking.md) document.

## Distributed Locking in Layered Solutions

The layered solution template does not include the distributed lock package by default unless it's a *Tiered* or *Public Website* application. To use distributed locking, you can add the [Volo.Abp.DistributedLock](https://www.nuget.org/packages/Volo.Abp.DistributedLocking) package to your project. This package provides a distributed lock mechanism that works with Redis. You can inject the `IAbpDistributedLock` service to acquire and release locks. Below is an example of using distributed locking in your application:

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
