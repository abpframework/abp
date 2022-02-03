# 仓储

"*在领域层和数据映射层之间进行中介,使用类似集合的接口来操作领域对象.*" (Martin Fowler).

实际上,仓储用于领域对象在数据库(参阅[实体](Entities.md))中的操作, 通常每个  **聚合根**  或不同的实体创建对应的仓储.

## 通用(泛型)仓储

ABP为每个聚合根或实体提供了  **默认的通用(泛型)仓储**  . 你可以在服务中[注入](Dependency-Injection.md) `IRepository<TEntity, TKey>` 使用标准的**CRUD**操作.

> 数据库提供程序层应正确配置为能够使用默认的通用存储库. 如果你已经使用启动模板创建了项目,则这些配置 **已经完成**了. 如果不是,请参考数据库提供程序文档([EF Core](Entity-Framework-Core.md) / [MongoDB](MongoDB.md))进行配置.

**默认通用仓储用法示例:**

````C#
public class PersonAppService : ApplicationService
{
    private readonly IRepository<Person, Guid> _personRepository;

    public PersonAppService(IRepository<Person, Guid> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task Create(CreatePersonDto input)
    {
        var person = new Person { Name = input.Name, Age = input.Age };

        await _personRepository.InsertAsync(person);
    }

    public List<PersonDto> GetList(string nameFilter)
    {
        var people = _personRepository
            .Where(p => p.Name.Contains(nameFilter))
            .ToList();

        return people
            .Select(p => new PersonDto {Id = p.Id, Name = p.Name, Age = p.Age})
            .ToList();
    }
}
````

> 参阅 "*IQueryable & 异步操作*" 部分了解如何使用 **异步扩展方法**, 如 `ToListAsync()` (建议始终使用异步) 而不是 `ToList()`.

在这个例子中;

* `PersonAppService` 在它的构造函数中注入了 `IRepository<Person, Guid>` .
* `Create` 方法使用了 `InsertAsync` 创建并保存新的实体.
* `GetList` 方法使用标准LINQ `Where` 和 `ToList` 方法在数据源中过滤并获取People集合.

> 上面的示例在[实体](Entities.md)与[DTO](Data-Transfer-Objects.md)之间使用了手动映射. 参阅 [对象映射](Object-To-Object-Mapping.md) 了解自动映射的使用方式.

通用仓储提供了一些开箱即用的标准CRUD功能:

* 提供 `Insert` 方法用于保存新实体.
* 提供 `Update` 和 `Delete` 方法通过实体或实体id更新或删除实体.
* 提供 `Delete` 方法使用条件表达式过滤删除多个实体.
* 实现了 `IQueryable<TEntity>`, 所以你可以使用LINQ和扩展方法 `FirstOrDefault`, `Where`, `OrderBy`, `ToList` 等...
* 所有方法都具有 **sync(同步)** 和 **async(异步)** 版本.

### 基础仓储

`IRepository<TEntity, TKey>` 接口扩展了标准 `IQueryable<TEntity>` 你可以使用标准LINQ方法自由查询.但是,某些ORM提供程序或数据库系统可能不支持`IQueryable`接口.

ABP提供了 `IBasicRepository<TEntity, TPrimaryKey>` 和 `IBasicRepository<TEntity>` 接口来支持这样的场景. 你可以扩展这些接口（并可选择性地从`BasicRepositoryBase`派生）为你的实体创建自定义存储库.

依赖于 `IBasicRepository` 而不是依赖 `IRepository` 有一个优点, 即使它们不支持 `IQueryable` 也可以使用所有的数据源, 但主要的供应商, 像 Entity Framework, NHibernate 或 MongoDb 已经支持了 `IQueryable`.

因此, 使用 `IRepository` 是典型应用程序的 **建议方法**. 但是可重用的模块开发人员可能会考虑使用 `IBasicRepository` 来支持广泛的数据源.

### 只读仓储

对于想要使用只读仓储的开发者,我们提供了`IReadOnlyRepository<TEntity, TKey>` 与 `IReadOnlyBasicRepository<Tentity, TKey>`接口.

### 无主键的通用(泛型)仓储

如果你的实体没有id主键 (例如, 它可能具有复合主键) 那么你不能使用上面定义的 `IRepository<TEntity, TKey>`, 在这种情况下你可以仅使用实体(类型)注入 `IRepository<TEntity>`.

> `IRepository<TEntity>` 有一些缺失的方法, 通常与实体的 `Id` 属性一起使用. 由于实体在这种情况下没有 `Id` 属性, 因此这些方法不可用. 比如 `Get` 方法通过id获取具有指定id的实体. 不过, 你仍然可以使用`IQueryable<TEntity>`的功能通过标准LINQ方法查询实体.



### 自定义仓储

对于大多数情况, 默认通用仓储就足够了.  但是, 你可能会需要为实体创建自定义仓储类.

#### 自定义仓储示例

ABP不会强制你实现任何接口或从存储库的任何基类继承. 它可以只是一个简单的POCO类. 但是建议继承现有的仓储接口和类, 获得开箱即用的标准方法使你的工作更轻松.

#### 自定义仓储接口

首先在领域层定义一个仓储接口:

```c#
public interface IPersonRepository : IRepository<Person, Guid>
{
    Task<Person> FindByNameAsync(string name);
}
```

此接口扩展了 `IRepository<Person, Guid>` 以使用已有的通用仓储功能.

#### 自定义仓储实现

自定义存储库依赖于你使用的数据访问工具. 在此示例中, 我们将使用Entity Framework Core:

````C#
public class PersonRepository : EfCoreRepository<MyDbContext, Person, Guid>, IPersonRepository
{
    public PersonRepository(IDbContextProvider<TestAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {

    }

    public async Task<Person> FindByNameAsync(string name)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Set<Person>()
            .Where(p => p.Name == name)
            .FirstOrDefaultAsync();
    }
}
````

你可以直接使用数据库访问提供程序 (本例中是 `DbContext` ) 来执行操作.

> 请参阅[EF Core](Entity-Framework-Core.md)或[MongoDb](MongoDB.md)了解如何自定义仓储.

## IQueryable & 异步操作

`IRepository` 继承自 `IQueryable`,这意味着你可以**直接使用LINQ扩展方法**. 如上面的*泛型仓储*示例.

**示例: 使用 `Where(...)` 和 `ToList()` 扩展方法**

````csharp
var people = _personRepository
    .Where(p => p.Name.Contains(nameFilter))
    .ToList();
````

`.ToList`, `Count()`... 是在 `System.Linq` 命名空间下定义的扩展方法. ([参阅所有方法](https://docs.microsoft.com/en-us/dotnet/api/system.linq.queryable)).

你通常想要使用 `.ToListAsync()`, `.CountAsync()`.... 来编写**真正的异步代码**.

但在你使用标准的[应用程序启动模板](Startup-Templates/Application.md)时会发现无法在应用层或领域层使用这些异步扩展方法,因为:

* 这里异步方法**不是标准LINQ方法**,它们定义在[Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)Nuget包中.
* 标准模板应用层与领域层**不引用**EF Core 包以实现数据库提供程序独立.

根据你的需求和开发模式,你可以根据以下选项使用异步方法.

> 强烈建议使用异步方法! 在执行数据库查询时不要使用同步LINQ方法,以便能够开发可伸缩的应用程序.

### 选项-1: 引用EF Core

最简单的方法是在你想要使用异步方法的项目直接引用EF Core包.

> 添加[Volo.Abp.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore)NuGet包到你的项目间接引用EF Core包. 这可以确保你的应用程序其余部分兼容正确版本的EF Core.

当你添加NuGet包后,你可以使用全功能的EF Core扩展方法.

**示例: 直接使用 `ToListAsync()`**

````csharp
var people = _personRepository
    .Where(p => p.Name.Contains(nameFilter))
    .ToListAsync();
````

此方法建议;

* 如果你正在开发一个应用程序并且**不打算在将来** 更新FE Core,或者如果以后需要更改,你可以**容忍**它. 如果你正在开发最终的应用程序,这是合理的.

#### MongoDB

如果使用的是MongoDB,则需要将[Volo.Abp.MongoDB] NuGet包添加到项目中. 但在这种情况下你也不能直接使用异步LINQ扩展(例如`ToListAsync`),因为MongoDB不提供 `IQueryable<T>`的异步扩展方法,而是提供 `IMongoQueryable<T>`. 你需要先将查询强制转换为 `IMongoQueryable<T>` 才能使用异步扩展方法.

**示例: 转换Cast `IQueryable<T>` 为 `IMongoQueryable<T>` 并且使用 `ToListAsync()`**

````csharp
var people = ((IMongoQueryable<Person>)_personRepository
    .Where(p => p.Name.Contains(nameFilter)))
    .ToListAsync();
````

### 选项-2: 自定义仓储方法

你始终可以创建自定义仓储方法并使用特定数据库提供程序的API,比如这里的异步扩展方法. 有关自定义存储库的更多信息,请参阅[EF Core](Entity-Framework-Core.md)或[MongoDb](MongoDB.md)文档.

此方法建议;

* 如果你想**完全隔离**你的领域和应用层和数据库提供程序.
* 如果你开发可**重用的[应用模块](Modules/Index.md)**,并且不想强制使用特定的数据库提供程序,这应该作为一种[最佳实践](Best-Practices/Index.md).

### 选项-3: IAsyncQueryableExecuter

`IAsyncQueryableExecuter` 是一个用于异步执行 `IQueryable<T>` 对象的服务,**不依赖于实际的数据库提供程序**.

**示例: 注入并使用 `IAsyncQueryableExecuter.ToListAsync()` 方法**

````csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace AbpDemo
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IAsyncQueryableExecuter _asyncExecuter;

        public ProductAppService(
            IRepository<Product, Guid> productRepository,
            IAsyncQueryableExecuter asyncExecuter)
        {
            _productRepository = productRepository;
            _asyncExecuter = asyncExecuter;
        }

        public async Task<ListResultDto<ProductDto>> GetListAsync(string name)
        {
            //Create the query
            var query = _productRepository
                .Where(p => p.Name.Contains(name))
                .OrderBy(p => p.Name);

            //Run the query asynchronously
            List<Product> products = await _asyncExecuter.ToListAsync(query);

            //...
        }
    }
}
````

> `ApplicationService` 和 `DomainService` 基类已经预属性注入了 `AsyncExecuter` 属性,所以你可直接使用.

ABP框架使用实际数据库提供程序的API异步执行查询.虽然这不是执行查询的常见方式,但它是使用异步API而不依赖于数据库提供者的最佳方式.

此方法建议;

* 如果你正在构建一个没有数据库提供程序集成包的**可重用库**,但是在某些情况下需要执行 `IQueryable<T>`对象.

例如,ABP框架在 `CrudAppService` 基类中(参阅[应用程序](Application-Services.md)文档)使用 `IAsyncQueryableExecuter`.
