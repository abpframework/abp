## Entity Framework Core 集成

本文介绍了如何将EF Core作为ORM提供程序集成到基于ABP的应用程序以及如何对其进行配置.

### 安装

`Volo.Abp.EntityFrameworkCore` 是EF Core 集成的主要nuget包. 将其安装到你的项目中(在分层应用程序中适用于 数据访问/基础设施层):

```shell
Install-Package Volo.Abp.EntityFrameworkCore
```

然后添加 `AbpEntityFrameworkCoreModule` 模块依赖项(`DependsOn` Attribute) 到 [module](Module-Development-Basics.md)(项目中的Mudole类):

````C#
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

### 创建 DbContext

你可以平常一样创建DbContext,它需要继承自 `AbpDbContext<T>`. 如下所示:

````C#
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MyCompany.MyProject
{
    public class MyDbContext : AbpDbContext<MyDbContext>
    {
        //...在这里添加 DbSet properties

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
    }
}
````

### 将DbContext注册到依赖注入

在module中的ConfigureServices方法使用 `AddAbpDbContext` 在[依赖注入](Dependency-Injection.md)系统注册DbContext类.

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

#### 添加默认仓储

ABP会自动为DbContext中的实体创建[默认仓储](Repositories.md). 需要在注册的时使用options添加`AddDefaultRepositories()`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

默认情况下为每个[聚合根实体](Entities.md)(`AggregateRoot`派生的子类)创建一个仓储. 如果想要为其他实体也创建仓储
请将`includeAllEntities` 设置为 `true`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
````

然后你就可以在服务中注入和使用 `IRepository<TEntity>` 或 `IQueryableRepository<TEntity>`.

假如你有一个主键是Guid名为Book实体(聚合根)

```csharp
public class Book : AggregateRoot<Guid>
{
    public string Name { get; set; }

    public BookType Type { get; set; }
}
```

在[领域服务](Domain-Services.md)中创建一个新的Book实例并且使用仓储持久化到数据库中

````csharp
public class BookManager : DomainService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookManager(IRepository<Book, Guid> bookRepository) //注入默认仓储
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

        await _bookRepository.InsertAsync(book); //使用仓储提供的标准方法

        return book;
    }
}
````

在这个示例中使用 `InsertAsync` 将新实例插入到数据库中

#### 添加自定义仓储

默认通用仓储可以满足大多数情况下的需求(它实现了`IQueryable`),但是你可能会需要自定义仓储与仓储方法.

假设你需要根据图书类型删除所有的书籍. 建议为自定义仓储定义一个接口:

````csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(BookType type);
}
````

你通常希望从IRepository派生以继承标准存储库方法. 然而,你没有必要这样做. 仓储接口在分层应用程序的领域层中定义,它在数据访问/基础设施层([启动模板](https://cn.abp.io/Templates)中的`EntityFrameworkCore`项目)中实现

IBookRepository接口的实现示例:

````csharp
public class BookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(BookType type)
    {
        await DbContext.Database.ExecuteSqlCommandAsync(
            $"DELETE FROM Books WHERE Type = {(int)type}"
        );
    }
}
````

现在可以在需要时[注入](Dependency-Injection.md)`IBookRepository`并使用`DeleteBooksByType`方法.

##### 覆盖默认通用仓储

即使创建了自定义仓储,仍可以注入使用默认通用仓储(在本例中是 `IRepository<Book, Guid>`). 默认仓储实现不会使用你创建的自定义仓储类.

如果要将默认仓储实现替换为自定义仓储,请在`AddAbpDbContext`使用options执行:

````csharp
context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
{
    options.AddDefaultRepositories();
    options.AddRepository<Book, BookRepository>();
});
````

在你想要覆盖默认仓储方法对其自定义时,这一点非常需要. 例如你可能希望自定义`DeleteAsync`方法覆盖默认实现

````csharp
public override async Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
````

#### 访问 EF Core API

大多数情况下应该隐藏仓储后面的EF Core API(这也是仓储的设计目地). 但是如果想要通过仓储访问DbContext实现,则可以使用`GetDbContext()`或`GetDbSet()`扩展方法. 例：

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

* `GetDbContext` 返回 `DbContext` 引用,而不是 `BookStoreDbContext`. 你可以释放它, 但大多数情况下你不会需要它.

> 要点: 你必须在使用`DbContext`的项目里引用`Volo.Abp.EntityFrameworkCore`包. 这会破坏封装,但在这种情况下,这就是你需要的.

#### 高级主题

##### 设置默认仓储类

默认的通用仓储的默认实现是`EfCoreRepository`类,你可以创建自己的实现,并将其做为默认实现

首先,像这样定义仓储类:

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

第一个用于具有[复合主键的实体](Entities.md),第二个用于具有单个主键的实体

建议从`EfCoreRepository`类继承并在需要时重写方法. 否则,你需要手动实现所有标准仓储方法.

现在,你可以使用SetDefaultRepositoryClasses Options

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

#### 为默认仓储设置Base DbContext类或接口

如果你的DbContext继承了另外一个DbContext或实现了一个接口,你可以使用这个基类或接口作为默认仓储的DbContext. 例:

````csharp
public interface IBookStoreDbContext : IEfCoreDbContext
{
    DbSet<Book> Books { get; }
}
````

`IBookStoreDbContext`接口是由`BookStoreDbContext`实现的. 然后你可以使用`AddDefaultRepositories`的泛型重载.

````csharp
context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
{
    options.AddDefaultRepositories<IBookStoreDbContext>();
    //...
});
````

现在,您的自定义仓储也可以使用`IBookStoreDbContext`接口:

````csharp
public class BookRepository : EfCoreRepository<IBookStoreDbContext, Book, Guid>, IBookRepository
{
    //...
}
````

使用DbContext接口的一个优点是它可以被其他实现替换.

#### 替换其他仓储

正确定义并使用DbContext接口后,任何其他实现都可以使用以下ReplaceDbContext options 替换它:

````csharp
context.Services.AddAbpDbContext<OtherDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreDbContext>();
});
````

在这个例子中,`OtherDbContext`实现了`IBookStoreDbContext`. 此功能允许你在开发时使用多个DbContext(每个模块一个),但在运行时可以使用单个DbContext(实现所有DbContext的所有接口).