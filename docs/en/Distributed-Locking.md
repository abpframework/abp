# Distributed Locking
Distributed locking is very useful to manage many processes that request the same object. 
In a normal case, accessing the same object from different applications can corrupt the value of resources. 
In terms of accuracy, we may need to protect the value that's why a lock will be necessary. 

> To use in ABP, you should download the below NuGet package or can open any command apps and run the below command.
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

This provider contains a method named `TryAcquireAsync` and this method returns null if the lock could not be handled.
`name` is mandatory. It keeps the locked provider name. This name should be unique, otherwise it will be seen as a different lock.
`timeout` is set as default. If it fells deadlock and doesn't work properly you can use define the time to kill it.
`cancellationToken` is set as default. It enables cooperative cancellation between threads, thread pool work items, or Task objects

ABP depends on [DistributedLock.Core](https://www.nuget.org/packages/DistributedLock.Core) library which provides a distributed locking system for concurrency control in a distributed environment. There are [many distributed lock providers](https://github.com/madelson/DistributedLock#implementations) including Redis, SqlServer and ZooKeeper. You may use the one you want. ABP implements the Redis provider for you and you may customize the others yourselves.

Firstly, you should add [DistributedLock.Redis](https://www.nuget.org/packages/DistributedLock.Redis) NuGet package to your project, then add the following code into the ConfigureService method of your ABP module class.

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

Also, you should add your Redis configuration in appsetting.json.
You may change the structure of the JSON file but it must match with the above configuration code.
````json
"Redis": {
    "Configuration": "127.0.0.1"
}
````

As mentioned the above, this implemantition is for Redis and for more detail you can visit [the official site](https://github.com/madelson/DistributedLock#implementations).