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
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public abstract class EntityCache_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IRepository<Product, Guid> ProductRepository;
    protected readonly IEntityCache<Product, Guid> ProductEntityCache;
    protected readonly IEntityCache<ProductCacheItem, Guid> ProductCacheItem;

    protected EntityCache_Tests()
    {
        ProductRepository = GetRequiredService<IRepository<Product, Guid>>();
        ProductEntityCache = GetRequiredService<IEntityCache<Product, Guid>>();
        ProductCacheItem = GetRequiredService<IEntityCache<ProductCacheItem, Guid>>();
    }

    [Fact]
    public async Task Should_Return_Null_IF_Entity_Not_Exist()
    {
        var notExistId = Guid.NewGuid();
        (await ProductEntityCache.FindAsync(notExistId)).ShouldBeNull();
        (await ProductCacheItem.FindAsync(notExistId)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Throw_EntityNotFoundException_IF_Entity_Not_Exist()
    {
        var notExistId = Guid.NewGuid();
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductEntityCache.GetAsync(notExistId));
        await Assert.ThrowsAsync<EntityNotFoundException<Product>>(() => ProductCacheItem.GetAsync(notExistId));
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
