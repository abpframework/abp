# Microservice Solution: Distributed Cache

````json
//[doc-nav]
{
  "Next": {
    "Name": "Multi-Tenancy in the Microservice solution",
    "Path": "solution-templates/microservice/multi-tenancy"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

Distributed caching is a common pattern in distributed systems to improve performance by storing frequently accessed data in a cache. This document explains how the distributed caching mechanism works in the microservice solution template. You can learn more in the [Distributed Cache](../../framework/fundamentals/caching.md) document.

## Distributed Cache in Microservice Solutions

The microservice solution template uses [Redis](https://redis.io/) to implement the distributed caching mechanism. You can configure the Redis connection in the `appsettings.json` file of the related projects. Here is an example configuration:

```json
  "Redis": {
    "Configuration": "localhost:6379"
  },
  "AbpDistributedCache": {
    "KeyPrefix": "Bookstore:"
  }
```

Additionally, you can configure the distributed cache key prefix in the `ConfigureServices` method. This approach helps prevent key conflicts if you use the same Redis server for multiple applications. Existing services in the microservice solution template already have this configuration. Here is an example configuration:

```csharp
Configure<AbpDistributedCacheOptions>(options =>
{
    options.KeyPrefix = configuration["AbpDistributedCache:KeyPrefix"] ?? "";
});
```

The distributed caching mechanism is often needed in the application layer to cache frequently accessed data. You can inject the `IDistributedCache<TCacheItem, TCacheKey>` interface into your application service class and use it to cache data. Here is an example:

```csharp
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
```

Lastly, you can use the [Entity Cache](../../framework/infrastructure/entity-cache.md) feature, which offers a pre-built caching mechanism. If the requested item is not in the cache, it retrieves the data from the database, and when related data is manipulated, it automatically removes the item from the cache to ensure data consistency.