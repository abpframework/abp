# Data Filtering

[Volo.Abp.Data](https://www.nuget.org/packages/Volo.Abp.Data) package defines services to automatically filter data on querying from a database.

## Pre-Defined Filters

ABP defines some filters out of the box.

### ISoftDelete

Used to mark an [entity](Entities.md) as deleted instead of actually deleting it. Implement the `ISoftDelete` interface to make your entity "soft delete".

Example:

````csharp
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore
{
    public class Book : AggregateRoot<Guid>, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; } //Defined by ISoftDelete
    }
}
````

`ISoftDelete` defines the `IsDeleted` property. When you delete a book using [repositories](Repositories.md), ABP automatically sets `IsDeleted` to true and protects it from actual deletion (you can also manually set the `IsDeleted` property to true if you need). In addition, it **automatically filters deleted entities** when you query the database.

> `ISoftDelete` filter is enabled by default and you can not get deleted entities from database unless you explicitly disable it. See the `IDataFilter` service below.

### IMultiTenant

[Multi-tenancy](Multi-Tenancy.md) is an efficient way of creating SaaS applications. Once you create a multi-tenant application, you typically want to isolate data between tenants. Implement `IMultiTenant` interface to make your entity "multi-tenant aware".

Example:

````csharp
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore
{
    public class Book : AggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; } //Defined by ISoftDelete

        public Guid? TenantId { get; set; } //Defined by IMultiTenant
    }
}
````

`IMultiTenant` interface defines the `TenantId` property which is then used to automatically filter the entities for the current tenant. See the [Multi-tenancy](Multi-Tenancy.md) document for more.

## IDataFilter Service: Enable/Disable Data Filters

You can control the filters using `IDataFilter` service.

Example:

````csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class MyBookService : ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Book, Guid> _bookRepository;

        public MyBookService(
            IDataFilter dataFilter,
            IRepository<Book, Guid> bookRepository)
        {
            _dataFilter = dataFilter;
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
        {
            //Temporary disable the ISoftDelete filter
            using (_dataFilter.Disable<ISoftDelete>())
            {
                return await _bookRepository.GetListAsync();
            }
        }
    }
}
````

* [Inject](Dependency-Injection.md) the `IDataFilter` service to your class.
* Use the `Disable` method within a `using` statement to create a code block where the `ISoftDelete` filter is disabled inside it (Always use it inside a `using` block to guarantee that the filter is reset to its previous state).

`IDataFilter.Enable` method can be used to enable a filter. `Enable` and `Disable` methods can be used in a nested way to define inner scopes.

## AbpDataFilterOptions

`AbpDataFilterOptions` can be used to [set options](Options.md) for the data filter system.

The example code below disables the `ISoftDelete` filter by default which will cause to include deleted entities when you query the database unless you explicitly enable the filter:

````csharp
Configure<AbpDataFilterOptions>(options =>
{
    options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: false);
});
````

> Carefully change defaults for global filters, especially if you are using a pre-built module which might be developed assuming the soft delete filter is turned on by default. But you can do it for your own defined filters safely.

## Defining Custom Filters

Defining and implementing a new filter highly depends on the database provider. ABP implements all pre-defined filters for all database providers.

When you need it, start by defining an interface (like `ISoftDelete` and `IMultiTenant`) for your filter and implement it for your entities.

Example:

````csharp
public interface IIsActive
{
    bool IsActive { get; }
}
````

Such an `IIsActive` interface can be used to filter active/passive data and can be easily implemented by any [entity](Entities.md):

````csharp
public class Book : AggregateRoot<Guid>, IIsActive
{
    public string Name { get; set; }

    public bool IsActive { get; set; } //Defined by IIsActive
}
````

### EntityFramework Core

ABP uses [EF Core's Global Query Filters](https://docs.microsoft.com/en-us/ef/core/querying/filters) system for the [EF Core Integration](Entity-Framework-Core.md). So, it is well integrated to EF Core and works as expected even if you directly work with `DbContext`.

Best way to implement a custom filter is to override `CreateFilterExpression` method for your `DbContext`. Example:

````csharp
protected bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
{
    var expression = base.CreateFilterExpression<TEntity>();

    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    {
        Expression<Func<TEntity, bool>> isActiveFilter =
            e => !IsActiveFilterEnabled || EF.Property<bool>(e, "IsActive");
        expression = expression == null 
            ? isActiveFilter 
            : CombineExpressions(expression, isActiveFilter);
    }

    return expression;
}
````

* Added a `IsActiveFilterEnabled` property to check if `IIsActive` is enabled or not. It internally uses the `IDataFilter` service introduced before.
* Overrided the `CreateFilterExpression` method, checked if given entity implements the `IIsActive` interface and combines the expressions if necessary.

### MongoDB

ABP implements data filters directly in the [repository](Repositories.md) level for the [MongoDB Integration](MongoDB.md). So, it works only if you use the repositories properly. Otherwise, you should manually filter the data.

Currently, the best way to implement a data filter for the MongoDB integration is to override the `AddGlobalFilters` in the repository derived from the `MongoDbRepository` class. Example:

````csharp
public class BookRepository : MongoDbRepository<BookStoreMongoDbContext, Book, Guid>
{
    protected override void AddGlobalFilters(List<FilterDefinition<Book>> filters)
    {
        if (DataFilter.IsEnabled<IIsActive>())
        {
            filters.Add(Builders<Book>.Filter.Eq(e => ((IIsActive)e).IsActive, true));
        }
    }
}
````

This example implements it only for the `Book` entity. If you want to implement for all entities (those implement the `IIsActive` interface), create your own custom MongoDB repository base class and override the `AddGlobalFilters` as shown below:

````csharp
public abstract class MyMongoRepository<TMongoDbContext, TEntity, TKey> : MongoDbRepository<TMongoDbContext, TEntity, TKey>
    where TMongoDbContext : IAbpMongoDbContext
    where TEntity : class, IEntity<TKey>
{
    protected MyMongoRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
           : base(dbContextProvider)
    {

    }

    protected override void AddGlobalFilters(List<FilterDefinition<TEntity>> filters)
    {
        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)) 
            && DataFilter.IsEnabled<IIsActive>())
        {
            filters.Add(Builders<TEntity>.Filter.Eq(e => ((IIsActive)e).IsActive, true));
        }
    }
}
````

> See "Set Default Repository Classes" section of the [MongoDb Integration document](MongoDB.md) to learn how to replace default repository base with your custom class.