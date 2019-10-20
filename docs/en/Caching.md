# Caching

ABP framework extends ASP.NET Core's distributed caching system.

## `IDistributedCache` Interface

ASP.NET Core defines the `IDistributedCache` interface to get/set cache values. But it has some difficulties:

* It works with **byte arrays** rather than .NET objects. So, you need to **serialize/deserialize** the objects you need to cache.
* It provides a **single key pool** for all cache items, so;
  * You need to care about the keys to distinguish **different type of objects**.
  * You need to care about the cache items of **different tenants** (see [multi-tenancy](Multi-Tenancy.md)).

> `IDistributedCache` is defined in the `Microsoft.Extensions.Caching.Abstractions` package. That means it is not only usable for ASP.NET Core applications, but also available to **any type of applications**.

> Default implementation of the `IDistributedCache` interface is the `MemoryDistributedCache` which works **in-memory**. See [ASP.NET Core's documentation](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) to see how to switch to **Redis** or another cache provider.

See [ASP.NET Core's distributed caching document](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed) for more information.

## `IDistributedCache<TCacheItem>` Interface

ABP framework defines the generic `IDistributedCache<TCacheItem>` interface in the [Volo.Abp.Caching](https://www.nuget.org/packages/Volo.Abp.Caching/) package. `TCacheItem` is the type of the object stored in the cache. 

`IDistributedCache<TCacheItem>` solves the difficulties explained above;

* It internally **serializes/deserializes** the cached objects. Uses **JSON** serialization by default, but can be overridden by replacing the `IDistributedCacheSerializer` service in the [dependency injection](Dependency-Injection.md) system.
* It automatically adds a **cache name** prefix to the cache keys based on the object type stored in the cache. Default cache name is the full name of the cache item class (`CacheItem` postfix is removed if your cache item class ends with it). You can use the `CacheName` attribute on the cache item class to set the cache name.
* It automatically adds the **current tenant id** to the cache key to distinguish cache items for different tenants (only works if your application is [multi-tenant](Multi-Tenancy.md)). Define `IgnoreMultiTenancy` attribute on the cache item class to disable this if you want to share the cached objects among all tenants in a multi-tenant application.
* Allows to define a **global cache key prefix** per application, so different applications can use their isolated key pools in a shared distributed cache source.

### Usage

An example class to store an item in the cache:

````csharp
public class BookCacheItem
{
    public string Name { get; set; }

    public float Price { get; set; }
}
````

You can inject and use the `IDistributedCache<BookCacheItem>` service to get/set `BookCacheItem` objects.

Example usage:

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

* This sample service uses the `GetOrAddAsync()` method to get a book item from the cache.
* If the book was not found in the cache, it calls the factory method (`GetBookFromDatabaseAsync` in this case) to retrieve the book item from the original source.
* `GetOrAddAsync` optionally gets a `DistributedCacheEntryOptions` which can be used to set the lifetime of the cached item.

Other methods of the `IDistributedCache<BookCacheItem>` are same as ASP.NET Core's `IDistributedCache` interface, so you can refer [it's documentation](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed).

## `IDistributedCache<TCacheItem, TCacheKey>` Interface

`IDistributedCache<TCacheItem>` interface assumes that the type of your cache key is `string` (so, you need to manually convert your key to string if you need to use a different kind of cache key). `IDistributedCache<TCacheItem, TCacheKey>` can be used when your cache key type is not `string`.

### Usage

An example class to store an item in the cache:

````csharp
public class BookCacheItem
{
    public string Name { get; set; }

    public float Price { get; set; }
}
````

Example usage (assumes that your cache key type is `Guid`):

````csharp
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
            bookId, //Guid type used as the cache key
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

* This sample service uses the `GetOrAddAsync()` method to get a book item from the cache.
* Since cache explicitly implemented as using  `Guid` as cache key, `Guid` value passed to  `_cache_GetOrAddAsync()` method.

`IDistributedCache<TCacheItem, TCacheKey>`  internally uses `ToString()` method of the key object to convert it to a string. If you need to use a complex object as the cache key, you need to override `ToString` method of your class.

An example class that is used as a cache key:

````csharp
public class UserInOrganizationCacheKey
{
    public Guid UserId { get; set; }
    
    public Guid OrganizationId { get; set; }
    
    //Builds the cache key
    public override string ToString()
    {
        return $"{UserId}_{OrganizationId}";
    }
}
````

Example usage:

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

### DistributedCacheOptions

TODO

