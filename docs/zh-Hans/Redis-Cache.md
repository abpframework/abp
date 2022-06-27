# Redis 缓存

ABP Framework [缓存系统](Caching.md) 拓展了 [ASP.NET Core 分布式缓存](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed). 因此, 标准 ASP.NET Core 分布式缓存支持的 **任何提供程序** 都可以在你的应用程序中使用，并且可以像 **微软文档** 那样进行配置.

不过, ABP为 Redis Cache 还提供了一个集成包: [Volo.Abp.Caching.StackExchangeRedis](https://www.nuget.org/packages/Volo.Abp.Caching.StackExchangeRedis). 
为何使用中这个包而不是标准的[Microsoft.Extensions.Caching.StackExchangeRedis](https://www.nuget.org/packages/Microsoft.Extensions.Caching.StackExchangeRedis/)包有两个原因.

1. 它实现 `SetManyAsync` 和 `GetManyAsync` 方法. 这些都不是 Microsoft 缓存库的标准方法，而是由ABP框架[缓存](Caching.md)系统添加的. 当你需要通过单个方法调用设置/获取多个缓存项时，它们**显著提高了性能**.
2. 它 **简化** 了 Redis 缓存 **配置** (将在下面解释).

> Volo.Abp.Caching.StackExchangeRedis 已经使用了 Microsoft.Extensions.Caching.StackExchangeRedis 包，但对其进行了扩展和改进.
## 安装

> 如果使用Redis，则此软件包已安装在应用程序启动模板中.
在项目`.csproj`的文件夹中打开命令行窗口并输入命令:

````bash
abp add-package Volo.Abp.Caching.StackExchangeRedis
````

## 配置

Volo.Abp.Caching.StackExchangeRedis 包自动从`IConfiguration`获取Redis[配置](Configuration.md).  因此，你可以在以下位置设置配置 `appsettings.json`:

````js
"Redis": { 
 "IsEnabled": "true",
 "Configuration": "127.0.0.1"
}
````
设置`IsEnabled`为可选的，如果未设置将默认视为`true`.


或者，你可以在[模块](Module-Development-Basics.md)的ConfigureServices方法中配置标准的[RedisCacheOptions](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.caching.stackexchangeredis.rediscacheoptions)类：

````csharp
Configure<RedisCacheOptions>(options =>
{
    //...
});
````

## 另请参阅

* [缓存](Caching.md)
