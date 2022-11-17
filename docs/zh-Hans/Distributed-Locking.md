# 分布式锁
分布式锁是一种管理多个应用程序访问同一资源的技术. 主要目的是同一时间只允许多个应用程序中的一个访问资源. 否则, 从不同的应用程序访问同一对象可能会破坏资源. 

> ABP当前的分布式锁实现基于[DistributedLock](https://github.com/madelson/DistributedLock)库.

## 安装

你可以打开一个命令行终端并输入以下命令来安装[Volo.Abp.DistributedLocking](https://www.nuget.org/packages/Volo.Abp.DistributedLocking)到你的项目中:

````bash
abp add-package Volo.Abp.DistributedLocking
````

这个库提供了使用分布式锁系统所需的API, 但是, 在使用它之前, 你应该配置一个提供程序.

### 配置一个提供程序

[DistributedLock](https://github.com/madelson/DistributedLock)库对[Redis](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.Redis.md)和[ZooKeeper](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.ZooKeeper.md)提供[多种实现](https://github.com/madelson/DistributedLock#implementations).

例如, 如果你想使用[Redis provider](https://github.com/madelson/DistributedLock/blob/master/docs/DistributedLock.Redis.md), 你应该将[DistributedLock.Redis](https://www.nuget.org/packages/DistributedLock.Redis) NuGet包添加到项目中, 然后将以下代码添加到ABP[模块](Module-Development-Basics.md)类的`ConfigureServices`方法中:

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

此代码从[配置](Configuration.md)获取Redis连接字符串, 因此你可以将以下行添加到`appsettings.json`文件:

````json
"Redis": {
    "Configuration": "127.0.0.1"
}
````

## 使用

有两种方法可以使用分布式锁API: ABP的`IAbpDistributedLock`抽象和[DistributedLock](https://github.com/madelson/DistributedLock)库的API.

### 使用IAbpDistributedLock服务

`IAbpDistributedLock`是ABP框架提供的一个用于简单使用分布式锁的服务.

**实例: 使用`IAbpDistributedLock.TryAcquireAsync`方法**

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

`TryAcquireAsync`可能无法获取锁. 如果无法获取锁, 则返回`null`. 在这种情况下, 你不应该访问资源. 如果句柄不为`null`, 则表示你已获得锁, 并且可以安全地访问资源.

`TryAcquireAsync`方法拥有以下参数:

* `name` (`string`, 必须): 锁的唯一名称. 不同的锁命名用于访问不同的资源.
* `timeout` (`TimeSpan`): 等待获取锁的超时值. 默认值为`TimeSpan.Zero`, 这意味着如果锁已经被另一个应用程序拥有, 它不会等待.
* `cancellationToken`: 取消令牌可在触发后取消操作.

### 配置

#### AbpDistributedLockOptions

`AbpDistributedLockOptions` 是配置分布式锁的主要选项类.

**示例: 设置应用程序的分布式锁Key前缀**

```csharp
Configure<AbpDistributedLockOptions>(options =>
{
    options.KeyPrefix = "MyApp1";
});
```

> 在你的[模块类](Module-Development-Basics.md)中的 `ConfigureServices` 方法进行配置.

##### 可用选项

* KeyPrefix (string, 默认值: null): 指定分布式锁名称前缀.

### 使用DistributedLock库的API

ABP的`IAbpDistributedLock`服务非常有限, 主要用于ABP框架的内部使用. 对于你自己的应用程序, 可以使用DistributedLock库自己的API. 参见[文档](https://github.com/madelson/DistributedLock)详细信息.

## Volo.Abp.DistributedLocking.Abstractions库

如果你正在构建一个可重用的库或应用程序模块, 那么对于作为单个实例运行的简单应用程序, 你可能不希望为模块带来额外的依赖关系. 在这种情况下, 你的库可以依赖于[Volo.Abp.DistributedLocking.Abstractions](https://nuget.org/packages/Volo.Abp.DistributedLocking.Abstractions)库, 它定义了`IAbpDistributedLock`服务, 并将其在进程内实现(实际上不是分布式的). 通过这种方式, 你的库可以在作为单个实例运行的应用程序中正常运行(没有分布式锁提供程序依赖项). 如果应用程序部署到[集群环境](Deployment/Clustered-Environment.md), 那么应用程序开发人员应该安装一个真正的分布式提供程序, 如*安装*部分所述.
