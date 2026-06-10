using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Shouldly;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class EntityCache_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IRepository<Product, Guid> ProductRepository;
    protected readonly IEntityCache<Product, Guid> ProductEntityCache;
    protected readonly IEntityCache<ProductCacheItem, Guid> ProductCacheItem;
    protected readonly IEntityCache<CustomProductCacheItem, Guid> CustomProductCacheItem;
    protected readonly IEntityCache<CustomProductCacheItemWithoutPriorRegistration, Guid> CustomProductCacheItemWithoutPriorRegistration;

    protected EntityCache_Tests()
    {
        ProductRepository = GetRequiredService<IRepository<Product, Guid>>();
        ProductEntityCache = GetRequiredService<IEntityCache<Product, Guid>>();
        ProductCacheItem = GetRequiredService<IEntityCache<ProductCacheItem, Guid>>();
        CustomProductCacheItem = GetRequiredService<IEntityCache<CustomProductCacheItem, Guid>>();
        CustomProductCacheItemWithoutPriorRegistration = GetRequiredService<IEntityCache<CustomProductCacheItemWithoutPriorRegistration, Guid>>();
    }

    [Fact]
    public async Task Should_Return_Null_IF_Entity_Not_Exist()
    {
        var notExistId = Guid.NewGuid();
        (await ProductEntityCache.FindAsync(notExistId)).ShouldBeNull();
        (await ProductCacheItem.FindAsync(notExistId)).ShouldBeNull();
    }

    [Fact]
    public async Task FindMany_Should_Return_Null_For_Not_Existing_Entities()
    {
        var notExistId = Guid.NewGuid();
        var result = await ProductEntityCache.FindManyAsync(new[] { notExistId });
        result.Count.ShouldBe(1);
        result[0].ShouldBeNull();

        var cacheItemResult = await ProductCacheItem.FindManyAsync(new[] { notExistId });
        cacheItemResult.Count.ShouldBe(1);
        cacheItemResult[0].ShouldBeNull();
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_IF_Entity_Not_Exist()
    {
        var notExistId = Guid.NewGuid();
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductEntityCache.GetAsync(notExistId));
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductCacheItem.GetAsync(notExistId));
    }

    [Fact]
    public async Task GetMany_Should_Throw_EntityNotFoundException_For_Not_Existing_Entities()
    {
        var notExistId = Guid.NewGuid();
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductEntityCache.GetManyAsync(new[] { notExistId }));
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductCacheItem.GetManyAsync(new[] { notExistId }));
    }

    [Fact]
    public async Task Should_Return_EntityCache()
    {
        var product = await ProductEntityCache.FindAsync(TestDataBuilder.ProductId);
        product.ShouldNotBeNull();
        product = await ProductEntityCache.FindAsync(TestDataBuilder.ProductId);
        product.ShouldNotBeNull();
        product.Id.ShouldBe(TestDataBuilder.ProductId);
        product.Name.ShouldBe("Product1");
        product.Price.ShouldBe(decimal.One);

        var productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem.Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItem.Name.ShouldBe("Product1");
        productCacheItem.Price.ShouldBe(decimal.One);
    }

    [Fact]
    public async Task FindMany_Should_Return_EntityCache()
    {
        var notExistId = Guid.NewGuid();
        var ids = new[] { TestDataBuilder.ProductId, notExistId };

        var products = await ProductEntityCache.FindManyAsync(ids);
        products.Count.ShouldBe(2);
        products[0].ShouldNotBeNull();
        products[0]!.Id.ShouldBe(TestDataBuilder.ProductId);
        products[0]!.Name.ShouldBe("Product1");
        products[0]!.Price.ShouldBe(decimal.One);
        products[1].ShouldBeNull();

        // Call again to test caching
        products = await ProductEntityCache.FindManyAsync(ids);
        products.Count.ShouldBe(2);
        products[0].ShouldNotBeNull();
        products[0]!.Id.ShouldBe(TestDataBuilder.ProductId);

        var productCacheItems = await ProductCacheItem.FindManyAsync(ids);
        productCacheItems.Count.ShouldBe(2);
        productCacheItems[0].ShouldNotBeNull();
        productCacheItems[0]!.Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItems[0]!.Name.ShouldBe("Product1");
        productCacheItems[0]!.Price.ShouldBe(decimal.One);
        productCacheItems[1].ShouldBeNull();
    }

    [Fact]
    public async Task GetMany_Should_Return_EntityCache()
    {
        var products = await ProductEntityCache.GetManyAsync(new[] { TestDataBuilder.ProductId });
        products.Count.ShouldBe(1);
        products[0].Id.ShouldBe(TestDataBuilder.ProductId);
        products[0].Name.ShouldBe("Product1");
        products[0].Price.ShouldBe(decimal.One);

        // Call again to test caching
        products = await ProductEntityCache.GetManyAsync(new[] { TestDataBuilder.ProductId });
        products.Count.ShouldBe(1);
        products[0].Id.ShouldBe(TestDataBuilder.ProductId);

        var productCacheItems = await ProductCacheItem.GetManyAsync(new[] { TestDataBuilder.ProductId });
        productCacheItems.Count.ShouldBe(1);
        productCacheItems[0].Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItems[0].Name.ShouldBe("Product1");
        productCacheItems[0].Price.ShouldBe(decimal.One);
    }

    [Fact]
    public async Task Should_Return_Null_IF_Deleted()
    {
        await ProductRepository.DeleteAsync(TestDataBuilder.ProductId);

        (await ProductEntityCache.FindAsync(TestDataBuilder.ProductId)).ShouldBeNull();
        (await ProductCacheItem.FindAsync(TestDataBuilder.ProductId)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Return_New_EntityCache_IF_Added()
    {
        var productId = Guid.NewGuid();
        (await ProductEntityCache.FindAsync(productId)).ShouldBeNull();
        (await ProductCacheItem.FindAsync(productId)).ShouldBeNull();

        var product = new Product(productId, "Product2", decimal.Zero);
        await ProductRepository.InsertAsync(product);

        product = await ProductEntityCache.FindAsync(product.Id);
        product.ShouldNotBeNull();
        product.Id.ShouldBe(productId);
        product.Name.ShouldBe("Product2");
        product.Price.ShouldBe(decimal.Zero);

        var productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem.Id.ShouldBe(productId);
        productCacheItem.Name.ShouldBe("Product2");
        productCacheItem.Price.ShouldBe(decimal.Zero);
    }

    [Fact]
    public async Task Should_Return_New_EntityCache_IF_Updated()
    {
        (await ProductEntityCache.FindAsync(TestDataBuilder.ProductId)).ShouldNotBeNull();
        (await ProductCacheItem.FindAsync(TestDataBuilder.ProductId)).ShouldNotBeNull();

        var product = await ProductRepository.FindAsync(TestDataBuilder.ProductId);
        product.Name = "Product2";
        product.Price = decimal.Zero;
        await ProductRepository.UpdateAsync(product);

        product = await ProductEntityCache.FindAsync(product.Id);
        product.ShouldNotBeNull();
        product.Id.ShouldBe(TestDataBuilder.ProductId);
        product.Name.ShouldBe("Product2");
        product.Price.ShouldBe(decimal.Zero);

        var productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem.Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItem.Name.ShouldBe("Product2");
        productCacheItem.Price.ShouldBe(decimal.Zero);
    }

    [Fact]
    public async Task FindMany_Should_Handle_Duplicate_Ids()
    {
        var ids = new[] { TestDataBuilder.ProductId, TestDataBuilder.ProductId };

        var products = await ProductEntityCache.FindManyAsync(ids);
        products.Count.ShouldBe(2);
        products[0].ShouldNotBeNull();
        products[0]!.Id.ShouldBe(TestDataBuilder.ProductId);
        products[1].ShouldNotBeNull();
        products[1]!.Id.ShouldBe(TestDataBuilder.ProductId);
    }

    [Fact]
    public async Task GetMany_Should_Handle_Duplicate_Ids()
    {
        var ids = new[] { TestDataBuilder.ProductId, TestDataBuilder.ProductId };

        var products = await ProductEntityCache.GetManyAsync(ids);
        products.Count.ShouldBe(2);
        products[0].Id.ShouldBe(TestDataBuilder.ProductId);
        products[1].Id.ShouldBe(TestDataBuilder.ProductId);
    }

    [Fact]
    public async Task ReplaceEntityCache_Should_Use_Custom_Mapping()
    {
        var product = await CustomProductCacheItem.FindAsync(TestDataBuilder.ProductId);
        product.ShouldNotBeNull();
        product.Name.ShouldBe("PRODUCT1");
    }

    [Fact]
    public async Task ReplaceEntityCache_Without_Prior_Registration_Should_Work()
    {
        var product = await CustomProductCacheItemWithoutPriorRegistration.FindAsync(TestDataBuilder.ProductId);
        product.ShouldNotBeNull();
        product.Name.ShouldBe("PRODUCT1");
    }

    [Fact]
    public async Task FindManyAsDictionary_Should_Return_Null_For_Not_Existing_Entities()
    {
        var notExistId = Guid.NewGuid();

        var result = await ProductEntityCache.FindManyAsDictionaryAsync(new[] { notExistId });
        result.Count.ShouldBe(1);
        result.ContainsKey(notExistId).ShouldBeTrue();
        result[notExistId].ShouldBeNull();

        var cacheItemResult = await ProductCacheItem.FindManyAsDictionaryAsync(new[] { notExistId });
        cacheItemResult.Count.ShouldBe(1);
        cacheItemResult.ContainsKey(notExistId).ShouldBeTrue();
        cacheItemResult[notExistId].ShouldBeNull();
    }

    [Fact]
    public async Task GetManyAsDictionary_Should_Throw_EntityNotFoundException_For_Not_Existing_Entities()
    {
        var notExistId = Guid.NewGuid();
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductEntityCache.GetManyAsDictionaryAsync(new[] { notExistId }));
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductCacheItem.GetManyAsDictionaryAsync(new[] { notExistId }));
    }

    [Fact]
    public async Task FindManyAsDictionary_Should_Return_EntityCache()
    {
        var notExistId = Guid.NewGuid();
        var ids = new[] { TestDataBuilder.ProductId, notExistId };

        var products = await ProductEntityCache.FindManyAsDictionaryAsync(ids);
        products.Count.ShouldBe(2);
        products[TestDataBuilder.ProductId].ShouldNotBeNull();
        products[TestDataBuilder.ProductId]!.Id.ShouldBe(TestDataBuilder.ProductId);
        products[TestDataBuilder.ProductId]!.Name.ShouldBe("Product1");
        products[TestDataBuilder.ProductId]!.Price.ShouldBe(decimal.One);
        products[notExistId].ShouldBeNull();

        // Call again to test caching
        products = await ProductEntityCache.FindManyAsDictionaryAsync(ids);
        products.Count.ShouldBe(2);
        products[TestDataBuilder.ProductId].ShouldNotBeNull();

        var productCacheItems = await ProductCacheItem.FindManyAsDictionaryAsync(ids);
        productCacheItems.Count.ShouldBe(2);
        productCacheItems[TestDataBuilder.ProductId].ShouldNotBeNull();
        productCacheItems[TestDataBuilder.ProductId]!.Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItems[TestDataBuilder.ProductId]!.Name.ShouldBe("Product1");
        productCacheItems[TestDataBuilder.ProductId]!.Price.ShouldBe(decimal.One);
        productCacheItems[notExistId].ShouldBeNull();
    }

    [Fact]
    public async Task GetManyAsDictionary_Should_Return_EntityCache()
    {
        var products = await ProductEntityCache.GetManyAsDictionaryAsync(new[] { TestDataBuilder.ProductId });
        products.Count.ShouldBe(1);
        products[TestDataBuilder.ProductId].Id.ShouldBe(TestDataBuilder.ProductId);
        products[TestDataBuilder.ProductId].Name.ShouldBe("Product1");
        products[TestDataBuilder.ProductId].Price.ShouldBe(decimal.One);

        // Call again to test caching
        products = await ProductEntityCache.GetManyAsDictionaryAsync(new[] { TestDataBuilder.ProductId });
        products.Count.ShouldBe(1);
        products[TestDataBuilder.ProductId].Id.ShouldBe(TestDataBuilder.ProductId);

        var productCacheItems = await ProductCacheItem.GetManyAsDictionaryAsync(new[] { TestDataBuilder.ProductId });
        productCacheItems.Count.ShouldBe(1);
        productCacheItems[TestDataBuilder.ProductId].Id.ShouldBe(TestDataBuilder.ProductId);
        productCacheItems[TestDataBuilder.ProductId].Name.ShouldBe("Product1");
        productCacheItems[TestDataBuilder.ProductId].Price.ShouldBe(decimal.One);
    }

    [Fact]
    public async Task FindManyAsDictionary_Should_Handle_Duplicate_Ids()
    {
        var ids = new[] { TestDataBuilder.ProductId, TestDataBuilder.ProductId };

        var products = await ProductEntityCache.FindManyAsDictionaryAsync(ids);
        products.Count.ShouldBe(1);
        products[TestDataBuilder.ProductId].ShouldNotBeNull();
        products[TestDataBuilder.ProductId]!.Id.ShouldBe(TestDataBuilder.ProductId);
    }

    [Fact]
    public async Task GetManyAsDictionary_Should_Handle_Duplicate_Ids()
    {
        var ids = new[] { TestDataBuilder.ProductId, TestDataBuilder.ProductId };

        var products = await ProductEntityCache.GetManyAsDictionaryAsync(ids);
        products.Count.ShouldBe(1);
        products[TestDataBuilder.ProductId].Id.ShouldBe(TestDataBuilder.ProductId);
    }

    [Fact]
    public void EntityCache_Default_Options_Should_Be_2_Minutes()
    {
        var productCache = GetRequiredService<IDistributedCache<EntityCacheItemWrapper<ProductCacheItem2>, Guid>>();

        var productOptions = GetDefaultCachingOptions(productCache);
        productOptions.AbsoluteExpirationRelativeToNow.ShouldBe(TimeSpan.FromMinutes(2));
        productOptions.SlidingExpiration.ShouldBeNull();
    }

    [Fact]
    public void EntityCache_Configured_Options_Should_Be_Applied()
    {
        var productCache = GetRequiredService<IDistributedCache<EntityCacheItemWrapper<Product>, Guid>>();
        var productCacheItemCache = GetRequiredService<IDistributedCache<EntityCacheItemWrapper<ProductCacheItem>, Guid>>();

        var productOptions = GetDefaultCachingOptions(productCache);
        productOptions.AbsoluteExpirationRelativeToNow.ShouldBe(TimeSpan.FromMinutes(7));
        productOptions.SlidingExpiration.ShouldBeNull();

        var productCacheItemOptions = GetDefaultCachingOptions(productCacheItemCache);
        productCacheItemOptions.AbsoluteExpirationRelativeToNow.ShouldBe(TimeSpan.FromMinutes(9));
        productCacheItemOptions.SlidingExpiration.ShouldBeNull();
    }

    private static DistributedCacheEntryOptions GetDefaultCachingOptions(object instance)
    {
        var internalCacheProperty = instance
            .GetType()
            .GetProperty("InternalCache", BindingFlags.Instance | BindingFlags.Public);

        if (internalCacheProperty != null)
        {
            instance = internalCacheProperty.GetValue(instance);
        }

        var defaultOptionsField = instance
            .GetType()
            .GetField("DefaultCacheOptions", BindingFlags.Instance | BindingFlags.NonPublic);

        return (DistributedCacheEntryOptions)defaultOptionsField.GetValue(instance);
    }
}

[Serializable]
public class Product : FullAuditedAggregateRoot<Guid>
{
    public Product()
    {

    }

    public Product(Guid id, string name, decimal price)
       : base(id)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

[Serializable]
[CacheName("ProductCacheItem")]
public class ProductCacheItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

[Serializable]
[CacheName("ProductCacheItem2")]
public class ProductCacheItem2
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

[Serializable]
[CacheName("CustomProductCacheItem")]
public class CustomProductCacheItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

public class CustomProductEntityCache : EntityCacheWithObjectMapper<Product, CustomProductCacheItem, Guid>
{
    public CustomProductEntityCache(
        IReadOnlyRepository<Product, Guid> repository,
        IDistributedCache<EntityCacheItemWrapper<CustomProductCacheItem>, Guid> cache,
        IUnitOfWorkManager unitOfWorkManager,
        IObjectMapper objectMapper)
        : base(repository, cache, unitOfWorkManager, objectMapper)
    {
    }

    protected override CustomProductCacheItem MapToValue(Product entity)
    {
        return new CustomProductCacheItem
        {
            Id = entity.Id,
            Name = entity.Name.ToUpperInvariant(),
            Price = entity.Price
        };
    }
}

[Serializable]
[CacheName("CustomProductCacheItemWithoutPriorRegistration")]
public class CustomProductCacheItemWithoutPriorRegistration
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

public class CustomProductEntityCacheWithoutPriorRegistration : EntityCacheWithObjectMapper<Product, CustomProductCacheItemWithoutPriorRegistration, Guid>
{
    public CustomProductEntityCacheWithoutPriorRegistration(
        IReadOnlyRepository<Product, Guid> repository,
        IDistributedCache<EntityCacheItemWrapper<CustomProductCacheItemWithoutPriorRegistration>, Guid> cache,
        IUnitOfWorkManager unitOfWorkManager,
        IObjectMapper objectMapper)
        : base(repository, cache, unitOfWorkManager, objectMapper)
    {
    }

    protected override CustomProductCacheItemWithoutPriorRegistration MapToValue(Product entity)
    {
        return new CustomProductCacheItemWithoutPriorRegistration
        {
            Id = entity.Id,
            Name = entity.Name.ToUpperInvariant(),
            Price = entity.Price
        };
    }
}
