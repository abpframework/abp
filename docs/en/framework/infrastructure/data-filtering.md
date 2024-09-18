# Data Filtering

[Volo.Abp.Data](https://www.nuget.org/packages/Volo.Abp.Data) package defines services to automatically filter data on querying from a database.

## Pre-Defined Filters

ABP defines some filters out of the box.

### ISoftDelete

Used to mark an [entity](../architecture/domain-driven-design/entities.md) as deleted instead of actually deleting it. Implement the `ISoftDelete` interface to make your entity "soft delete".

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

`ISoftDelete` defines the `IsDeleted` property. When you delete a book using [repositories](../architecture/domain-driven-design/repositories.md), ABP automatically sets `IsDeleted` to true and protects it from actual deletion (you can also manually set the `IsDeleted` property to true if you need). In addition, it **automatically filters deleted entities** when you query the database.

> `ISoftDelete` filter is enabled by default and you can not get deleted entities from database unless you explicitly disable it. See the `IDataFilter` service below.

> Soft-delete entities can be hard-deleted when you use `HardDeleteAsync` method on the repositories.

### IMultiTenant

[Multi-tenancy](../architecture/multi-tenancy) is an efficient way of creating SaaS applications. Once you create a multi-tenant application, you typically want to isolate data between tenants. Implement `IMultiTenant` interface to make your entity "multi-tenant aware".

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

`IMultiTenant` interface defines the `TenantId` property which is then used to automatically filter the entities for the current tenant. See the [Multi-tenancy](../architecture/multi-tenancy) document for more.

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

* [Inject](../fundamentals/dependency-injection.md) the `IDataFilter` service to your class.
* Use the `Disable` method within a `using` statement to create a code block where the `ISoftDelete` filter is disabled inside it.

In addition to the `Disable<T>()` method;

* `IDataFilter.Enable<T>()` method can be used to enable a filter. `Enable` and `Disable` methods can be used in a **nested** way to define inner scopes.

* `IDataFilter.IsEnabled<T>()` can be used to check whether a filter is currently enabled or not.

> Always use the `Disable` and `Enable` methods it inside a `using` block to guarantee that the filter is reset to its previous state.

### The Generic IDataFilter Service

`IDataFilter` service has a generic version, `IDataFilter<TFilter>` that injects a more restricted and explicit data filter based on the filter type.

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
        private readonly IDataFilter<ISoftDelete> _softDeleteFilter;
        private readonly IRepository<Book, Guid> _bookRepository;

        public MyBookService(
            IDataFilter<ISoftDelete> softDeleteFilter,
            IRepository<Book, Guid> bookRepository)
        {
            _softDeleteFilter = softDeleteFilter;
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
        {
            //Temporary disable the ISoftDelete filter
            using (_softDeleteFilter.Disable())
            {
                return await _bookRepository.GetListAsync();
            }
        }
    }
}
````

* This usage determines the filter type while injecting the `IDataFilter<T>` service.
* In this case you can use the `Disable()` and `Enable()` methods without specifying the filter type.

## AbpDataFilterOptions

`AbpDataFilterOptions` can be used to [set options](../fundamentals/options.md) for the data filter system.

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

Such an `IIsActive` interface can be used to filter active/passive data and can be easily implemented by any [entity](../architecture/domain-driven-design/entities.md):

````csharp
public class Book : AggregateRoot<Guid>, IIsActive
{
    public string Name { get; set; }

    public bool IsActive { get; set; } //Defined by IIsActive
}
````

### EntityFramework Core

ABP uses [EF Core's Global Query Filters](https://docs.microsoft.com/en-us/ef/core/querying/filters) system for the [EF Core Integration](../data/entity-framework-core). So, it is well integrated to EF Core and works as expected even if you directly work with `DbContext`.

Best way to implement a custom filter is to override  `ShouldFilterEntity` and `CreateFilterExpression` method for your `DbContext`. Example:

````csharp
protected bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
{
    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    {
        return true;
    }

    return base.ShouldFilterEntity<TEntity>(entityType);
}

protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
{
    var expression = base.CreateFilterExpression<TEntity>(modelBuilder);

    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    {
        Expression<Func<TEntity, bool>> isActiveFilter = e => !IsActiveFilterEnabled || EF.Property<bool>(e, "IsActive");
        expression = expression == null ? isActiveFilter : QueryFilterExpressionHelper.CombineExpressions(expression, isActiveFilter);
    }

    return expression;
}
````

* Added a `IsActiveFilterEnabled` property to check if `IIsActive` is enabled or not. It internally uses the `IDataFilter` service introduced before.
* Overrided the `ShouldFilterEntity` and `CreateFilterExpression` methods, checked if given entity implements the `IIsActive` interface and combines the expressions if necessary.

In addition you can also use `HasAbpQueryFilter` to set a filter for an entity. It will combine your filter with ABP EF Core builtin global query filters.

````csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    modelBuilder.Entity<MyEntity>(b =>
    {
        b.HasAbpQueryFilter(e => e.Name.StartsWith("abp"));
    });
}
````

#### Using User-defined function mapping for global filters

Using [User-defined function mapping](https://learn.microsoft.com/en-us/ef/core/querying/user-defined-function-mapping) for global filters will gain performance improvements. 

To use this feature, you need to change your DbContext like below:

````csharp
protected bool IsActiveFilterEnabled => DataFilter?.IsEnabled<IIsActive>() ?? false;

protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
{
    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    {
        return true;
    }

    return base.ShouldFilterEntity<TEntity>(entityType);
}

protected override Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>(ModelBuilder modelBuilder)
{
    var expression = base.CreateFilterExpression<TEntity>(modelBuilder);

    if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)))
    {
        Expression<Func<TEntity, bool>> isActiveFilter = e => !IsActiveFilterEnabled || EF.Property<bool>(e, "IsActive");

        if (UseDbFunction())
        {
            isActiveFilter = e => IsActiveFilter(((IIsActive)e).IsActive, true);

            var abpEfCoreCurrentDbContext = this.GetService<AbpEfCoreCurrentDbContext>();
            modelBuilder.HasDbFunction(typeof(MyProjectNameDbContext).GetMethod(nameof(IsActiveFilter))!)
                .HasTranslation(args =>
                {
                    // (bool isActive, bool boolParam)
                    var isActive = args[0];
                    var boolParam = args[1];

                    if (abpEfCoreCurrentDbContext.Context?.DataFilter.IsEnabled<IIsActive>() == true)
                    {
                        // isActive == true
                        return new SqlBinaryExpression(
                            ExpressionType.Equal,
                            isActive,
                            new SqlConstantExpression(Expression.Constant(true), boolParam.TypeMapping),
                            boolParam.Type,
                            boolParam.TypeMapping);
                    }

                    // empty where sql
                    return new SqlConstantExpression(Expression.Constant(true), boolParam.TypeMapping);
                });
        }

        expression = expression == null ? isActiveFilter : QueryFilterExpressionHelper.CombineExpressions(expression, isActiveFilter);
    }

    return expression;
}

public static bool IsActiveFilter(bool isActive, bool boolParam)
{
    throw new NotSupportedException(AbpEfCoreDataFilterDbFunctionMethods.NotSupportedExceptionMessage);
}

public override string GetCompiledQueryCacheKey()
{
    return $"{base.GetCompiledQueryCacheKey()}:{IsActiveFilterEnabled}";
}
````

##### Make User-defined function mapping compatible with [DevExtreme.AspNet.Data's](https://github.com/DevExpress/DevExtreme.AspNet.Data) `IAsyncAdapter`

If you are using [DevExtreme.AspNet.Data](https://github.com/DevExpress/DevExtreme.AspNet.Data) and `User-defined function mapping`, You need to create a custom adapter to compatible with `IAsyncAdapter`.

````csharp
var adapter = new AbpDevExtremeAsyncAdapter();
CustomAsyncAdapters.RegisterAdapter(typeof(AbpEntityQueryProvider), adapter);
````

````csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data.Async;

namespace Abp.EntityFrameworkCore;

public class AbpDevExtremeAsyncAdapter : IAsyncAdapter
{
    private readonly MethodInfo _countAsyncMethod;
    private readonly MethodInfo _toListAsyncMethod;

    public AbpDevExtremeAsyncAdapter()
    {
        var extensionsType = Type.GetType("Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions, Microsoft.EntityFrameworkCore");
        _countAsyncMethod = FindQueryExtensionMethod(extensionsType, "CountAsync");
        _toListAsyncMethod = FindQueryExtensionMethod(extensionsType, "ToListAsync");
    }

    public Task<int> CountAsync(IQueryProvider provider, Expression expr, CancellationToken cancellationToken)
    {
        return InvokeCountAsync(_countAsyncMethod, provider, expr, cancellationToken);
    }

    public Task<IEnumerable<T>> ToEnumerableAsync<T>(IQueryProvider provider, Expression expr, CancellationToken cancellationToken)
    {
        return InvokeToListAsync<T>(_toListAsyncMethod, provider, expr, cancellationToken);
    }

    static MethodInfo FindQueryExtensionMethod(Type extensionsType, string name)
    {
        return extensionsType.GetMethods().First(m =>
        {
            if (!m.IsGenericMethod || m.Name != name)
            {
                return false;
            }
            var parameters = m.GetParameters();
            return parameters.Length == 2 && parameters[1].ParameterType == typeof(CancellationToken);
        });
    }

    static Task<int> InvokeCountAsync(MethodInfo method, IQueryProvider provider, Expression expr, CancellationToken cancellationToken)
    {
        var countArgument = ((MethodCallExpression)expr).Arguments[0];
        var query = provider.CreateQuery(countArgument);
        return (Task<int>)InvokeQueryExtensionMethod(method, query.ElementType, query, cancellationToken);
    }

    static async Task<IEnumerable<T>> InvokeToListAsync<T>(MethodInfo method, IQueryProvider provider, Expression expr, CancellationToken cancellationToken)
    {
        return await (Task<List<T>>)InvokeQueryExtensionMethod(method, typeof(T), provider.CreateQuery(expr), cancellationToken);
    }

    static object InvokeQueryExtensionMethod(MethodInfo method, Type elementType, IQueryable query, CancellationToken cancellationToken)
    {
        return method.MakeGenericMethod(elementType).Invoke(null, new object[] { query, cancellationToken });
    }
}
````

### MongoDB

ABP abstracts the `IMongoDbRepositoryFilterer` interface to implement data filtering for the [MongoDB Integration](../data/mongodb), it works only if you use the repositories properly. Otherwise, you should manually filter the data.

Currently, the best way to implement a data filter for the MongoDB integration is to create a derived class of `MongoDbRepositoryFilterer` and override `FilterQueryable`. Example:

````csharp
[ExposeServices(typeof(IMongoDbRepositoryFilterer<Book, Guid>))]
public class BookMongoDbRepositoryFilterer : MongoDbRepositoryFilterer<Book, Guid> , ITransientDependency
{
    public BookMongoDbRepositoryFilterer(
        IDataFilter dataFilter,
        ICurrentTenant currentTenant) :
        base(dataFilter, currentTenant)
    {
    }

    public override TQueryable FilterQueryable<TQueryable>(TQueryable query)
    {
        if (DataFilter.IsEnabled<IIsActive>())
        {
            return (TQueryable)query.Where(x => x.IsActive);
        }

        return base.FilterQueryable(query);
    }
}
````

This example implements it only for the `Book` entity. If you want to implement for all entities (those implement the `IIsActive` interface), create your own custom MongoDB repository filterer base class and override the `AddGlobalFilters` as shown below:

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
        base.AddGlobalFilters(filters);

        if (typeof(IIsActive).IsAssignableFrom(typeof(TEntity)) 
            && DataFilter.IsEnabled<IIsActive>())
        {
            filters.Add(Builders<TEntity>.Filter.Eq(e => ((IIsActive)e).IsActive, true));
        }
    }
}


public class MyMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //.......
        context.Services
            .Replace(ServiceDescriptor.Transient(typeof(IMongoDbRepositoryFilterer<,>),typeof(MyMongoDbRepositoryFilterer<,>)));
    }
}
````