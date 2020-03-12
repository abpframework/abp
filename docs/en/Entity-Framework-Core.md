# Entity Framework Core Integration

This document explains how to integrate EF Core as an ORM provider to ABP based applications and how to configure it.

## Installation

`Volo.Abp.EntityFrameworkCore` is the main nuget package for the EF Core integration. Install it to your project (for a layered application, to your data/infrastructure layer):

```` shell
Install-Package Volo.Abp.EntityFrameworkCore
````

Then add `AbpEntityFrameworkCoreModule` module dependency (`DependsOn` attribute) to your [module](Module-Development-Basics.md):

````c#
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> Note: Instead, you can directly download a [startup template](https://abp.io/Templates) with EF Core pre-installed.

### Database Management System Selection

Entity Framework Core supports various database management systems ([see all](https://docs.microsoft.com/en-us/ef/core/providers/)). ABP framework and this document doesn't depend on any specific DBMS.

If you are creating a [reusable application module](Modules/Index.md), avoid to depend on a specific DBMS package. However, in a final application you eventually will select a DBMS.

See [Switch to Another DBMS for Entity Framework Core](Entity-Framework-Core-Other-DBMS.md) document to learn how to switch the DBMS.

## Creating DbContext

You can create your DbContext as you normally do. It should be derived from `AbpDbContext<T>` as shown below:

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

### Configure the Connection String Selection

If you have multiple databases in your application, you can configure the connection string name for your DbContext using the `[ConnectionStringName]` attribute. Example:

```csharp
[ConnectionStringName("MySecondConnString")]
public class MyDbContext : AbpDbContext<MyDbContext>
{
    
}
```

If you don't configure, the `Default` connection string is used. If you configure a specific connection string name, but not define this connection string name in the application configuration then it fallbacks to the `Default` connection string (see [the connection strings document](Connection-Strings.md) for more information).

## Registering DbContext To Dependency Injection

Use `AddAbpDbContext` method in your module to register your DbContext class for [dependency injection](Dependency-Injection.md) system.

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

ABP can automatically create default [generic repositories](Repositories.md) for the entities in your DbContext. Just use `AddDefaultRepositories()` option on the registration:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

This will create a repository for each [aggregate root entity](Entities.md) (classes derived from `AggregateRoot`) by default. If you want to create repositories for other entities too, then set `includeAllEntities` to `true`:

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

(`BookType` is a simple enum here) And you want to create a new `Book` entity in a [domain service](Domain-Services.md):

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

You generally want to derive from the `IRepository` to inherit standard repository methods. However, you don't have to. Repository interfaces are defined in the domain layer of a layered application. They are implemented in the data/infrastructure layer (`EntityFrameworkCore` project in a [startup template](https://abp.io/Templates)).

Example implementation of the `IBookRepository` interface:

````csharp
public class BookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(BookType type)
    {
        await DbContext.Database.ExecuteSqlRawAsync(
            $"DELETE FROM Books WHERE Type = {(int)type}"
        );
    }
}
````

Now, it's possible to [inject](Dependency-Injection.md) the `IBookRepository` and use the `DeleteBooksByType` method when needed.

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
public override async Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
````

### Access to the EF Core API

In most cases, you want to hide EF Core APIs behind a repository (this is the main purpose of the repository pattern). However, if you want to access the `DbContext` instance over the repository, you can use `GetDbContext()` or `GetDbSet()` extension methods. Example:

````csharp
public class BookService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public void Foo()
    {
        DbContext dbContext = _bookRepository.GetDbContext();
        DbSet<Book> books = _bookRepository.GetDbSet();
    }
}
````

* `GetDbContext` returns a `DbContext` reference instead of `BookStoreDbContext`. You can cast it, however in most cases you don't need it.

> Important: You must reference to the `Volo.Abp.EntityFrameworkCore` package from the project you want to access to the DbContext. This breaks encapsulation, but this is what you want in that case.

### Advanced Topics

#### Set Default Repository Classes

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

First one is for [entities with composite keys](Entities.md), second one is for entities with single primary key.

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

#### Set Base DbContext Class or Interface for Default Repositories

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

#### Replace Other DbContextes

Once you properly define and use an interface for DbContext, then any other implementation can replace it using the `ReplaceDbContext` option:

````csharp
context.Services.AddAbpDbContext<OtherDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreDbContext>();
});
````

In this example, `OtherDbContext` implements `IBookStoreDbContext`. This feature allows you to have multiple DbContext (one per module) on development, but single DbContext (implements all interfaces of all DbContexts) on runtime.
