# Distributed Locking
Distributed locking is a technique to manage many applications that try to access the same resource. The main purpose is to allow only one of many applications to access the same resource at the same time. Otherwise, accessing the same object from various applications may corrupt the value of the resources. 

> ABP's current distributed locking implementation is based on the [DistributedLock](https://github.com/madelson/DistributedLock) library.

## Installation

You can open a command-line terminal and type the following command to install the [Volo.Abp.DistributedLocking](https://www.nuget.org/packages/Volo.Abp.DistributedLocking) package into your project:

````bash
abp add-package Volo.Abp.DistributedLocking
````

This package provides the necessary API to use the distributed locking system, however, you should configure a provider before using it.

### Configuring a Provider

The [DistributedLock](https://github.com/madelson/DistributedLock) library provides [various of implementations](https://github.com/madelson/DistributedLock#implementations) for the locking, like [Redis](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.Redis.md) and [ZooKeeper](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.ZooKeeper.md).

For example, if you want to use the [Redis provider](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.Redis.md), you should add [DistributedLock.Redis](https://www.nuget.org/packages/DistributedLock.Redis) NuGet package to your project, then add the following code into the `ConfigureServices` method of your ABP [module](Module-Development-Basics.md) class:

````csharp
using Medallion.Threading;
using Medallion.Threading.Redis;

namespace AbpDemo
{
    [DependsOn(
            typeof(AbpDistributedLockingModule)
            //If you have the other dependencies, you should do here
    )]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
        
            context.Services.AddSingleton<IDistributedLockProvider>(sp =>
            {
                var connection = ConnectionMultiplexer
                    .Connect(configuration["Redis:Configuration"]);
                return new 
                    RedisDistributedSynchronizationProvider(connection.GetDatabase());
            });
        }
    }
}
````

This code gets the Redis connection string from the [configuration](Configuration.md), so you can add the following lines to your `appsettings.json` file:

````json
"Redis": {
    "Configuration": "127.0.0.1"
}
````

## Usage

There are two ways to use the distributed locking API: ABP's `IAbpDistributedLock` abstraction and [DistributedLock](https://github.com/madelson/DistributedLock) library's API.

### Using the IAbpDistributedLock Service

`IAbpDistributedLock` is a simple service provided by the ABP framework for simple usage of distributed locking.

**Example: Using the `IAbpDistributedLock.TryAcquireAsync` method**

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
            await using (var handle = 
                         await _distributedLock.TryAcquireAsync("MyLockName"))
            {
                if (handle != null)
                {
                    // your code that access the shared resource
                }
            }   
        }
    }
}
````

`TryAcquireAsync` may not acquire the lock. It returns `null` if the lock could not be acquired. In this case, you shouldn't access the resource. If the handle is not `null`, it means that you've obtained the lock and can safely access the resource.

`TryAcquireAsync` method gets the following parameters:

* `name` (`string`, required): Unique name of your lock. Different named locks are used to access different resources.
* `timeout` (`TimeSpan`): A timeout value to wait to obtain the lock. Default value is `TimeSpan.Zero`, which means it doesn't wait if the lock is already owned by another application.
* `cancellationToken`: A cancellation token that can be triggered later to cancel the operation.

### Configuration

#### AbpDistributedLockOptions

`AbpDistributedLockOptions` is the main options class to configure the distributed locking.

**Example: Set the distributed lock key prefix for the application**

Configure<AbpDistributedLockOptions>(options =>
{
    options.KeyPrefix = "MyApp1";
});

> Write that code inside the `ConfigureServices` method of your [module class](Module-Development-Basics.md).

##### Available Options

* KeyPrefix (string, default: null): Specify the lock name prefix.

### Using DistributedLock Library's API

ABP's `IAbpDistributedLock` service is very limited and mainly designed to be internally used by the ABP Framework. For your own applications, you can use the DistributedLock library's own API. See its [own documentation](https://github.com/madelson/DistributedLock) for details.

## The Volo.Abp.DistributedLocking.Abstractions Package

If you are building a reusable library or an application module, then you may not want to bring an additional dependency to your module for simple applications that run as a single instance. In this case, your library can depend on the [Volo.Abp.DistributedLocking.Abstractions](https://nuget.org/packages/Volo.Abp.DistributedLocking.Abstractions) package which defines the `IAbpDistributedLock` service and implements it as in-process (not distributed actually). In this way, your library can run properly (without a distributed lock provider dependency) in an application that runs as a single instance. If the application is deployed to a [clustered environment](Deployment/Clustered-Environment.md), then the application developer should install a real distributed provider as explained in the *Installation* section.
