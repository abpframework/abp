using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        await Assert.ThrowsAsync<EntityNotFoundException>(() => ProductEntityCache.GetAsync(notExistId));
        await Assert.ThrowsAsync<EntityNotFoundException>(() => ProductCacheItem.GetAsync(notExistId));
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
