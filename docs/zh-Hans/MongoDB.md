## MongoDB 集成

本文会介绍如何将MongoDB集成到基于ABP的应用程序中以及如何配置它

### 安装

 集成MongoDB需要用到`Volo.Abp.MongoDB`这个包.将它安装到你的项目中(如果是多层架构,安装到数据层和基础设施层):

```
Install-Package Volo.Abp.MongoDB
```

然后添加 `AbpMongoDbModule` 依赖到你的 [模块](Module-Development-Basics.md)中:

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

### 创建一个Mongo Db Context

ABP中引入了 **Mongo Db Context** 的概念(跟Entity Framework Core的DbContext很像)让使用和配置集合变得更简单.举个例子:

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

* 继承 `AbpMongoDbContext` 类
* 为每一个mongo集合添加一个公共的 `IMongoCollection<TEntity>` 属性.ABP默认使用这些属性创建默认的仓储
* 重写 `CreateModel` 方法,可以在方法中配置集合(如设置集合在数据库中的名字)

### 将 Db Context 注入到依赖注入中

在你的模块中使用 `AddAbpDbContext` 方法将Db Context注入到[依赖注入](Dependency-Injection.md)系统中.

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

#### 添加默认的仓储

在注入的时候使用 `AddDefaultRepositories()`, ABP就能自动为Db Context中的每一个实体创建[仓储](Repositories.md):

````C#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

这样就会默认为每一个聚合根实体(继承自AggregateRoot的类)创建一个仓储.如果你也想为其他的实体创建仓储,将 `includeAllEntities` 设置为 `true`就可以了:

```c#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
```

现在可以在你的服务中注入并使用`IRepository<TEntity>` 或 `IQueryableRepository<TEntity>`了.比如你有一个主键类型为`Guid`的`Book`实体:

```csharp
public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }
}
```

(`BookType`是个枚举)你想在[领域服务](Domain-Services.md)中创建一个`Book`实体:

```csharp
public class BookManager : DomainService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookManager(IRepository<Book, Guid> bookRepository) //注入默认的仓储
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

        await _bookRepository.InsertAsync(book);
        //使用仓储中的方法

        return book;
    }
}
```

这是一个使用`InsertAsync`方法将一个实体插入到数据库的例子.

#### 添加自定义仓储

大多数情况下默认的泛型仓储已经足够用了(因为它们实现了`IQueryable`).然而,你可能需要创建自定义的仓库并添加自己的仓储方法.

比如你想要通过books类型删除书籍.建议像下面这样为你的仓储定义一个接口:

```csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(
        BookType type,
        CancellationToken cancellationToken = default(CancellationToken)
    );
}
```

通常你希望从`IRepository`中继承标准的仓储方法.其实,你不必那么做.仓储接口定义在领域层,在数据层/基础设施层实现.([启动模板](https://abp.io/Templates)中的`MongoDB`项目)

实现`IBookRepository`接口的例子:

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

现在,就能在需要的时候[注入](Dependency-Injection.md)`IBookRepository`并使用`DeleteBooksByType`方法了.

##### 重写默认的泛型仓储

即使你创建了自定义仓储,你仍然可以注入默认的泛型仓储(本例中的`IRepository<Book, Guid>`).默认的仓储实现不会使用你创建的类.

如果你想用自定义的仓储替换默认的仓储实现,在`AddMongoDbContext`中做:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories();
    options.AddRepository<Book, BookRepository>(); //替换 IRepository<Book, Guid>
});
```

当你想**重写基础仓储方法**时,这一点尤为重要.例如,你想要重写`DeleteAsync`方法,以便更有效的删除实体:

```csharp
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: 自定义实现删除方法
}
```

#### 访问MongoDB API

大多数情况下,你想要将MongoDB API隐藏在仓储后面(这是仓储的主要目的).如果你想在仓储之上访问MongoDB API,你可以使用`GetDatabaseAsync()`, `GetAggregateAsync()` 或`GetCollectionAsync()`方法.例如:

```csharp
public class BookService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task FooAsync()
    {
        IMongoDatabase database = await _bookRepository.GetDatabaseAsync();
        IMongoCollection<Book> books = await _bookRepository.GetCollectionAsync();
        IAggregateFluent<Book> bookAggregate = await _bookRepository.GetAggregateAsync();
    }
}
```

> 重要:如果你想访问MongoDB API,你需要在你的项目中引用`Volo.Abp.MongoDB`.这会破坏封装,但在这种情况下,这就是你想要的.

#### 事务

MongoDB在4.0版本开始支持事务, ABP在3.2版本加入了对MongoDb事务的支持. 如果你升级到3.2版本,需要将[MongoDbSchemaMigrator](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.MongoDB/MongoDb/MongoDbMyProjectNameDbSchemaMigrator.cs)添加到你的 `.MongoDB` 项目中.

[启动模板](Startup-templates/Index.md)默认在 `.MongoDB` 项目中**禁用**了工作单元事务. 如果你的MongoDB服务器支持事务,你可以手动启用工作单元的事务:

```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Enabled;
});
```

#### 高级主题

##### 设置默认的仓储类

默认的泛型仓储默认被`MongoDbRepository`类实现.你可以创建自己的实现并在默认的仓储中使用.

首先,像下面这样定义你的仓储类:

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

第一个是[复合主键的实体](Entities.md),第二个是只有一个主键的实体.

如果需要重写方法建议继承`MongoDbRepository`类,否则,你需要手动实现所有的仓储方法.

现在,你可以使用`SetDefaultRepositoryClasses`:

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

##### 为默认的仓储设置基类或接口

如果你的MongoDbContext继承自另一个MongoDbContext或者实现了某个接口,你可以使用这个基类或者接口作为默认仓储的类型.如:
```csharp
public interface IBookStoreMongoDbContext : IAbpMongoDbContext
{
    Collection<Book> Books { get; }
}
```

`IBookStoreMongoDbContext`被`BookStoreMongoDbContext`类实现.然后你就可以在`AddDefaultRepositories`中使用:

```csharp
context.Services.AddMongoDbContext<BookStoreMongoDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreMongoDbContext>();
    //...
});
```

现在,你自定义的`BookRepository`类也可以使用`IBookStoreMongoDbContext`接口:

```csharp
public class BookRepository
    : MongoDbRepository<IBookStoreMongoDbContext, Book, Guid>,
      IBookRepository
{
    //...
}
```

为MongoDbContext使用接口的优点就是它可以被另一个实现替换.

##### 替换其他的DbContexts

一旦你正确定义并为MongoDbContext使用了接口,其他的实现就可以使用`ReplaceDbContext`来替换:

```csharp
context.Services.AddMongoDbContext<OtherMongoDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreMongoDbContext>();
});
```

这个例子中,`OtherMongoDbContext`实现了`IBookStoreMongoDbContext`.这个特性允许你在发开的时候使用多个MongoDbContext(每个模块一个),但是运行的时候只能使有一个MongoDbContext(实现所有MongoDbContexts的所有接口)
