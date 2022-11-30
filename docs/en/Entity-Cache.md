# Entity Cache

ABP Framework provides **Distributed Entity Caching System** for caching entities. 

You can use this caching mechanism if you want to cache your entity objects automatically and retrieve them from a cache instead of querying it from a database repeatedly.

## How Distributed Entity Caching System Works?

ABP's Entity Caching System does the following operations on behalf of you:

* It gets the entity from the database (by using the [Repositories](Repositories.md)) in its first call and then gets from the cache in subsequent calls.
* It automatically invalidates the cached entity if the entity is updated or deleted. Thus, it will be retrieved from the database in the next call and will be re-cached.
* It uses the cache class's **FullName** as a cache name by default. You can use the `CacheName` attribute on the cache item class to set the cache name.

## Installation

[Volo.Abp.Caching](https://www.nuget.org/packages/Volo.Abp.Caching) is the main package for the ABP's caching system and it's already installed in [the application startup template](Startup-Templates/Index.md). So, you don't need to install it manually.

## Usage

`IEntityCache<TEntityCacheItem, TKey>` is a simple service provided by the ABP Framework for caching entities. It's designed as read-only and contains two methods: `FindAsync` and `GetAsync`.

### Caching Entities 

**Example: `Product` entity**

```csharp
[CacheName("Products")]
public class Product : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int StockCount { get; set; }
}
```

* This example uses the `CacheName` attribute for the `Product` class to set the cache name. By default, the cache class's **FullName** is used for the cache name.

If you want to cache this entity, first you should configure the [dependency injection](Dependency-Injection.md) to register the `IEntityCache` service in the `ConfigureServices` method of your [module class](Module-Development-Basics.md):

```csharp
context.Services.AddEntityCache<Product, Guid>();
```

Then configure the [object mapper](https://docs.abp.io/en/abp/latest/Object-To-Object-Mapping) (for `Product` to `ProductDto` mapping):

```csharp
public class MyProjectNameAutoMapperProfile : Profile
{
    public MyProjectNameAutoMapperProfile()
    {
        //other mappings...

        CreateMap<Product, ProductDto>();
    }
}
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

* Here, we've directly cached the `Product` entity. In that case, the `Product` class must be serializable. Sometimes this might not be possible and you may want to use another class to store the cache data. For example, we may want to use the `ProductDto` class instead of the `Product` class for the cached object if the `Product` entity is not serializable.

### Caching Cache Item Classes

`IEntityCache<TEntity, TEntityCacheItem, TKey>` service can be used for caching other cache item classes if the entity is not serializable.

**Example: `ProductDto` class**

```csharp
public class ProductDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int StockCount { get; set; }
}
```

Register the entity cache services to [dependency injection](Dependency-Injection.md) in the `ConfigureServices` method of your [module class](Module-Development-Basics.md):

```csharp
context.Services.AddEntityCache<Product, ProductDto, Guid>();
```

Configure the [object mapper](https://docs.abp.io/en/abp/latest/Object-To-Object-Mapping) (for `Product` to `ProductDto` mapping):

```csharp
public class MyProjectNameAutoMapperProfile : Profile
{
    public MyProjectNameAutoMapperProfile()
    {
        //other mappings...

        CreateMap<Product, ProductDto>();
    }
}
```

Then, you can inject the `IEntityCache<ProductDto, Guid>` service wherever you want:

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

## Configurations

### Registering the Entity Cache Services

You can use one of the `AddEntityCache` methods to register entity cache services to the [Dependency Injection](Dependency-Injection.md) system.

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();

    //other configurations...

    //directly cache the entity object (Basket)
    context.Services.AddEntityCache<Basket, Guid>();

    //cache the ProductDto class
    context.Services.AddEntityCache<Product, ProductDto, Guid>();
}
```

* You can register entity cache by using the `context.Services.AddEntityCache<TEntity, TKey>()` method for directly cache the entity object.
* Or alternatively, you can use the `context.Services.AddEntityCache<TEntity, TEntityCacheItem, TKey>()` method to configure entities that are mapped to a cache item.

### Caching Options

All of the `context.Services.AddEntityCache()` methods get an optional `DistributedCacheEntryOptions` parameter where you can easily configure the caching options:

```csharp
context.Services.AddEntityCache<Product, ProductDto, Guid>(
    new DistributedCacheEntryOptions
    {
        SlidingExpiration = TimeSpan.FromMinutes(30)
    }
);
```

> The default cache duration is **2 minutes** with the `AbsoluteExpirationRelativeToNow` configuration and by configuring the `DistributedCacheEntryOptions` you can change it easily.

## Additonal Notes

* Entity classes should be serializable/deserializable to/from JSON to be cached (because it's serialized to JSON when saving in the [Distributed Cache](Caching.md)). If your entity class is not serializable, you can consider using a cache-item/DTO class instead, as mentioned in the *Usage* section above.
* Entity Caching System is designed as **read-only**. So, you shouldn't make changes to the same entity when you use the entity cache. Instead, you should always read it from the database to ensure transactional consistency.

## See Also

* [Caching](Caching.md)