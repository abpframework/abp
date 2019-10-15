# 缓存

ABP框架扩展了ASP.NET Core的分布式缓存系统.

## IDistributedCache 接口

ASP.NET Core 定义了 `IDistributedCache` 接口用于 get/set 缓存值 . 但是会有以下问题:

* 它适用于 **byte 数组** 而不是 .NET 对象. 因此你需要对缓存的对象进行**序列化/反序列化**.
* 它为所有的缓存项提供了 **单个 key 池** , 因此 ;
  * 你需要注意键区分 **不同类型的对象**.
  * 你需要注意**不同租户**(参见[多租户](Multi-Tenancy.md))的缓存项.

> `IDistributedCache` 定义在 `Microsoft.Extensions.Caching.Abstractions` 包中. 这使它不仅适用于ASP.NET Core应用程序, 也可用于**任何类型的程序**.

> `IDistributedCache` 接口的默认实现是 `MemoryDistributedCache` 它使用**内存**工作. 参见 [ASP.NET Core文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed) 了解如何切换到 **Redis** 或其他缓存提供程序.

有关更多信息, 参见 [ASP.NET Core 分布式缓存文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed).

## IDistributedCache<TCacheItem> 接口

ABP框架在[Volo.Abp.Caching](https://www.nuget.org/packages/Volo.Abp.Caching/)包定义了通用的泛型 `IDistributedCache<TCacheItem>` 接口. `TCacheItem` 是存储在缓存中的对象类型.

`IDistributedCache<TCacheItem>` 接口了上述中的问题;

* 它在内部 **序列化/反序列化** 缓存对象. 默认使用 **JSON** 序列化, 但可以替换[依赖注入](Dependency-Injection.md)系统中 `IDistributedCacheSerializer` 服务的实现来覆盖默认的处理.
* 它根据缓存中对象类型自动向缓存key添加 **缓存名称** 前缀. 默认缓存名是缓存对象类的全名(如果你的类名以`CacheItem` 结尾, 那么`CacheItem` 会被忽略,不应用到缓存名称上). 你也可以在缓存类上使用 `CacheName` 设置换缓存的名称.
* 它自动将当前的**租户id**添加到缓存键中, 以区分不同租户的缓存项 (只有在你的应用程序是[多租户](Multi-Tenancy.md)的情况下生效). 在缓存类上应用 `IgnoreMultiTenancy` attribute, 可以在所有的租户间共享缓存.
* 允许为每个应用程序定义 **全局缓存键前缀** ,不同的应用程序可以在共享的分布式缓存中拥有自己的隔离池.

### 使用方式

缓存中存储项的示例类:

````csharp
public class BookCacheItem
{
    public string Name { get; set; }

    public float Price { get; set; }
}
````

你可以注入 `IDistributedCache<BookCacheItem>` 服务用于 get/set `BookCacheItem` 对象.

使用示例:

````csharp
public class BookService : ITransientDependency
{
    private readonly IDistributedCache<BookCacheItem> _cache;

    public BookService(IDistributedCache<BookCacheItem> cache)
    {
        _cache = cache;
    }

    public async Task<BookCacheItem> GetAsync(Guid bookId)
    {
        return await _cache.GetOrAddAsync(
            bookId.ToString(), //Cache key
            async () => await GetBookFromDatabaseAsync(bookId),
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
            }
        );
    }

    private Task<BookCacheItem> GetBookFromDatabaseAsync(Guid bookId)
    {
        //TODO: get from database
    }
}
````

* 示例服务代码中的 `GetOrAddAsync()` 方法从缓存中获取图书项.
* 如果没有在缓存中找到图书,它会调用工厂方法 (本示例中是 `GetBookFromDatabaseAsync`)从原始数据源中获取图书项.
* `GetOrAddAsync` 有一个可选参数 `DistributedCacheEntryOptions` , 可用于设置缓存的生命周期.

`IDistributedCache<BookCacheItem>` 的其他方法与ASP.NET Core的`IDistributedCache` 接口相同, 你可以参考 [ASP.NET Core文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed).

### DistributedCacheOptions

TODO