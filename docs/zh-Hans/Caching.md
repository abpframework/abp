# 缓存

ABP框架扩展了 [ASP.NET Core的分布式缓存系统](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed).

## 安装

> 默认情况下 [启动模板](Startup-Templates/Application.md) 已经安装了这个包. 所以大部分情况下你不需要手动安装.

[Volo.Abp.Caching](https://www.nuget.org/packages/Volo.Abp.Caching)是缓存系统的核心包. 可以使用 [ABP CLI](CLI.md) 的add-package命令将其安装到项目中：

```
abp add-package Volo.Abp.Caching
```
你需要在包含 `csproj` 文件的文件夹中的命令行终端上运行此命令(请参阅 [其他选项](https://abp.io/package-detail/Volo.Abp.Caching) 安装).

## 使用方式

### `IDistributedCache` 接口

ASP.NET Core 定义了 `IDistributedCache` 接口用于 get/set 缓存值. 但是会有以下问题:

* 它适用于 **byte 数组** 而不是 .NET 对象. 因此你需要对缓存的对象进行**序列化/反序列化**.
* 它为所有的缓存项提供了 **单个 key 池** , 因此;
  * 你需要注意键区分 **不同类型的对象**.
  * 你需要注意**不同租户**(参见[多租户](Multi-Tenancy.md))的缓存项.

> `IDistributedCache` 定义在 `Microsoft.Extensions.Caching.Abstractions` 包中. 这使它不仅适用于ASP.NET Core应用程序, 也可用于**任何类型的程序**.

> `IDistributedCache` 接口的默认实现是 `MemoryDistributedCache` 它使用**内存**工作. 参见 [ASP.NET Core文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed) 了解如何切换到 **Redis** 或其他缓存提供程序. 此外, 如果要将Redis用作分布式缓存服务器, [Redis缓存](Redis-Cache.md) 文档. 

有关更多信息, 参见 [ASP.NET Core 分布式缓存文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed).

### `IDistributedCache<TCacheItem>` 接口

ABP框架在[Volo.Abp.Caching](https://www.nuget.org/packages/Volo.Abp.Caching/)包定义了通用的泛型 `IDistributedCache<TCacheItem>` 接口. `TCacheItem` 是存储在缓存中的对象类型.

`IDistributedCache<TCacheItem>` 接口解决了上述中的问题;

* 它在内部 **序列化/反序列化** 缓存对象. 默认使用 **JSON** 序列化, 但可以替换[依赖注入](Dependency-Injection.md)系统中 `IDistributedCacheSerializer` 服务的实现来覆盖默认的处理.
* 它根据缓存中对象类型自动向缓存key添加 **缓存名称** 前缀. 默认缓存名是缓存对象类的全名(如果你的类名以`CacheItem` 结尾, 那么`CacheItem` 会被忽略,不应用到缓存名称上). 你也可以在缓存类上使用 **`CacheName` 特性** 设置缓存的名称.
* 它自动将**当前的租户id**添加到缓存键中, 以区分不同租户的缓存项 (只有在你的应用程序是[多租户](Multi-Tenancy.md)的情况下生效). 如果要在多租户应用程序中的所有租户之间共享缓存对象, 请在缓存项类上定义`IgnoreMultiTenancy`特性以禁用此选项. 
* 允许为每个应用程序定义 **全局缓存键前缀** , 不同的应用程序可以在共享的分布式缓存中拥有自己的隔离池.
* 它可以在任何可能绕过缓存的情况下 **容忍错误** 的发生. 这在缓存服务器出现临时问题时非常有用.
* 它有 `GetManyAsync` 和 `SetManyAsync` 等方法, 可以显著提高**批处理**的性能. 

**示例: 在缓存中存储图书名称和价格**

````csharp
namespace MyProject
{
    public class BookCacheItem
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

你可以注入 `IDistributedCache<BookCacheItem>` 服务用于 get/set `BookCacheItem` 对象.

````csharp
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace MyProject
{
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
                bookId.ToString(), //缓存键
                async () => await GetBookFromDatabaseAsync(bookId),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                }
            );
        }

        private Task<BookCacheItem> GetBookFromDatabaseAsync(Guid bookId)
        {
            //TODO: 从数据库获取数据
        }
    }
}
````

* 示例服务代码中的 `GetOrAddAsync()` 方法从缓存中获取图书项. `GetOrAddAsync`是ABP框架在 ASP.NET Core 分布式缓存方法中添增的附加方法.
* 如果没有在缓存中找到图书,它会调用工厂方法 (本示例中是 `GetBookFromDatabaseAsync`)从原始数据源中获取图书项.
* `GetOrAddAsync` 有一个可选参数 `DistributedCacheEntryOptions` , 可用于设置缓存的生命周期.

`IDistributedCache<BookCacheItem>` 与ASP.NET Core的`IDistributedCache` 接口拥有相同的方法, 你可以参考 [ASP.NET Core文档](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/distributed).

### `IDistributedCache<TCacheItem, TCacheKey>` 接口

`IDistributedCache<TCacheItem>` 接口默认了**缓存键**是 `string` 类型 (如果你的键不是string类型需要进行手动类型转换). 但当缓存键的类型不是`string`时, 可以使用`IDistributedCache<TCacheItem, TCacheKey>`.

**示例: 在缓存中存储图书名称和价格**

示例缓存项

````csharp
using Volo.Abp.Caching;

namespace MyProject
{
    [CacheName("Books")]
    public class BookCacheItem
    {
        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

* 在本例中使用`CacheName`特性给`BookCacheItem`类设置缓存名称.

你可以注入 `IDistributedCache<BookCacheItem, Guid>` 服务用于 get/set `BookCacheItem` 对象.

````csharp
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;

namespace MyProject
{
    public class BookService : ITransientDependency
    {
        private readonly IDistributedCache<BookCacheItem, Guid> _cache;

        public BookService(IDistributedCache<BookCacheItem, Guid> cache)
        {
            _cache = cache;
        }

        public async Task<BookCacheItem> GetAsync(Guid bookId)
        {
            return await _cache.GetOrAddAsync(
                bookId, //Guid类型作为缓存键
                async () => await GetBookFromDatabaseAsync(bookId),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddHours(1)
                }
            );
        }
        private Task<BookCacheItem> GetBookFromDatabaseAsync(Guid bookId)
        {
            //TODO: 从数据库获取数据
        }
    }
}
````

* 示例服务中 `GetOrAddAsync()` 方法获取缓存的图书项.
* 我们采用了 `Guid` 做为键, 在 `_cache_GetOrAddAsync()` 方法中传入 `Guid` 类型的bookid.

#### 复杂类型的缓存键

`IDistributedCache<TCacheItem, TCacheKey>` 在内部使用键对象的 `ToString()` 方法转换类型为string. 如果你的将复杂对象做为缓存键,那么需要重写类的 `ToString` 方法.

举例一个作为缓存键的类:

````csharp
public class UserInOrganizationCacheKey
{
    public Guid UserId { get; set; }
 
    public Guid OrganizationId { get; set; }

    //构建缓存key
    public override string ToString()
    {
        return $"{UserId}_{OrganizationId}";
    }
}
````

用法示例:

````csharp
public class BookService : ITransientDependency
{
    private readonly IDistributedCache<UserCacheItem, UserInOrganizationCacheKey> _cache;

    public BookService(
        IDistributedCache<UserCacheItem, UserInOrganizationCacheKey> cache)
    {
        _cache = cache;
    }
    
    ...
}
````

## 配置

### AbpDistributedCacheOptions
`AbpDistributedCacheOptions` 是配置缓存的主要[Option类](Options.md).

**示例：为应用程序设置缓存键前缀**

```csharp
Configure<AbpDistributedCacheOptions>(options =>
{
    options.KeyPrefix = "MyApp1";
});
```
> 在[模块类](Module-Development-Basics.md)的`ConfigureServices`方法中添加代码. 

#### 可用选项

* `HideErrors` (`bool`, 默认: `true`): 启用/禁用隐藏从缓存服务器写入/读取值时的错误.
* `KeyPrefix` (`string`, 默认: `null`): 如果你的缓存服务器由多个应用程序共同使用, 则可以为应用程序的缓存键设置一个前缀. 在这种情况下, 不同的应用程序不能覆盖彼此的缓存内容.
* `GlobalCacheEntryOptions` (`DistributedCacheEntryOptions`): 用于设置保存缓内容却没有指定选项时, 默认的分布式缓存选项 (例如 `AbsoluteExpiration` 和 `SlidingExpiration`). `SlidingExpiration`的默认值设置为20分钟.

## 错误处理

当为你的对象设计缓存时, 通常会首先尝试从缓存中获取值. 如果在缓存中找不到该值, 则从**来源**查询对象. 它可能在**数据库**中, 或者可能需要通过HTTP调用远程服务器. 

在大多数情况下, 你希望**容忍缓存错误**; 如果缓存服务器出现错误, 也不希望取消该操作. 相反, 你可以默默地隐藏(并记录)错误并**从来源查询**. 这就是ABP框架默认的功能. 

ABP的分布式缓存 [异常处理](Exception-Handling.md), 默认记录并隐藏错误. 有一个全局修改该功能的选项(参见下面的选项内容). 

所有的`IDistributedCache<TCacheItem>` (和 `IDistributedCache<TCacheItem, TCacheKey>`)方法都有一个可选的参数`hideErrors`, 默认值为`null`. 如果此参数设置为`null`, 则全局生效, 否则你可以选择单个方法调用时隐藏或者抛出异常. 

## 批量操作

ABP的分布式缓存接口定义了以下批量操作方法,当你需要在一个方法中调用多次缓存操作时,这些方法可以提高性能

* `SetManyAsync` 和 `SetMany` 方法可以用来向缓存中设置多个值.
* `GetManyAsync` 和 `GetMany` 方法可以用来从缓存中获取多个值.
* `GetOrAddManyAsync` 和 `GetOrAddMany` 方法可以用来从缓存中获取并添加缺少的值.
* `RefreshManyAsync` 和 `RefreshMany` 方法可以来用重置多个值的滚动过期时间.
* `RemoveManyAsync` 和 `RemoveMany` 方法可以用来从缓存中删除多个值.

> 这些不是标准的ASP.NET Core缓存方法, 所以某些提供程序可能不支持. [ABP Redis集成包](Redis-Cache.md)实现了它们. 如果提供程序不支持,会回退到 `SetAsync` 和 `GetAsync` ... 方法(循环调用).

## 高级主题

### 工作单元级别的缓存

分布式缓存服务提供了一个有趣的功能. 假设你已经更新了数据库中某本书的价格, 然后将新价格设置到缓存中, 以便以后使用缓存的值. 如果设置缓存后出现异常, 并且更新图书价格的**事务被回滚了**, 该怎么办？在这种情况下, 缓存值是错误的. 

`IDistributedCache<..>`方法提供一个可选参数, `considerOuw`, 默认为`false`. 如果将其设置为`true`, 则你对缓存所做的更改不会应用于真正的缓存存储, 而是与当前的[工作单元](Unit-Of-Work.md)关联. 你将获得在同一工作单元中设置的缓存值, 但**仅当前工作单元成功时**更改才会生效. 

### IDistributedCacheSerializer

`IDistributedCacheSerializer`服务用于序列化和反序列化缓存内容. 默认实现是`Utf8JsonDistributedCacheSerializer`类, 它使用`IJsonSerializer`服务将对象转换为[JSON](Json-Serialization.md), 反之亦然. 然后, 它使用UTC8编码将JSON字符串转换为分布式缓存接受的字节数组. 

如果你想实现自己的序列化逻辑, 可以自己实现并[替换](Dependency-Injection.md) 此服务. 

### IDistributedCacheKeyNormalizer

默认情况下, `IDistributedCacheKeyNormalizer`是由`DistributedCacheKeyNormalizer`类实现的. 它将缓存名称、应用程序缓存前缀和当前租户id添加到缓存键中. 如果需要更高级的键规范化, 可以自己实现并[替换](Dependency-Injection.md)此服务. 

## 另请参阅

* [Redis 缓存](Redis-Cache.md)
