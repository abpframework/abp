using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.TestApp.Testing;

public class EntityCache_Tests<TStartupModule> : TestAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly IRepository<Product, Guid> ProductRepository;
    protected readonly IEntityCache<Product, Guid> ProductEntityCache;
    protected readonly IEntityCache<ProductCacheItem, Guid> ProductCacheItem;

    public EntityCache_Tests()
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
        var id = Guid.NewGuid();
        var product = await ProductRepository.InsertAsync(new Product(id, "Product1", decimal.One));
        
        product = await ProductEntityCache.FindAsync(product.Id);
        product.ShouldNotBeNull();
        product.Id.ShouldBe(id);
        product.Name.ShouldBe("Product1");
        product.Price.ShouldBe(decimal.One);
        
        var productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem.Id.ShouldBe(id);
        productCacheItem.Name.ShouldBe("Product1");
        productCacheItem.Price.ShouldBe(decimal.One);
    }
    
    [Fact]
    public async Task Should_Return_Null_IF_Deleted()
    {
        var id = Guid.NewGuid();
        var product = await ProductRepository.InsertAsync(new Product(id, "Product1", decimal.One));
        (await ProductEntityCache.FindAsync(id)).ShouldNotBeNull();
        (await ProductCacheItem.FindAsync(id)).ShouldNotBeNull();

        await ProductRepository.DeleteAsync(id);
        
        (await ProductEntityCache.FindAsync(product.Id)).ShouldBeNull();
        (await ProductCacheItem.FindAsync(product.Id)).ShouldBeNull();
    }

    [Fact]
    public async Task Should_Return_New_EntityCache_IF_Updated()
    {
        var id = Guid.NewGuid();
        var product = await ProductRepository.InsertAsync(new Product(id, "Product1", decimal.One));
        (await ProductEntityCache.FindAsync(id)).ShouldNotBeNull();
        (await ProductCacheItem.FindAsync(id)).ShouldNotBeNull();
        
        product.Name = "Product2";
        product.Price = decimal.Zero;
        await ProductRepository.UpdateAsync(product);
        
        product = await ProductEntityCache.FindAsync(product.Id);
        product.ShouldNotBeNull();
        product.Id.ShouldBe(id);
        product.Name.ShouldBe("Product2");
        product.Price.ShouldBe(decimal.Zero);
        
        var productCacheItem = await ProductCacheItem.FindAsync(product.Id);
        productCacheItem.ShouldNotBeNull();
        productCacheItem.Id.ShouldBe(id);
        productCacheItem.Name.ShouldBe("Product2");
        productCacheItem.Price.ShouldBe(decimal.Zero);
    }
}

[Serializable]
public class Product : Entity<Guid>
{
    public Product()
    {
        
    }

    public Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    [JsonInclude]
    public override Guid Id { get; protected set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}

[Serializable]
public class ProductCacheItem
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}