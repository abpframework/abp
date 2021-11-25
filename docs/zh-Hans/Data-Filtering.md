# 数据过滤

[Volo.Abp.Data](https://www.nuget.org/packages/Volo.Abp.Data) 包定义了在查询数据库时自动过滤数据的服务.

## 预定义的过滤

ABP定义了一些开箱即用的过滤.

### ISoftDelete

将[实体](Entities.md)标记为已删除,并不是物理删除. 实现 `ISoftDelete` 接口将你的实体"软删除".

示例:

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

`ISoftDelete` 定义了 `IsDeleted` 属性. 当你使用[仓储](Repositories.md)删除一条记录时, ABP会自动将 `IsDeleted` 设置为true,并将删除操作替换为修改操作(如果需要,也可以手动将 `IsDeleted` 设置为true). 在查询数据库时会**自动过滤软删除的实体**.

> `ISoftDelete` 过滤默认启用, 想要真正的从数据库删除实体需要显示的禁用过滤. 参见下面提到的 `IDataFilter` 服务.

### IMultiTenant

[多租户](Multi-Tenancy.md) 是创建 SaaS 应用程序的有效方法. 多租户应用程序通常需要在租户间隔离数据. 实现 `IMultiTenant` 接口使你的实体支持 "多租户".

示例:

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

`IMultiTenant` 接口定义了 `TenantId` 属性用于自动过滤当前租户实体. 更多信息参见[多租户](Multi-Tenancy.md)文档.

## IDataFilter 服务: 启用/禁用 数据过滤

你可以使用 `IDataFilter` 服务控制数据过滤.

示例:

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

* [注入](Dependency-Injection.md) `IDataFilter` 服务到你的类中.
* 在 `using` 语句中使用 `Disable` 方法创建一个代码块,其中禁用了 `ISoftDelete` 过滤器(始终与 `using` 搭配使用,确保代码块执行后将过滤重置为之前的状态).

`IDataFilter.Enable` 方法可以启用过滤. 可以嵌套使用 `Enable` 和 `Disable` 方法定义内部作用域.

## AbpDataFilterOptions

`AbpDataFilterOptions` 用于设置数据过滤系统[选项](Options.md).

下面的示例代码在默认情况下禁用了 `ISoftDelete` 过滤,除非显示启用,在查询数据库时会包含标记为已删除的实体:

````csharp
Configure<AbpDataFilterOptions>(options =>
{
    options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: false);
});
````

> 更改全局过滤的默认值需要小心,特别是在你使用预构建的模块时该模块可能是在默认启用软删除过滤的情况下开发的. 但你可以安全的为自己定义的数据过滤执行此操作.

## 自定义数据过滤

定义和实现新的过滤很大程序上取决与数据库提供者. ABP为所有的数据库提供者实现了预构建的过滤.

首先为过滤定义一个接口 (如 `ISoftDelete` 和 `IMultiTenant`) 然后用实体实现它.

示例:

````csharp
public interface IIsActive
{
    bool IsActive { get; }
}
````

`IIsActive` 接口可以过滤活跃/消极数据,任何[实体](Entities.md)都可以实现它:

````csharp
public class Book : AggregateRoot<Guid>, IIsActive
{
    public string Name { get; set; }

    public bool IsActive { get; set; } //Defined by IIsActive
}
````

### EntityFramework Core

ABP使用[EF Core的全局过滤](https://docs.microsoft.com/en-us/ef/core/querying/filters)系统用于[EF Core 集成](Entity-Framework-Core.md). 所以它很好的集成到EF Core中,即使你直接使用 `DbContext` 它也可以正常工作.

实现自定义过滤的最佳方法是为重写你的 `DbContext` 的 `ShouldFilterEntity` 和 `CreateFilterExpression` 方法. 示例:

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

* 添加 `IsActiveFilterEnabled` 属性用于检查是否启用了 `IIsActive` . 内部使用了之前介绍到的 `IDataFilter` 服务.
* 重写 `ShouldFilterEntity` 和 `CreateFilterExpression` 方法检查给定实体是否实现 `IIsActive` 接口,在必要时组合表达式.

### MongoDB

ABP抽象了 `IMongoDbRepositoryFilterer` 接口为[MongoDB 集成](MongoDB.md)实现数据过滤, 只有正确的使用仓储,它才会工作. 否则你需要手动过滤数据.

目前为MongoDB集成实现数据过滤的最佳方法是重写派生自 `MongoDbRepositoryFilterer` 基类的 `AddGlobalFilters` 方法:

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

    public override void AddGlobalFilters(List<FilterDefinition<Book>> filters)
    {
        base.AddGlobalFilters(filters);

        if (DataFilter.IsEnabled<IIsActive>())
        {
            filters.Add(Builders<Book>.Filter.Eq(e => ((IIsActive)e).IsActive, true));
        }
    }
}
````

示例中仅为 `Book` 实体实现了过滤. 如果你想要为所有的实体实现过滤 (实现了 `IIsActive` 接口的实体),可以创建自己的 `MongoDbRepositoryFilterer` 基类并重 写 `AddGlobalFilters` 方法. 如下所示:

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

> 参阅[MongoDb集成文档](MongoDB.md)的 "设置默认仓储" 部分了解如何使用自定义类替换默认仓储.