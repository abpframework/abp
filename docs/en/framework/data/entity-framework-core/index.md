# Entity Framework Core Integration

This document explains how to integrate EF Core as an ORM provider to ABP based applications and how to configure it.

## Installation

`Volo.Abp.EntityFrameworkCore` is the main NuGet package for the EF Core integration. Install it to your project (for a layered application, to your data/infrastructure layer):

```` shell
abp add-package Volo.Abp.EntityFrameworkCore
````

> If you haven't done it yet, you first need to install the [ABP CLI](../../../cli/index.md). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.EntityFrameworkCore).
>

> Note: Instead, you can directly download a [startup template](https://abp.io/Templates) with EF Core pre-installed.

### Database Management System Selection

Entity Framework Core supports various database management systems ([see all](https://docs.microsoft.com/en-us/ef/core/providers/)). ABP and this document don't depend on any specific DBMS. If you are creating a [reusable application module](../../../modules), avoid to depend on a specific DBMS package. However, in a final application you eventually will select a DBMS.

> See [Switch to Another DBMS for Entity Framework Core](./other-dbms.md) document to learn how to switch the DBMS.

## Creating DbContext

Your `DbContext` class should be derived from `AbpDbContext<T>` as shown below:

````C#
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompany.MyProject
{
    public class MyDbContext : AbpDbContext<MyDbContext>
    {
        //...your DbSet properties here

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
````

### About the EF Core Fluent Mapping

The [application startup template](../../../solution-templates/layered-web-application) has been configured to use the [EF Core fluent configuration API](https://docs.microsoft.com/en-us/ef/core/modeling/) to map your entities to your database tables.

You can still use the **data annotation attributes** (like `[Required]`) on the properties of your entity while the ABP documentation generally follows the **fluent mapping API** approach. It is up to you.

ABP has some **base entity classes** and **conventions** (see the [entities document](../../architecture/domain-driven-design/entities.md)) and it provides some useful **extension methods** to configure the properties inherited from the base entity classes.

#### ConfigureByConvention Method

`ConfigureByConvention()` is the main extension method that **configures all the base properties** and conventions for your entities. So, it is a **best practice** to call this method for all your entities, in your fluent mapping code.

**Example**: Assume that you've a `Book` entity derived from `AggregateRoot<Guid>` base class:

````csharp
public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
}
````

You can override the `OnModelCreating` method in your `DbContext` and configure the mapping as shown below:

````csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    //Always call the base method
    base.OnModelCreating(builder);

    builder.Entity<Book>(b =>
    {
        b.ToTable("Books");

        //Configure the base properties
        b.ConfigureByConvention();

        //Configure other properties (if you are using the fluent API)
        b.Property(x => x.Name).IsRequired().HasMaxLength(128);
    });
}
````

* Calling `b.ConfigureByConvention()` is important here to properly **configure the base properties**.
* You can configure the `Name` property here or you can use the **data annotation attributes** (see the [EF Core document](https://docs.microsoft.com/en-us/ef/core/modeling/entity-properties)).

> While there are many extension methods to configure your base properties, `ConfigureByConvention()` internally calls them if necessary. So, it is enough to call it.

### Configure the Connection String Selection

If you have multiple databases in your application, you can configure the connection string name for your `DbContext` using the `[ConnectionStringName]` attribute. Example:

```csharp
[ConnectionStringName("MySecondConnString")]
public class MyDbContext : AbpDbContext<MyDbContext>
{

}
```

If you don't configure, the `Default` connection string is used. If you configure a specific connection string name, but not define this connection string name in the application configuration then it fallbacks to the `Default` connection string (see [the connection strings document](../../fundamentals/connection-strings.md) for more information).

### AbpDbContextOptions

`AbpDbContextOptions` is used to configure the `DbContext` options. When you create a new solution with the ABP's application startup template, you will see a simple configuration (in the `EntityFrameworkCore` integration project's module class) as shown below:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer();
});
````

That configuration configures the default DBMS as SQL Server for all the `DbContext`s of the application. That configuration was a shorthand notation and it can be done with the following code block:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(opts =>
    {
        opts.UseSqlServer();
    });
});
````

`options.Configure(...)` method has more options to configure. For example, you can set `DbContextOptions` (EF Core's native options) as shown below:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(opts =>
    {
        opts.DbContextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
});
````

Add actions for the `ConfigureConventions` and `OnModelCreating` methods of the `DbContext` as shown below:

````csharp
options.DefaultConventionAction = (dbContext, builder) =>
{
    // This action is called for ConfigureConventions method of all DbContexts.
};

options.ConfigureConventions<YourDbContext>((dbContext, builder) =>
{
    // This action is called for ConfigureConventions method of specific DbContext.
});

options.DefaultOnModelCreatingAction = (dbContext, builder) =>
{
    // This action is called for OnModelCreating method of all DbContexts.
};

options.ConfigureOnModelCreating<YourDbContext>((dbContext, builder) =>
{
    // This action is called for OnModelCreating method of specific DbContext.
});
````

If you have a single `DbContext` or you have multiple `DbContext`s but want to use the same DBMS and configuration for all, you can leave it as is. However, if you need to configure a different DBMS or customize the configuration for a specific `DbContext`, you can specify it as shown below:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    // Default configuration for all DbContexts
    options.Configure(opts =>
    {
        opts.UseSqlServer();
    });

    // Customized configuration for a specific DbContext
    options.Configure<MyOtherDbContext>(opts =>
    {
        opts.UseMySQL();
    });
});
````

> See [Switch to Another DBMS for Entity Framework Core](./other-dbms.md) document to learn how to configure the DBMS.

## Registering DbContext To Dependency Injection

Use `AddAbpDbContext` method in your module to register your `DbContext` class for [dependency injection](../../fundamentals/dependency-injection.md) system.

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyDbContext>();

            //...
        }
    }
}
````

### Add Default Repositories

ABP can automatically create default [generic repositories](../../architecture/domain-driven-design/repositories.md) for the entities in your DbContext. Just use `AddDefaultRepositories()` option on the registration:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

This will create a repository for each [aggregate root entity](../../architecture/domain-driven-design/entities.md) (classes derived from `AggregateRoot`) by default. If you want to create repositories for other entities too, then set `includeAllEntities` to `true`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
````

Then you can inject and use `IRepository<TEntity, TPrimaryKey>` in your services. Assume that you have a `Book` entity with `Guid` primary key:

```csharp
public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }
}
```

(`BookType` is a simple `enum` here and not important) And you want to create a new `Book` entity in a [domain service](../../architecture/domain-driven-design/domain-services.md):

````csharp
public class BookManager : DomainService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    //inject default repository to the constructor
    public BookManager(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Book> CreateBook(string name, BookType type)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var book = new Book
        {
            Id = GuidGenerator.Create(),
            Name = name,
            Type = type
        };

        //Use a standard repository method
        await _bookRepository.InsertAsync(book);

        return book;
    }
}
````

This sample uses `InsertAsync` method to insert a new entity to the database.

### Add Custom Repositories

Default generic repositories are powerful enough in most cases (since they implement `IQueryable`). However, you may need to create a custom repository to add your own repository methods. Assume that you want to delete all books by type.

It's suggested to define an interface for your custom repository:

````csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(BookType type);
}
````

You generally want to derive from the `IRepository` to inherit standard repository methods (while, you don't have to do). Repository interfaces are defined in the domain layer of a layered application. They are implemented in the data/infrastructure layer (`EntityFrameworkCore` project in a [startup template](https://abp.io/Templates)).

Example implementation of the `IBookRepository` interface:

````csharp
public class BookRepository
    : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(BookType type)
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync(
            $"DELETE FROM Books WHERE Type = {(int)type}"
        );
    }
}
````

Now, it's possible to [inject](../../fundamentals/dependency-injection.md) the `IBookRepository` and use the `DeleteBooksByType` method when needed.

#### Override the Default Generic Repository

Even if you create a custom repository, you can still inject the default generic repository (`IRepository<Book, Guid>` for this example). Default repository implementation will not use the class you have created.

If you want to replace default repository implementation with your custom repository, do it inside the `AddAbpDbContext` options:

````csharp
context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
{
    options.AddDefaultRepositories();

    //Replaces IRepository<Book, Guid>
    options.AddRepository<Book, BookRepository>();
});
````

This is especially important when you want to **override a base repository method** to customize it. For instance, you may want to override `DeleteAsync` method to delete a specific entity in a more efficient way:

````csharp
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
````

## Loading Related Entities

Assume that you've an `Order` with a collection of `OrderLine`s and the `OrderLine` has a navigation property to the `Order`:

````csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace MyCrm
{
    public class Order : AggregateRoot<Guid>, IHasCreationTime
    {
        public Guid CustomerId { get; set; }
        public DateTime CreationTime { get; set; }

        public ICollection<OrderLine> Lines { get; set; } //Sub collection

        public Order()
        {
            Lines = new Collection<OrderLine>();
        }
    }

    public class OrderLine : Entity<Guid>
    {
        public Order Order { get; set; } //Navigation property
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
    }
}

````

And defined the database mapping as shown below:

````csharp
builder.Entity<Order>(b =>
{
    b.ToTable("Orders");
    b.ConfigureByConvention();

    //Define the relation
    b.HasMany(x => x.Lines)
        .WithOne(x => x.Order)
        .HasForeignKey(x => x.OrderId)
        .IsRequired();
});

builder.Entity<OrderLine>(b =>
{
    b.ToTable("OrderLines");
    b.ConfigureByConvention();
});
````

When you query an `Order`, you may want to **include** all the `OrderLine`s in a single query or you may want to **load them later** on demand.

> Actually these are not directly related to the ABP. You can follow the [EF Core documentation](https://docs.microsoft.com/en-us/ef/core/querying/related-data/) to learn all the details. This section will cover some topics related to the ABP.

### Eager Loading / Load With Details

You have different options when you want to load the related entities while querying an entity.

#### Repository.WithDetails

`IRepository.WithDetailsAsync(...)` can be used to get an `IQueryable<T>` by including one relation collection/property.

**Example: Get an order with lines**

````csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AbpDemo.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderManager(IRepository<Order, Guid> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task TestWithDetails(Guid id)
        {
            //Get a IQueryable<T> by including sub collections
            var queryable = await _orderRepository.WithDetailsAsync(x => x.Lines);

            //Apply additional LINQ extension methods
            var query = queryable.Where(x => x.Id == id);

            //Execute the query and get the result
            var order = await AsyncExecuter.FirstOrDefaultAsync(query);
        }
    }
}
````

> `AsyncExecuter` is used to execute async LINQ extensions without depending on the EF Core. If you add EF Core NuGet package reference to your project, then you can directly use `await query.FirstOrDefaultAsync()`. But, this time you depend on the EF Core in your domain layer. See the [repository document](../../architecture/domain-driven-design/repositories.md) to learn more.

**Example: Get a list of orders with their lines**

````csharp
public async Task TestWithDetails()
{
    //Get a IQueryable<T> by including sub collections
    var queryable = await _orderRepository.WithDetailsAsync(x => x.Lines);

    //Execute the query and get the result
    var orders = await AsyncExecuter.ToListAsync(queryable);
}
````

> `WithDetailsAsync` method can get more than one expression parameter if you need to include more than one navigation property or collection.

#### DefaultWithDetailsFunc

If you don't pass any expression to the `WithDetailsAsync` method, then it includes all the details using the `DefaultWithDetailsFunc` option you provide.

You can configure `DefaultWithDetailsFunc` for an entity in the `ConfigureServices` method of your [module](../../architecture/modularity/basics.md) in your `EntityFrameworkCore` project.

**Example: Include `Lines` while querying an `Order`**

````csharp
Configure<AbpEntityOptions>(options =>
{
    options.Entity<Order>(orderOptions =>
    {
        orderOptions.DefaultWithDetailsFunc = query => query.Include(o => o.Lines);
    });
});
````

> You can fully use the EF Core API here since this is located in the EF Core integration project.

Then you can use the `WithDetails` without any parameter:

````csharp
public async Task TestWithDetails()
{
    //Get a IQueryable<T> by including all sub collections
    var queryable = await _orderRepository.WithDetailsAsync();

    //Execute the query and get the result
    var orders = await AsyncExecuter.ToListAsync(queryable);
}
````

`WithDetailsAsync()` executes the expression you've setup as the `DefaultWithDetailsFunc`.

#### Repository Get/Find Methods

Some of the standard [Repository](../../architecture/domain-driven-design/repositories.md) methods have optional `includeDetails` parameters;

* `GetAsync` and `FindAsync` gets `includeDetails` with default value is `true`.
* `GetListAsync` and `GetPagedListAsync` gets `includeDetails` with default value is `false`.

That means, the methods return a **single entity includes details** by default while list returning methods don't include details by default. You can explicitly pass `includeDetails` to change the behavior.

> These methods use the `DefaultWithDetailsFunc` option that is explained above.

**Example: Get an order with details**

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id);
}
````

**Example: Get an order without details**

````csharp
public async Task TestWithoutDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id, includeDetails: false);
}
````

**Example: Get list of entities with details**

````csharp
public async Task TestWithDetails()
{
    var orders = await _orderRepository.GetListAsync(includeDetails: true);
}
````

#### Alternatives

The repository pattern tries to encapsulate the EF Core, so your options are limited. If you need an advanced scenario, you can follow one of the options;

* Create a custom repository method and use the complete EF Core API.
* Reference to the `Volo.Abp.EntityFrameworkCore` package from your project. In this way, you can directly use `Include` and `ThenInclude` in your code.

See also [eager loading document](https://docs.microsoft.com/en-us/ef/core/querying/related-data/eager) of the EF Core.

### Explicit / Lazy Loading

If you don't include relations while querying an entity and later need to access to a navigation property or collection, you have different options.

#### EnsurePropertyLoadedAsync / EnsureCollectionLoadedAsync

Repositories provide `EnsurePropertyLoadedAsync` and `EnsureCollectionLoadedAsync` extension methods to **explicitly load** a navigation property or sub collection.

**Example: Load Lines of an Order when needed**

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id, includeDetails: false);
    //order.Lines is empty on this stage

    await _orderRepository.EnsureCollectionLoadedAsync(order, x => x.Lines);
    //order.Lines is filled now
}
````

`EnsurePropertyLoadedAsync` and `EnsureCollectionLoadedAsync` methods do nothing if the property or collection was already loaded. So, calling multiple times has no problem.

See also [explicit loading document](https://docs.microsoft.com/en-us/ef/core/querying/related-data/explicit) of the EF Core.

#### Lazy Loading with Proxies

Explicit loading may not be possible in some cases, especially when you don't have a reference to the `Repository` or `DbContext`. Lazy Loading is a feature of the EF Core that loads the related properties / collections when you first access to it.

To enable lazy loading;

1. Install the [Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/) package into your project (typically to the EF Core integration project)
2. Configure `UseLazyLoadingProxies` for your `DbContext` (in the `ConfigureServices` method of your module in your EF Core project). Example:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.PreConfigure<MyCrmDbContext>(opts =>
    {
        opts.DbContextOptions.UseLazyLoadingProxies(); //Enable lazy loading
    });

    options.UseSqlServer();
});
````

3. Make your navigation properties and collections `virtual`. Examples:

````csharp
public virtual ICollection<OrderLine> Lines { get; set; } //virtual collection
public virtual Order Order { get; set; } //virtual navigation property
````

Once you enable lazy loading and arrange your entities, you can freely access to the navigation properties and collections:

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id);
    //order.Lines is empty on this stage

    var lines = order.Lines;
    //order.Lines is filled (lazy loaded)
}
````

Whenever you access to a property/collection, EF Core automatically performs an additional query to load the property/collection from the database.

> Lazy loading should be carefully used since it may cause performance problems in some specific cases.

See also [lazy loading document](https://docs.microsoft.com/en-us/ef/core/querying/related-data/lazy) of the EF Core.

## Read-Only Repositories

ABP provides read-only [repository](../../architecture/domain-driven-design/repositories.md) interfaces (`IReadOnlyRepository<...>` or `IReadOnlyBasicRepository<...>`) to explicitly indicate that your purpose is to query data, but not change it. If so, you can inject these interfaces into your services.

Entity Framework Core read-only repository implementation uses [EF Core's No-Tracking feature](https://learn.microsoft.com/en-us/ef/core/querying/tracking#no-tracking-queries). That means the entities returned from the repository will not be tracked by the EF Core [change tracker](https://learn.microsoft.com/en-us/ef/core/change-tracking/), because it is expected that you won't update entities queried from a read-only repository. If you need to track the entities, you can still use the [AsTracking()](https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.entityframeworkqueryableextensions.astracking) extension method on the LINQ expression, or `EnableTracking()` extension method on the repository object (See *Enabling / Disabling the Change Tracking* section in this document).

> This behavior works only if the repository object is injected with one of the read-only repository interfaces (`IReadOnlyRepository<...>` or `IReadOnlyBasicRepository<...>`). It won't work if you have injected a standard repository (e.g. `IRepository<...>`) then casted it to a read-only repository interface.

## Enabling / Disabling the Change Tracking

In addition to the read-only repositories, ABP allows to manually control the change tracking behavior for querying objects. Please see the *Enabling / Disabling the Change Tracking* section of the [Repositories documentation](../../architecture/domain-driven-design/repositories.md) to learn how to use it.

## Access to the EF Core API

In most cases, you want to hide EF Core APIs behind a repository (this is the main purpose of the repository pattern). However, if you want to access the `DbContext` instance over the repository, you can use `GetDbContext()` or `GetDbSet()` extension methods. Example:

````csharp
public async Task TestAsync()
{
    var dbContext = await _orderRepository.GetDbContextAsync();
    var dbSet = await _orderRepository.GetDbSetAsync();
    //var dbSet = dbContext.Set<Order>(); //Alternative, when you have the DbContext
}
````

* `GetDbContextAsync` returns a `DbContext` reference instead of `BookStoreDbContext`. You can cast it if you need. However, you don't need it in most cases.

> Important: You must reference to the `Volo.Abp.EntityFrameworkCore` package from the project you want to access to the `DbContext`. This breaks encapsulation, but this is what you want in that case.

## Extra Properties & Object Extension Manager

Extra Properties system allows you to set/get dynamic properties to entities those implement the `IHasExtraProperties` interface. It is especially useful when you want to add custom properties to the entities defined in an [application module](../../../modules), when you use the module as package reference.

By default, all the extra properties of an entity are stored as a single `JSON` object in the database.

Entity extension system allows you to to store desired extra properties in separate fields in the related database table. For more information about the extra properties & the entity extension system, see the following documents:

* [Customizing the Application Modules: Extending Entities](../../architecture/modularity/extending/customizing-application-modules-extending-entities.md)
* [Entities](../../architecture/domain-driven-design/entities.md)

This section only explains the EF Core related usage of the `ObjectExtensionManager`.

### ObjectExtensionManager.Instance

`ObjectExtensionManager` implements the singleton pattern, so you need to use the static `ObjectExtensionManager.Instance` to perform all the operations.

### MapEfCoreProperty

`MapEfCoreProperty` is a shortcut extension method to define an extension property for an entity and map to the database.

**Example**: Add `Title` property (database field) to the `IdentityRole` entity:

````csharp
ObjectExtensionManager.Instance
    .MapEfCoreProperty<IdentityRole, string>(
        "Title",
        (entityBuilder, propertyBuilder) =>
        {
            propertyBuilder.HasMaxLength(64);
        }
    );
````

### MapEfCoreEntity

`MapEfCoreEntity` is a shortcut extension method to configure the `Entity`.

**Example**: Set the max length of `Name` to the `IdentityRole` entity:

````csharp
ObjectExtensionManager.Instance
    .MapEfCoreEntity<IdentityRole>(builder =>
    {
        builder.As<EntityTypeBuilder<IdentityRole>>().Property(x => x.Name).HasMaxLength(200);
    });
````

### MapEfCoreDbContext

`MapEfCoreDbContext` is a shortcut extension method to configure the `DbContext`.

**Example**: Set the max length of `Name` to the `IdentityRole` entity of `IdentityDbContext`:

````csharp
ObjectExtensionManager.Instance.MapEfCoreDbContext<IdentityDbContext>(b =>
{
    b.Entity<IdentityRole>().Property(x => x.Name).HasMaxLength(200);
});
````

If the related module has implemented this feature(explained below), then the new property is added to the model or the DbContext/Entity configure changed. Then you need to run the standard `Add-Migration` and `Update-Database` commands to update your database to add the new field.

> The `MapEfCoreProperty`, `MapEfCoreEntity` and `MapEfCoreDbContext` methods must be called before using the related `DbContext`. It is a static method. The best way is to use it in your application as earlier as possible. The application startup template has a `YourProjectNameEfCoreEntityExtensionMappings` class that is safe to use this method inside.

### ConfigureEfCoreEntity, ApplyObjectExtensionMappings and TryConfigureObjectExtensions

If you are building a reusable module and want to allow application developers to add properties to your entities, you can use the `ConfigureEfCoreEntity`, `ApplyObjectExtensionMappings` and `TryConfigureObjectExtensions` extension methods in your entity mapping.

**Example**:
````csharp
public static class QADbContextModelCreatingExtensions
{
    public static void ConfigureQA(
        this ModelBuilder builder,
        Action<QAModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new QAModelBuilderConfigurationOptions(
            QADatabaseDbProperties.DbTablePrefix,
            QADatabaseDbProperties.DbSchema
        );

        optionsAction?.Invoke(options);

        builder.Entity<QA_Question>(b =>
        {
            b.ToTable(options.TablePrefix + "Questions", options.Schema);
            b.ConfigureByConvention();
            //...

            //Call this in the end of buildAction.
            b.ApplyObjectExtensionMappings();
        });

        //...

        //Call this in the end of ConfigureQA.
        builder.TryConfigureObjectExtensions<QADbContext>();
    }
}
````

> If you call `ConfigureByConvention()` extension method (like `b.ConfigureByConvention()` for this example), ABP internally calls the `ConfigureObjectExtensions` and `ConfigureEfCoreEntity` methods. It is a **best practice** to use the `ConfigureByConvention()` method since it also configures database mapping for base properties by convention.

> The `Object Extension` feature need the `Change Tracking`, which means you can't use the read-only repositories for the entities that have `extension properties(MapEfCoreProperty)`, Please see the [Repositories documentation](../../architecture/domain-driven-design/repositories.md) to learn the change tracking behavior.

See the "*ConfigureByConvention Method*" section above for more information.

## Advanced Topics

### Controlling the Multi-Tenancy

If your solution is [multi-tenant](../../architecture/multi-tenancy), tenants may have **separate databases**, you have **multiple** `DbContext` classes in your solution and some of your `DbContext` classes should be usable **only from the host side**, it is suggested to add `[IgnoreMultiTenancy]` attribute on your `DbContext` class. In this case, ABP guarantees that the related `DbContext` always uses the host [connection string](../../fundamentals/connection-strings.md), even if you are in a tenant context.

**Example:**

````csharp
[IgnoreMultiTenancy]
public class MyDbContext : AbpDbContext<MyDbContext>
{
    ...
}
````

Do not use the `[IgnoreMultiTenancy]` attribute if any one of your entities in your `DbContext` can be persisted in a tenant database.

> When you use repositories, ABP already uses the host database for the entities don't implement the `IMultiTenant` interface. So, most of time you don't need to `[IgnoreMultiTenancy]` attribute if you are using the repositories to work with the database.

### Set Default Repository Classes

Default generic repositories are implemented by `EfCoreRepository` class by default. You can create your own implementation and use it for all the default repository implementations.

First, define your default repository classes like that:

```csharp
public class MyRepositoryBase<TEntity>
    : EfCoreRepository<BookStoreDbContext, TEntity>
      where TEntity : class, IEntity
{
    public MyRepositoryBase(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}

public class MyRepositoryBase<TEntity, TKey>
    : EfCoreRepository<BookStoreDbContext, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public MyRepositoryBase(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
```

First one is for [entities with composite keys](../../architecture/domain-driven-design/entities.md), second one is for entities with single primary key.

It's suggested to inherit from the `EfCoreRepository` class and override methods if needed. Otherwise, you will have to implement all the standard repository methods manually.

Now, you can use `SetDefaultRepositoryClasses` option:

```csharp
context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
{
    options.SetDefaultRepositoryClasses(
        typeof(MyRepositoryBase<,>),
        typeof(MyRepositoryBase<>)
    );

    //...
});
```

### Set Base DbContext Class or Interface for Default Repositories

If your DbContext inherits from another DbContext or implements an interface, you can use that base class or interface as DbContext for default repositories. Example:

````csharp
public interface IBookStoreDbContext : IEfCoreDbContext
{
    DbSet<Book> Books { get; }
}
````

`IBookStoreDbContext` is implemented by the `BookStoreDbContext` class. Then you can use generic overload of the `AddDefaultRepositories`:

````csharp
context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreDbContext>();
    //...
});
````

Now, your custom `BookRepository` can also use the `IBookStoreDbContext` interface:

````csharp
public class BookRepository : EfCoreRepository<IBookStoreDbContext, Book, Guid>, IBookRepository
{
    //...
}
````

One advantage of using an interface for a DbContext is then it will be replaceable by another implementation.

### Replace Other DbContextes

Once you properly define and use an interface for DbContext, then any other implementation can use the following ways to replace it:

#### ReplaceDbContext Attribute

```csharp
[ReplaceDbContext(typeof(IBookStoreDbContext))]
public class OtherDbContext : AbpDbContext<OtherDbContext>, IBookStoreDbContext
{
    //...
}
```

#### ReplaceDbContext Option

````csharp
context.Services.AddAbpDbContext<OtherDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreDbContext>();
});
````

In this example, `OtherDbContext` implements `IBookStoreDbContext`. This feature allows you to have multiple DbContext (one per module) on development, but single DbContext (implements all interfaces of all DbContexts) on runtime.

#### Replacing with Multi-Tenancy

It is also possible to replace a DbContext based on the [multi-tenancy](../../architecture/multi-tenancy) side. `ReplaceDbContext` attribute and  `ReplaceDbContext` method can get a `MultiTenancySides` option with a default value of `MultiTenancySides.Both`.

**Example:** Replace DbContext only for tenants, using the `ReplaceDbContext` attribute

````csharp
[ReplaceDbContext(typeof(IBookStoreDbContext), MultiTenancySides.Tenant)]
````

**Example:** Replace DbContext only for the host side, using the `ReplaceDbContext` method

````csharp
options.ReplaceDbContext<IBookStoreDbContext>(MultiTenancySides.Host);
````

### Split Queries

ABP enables [split queries](https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries) globally by default for better performance. You can change it as needed.

**Example**

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer(optionsBuilder =>
    {
        optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
});
````

### Customize Bulk Operations

If you have better logic or using an external library for bulk operations, you can override the logic via implementing `IEfCoreBulkOperationProvider`.

- You may use example template below:

```csharp
public class MyCustomEfCoreBulkOperationProvider
    : IEfCoreBulkOperationProvider, ITransientDependency
{
    public async Task DeleteManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task InsertManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task UpdateManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }
}
```

## See Also

* [Entities](../../architecture/domain-driven-design/entities.md)
* [Repositories](../../architecture/domain-driven-design/repositories.md)
* [Video tutorial](https://abp.io/video-courses/essentials/abp-ef-core)