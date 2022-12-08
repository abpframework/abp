# Entity Cache

ABP Framework provides an entity caching system that works on top of the [distributed caching](Caching.md) system. It does the following operations on behalf of you:

* Gets the entity from the database (by using the [repositories](Repositories.md)) in its first call and then gets it from the cache in subsequent calls.
* Automatically invalidates the cached entity if the entity is updated or deleted. Thus, it will be retrieved from the database in the next call and will be re-cached.

## Caching Entity Objects

`IEntityCache<TEntityCacheItem, TKey>` is a simple service provided by the ABP Framework for caching entities. Assume that you have a `Product` entity as shown below:

```csharp
public class Product : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int StockCount { get; set; }
}
```

If you want to cache this entity, you should first configure the [dependency injection](Dependency-Injection.md) system to register the `IEntityCache` service in the `ConfigureServices` method of your [module class](Module-Development-Basics.md):

```csharp
context.Services.AddEntityCache<Product, Guid>();
```

Now you can inject the `IEntityCache<Product, Guid>` service wherever you need:

```csharp
public class ProductAppService : ApplicationService, IProductAppService
{
    private readonly IEntityCache<Product, Guid> _productCache;

    public ProductAppService(IEntityCache<Product, Guid> productCache)
    {
        _productCache = productCache;
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productCache.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }
}
```

> Note that we've used the `ObjectMapper` service to map from `Product` to `ProductDto`. You should configure that [object mapping](Object-To-Object-Mapping.md) to make that example service properly works.

That's all. The cache name (in the distributed cache server) will be full name (with namespace) of the `Product` class. You can use the `[CacheName]` attribute to change it. Please refer to the [caching document](Caching.md) for details.

## Using a Cache Item Class

In the previous section, we've directly cached the `Product` entity. In that case, the `Product` class must be serializable to JSON (and deserializable from JSON). Sometimes that might not be possible or you may want to use another class to store the cache data. For example, we may want to use the `ProductDto` class instead of the `Product` class for the cached object if the `Product` entity.

Assume that we've created a `ProductDto` class as shown below:

```csharp
public class ProductDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int StockCount { get; set; }
}
```

Now, we can register the entity cache services to [dependency injection](Dependency-Injection.md) in the `ConfigureServices` method of your [module class](Module-Development-Basics.md) with three generic parameters, as shown below:

```csharp
context.Services.AddEntityCache<Product, ProductDto, Guid>();
```

Since the entity cache system will perform the [object mapping](Object-To-Object-Mapping.md) (from `Product` to `ProductDto`), we should configure the object map. Here, an example configuration with [AutoMapper](https://automapper.org/):

```csharp
public class MyMapperProfile : Profile
{
    public MyMapperProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}
```

Now, you can inject the `IEntityCache<ProductDto, Guid>` service wherever you want:

```csharp
public class ProductAppService : ApplicationService, IProductAppService
{
    private readonly IEntityCache<ProductDto, Guid> _productCache;

    public ProductAppService(IEntityCache<ProductDto, Guid> productCache)
    {
        _productCache = productCache;
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        return await _productCache.GetAsync(id);
    }
}
```

Notice that the `_productCache.GetAsync` method already returns a `ProductDto` object, so we could directly return it from out application service.

## Configuration

All of the `context.Services.AddEntityCache()` methods get an optional `DistributedCacheEntryOptions` parameter where you can easily configure the caching options:

```csharp
context.Services.AddEntityCache<Product, ProductDto, Guid>(
    new DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(30)
    }
);
```

> The default cache duration is **2 minutes** with the `AbsoluteExpirationRelativeToNow` configuration.

## Additional Notes

* Entity classes should be serializable/deserializable to/from JSON to be cached (because it's serialized to JSON when saving in the [Distributed Cache](Caching.md)). If your entity class is not serializable, you can consider using a cache-item/DTO class instead, as explained before.
* Entity Caching System is designed as **read-only**. You should use the standard [repository](Repositories.md) methods to manipulate the entity if you need. If you need to manipulate (update) the entity, do not get it from the entity cache. Instead, read it from the repository, change it and update using the repository.

## See Also

* [Distributed caching](Caching.md)
* [Entities](Entities.md)
* [Repositories](Repositories.md)
