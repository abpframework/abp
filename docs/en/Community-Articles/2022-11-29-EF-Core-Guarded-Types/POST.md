# Value generation for DDD guarded types with Entity Framework Core 7.0

In domain-driven design (DDD), *guarded keys* can improve the type safety of key properties. This is achieved by wrapping the key type in another type which is specific to the use of the key. In this article, I will explain the cases why you may need to use guarded types and discuss the advantages and limitations when implementing to an ABP application.

> You can find the source code of the example application [here](https://github.com/abpframework/abp-samples/tree/master/EfCoreGuardedTypeDemo).

## The Problem

While developing an applications, there are many cases where we manually assign foreign keys that can be in guid type or integer type, etc. This manual assignment mistakes can cause miss-match of unique identifiers, such as **assigning a product ID to a category**, that can be hard to detect in the future. 

Here is a very simplified sample of wrong assignment when trying to update a product category:

````csharp
public class ProductAppService : MyProductStoreAppService, IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductAppService(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task UpdateProductCategoryAsync(Guid productId, Guid categoryId)
    {
        var productToUpdate = await _productRepository.GetAsync(productId);
        productToUpdate.CategoryId = productId; // Wrong assignment that causes error only at run-time

        await _productRepository.UpdateAsync(productToUpdate);
    }
}
````

While the sample demonstrates a very simple mistake, it is easier to come across similar mistakes when the business logic gets more complex especially when you are using methods with **multiple foreign key arguments**. The next section offers using guarded types to prevent these kind of problems as a solution to the problem.

## The Solution

Strongly-typed IDs (*guarded keys*) is a DDD approach to address this problem. One of the main problems with .NET users was handling the persisting these objects. With EFCore7, key properties can be guarded with type safety seamlessly. 

To use guarded keys, update your aggregate root or entity unique identifier with a complex type to overcome *primitive obsession*:

````csharp
public readonly struct CategoryId
{
    public CategoryId(Guid value) => Value = value;
    public Guid Value { get; }
}

public readonly struct ProductId
{
    public ProductId(Guid value) => Value = value;
    public Guid Value { get; }
}
````

You can now use these keys for your aggregate roots or entities:

```csharp
public class Product : AggregateRoot<ProductId>
{
    public ProductId Id { get; set; }
    public string Name { get; set; }
    public CategoryId CategoryId { get; set; }

    private Product() { }

    public Product(ProductId id, string name) : base(id)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
    }
}

public class Category : AggregateRoot<CategoryId>
{
    public CategoryId Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; } = new();

    private Category() { }

    public Category(CategoryId id, string name) : base(id)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
    }
}
```

`ProductId` and `CategoryId` guarded key types shown in the sample use `Guid` key values, which means Guid values will be used in the mapped database tables. This is achieved by defining [value converters](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions) for the types. 

Override the `ConfigureConventions` method of your DbContext to use the value converters:

````csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Properties<ProductId>().HaveConversion<ProductIdConverter>();
    configurationBuilder.Properties<CategoryId>().HaveConversion<CategoryIdConverter>();
}

private class ProductIdConverter : ValueConverter<ProductId, Guid>
{
    public ProductIdConverter()
        : base(v => v.Value, v => new(v))
        {
        }
}

private class CategoryIdConverter : ValueConverter<CategoryId, Guid>
{
    public CategoryIdConverter()
        : base(v => v.Value, v => new(v))
        {
        }
}
````

>  The code here uses `struct` types. This means they have appropriate value-type semantics for use as keys. If `class` types are used instead, then they need to either override equality semantics or also specify a [value comparer](https://learn.microsoft.com/en-us/ef/core/modeling/value-comparers).

Now, you can use generic (or custom) repositories of ABP using the guarded type as the key for the repository:

```csharp
public class ProductStoreDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Category, CategoryId> _categoryRepository;
    private readonly IRepository<Product, ProductId> _productRepository;

    public ProductStoreDataSeedContributor(
        IRepository<Category, CategoryId> categoryRepository,
        IRepository<Product, ProductId> productRepository
    )
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    // ...
}
```

You can also use `integer` as guarded type for your key properties and use [Sequence-based key generation for SQL Server](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#sequence-based-key-generation-for-sql-server) for value generation.

## Discussions

In this section, I will discuss the use cases of guarded types and limitations when implementing to an ABP application.

### Do I need to use guarded types?

If you are already following the best practices of  [Domain-Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design), you are aware that **updates** and **creations** of an aggregate are done **over** the aggregate root itself as a whole unit. And entity state changes of an aggregate root should be done using the [domain services](https://docs.abp.io/en/abp/latest/Domain-Services). Domain services should already validate the entity.

**Example: Using domain service to update product:**

````csharp
public class ProductManager : DomainService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductManager(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product> AssignCategory(Product product, Category category)
    {
        // ...
        
        product.CategoryId = category.Id;
        
        //..
    }
}
````

In this sample, domain service validates that both **product** and the **category** entities, passed by the application layer, are valid objects since they are not key properties. However, manual assignment is already in place and more complex the domain logic, higher to miss out mistakes. At the end, it will depend on your tolerance level for developer errors comparing to the time you want to spend time on additional code base for guarded types.

### Limitations

One important limitation is automatic value generation when using `Guid` as guarded type for your key properties. The basic repository can not generate the unique identifier automatically by the time this article is written:

```csharp
public readonly struct ProductId
{
    public ProductId(Guid value) => Value = value;
    public Guid Value { get; }
}
```

you need to generate the unique identifier **manually**:

````csharp
var newProduct = await _productRepository.InsertAsync(
    new Product(new ProductId(_guidGenerator.Create()), "New product")
);
````

## Conclusion

In this article, I tried to explain DDD guarded types for key properties and value generation for these properties using Entity Framework 7.0 and ABP.

Using strongly-typed key properties reduce the chance of unnoticed errors. Admittedly it increases the code complexity by adding new types to your solution with extra coding, especially if you are using classes as keys. Guarded types provide improved safety for your code base at the expense of additional code complexity as in many DDD concepts and patterns.

If you have a large team working on a large scale solution containing complex business logics where key assignments are abundant or if you are using methods with multiple foreign key arguments, I personally suggest using guarded types.

## The Source Code

* You can find the full source code of the example application [here](https://github.com/abpframework/abp-samples/tree/master/EfCoreGuardedTypeDemo).

## See Also

* [What's new in EF Core 7.0](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew)
* [ABP Framework: Domain Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design)