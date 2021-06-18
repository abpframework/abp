# Entity Framework Core 集成

本文介绍了如何将EF Core作为ORM提供程序集成到基于ABP的应用程序以及如何对其进行配置.

## 安装

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

> 注: 你可以直接下载预装EF Core的[启动模板](https://abp.io/Templates).

### 数据库管理系统选择

EF Core支持多种数据库管理系统([查看全部](https://docs.microsoft.com/en-us/ef/core/providers/)). ABP框架和本文档不依赖于任何特定的DBMS.

如果要创建一个可重用的[应用程序模块](Modules/Index.md),应避免依赖于特定的DBMS包.但在最终的应用程序中,始终会选择一个DBMS.

参阅[为Entity Framework Core切换到其他DBMS](Entity-Framework-Core-Other-DBMS.md)文档学习如何切换DBMS.

## 创建 DbContext

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

### 关于EF Core Fluent Mapping

[应用程序启动模板](Startup-Templates/Application.md)已配置使用[EF Core fluent configuration API](https://docs.microsoft.com/en-us/ef/core/modeling/)映射你的实体到数据库表.

你依然为你的实体属性使用**data annotation attributes**(像`[Required]`),而ABP文档通常遵循**fluent mapping API** approach方法. 如何使用取决与你.

ABP框架有一些**实体基类**和**约定**(参阅[实体文档](Entities.md))提供了一些有用的扩展方法来配置从基本实体类继承的属性.

#### ConfigureByConvention 方法

`ConfigureByConvention()` 是主要的扩展方法,它对你的实体**配置所有的基本属性**和约定. 所以在你的流利映射代码中为你所有的实体调用这个方法是 **最佳实践**,

**示例**: 假设你有一个直接继承 `AggregateRoot<Guid>` 基类的 `Book` 实体:

````csharp
public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
}
````

你可以在你的 `DbContext` 重写 `OnModelCreating` 方法并且做以下配置:

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

* 这里调用了 `b.ConfigureByConvention()` 它对于**配置基本属性**非常重要.
* 你可以在这里配置 `Name` 属性或者使用**data annotation attributes**(参阅[EF Core 文档](https://docs.microsoft.com/zh-cn/ef/core/modeling/entity-properties)).

> 尽管有许多扩展方法可以配置基本属性,但如果需要 `ConfigureByConvention()` 内部会调用它们. 因此仅调用它就足够了.

### 配置连接字符串选择

如果你的应用程序有多个数据库,你可以使用 `connectionStringName]` Attribute为你的DbContext配置连接字符串名称.
例:

```csharp
[ConnectionStringName("MySecondConnString")]
public class MyDbContext : AbpDbContext<MyDbContext>
{

}
```

如果不进行配置,则使用`Default`连接字符串. 如果你配置特定的连接字符串的名称,但在应用程序配置中没有定义这个连接字符串名称,那么它会回退到`Default`连接字符串(参阅[连接字符串文档](Connection-Strings.md)了解更多信息).

## 将DbContext注册到依赖注入

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

### 添加默认仓储

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

你通常希望从IRepository派生以继承标准存储库方法. 然而,你没有必要这样做. 仓储接口在分层应用程序的领域层中定义,它在数据访问/基础设施层([启动模板](https://abp.io/Templates)中的`EntityFrameworkCore`项目)中实现

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
        await DbContext.Database.ExecuteSqlRawAsync(
            $"DELETE FROM Books WHERE Type = {(int)type}"
        );
    }
}
````

现在可以在需要时[注入](Dependency-Injection.md)`IBookRepository`并使用`DeleteBooksByType`方法.

#### 覆盖默认通用仓储

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
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
````

## 访问 EF Core API

大多数情况下应该隐藏仓储后面的EF Core API(这也是仓储的设计目地). 但是如果想要通过仓储访问DbContext实现,则可以使用`GetDbContext()`或`GetDbSet()`扩展方法. 例:

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

## Extra Properties & Object Extension Manager

额外属性系统允许你为实现了 `IHasExtraProperties` 的实体set/get动态属性. 当你想将自定义属性添加到[应用程序模块](Modules/Index.md)中定义的实体时,它特别有用.

默认,实体的所有额外属性存储在数据库的一个 `JSON` 对象中.

实体扩展系统允许你存储额外属性在数据库的单独字段中. 有关额外属性和实体扩展系统的更多信息,请参阅下列文档:

* [自定义应用模块: 扩展实体](Customizing-Application-Modules-Extending-Entities.md)
* [实体](Entities.md)

本节只解释了 EF Core相关的 `ObjectExtensionManager` 及其用法.

### ObjectExtensionManager.Instance

`ObjectExtensionManager` 实现单例模式,因此你需要使用静态的 `ObjectExtensionManager.Instance` 来执行所有操作.

### MapEfCoreProperty

`MapEfCoreProperty` 是一种快捷扩展方法,用于定义实体的扩展属性并映射到数据库.

**示例**: 添加 `Title` 属性 (数据库字段)到 `IdentityRole` 实体:

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

如果相关模块已实现此功能(通过使用下面说明的 `ConfigureEfCoreEntity`)则将新属性添加到模型中. 然后你需要运行标准的 `Add-Migration` 和 `Update-Database` 命令更新数据库以添加新字段.

>`MapEfCoreProperty` 方法必须在使用相关的 `DbContext` 之前调用,它是一个静态方法. 最好的方法是尽早的应用程序中使用它. 应用程序启动模板含有 `YourProjectNameEntityExtensions` 类,可以在放心的在此类中使用此方法.

### ConfigureEfCoreEntity

如果你正在开发一个可重用使用的模块,并允许应用程序开发人员将属性添加到你的实体,你可以在实体映射使用 `ConfigureEfCoreEntity` 扩展方法,但是在配置实体映射时可以使用快捷的扩展方法 `ConfigureObjectExtensions`:

````csharp
builder.Entity<YourEntity>(b =>
{
    b.ConfigureObjectExtensions();
    //...
});
````

如果你调用 `ConfigureByConvention()` 扩展方法(在此示例中 `b.ConfigureByConvention`),ABP框架内部会调用 `ConfigureObjectExtensions` 方法. 使用 `ConfigureByConvention` 方法是**最佳实践**,因为它还按照约定配置基本属性的数据库映射.

参阅上面提到的 "*ConfigureByConvention 方法*" 了解更多信息.

## 高级主题

### 设置默认仓储类

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

### 为默认仓储设置Base DbContext类或接口

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

现在,你的自定义仓储也可以使用`IBookStoreDbContext`接口:

````csharp
public class BookRepository : EfCoreRepository<IBookStoreDbContext, Book, Guid>, IBookRepository
{
    //...
}
````

使用DbContext接口的一个优点是它可以被其他实现替换.

### 替换其他仓储

正确定义并使用DbContext接口后,任何其他实现都可以使用以下方法替换它:

**ReplaceDbContextAttribute**

```csharp
[ReplaceDbContext(typeof(IBookStoreDbContext))]
public class OtherDbContext : AbpDbContext<OtherDbContext>, IBookStoreDbContext
{
    //...
}
```

**ReplaceDbContext option**

````csharp
context.Services.AddAbpDbContext<OtherDbContext>(options =>
{
    //...
    options.ReplaceDbContext<IBookStoreDbContext>();
});
````

在这个例子中,`OtherDbContext`实现了`IBookStoreDbContext`. 此功能允许你在开发时使用多个DbContext(每个模块一个),但在运行时可以使用单个DbContext(实现所有DbContext的所有接口).

## 另请参阅

* [实体](Entities.md)
