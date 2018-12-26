## MongoDB Integration

This document explains how to integrate MongoDB as a database provider to ABP based applications and how to configure it.

### Installation

`Volo.Abp.MongoDB` is the main nuget package for the MongoDB integration. Install it to your project (for a layered application, to your data/infrastructure layer):

```
Install-Package Volo.Abp.MongoDB
```

Then add `AbpMongoDbModule` module dependency to your [module](Module-Development-Basics.md):

```c#
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

### Creating a Mongo Db Context

ABP introduces **Mongo Db Context** concept (which is similar to Entity Framework Core's DbContext) to make it easier to use collections and configure them. An example is shown below:

```c#
public class MyDbContext : AbpMongoDbContext
{
    public IMongoCollection<Question> Questions => Collection<Question>();

    public IMongoCollection<Category> Categories => Collection<Category>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(b =>
        {
            b.CollectionName = "Questions";
        });
    }
}
```

* It's derived from `AbpMongoDbContext` class.
* Adds a public `IMongoCollection<TEntity>` property for each mongo collection. ABP uses these properties to create default repositories by default.
* Overriding `CreateModel` method allows to configure collections (like their collection name in the database).

### Registering DbContext To Dependency Injection

Use `AddAbpDbContext` method in your module to register your DbContext class for [dependency injection](Dependency-Injection.md) system.

```c#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<MyDbContext>();

            //...
        }
    }
}
```

#### Add Default Repositories

ABP can automatically create default [generic repositories](Repositories.md) for the entities in your DbContext. Just use `AddDefaultRepositories()` option on the registration:

````C#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

This will create a repository for each [aggregate root entity](Entities.md) (classes derived from `AggregateRoot`) by default. If you want to create repositories for other entities too, then set `includeAllEntities` to `true`:

```c#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
```

Then you can inject and use `IRepository<TEntity, TPrimaryKey>` in your services. Assume that you have a `Book` entity with `Guid` primary key:

```csharp
public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }
}
```

(`BookType` is a simple enum here) And you want to create a new `Book` entity in a [domain service](Domain-Services.md):

```csharp
public class BookManager : DomainService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookManager(IRepository<Book, Guid> bookRepository) //inject default repository
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

        await _bookRepository.InsertAsync(book); //Use a standard repository method

        return book;
    }
}
```

This sample uses `InsertAsync` method to insert a new entity to the database.

#### Add Custom Repositories

Default generic repositories are powerful enough in most cases (since they implement `IQueryable`). However, you may need to create a custom repository to add your own repository methods.

Assume that you want to delete all books by type. It's suggested to define an interface for your custom repository:

```csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(
        BookType type,
        CancellationToken cancellationToken = default(CancellationToken)
    );
}
```

You generally want to derive from the `IRepository` to inherit standard repository methods. However, you don't have to. Repository interfaces are defined in the domain layer of a layered application. They are implemented in the data/infrastructure layer (`MongoDB` project in a [startup template](https://abp.io/Templates)).

Example implementation of the `IBookRepository` interface:

```csharp
public class BookRepository : 
    MongoDbRepository<BookStoreMongoDbContext, Book, Guid>,
    IBookRepository
{
    public BookRepository(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(
        BookType type,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        await Collection.DeleteManyAsync(
            Builders<Book>.Filter.Eq(b => b.Type, type),
            cancellationToken
        );
    }
}
```

Now, it's possible to [inject](Dependency-Injection.md) the `IBookRepository` and use the `DeleteBooksByType` method when needed.

##### Override Default Generic Repository

Even if you create a custom repository, you can still inject the default generic repository (`IRepository<Book, Guid>` for this example). Default repository implementation will not use the class you have created.

If you want to replace default repository implementation with your custom repository, do it inside `AddMongoDbContext` options:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories();
    options.AddRepository<Book, BookRepository>(); //Replaces IRepository<Book, Guid>
});
```

This is especially important when you want to **override a base repository method** to customize it. For instance, you may want to override `DeleteAsync` method to delete an entity in a more efficient way:

```csharp
public override async Task DeleteAsync(
    Guid id, 
    bool autoSave = false, 
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
```

#### Access to the MongoDB API

In most cases, you want to hide MongoDB APIs behind a repository (this is the main purpose of the repository). However, if you want to access the MongoDB API over the repository, you can use `GetDatabase()` or `GetCollection()` extension methods. Example:

```csharp
public class BookService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public void Foo()
    {
        IMongoDatabase database = _bookRepository.GetDatabase();
        IMongoCollection<Book> books = _bookRepository.GetCollection();
    }
}
```

> Important: You must reference to the `Volo.Abp.MongoDB` package from the project you want to access to the MongoDB API. This breaks encapsulation, but this is what you want in that case.

#### Advanced Topics

##### Set Default Repository Classes

Default generic repositories are implemented by `MongoDbRepository` class by default. You can create your own implementation and use it for default repository implementation.

First, define your repository classes like that:

```csharp
public class MyRepositoryBase<TEntity>
    : MongoDbRepository<BookStoreMongoDbContext, TEntity>
    where TEntity : class, IEntity
{
    public MyRepositoryBase(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}

public class MyRepositoryBase<TEntity, TKey>
    : MongoDbRepository<BookStoreMongoDbContext, TEntity, TKey>
    where TEntity : class, IEntity<TKey>
{
    public MyRepositoryBase(IMongoDbContextProvider<BookStoreMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
```

First one is for [entities with composite keys](Entities.md), second one is for entities with single primary key.

It's suggested to inherit from the `MongoDbRepository` class and override methods if needed. Otherwise, you will have to implement all standard repository methods manually.

Now, you can use `SetDefaultRepositoryClasses` option:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.SetDefaultRepositoryClasses(
        typeof(MyRepositoryBase<,>),
        typeof(MyRepositoryBase<>)
    );
    //...
});
```

##### Set Base MongoDbContext Class or Interface for Default Repositories

If your MongoDbContext inherits from another MongoDbContext or implements an interface, you can use that base class or interface as the MongoDbContext for default repositories. Example:

```csharp
public interface IBookStoreMongoDbContext : IAbpMongoDbContext
{
    Collection<Book> Books { get; }
}
```

`IBookStoreMongoDbContext` is implemented by the `BookStoreMongoDbContext` class. Then you can use generic overload of the `AddDefaultRepositories`:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreMongoDbContext>();
    //...
});
```

Now, your custom `BookRepository` can also use the `IBookStoreMongoDbContext` interface:

```csharp
public class BookRepository
    : MongoDbRepository<IBookStoreMongoDbContext, Book, Guid>,
      IBookRepository
{
    //...
}
```

One advantage of using interface for a MongoDbContext is then it becomes replaceable by another implementation.

##### Replace Other DbContextes

Once you properly define and use an interface for a MongoDbContext , then any other implementation can replace it using the `ReplaceDbContext` option:

```csharp
context.Services.AddMongoDbContext<OtherMongoDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreMongoDbContext>();
});
```

In this example, `OtherMongoDbContext` implements `IBookStoreMongoDbContext`. This feature allows you to have multiple MongoDbContext (one per module) on development, but single MongoDbContext (implements all interfaces of all MongoDbContexts) on runtime.