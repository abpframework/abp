# Distributed Lock
Distributed locks are very useful to manage many processes that request the same object. 
In a normal case, accessing the same object from different threads or services can corrupt the value of objects. 
In terms of accuracy, we may need to protect the value that's why a lock will be necessary. 
On another hand, it will protect from more times triggered hence you will provide more efficiency.
To summarize, It's used to handle conflict between these requests. Once obtaining anyone it, the others need to wait till give up free.

# Providers
Distributed locks system provides an abstraction that can be implemented by any vendor/provider.

 * `MedallionAbpDistributedLock`
ABP depends on [DistributedLock.Core](https://www.nuget.org/packages/DistributedLock.Core) library which provides a distributed locking system for concurrency control in a distributed environment. There are [many distributed lock providers](https://github.com/madelson/DistributedLock#implementations) including Redis, SqlServer and ZooKeeper. You may use the one you want. Here, I will show the Redis provider.

This provider contains a method named `TryAcquireAsync` and this method returns null if the lock could not be handled.
> `Name` is a mandatory field. It keeps the locked provider name.
> `Timeout` is set as default. If it fells deadlock and doesn't work properly you can use define the time to kill it.
> `CancellationToken` is set as default. It enables cooperative cancellation between threads, thread pool work items, or Task objects


Also, you should add [DistributedLock.Redis](https://www.nuget.org/packages/DistributedLock.Redis) NuGet package to your project, then add the following code into the ConfigureService method of your ABP module class.

````csharp
using Medallion.Threading;
using Medallion.Threading.Redis;
namespace AbpDemo
{
	public class MyModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			//the other configurations
			var configuration = context.Services.GetConfiguration();
			
			context.Services.AddSingleton<IDistributedLockProvider>(sp =>
			{
				var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
				return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
			});
		}
	}
}
````

Also, you should add your Redis configuration in appsetting.json
````json
"Redis": {
    "Configuration": "127.0.0.1"
}
````

> To use in ABP, you should download the below NuGet package.
It already contains DistributedLocking.Abstractions  and no need to download as well.
````powershell
abp add-package Volo.Abp.DistributedLocking
````

````csharp
using Volo.Abp.DistributedLocking; 
namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly IAbpDistributedLock _distributedLock;
		public MyService(IAbpDistributedLock distributedLock)
        {
            _distributedLock = distributedLock;
        }
        
        public async Task MyMethodAsync()
        {
            await using (var handle = await _distributedLock.TryAcquireAsync("NameOfLock"))
            {
                if (handle != null)
                {
                    //your code
                }
            }   
        }
    }
}
````