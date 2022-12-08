# Injecting Service Dependencies to Entities with Entity Framework Core 7.0

[Dependency injection](https://docs.abp.io/en/abp/latest/Dependency-Injection) is a widely-used pattern of obtaining references to other services from our classes. It is a built-in feature when you develop ASP.NET Core applications. In this article, I will explain why we may need to have references to other services in an entity class and how we can implement Entity Framework Core's new `IMaterializationInterceptor` interface to provide these services to the entities using the standard dependency injection system.

> You can find the source code of the example application [here](https://github.com/abpframework/abp-samples/tree/master/EfCoreEntityDependencyInjectionDemo).

## The Problem

While developing applications based on [Domain-Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design) (DDD) patterns, we typically write our business code inside [application services](https://docs.abp.io/en/abp/latest/Application-Services), [domain services](https://docs.abp.io/en/abp/latest/Domain-Services) and [entities](https://docs.abp.io/en/abp/latest/Entities). Since the application and domain service instances are created by the dependency injection system, they can inject services into their constructors.

Here, an example domain service that injects a repository into its constructor:

````csharp
public class ProductManager : DomainService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductManager(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    //...
}
````

`ProductManager` can then use the `_productRepository` object in its methods to perform its business logic. In the following example, `ChangeCodeAsync` method is used to change a product's code (the `ProductCode` property) by ensuring uniqueness of product codes in the system:

````csharp
public class ProductManager : DomainService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductManager(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task ChangeCodeAsync(Product product, string newProductCode)
    {
        Check.NotNull(product, nameof(product));
        Check.NotNullOrWhiteSpace(newProductCode, nameof(newProductCode));

        if (product.ProductCode == newProductCode)
        {
            return;
        }
        
        if (await _productRepository.AnyAsync(x => x.ProductCode == newProductCode))
        {
            throw new ApplicationException(
                "Product code is already used: " + newProductCode);
        }

        product.ProductCode = newProductCode;
    }
}
````

Here, the `ProductManager` forces the rule "product code must be unique". Let's see the `Product` entity class too:

````csharp
public class Product : AuditedAggregateRoot<Guid>
{
    public string ProductCode { get; internal set; }
    
    public string Name { get; private set; }
    
    private Product()
    {
        /* This constructor is used by EF Core while
           getting the Product from database */
    }
    
    /* Primary constructor that should be used in the application code */
    public Product(string productCode, string name)
    {
        ProductCode = Check.NotNullOrWhiteSpace(productCode, nameof(productCode));
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
````

You see that the `ProductCode` property's setter is `internal`, which makes possible to set it from the `ProductManager` class as shown before.

This design has a problem: We had to make the `ProductCode` setter `internal`. Now, any developer may forget to use the `ProductManager.ChangeCodeAsync` method, and can directly set the `ProductCode` on the entity. So, we can't completely force the "product code must be unique" rule.

It would be better to move the `ChangeCodeAsync` method into the `Product` class and make the `ProductCode` property's setter `private`:

````csharp
public class Product : AuditedAggregateRoot<Guid>
{
    public string ProductCode { get; private set; }
    
    public string Name { get; private set; }
    
    // ...
    
    public async Task ChangeCodeAsync(string newProductCode)
    {
        Check.NotNullOrWhiteSpace(newProductCode, nameof(newProductCode));

        if (newProductCode == ProductCode)
        {
            return;
        }
        
        /* ??? HOW TO INJECT THE PRODUCT REPOSITORY HERE ??? */        
        if (await _productRepository.AnyAsync(x => x.ProductCode == newProductCode))
        {
            throw new ApplicationException("Product code is already used: " + newProductCode);
        }
        
        ProductCode = newProductCode;
    }
}
````

With that design, there is no way to set the `ProductCode` without applying the rule "product code must be unique". Great! But we have a problem: An entity class can not inject dependencies into its constructor, because an entity is not created using the dependency injection system. There are two common points of creating an entity:

* We can create an entity in our application code, using the standard `new` keyword, like `var product = new Product(...);`.
* Entity Framework (and any other ORM / database provider) creates entities after getting them from the database. They typically use the empty (default) constructor of the entity to create it, then sets the properties coming from the database query.

So, how we can use the product repository in the `Product.ChangeCodeAsync` method? If we forget the dependency injection system, we would think to add the repository as a parameter to the `ChangeCodeAsync` method and delegate the responsibility of obtaining the service reference to the caller of that method:

````csharp
public async Task ChangeCodeAsync(
    IRepository<Product, Guid> productRepository, string newProductCode)
{
    Check.NotNull(productRepository, nameof(productRepository));
    Check.NotNullOrWhiteSpace(newProductCode, nameof(newProductCode));

    if (newProductCode == ProductCode)
    {
        return;
    }

    if (await productRepository.AnyAsync(x => x.ProductCode == newProductCode))
    {
        throw new ApplicationException(
            "Product code is already used: " + newProductCode);
    }
    
    ProductCode = newProductCode;
}
````

However, that design would make hard to use the `ChangeCodeAsync` method, and also exposes its internal dependencies to outside. If we need another dependency in the `ChangeCodeAsync` method later, we should add another parameter, which will effect all the application code that uses the `ChangeCodeAsync` method. I think that's not reasonable. The next section offers a better and a more generic solution to the problem.

## The Solution

First of all, we can introduce an interface that should be implemented by the entity classes which needs to use services in their methods:

````csharp
public interface IInjectServiceProvider
{
     ICachedServiceProvider ServiceProvider { get; set; }
}
````

`ICachedServiceProvider` is a service that is provided by the ABP Framework. It extends the standard `IServiceProvider`, but caches the resolved services. Basically, it internally resolves a service only a single time, even if you resolve the service from it multiple times. The `ICachedServiceProvider` service itself is a scoped service, means it is created only once in a scope. We can use it to optimize the service resolution, however, the standard `IServiceProvider` would work as expected.

Next, we can implement the `IInjectServiceProvider` for our `Product` entity:

````csharp
public class Product : AuditedAggregateRoot<Guid>, IInjectServiceProvider
{
    public ICachedServiceProvider ServiceProvider { get; set; }

    //...
}
````

I will explain how to set the `ServiceProvider` property later, but first see how to use it in our `Product.ChangeCodeAsync` method. Here, the final `Product` class:

````csharp
public class Product : AuditedAggregateRoot<Guid>, IInjectServiceProvider
{
    public string ProductCode { get; internal set; }
    
    public string Name { get; private set; }
    
    public ICachedServiceProvider ServiceProvider { get; set; }

    private Product()
    {
        /* This constructor is used by EF Core while
           getting the Product from database */
    }
    
    /* Primary constructor that should be used in the application code */
    public Product(string productCode, string name)
    {
        ProductCode = Check.NotNullOrWhiteSpace(productCode, nameof(productCode));
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
    
    public async Task ChangeCodeAsync(string newProductCode)
    {
        Check.NotNullOrWhiteSpace(newProductCode, nameof(newProductCode));

        if (newProductCode == ProductCode)
        {
            return;
        }

        var productRepository = ServiceProvider
            .GetRequiredService<IRepository<Product, Guid>>();
        
        if (await productRepository.AnyAsync(x => x.ProductCode == newProductCode))
        {
            throw new ApplicationException("Product code is already used: " + newProductCode);
        }
        
        ProductCode = newProductCode;
    }
}
````

The `ChangeCodeAsync` method gets the product repository from the `ServiceProvider` and uses it to check if there is another product with the given `newProductCode` value.

Now, let's explain how to set the `ServiceProvider` value...

### Entity Framework Core Configuration

Entity Framework 7.0 introduces the `IMaterializationInterceptor` interceptor that allows us to manipulate an entity object just after the entity object is created as a result of database query.

We can write the following interceptor that sets the `ServiceProvider` property of an entity, if it implements the `IInjectServiceProvider` interface:

````csharp
public class ServiceProviderInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(
        MaterializationInterceptionData materializationData, 
        object instance)
    {
        if (instance is IInjectServiceProvider entity)
        {
            entity.ServiceProvider = materializationData
                .Context
                .GetService<ICachedServiceProvider>();
        }

        return instance;
    }
}
````

> Lifetime of the resolved services are tied to the lifetime of the related `DbContext` instance. So, you don't need to care if the resolved dependencies are disposed. ABP's [unit of work](https://docs.abp.io/en/abp/latest/Unit-Of-Work) system already disposes the `DbContext` instance when the unit of work is completed.

Once we defined such an interceptor, we should configure our `DbContext` class to use it. You can do it by overriding the `OnConfiguring` method in your `DbContext` class:

````csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.AddInterceptors(new ServiceProviderInterceptor());
}
````

Finally, you should ignore the `ServiceProvider` property in your entity mapping configuration in your `DbContext` (because we don't want to map it to a database table field):

````csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    // ...
    builder.Entity<Product>(b =>
    {
        // ...
        /* We should ignore the ServiceProvider on mapping! */
        b.Ignore(x => x.ServiceProvider);
    });
}
````

That's all. From now, EF Core will set the `ServiceProvider` property for you.

### Manually Creating Entities

While EF Core seamlessly set the `ServiceProvider` property while getting entities from database, you should still set it manually while creating new entities yourself.

**Example: Set `ServiceProvider` property while creating a new Product entity:**

````csharp
public async Task CreateAsync(CreateProductDto input)
{
    var product = new Product(input.ProductCode, input.Name)
    {
        ServiceProvider = _cachedServiceProvider
    };
    
    await _productRepository.InsertAsync(product);
}
````

Here, you may think that it is not necessary to set the `ServiceProvider`, because we haven't used the `ChangeCodeAsync` method. You are definitely right; It is not needed in this example, because it is clear to see the entity object is not used between the entity creation and saving it to the database. However, if you call a method of the entity, or pass it to another service before inserting into the database, you may not know if the `ServiceProvider` will be needed. So, you should carefully use it.

Basically, I've introduced the problem and the solution. In the next section, I will explain some limitations of that design and some of my other thoughts.

## Discussions

In this section, I will first discuss a slightly different way of obtaining services. Then I will explain limitations and problems of injecting services into entities.

### Why injected a service provider, but not the services?

As an obvious question, you may ask why we've property-injected a service provider object, then resolved the services manually. Can't we directly property-inject our dependencies?

**Example: Property-inject the `IRepository<Product, Guid>` service:**

````csharp
public class Product : AuditedAggregateRoot<Guid>
{
    // ...
    
    public IRepository<Product, Guid> ProductRepository { get; set; }
    
    public async Task ChangeCodeAsync(string newProductCode)
    {
        Check.NotNullOrWhiteSpace(newProductCode, nameof(newProductCode));

        if (newProductCode == ProductCode)
        {
            return;
        }

        if (await ProductRepository.AnyAsync(x => x.ProductCode == newProductCode))
        {
            throw new ApplicationException("Product code is already used: " + newProductCode);
        }
        
        ProductCode = newProductCode;
    }
}
````

Now, we don't need to implement the `IInjectServiceProvider` interface and manually resolve the `IRepository<Product, Guid>` object from the `ServiceProvider`. You see that the `ChangeCodeAsync` method is much simpler now.

So, how to set `ProductRepository`? For the EF Core interceptor part, you can somehow get all public properties of the entity via reflection. Then, for each property, check if such a service does exist, and set it from the dependency injection system if available. Surely, that will be less performant, but will work if you can truly implement. On the other hand, it would be extra hard to set all the dependencies of the entity while manually creating it using the `new` keyword. So, personally I wouldn't recommend that approach.

### Limitations

One important limitation is that you can not use the services inside your entity's constructor code. Ideally, the constructor of the `Product` class should check if the product code is already used before. See the following constructor:

````csharp
public Product(string productCode, string name)
{
    ProductCode = Check.NotNullOrWhiteSpace(productCode, nameof(productCode));
    Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    
    /* Can not check if product code is already used by another product? */
}
````

It is not possible to use the product repository here, because;

1. The services are property-injected. That means they will be set after the object creation has completed.
2. Even if the service is available, it won't be truly possible to call async code in a constructor. You know constructors can not be async in C#, but the repository and other service methods are generally designed as async.

So, if you want to force the "product code must be unique" rule, you should create an async domain service method (like `ProductManager.CreateAsync(...)`) and always use it to create products (you can make the `Product` class constructor `internal` to not allow to use it in the application layer).

### Design Problems

Beside the technical limitations, coupling your entities to external services is generally considered as a bad design. It makes your entities over-complicated, hard to test, and generally leads to take too much responsibility over the time.

## Conclusion

In this article, I tried to investigate all aspects of injecting services into entity classes. I explained how to use Entity Framework 7.0 `IMaterializationInterceptor` to implement property-injection pattern while getting entities from database.

Injecting services into entities seems a certain way of forcing some business rules in your entities. However, because of the current technical limitations, design issues and usage difficulties, I don't suggest to depend on services in your entities. Instead, create domain services when you need to implement a business rule that depends on external services and entities.

## The Source Code

* You can find the full source code of the example application [here](https://github.com/abpframework/abp-samples/tree/master/EfCoreEntityDependencyInjectionDemo).
* You can see [this pull request](https://github.com/abpframework/abp-samples/pull/207/files) for the changes I've done after creating the application.

## See Also

* [What's new in EF Core 7.0](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew)
* [ABP Framework: Dependency Injection](https://docs.abp.io/en/abp/latest/Dependency-Injection)
* [ABP Framework: Domain Driven Design](https://docs.abp.io/en/abp/latest/Domain-Driven-Design)