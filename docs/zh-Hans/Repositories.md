## 仓储

"*在领域层和数据映射层之间进行中介,使用类似集合的接口来操作领域对象.*" (Martin Fowler).

实际上,仓储用于领域对象在数据库(参阅[实体](Entities.md))中的操作, 通常每个  **聚合根**  或不同的实体创建对应的仓储.

### 通用(泛型)仓储

ABP为每个聚合根或实体提供了  **默认的通用(泛型)仓储**  . 你可以在服务中[注入](Dependency-Injection.md) `IRepository<TEntity, TKey>` 使用标准的**CRUD**操作. 用法示例:

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

#### 无主键的通用(泛型)仓储

如果你的实体没有id主键 (例如, 它可能具有复合主键) 那么你不能使用上面定义的 `IRepository<TEntity, TKey>`, 在这种情况下你可以仅使用实体(类型)注入 `IRepository<TEntity>`.

> `IRepository<TEntity>` 有一些缺失的方法, 通常与实体的 `Id` 属性一起使用. 由于实体在这种情况下没有 `Id` 属性, 因此这些方法不可用. 比如 `Get` 方法通过id获取具有指定id的实体. 不过, 你仍然可以使用`IQueryable<TEntity>`的功能通过标准LINQ方法查询实体.

### 基础仓储

`IRepository<TEntity, TKey>` 接口扩展了标准 `IQueryable<TEntity>` 你可以使用标准LINQ方法自由查询.但是,某些ORM提供程序或数据库系统可能不支持`IQueryable`接口.

ABP提供了 `IBasicRepository<TEntity, TPrimaryKey>` 和 `IBasicRepository<TEntity>` 接口来支持这样的场景. 你可以扩展这些接口（并可选择性地从`BasicRepositoryBase`派生）为你的实体创建自定义存储库.

依赖于 `IBasicRepository` 而不是依赖 `IRepository` 有一个优点, 即使它们不支持 `IQueryable` 也可以使用所有的数据源, 但主要的供应商, 像 Entity Framework, NHibernate 或 MongoDb 已经支持了 `IQueryable`.

因此, 使用 `IRepository` 是典型应用程序的 **建议方法**. 但是可重用的模块开发人员可能会考虑使用 `IBasicRepository` 来支持广泛的数据源.

### 自定义仓储

对于大多数情况, 默认通用仓储就足够了.  但是, 你可能会需要为实体创建自定义仓储类.

#### 自定义仓储示例

ABP不会强制你实现任何接口或从存储库的任何基类继承. 它可以只是一个简单的POCO类. 但是建议继承现有的仓储接口和类, 获得开箱即用的标准方法使你的工作更轻松.

##### 自定义仓储接口

首先在领域层定义一个仓储接口:

```c#
public interface IPersonRepository : IRepository<Person, Guid>
{
    Task<Person> FindByNameAsync(string name);
}
```

此接口扩展了 `IRepository<Person, Guid>` 以使用已有的通用仓储功能.

##### 自定义仓储实现

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
        return await DbContext.Set<Person>()
            .Where(p => p.Name == name)
            .FirstOrDefaultAsync();
    }
}
````

你可以直接使用数据库访问提供程序 (本例中是 `DbContext` ) 来执行操作. 有关基于EF Core的自定义仓储的更多信息, 请参阅[EF Core 集成文档](Entity-Framework-Core.md).