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

EF Core支持多种数据库管理系统([查看全部](https://docs.microsoft.com/zh-cn/ef/core/providers/)). ABP框架和本文档不依赖于任何特定的DBMS. 如果要创建一个可重用的[应用程序模块](Modules/Index.md),应避免依赖于特定的DBMS包.但在最终的应用程序中,始终会选择一个DBMS.

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

[应用程序启动模板](Startup-Templates/Application.md)已配置使用[EF Core fluent configuration API](https://docs.microsoft.com/zh-cn/ef/core/modeling/)映射你的实体到数据库表.

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

如果你的应用程序有多个数据库,你可以使用 `[connectionStringName]` Attribute为你的DbContext配置连接字符串名称.例:

```csharp
[ConnectionStringName("MySecondConnString")]
public class MyDbContext : AbpDbContext<MyDbContext>
{

}
```

如果不进行配置,则使用`Default`连接字符串. 如果你配置特定的连接字符串的名称,但在应用程序配置中没有定义这个连接字符串名称,那么它会回退到`Default`连接字符串(参阅[连接字符串文档](Connection-Strings.md)了解更多信息).

### AbpDbContextOptions

`AbpDbContextOptions` 用于配置 `DbContext`. 当你使用ABP的应用程序启动模板新建解决方案时, 你会看到一个简单的配置 (在 `EntityFrameworkCore` 集成项目模块类) 如下:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer();
});
````

上面的配置为应用程序的所有 `DbContext`使用SQL Server作为默认DBMS. 上面的配置是简化的写法, 它也可以使用下面的方法进行配置:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(opts =>
    {
        opts.UseSqlServer();
    });
});
````

`options.Configure(...)` 方法有更多的选项进行配置. 例如, 你可以设置 `DbContextOptions` (EF Core自有的配置):

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(opts =>
    {
        opts.DbContextOptions.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
});
````

如果你有唯一的 `DbContext` 或者有多个 `DbContext`, 但是希望对所有的 `DbContext` 使用相同的DBMS, 你无需更多的配置. 但是, 如果你需要为某个特定的 `DbContext` 配置不同的DBMS或对配置进行定制, 你可以进行如下定制:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    // Default configuration for all DbContexts
    options.Configure(opts =>
    {
        opts.UseSqlServer();
    });

    // Customized configuration for a specific DbContext
    options.Configure<MyOtherDbContext>(opts =>
    {
        opts.UseMySQL();
    });
});
````

> 参阅 [为Entity Framework Core切换DBMS](Entity-Framework-Core-Other-DBMS.md) 文档学习如何配置DBMS.

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

默认情况下为每个[聚合根实体](Entities.md)(`AggregateRoot`派生的子类)创建一个仓储. 如果想要为其他实体也创建仓储, 请将`includeAllEntities` 设置为 `true`:

````C#
services.AddAbpDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
````

然后你就可以在服务中注入和使用 `IRepository<TEntity>` 或 `IQueryableRepository<TEntity>`. 假如你有一个主键是Guid名为Book实体(聚合根)

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

在这个示例中使用 `InsertAsync` 将新实例插入到数据库中

#### 添加自定义仓储

默认通用仓储可以满足大多数情况下的需求(它实现了`IQueryable`),但是你可能会需要自定义仓储与仓储方法. 假设你需要根据图书类型删除所有的书籍.

建议为自定义仓储定义一个接口:

````csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task DeleteBooksByType(BookType type);
}
````

你通常希望从IRepository派生以继承标准存储库方法. 然而,你没有必要这样做. 仓储接口在分层应用程序的领域层中定义,它在数据访问/基础设施层([启动模板](https://abp.io/Templates)中的`EntityFrameworkCore`项目)中实现

IBookRepository接口的实现示例:

````csharp
public class BookRepository
    : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task DeleteBooksByType(BookType type)
    {
        var dbContext = await GetDbContextAsync();
        await dbContext.Database.ExecuteSqlRawAsync(
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

    //Replaces IRepository<Book, Guid>
    options.AddRepository<Book, BookRepository>();
});
````

在你想要覆盖默认仓储方法对其自定义时,这一点非常需要. 例如你可能希望自定义`DeleteAsync`方法覆盖默认实现, 以使用更有效的方式删除特定的实体.

````csharp
public async override Task DeleteAsync(
    Guid id,
    bool autoSave = false,
    CancellationToken cancellationToken = default)
{
    //TODO: Custom implementation of the delete method
}
````

## 加载关联实体

假设你拥有带有`OrderLine`集合的`Order`,并且`OrderLine`具有`Order`的导航属性:

````csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace MyCrm
{
    public class Order : AggregateRoot<Guid>, IHasCreationTime
    {
        public Guid CustomerId { get; set; }
        public DateTime CreationTime { get; set; }

        public ICollection<OrderLine> Lines { get; set; } //子集合

        public Order()
        {
            Lines = new Collection<OrderLine>();
        }
    }

    public class OrderLine : Entity<Guid>
    {
        public Order Order { get; set; } //导航属性
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
    }
}

````

然后象下面显示的这样定义数据库映射:

````csharp
builder.Entity<Order>(b =>
{
    b.ToTable("Orders");
    b.ConfigureByConvention();

    //定义关系
    b.HasMany(x => x.Lines)
        .WithOne(x => x.Order)
        .HasForeignKey(x => x.OrderId)
        .IsRequired();
});

builder.Entity<OrderLine>(b =>
{
    b.ToTable("OrderLines");
    b.ConfigureByConvention();
});
````

当你查询一个 `Order`, 你可能想要在单个查询中**包含**所有的 `OrderLine`s 或根据需要在**以后加载它们**.

> 实际上这与ABP框架没有直接关系. 你可以按照 [EF Core 文档](https://docs.microsoft.com/zh-cn/ef/core/querying/related-data/) 了解全部细节. 本节将涵盖与 ABP 框架相关的一些主题.

### 预先加载 / 包含子对象的加载

当你想加载一个带有关联实体的实体时,可以使用不同的选项.

#### Repository.WithDetails

`IRepository.WithDetailsAsync(...)` 可以通过包含一个关系收集/属性来获得 `IQueryable<T>` .

**示例: 获取一个带有 `lines` 的 `order` 对象**

````csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace AbpDemo.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IRepository<Order, Guid> _orderRepository;

        public OrderManager(IRepository<Order, Guid> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task TestWithDetails(Guid id)
        {
            //通过包含子集合获取一个 IQueryable<T>
            var queryable = await _orderRepository.WithDetailsAsync(x => x.Lines);

            //应用其他的 LINQ 扩展方法
            var query = queryable.Where(x => x.Id == id);

            //执行此查询并获取结果
            var order = await AsyncExecuter.FirstOrDefaultAsync(query);
        }
    }
}
````

> `AsyncExecuter` 用于执行异步 LINQ 扩展,而无需依赖 EF Core. 如果你将 EF Core NuGet 包引用添加到你的项目中,则可以直接使用 `await query.FirstOrDefaultAsync()`. 但是, 这次你依赖于域层中的 EF Core. 请参阅 [仓储文档](Repositories.md) 以了解更多.

**示例: 获取一个包含 `lines` 的 `orders` 列表**

````csharp
public async Task TestWithDetails()
{
    //通过包含一个子集合获取一个 IQueryable<T>
    var queryable = await _orderRepository.WithDetailsAsync(x => x.Lines);

    //执行此查询并获取结果
    var orders = await AsyncExecuter.ToListAsync(queryable);
}
````

> 如果你需要包含多个导航属性或集合,`WithDetailsAsync`方法可以获得多个表达参数.

#### DefaultWithDetailsFunc

如果你没有将任何表达式传递到 `WithDetailsAsync` 方法,则它包括使用你提供的 `DefaultWithDetailsFunc` 选项的所有详细信息.

你可以在你的 `EntityFrameworkCore` 项目[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法为一个实体配置 `DefaultWithDetailsFunc`.

**示例: 在查询一个 `Order` 时包含 `Lines`**

````csharp
Configure<AbpEntityOptions>(options =>
{
    options.Entity<Order>(orderOptions =>
    {
        orderOptions.DefaultWithDetailsFunc = query => query.Include(o => o.Lines);
    });
});
````

> 你可以在这里完全使用 EF Core API,因为这位于 EF Core集成项目中.

然后你可以不带任何参数地调用 `WithDetails` 方法:

````csharp
public async Task TestWithDetails()
{
    //通过包含一个子集合获取一个 IQueryable<T>
    var queryable = await _orderRepository.WithDetailsAsync();

    //执行此查询并获取结果
    var orders = await AsyncExecuter.ToListAsync(queryable);
}
````

`WithDetailsAsync()` 执行你已经在 `DefaultWithDetailsFunc` 中设置的表达式.

#### 仓储 Get/Find 方法

有些标准的 [仓储](Repositories.md) 方法带有可选的 `includeDetails` 参数;

* `GetAsync` 和 `FindAsync` 方法带有默认值为 `true` 的 `includeDetails`.
* `GetListAsync` 和 `GetPagedListAsync` 方法带有默认值为 `false` 的 `includeDetails`.

这意味着,默认情况下返回**包含子对象的单个实体**,而列表返回方法则默认不包括子对象信息.你可以明确通过 `includeDetails` 来更改此行为.

> 这些方法使用上面解释的 `DefaultWithDetailsFunc` 选项.

**示例:获取一个包含子对象的 `order`**

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id);
}
````

**示例:获取一个不包含子对象的 `order`**

````csharp
public async Task TestWithoutDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id, includeDetails: false);
}
````

**示例:获取一个包含子对象的实体列表**

````csharp
public async Task TestWithDetails()
{
    var orders = await _orderRepository.GetListAsync(includeDetails: true);
}
````

#### 更多的替代选择

存储库模式尝试封装 EF Core, 因此你的选项是有限的. 如果你需要高级方案,你可以按照其中一个选项执行:

* 创建自定义存储库方法并使用完整的 EF Core API.
* 在你的项目中引用 `Volo.Abp.EntityFrameworkCore` . 通过这种方式,你可以直接在代码中使用 `Include` 和 `ThenInclude` .

请参阅 EF Core 的 [预先加载文档](https://docs.microsoft.com/zh-cn/ef/core/querying/related-data/eager).

### 显式 / 延迟加载

如果你在查询实体时不包括关系,并且以后需要访问导航属性或集合,则你有不同的选择.

#### EnsurePropertyLoadedAsync / EnsureCollectionLoadedAsync

仓储提供 `EnsurePropertyLoadedAsync` 和 `EnsureCollectionLoadedAsync` 扩展方法来**显示加载**一个导航属性或子集合.

**示例: 在需要时加载一个 `Order` 的 `Lines`**

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id, includeDetails: false);
    //order.Lines 此时是空的

    await _orderRepository.EnsureCollectionLoadedAsync(order, x => x.Lines);
    //order.Lines 被填充
}
````

如果导航属性或集合已经被加载那么 `EnsurePropertyLoadedAsync` 和 `EnsureCollectionLoadedAsync` 方法不做任何处理. 所以,调用多次也没有问题.

请参阅 EF Core 的[显示加载文档](https://docs.microsoft.com/zh-cn/ef/core/querying/related-data/explicit).

#### 使用代理的延时加载

在某些情况下,可能无法使用显式加载,尤其是当你没有引用 `Repository` 或 `DbContext`时.延时加载是 EF Core 加载关联属性/集合的一个功能, 当你第一次访问它.

启用延时加载:

1. 安装 [Microsoft.EntityFrameworkCore.Proxies](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Proxies/) 包到你的项目(通常是 EF Core 集成项目)
2. 为你的 `DbContext` 配置 `UseLazyLoadingProxies` (在 EF Core 项目的模块的 `ConfigureServices` 方法中). 例如:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.PreConfigure<MyCrmDbContext>(opts =>
    {
        opts.DbContextOptions.UseLazyLoadingProxies(); //启用延时加载
    });

    options.UseSqlServer();
});
````

3. 使你的导航属性和集合是 `virtual`. 例如:

````csharp
public virtual ICollection<OrderLine> Lines { get; set; } //虚集合
public virtual Order Order { get; set; } //虚导航属性
````

启用延时加载并整理实体后,你可以自由访问导航属性和集合:

````csharp
public async Task TestWithDetails(Guid id)
{
    var order = await _orderRepository.GetAsync(id);
    //order.Lines 此时是空的

    var lines = order.Lines;
    //order.Lines 被填充 (延时加载)
}
````

每当你访问属性/集合时,EF Core 都会自动执行额外的查询,从数据库中加载属性/集合.

> 应谨慎使用延时加载,因为它可能会在某些特定情况下导致性能问题.

请参阅 EF Core 的[延时加载文档](https://docs.microsoft.com/zh-cn/ef/core/querying/related-data/lazy).

## 访问 EF Core API

大多数情况下应该隐藏仓储后面的EF Core API(这也是仓储的设计目的). 但是如果想要通过仓储访问DbContext实现,则可以使用`GetDbContext()`或`GetDbSet()`扩展方法. 例如:

````csharp
public async Task TestAsync()
{
    var dbContext = await _orderRepository.GetDbContextAsync();
    var dbSet = await _orderRepository.GetDbSetAsync();
    //var dbSet = dbContext.Set<Order>(); //Alternative, when you have the DbContext
}
````

* `GetDbContext` 返回 `DbContext` 引用,而不是 `BookStoreDbContext`. 你可以强制转化它, 但大多数情况下你不会需要它.

> 要点: 你必须在使用`DbContext`的项目里引用`Volo.Abp.EntityFrameworkCore`包. 这会破坏封装,但在这种情况下,这就是你需要的.

## 额外属性 & Object Extension Manager

额外属性系统允许你为实现了 `IHasExtraProperties` 的实体set/get动态属性. 当你想将自定义属性添加到[应用程序模块](Modules/Index.md)中定义的实体时,它特别有用.

默认情况下, 实体的所有额外属性存储在数据库的一个 `JSON` 对象中.

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

### MapEfCoreEntity

`MapEfCoreEntity` 一个配置 `Entity` 的快捷扩展方法.

**示例**: 设置 `IdentityRole` 实体的 `Name` 的最大长度:

````csharp
ObjectExtensionManager.Instance
    .MapEfCoreEntity<IdentityRole>(builder =>
    {
        builder.As<EntityTypeBuilder<IdentityRole>>().Property(x => x.Name).HasMaxLength(200);
    });
````

### MapEfCoreDbContext

`MapEfCoreDbContext` 一个配置 `DbContext` 的快捷扩展方法.

**示例**: 设置 `IdentityDbContext` 的 `IdentityRole` 实体的 `IdentityRole` 的最大长度:

````csharp
ObjectExtensionManager.Instance.MapEfCoreDbContext<IdentityDbContext>(b =>
{
    b.Entity<IdentityRole>().Property(x => x.Name).HasMaxLength(200);
});
````

如果相关模块已实现此功能(通过使用下面说明的 `ConfigureEfCoreEntity`)则将新属性添加到模型中. 然后你需要运行标准的 `Add-Migration` 和 `Update-Database` 命令更新数据库以添加新字段.

>`MapEfCoreProperty`, `MapEfCoreEntity` and `MapEfCoreDbContext` 方法必须在使用相关的 `DbContext` 之前调用,它是一个静态方法. 最好的方法是尽早的应用程序中使用它. 应用程序启动模板含有 `YourProjectNameEfCoreEntityExtensionMappings` 类,可以在放心的在此类中使用此方法.

### ConfigureEfCoreEntity, ApplyObjectExtensionMappings 和 TryConfigureObjectExtensions

如果你正在开发一个可重用使用的模块,并允许应用程序开发人员将属性添加到你的实体,你可以在实体映射使用 `ConfigureEfCoreEntity` 扩展方法,但是在配置实体映射时可以使用快捷的扩展方法 `ConfigureObjectExtensions`:

**示例**:
````csharp
public static class QADbContextModelCreatingExtensions
{
    public static void ConfigureQA(
        this ModelBuilder builder,
        Action<QAModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new QAModelBuilderConfigurationOptions(
            QADatabaseDbProperties.DbTablePrefix,
            QADatabaseDbProperties.DbSchema
        );

        optionsAction?.Invoke(options);

        builder.Entity<QA_Question>(b =>
        {
            b.ToTable(options.TablePrefix + "Questions", options.Schema);
            b.ConfigureByConvention();
            //...

            //Call this in the end of buildAction.
            b.ApplyObjectExtensionMappings();
        });

        //...

        //Call this in the end of ConfigureQA.
        builder.TryConfigureObjectExtensions<QADbContext>();
    }
}
````

如果你调用 `ConfigureByConvention()` 扩展方法(在此示例中 `b.ConfigureByConvention`),ABP框架内部会调用 `ConfigureObjectExtensions` 和 `ConfigureEfCoreEntity` 方法. 使用 `ConfigureByConvention` 方法是**最佳实践**,因为它还按照约定配置基本属性的数据库映射.

参阅上面提到的 "*ConfigureByConvention 方法*" 了解更多信息.

## 高级主题

### 控制多租户

如果你的方案是基于 [多租户](Multi-Tenancy.md)的, 租户可以拥有 **独立数据库**, 你在解决方案中可以拥有 **多个** `DbContext` 类, 并且其中的一些 `DbContext` 类 **只能在主机端** 可用, 这种情况下建议在 `DbContext` 类上添加 `[IgnoreMultiTenancy]` 属性. ABP 保证相关的 `DbContext` 始终使用主机 [连接字符串](Connection-Strings.md), 即使你在租户上下文中.

**示例:**

````csharp
[IgnoreMultiTenancy]
public class MyDbContext : AbpDbContext<MyDbContext>
{
    ...
}
````

不要使用 `[IgnoreMultiTenancy]` 特性如果 `DbContext` 中任何一个实体可以被持久化到多租户数据库中.

> 当你使用repositories时, ABP 已经为未实现`IMultiTenant`接口的实体使用了主机数据库. 所以, 如果你使用repositories访问数据库, 多数时候你不需要 `[IgnoreMultiTenancy]` 特性.

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

### 拆分查询

ABP 为了更好的性能, 默认全局启用 [拆分查询](https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries). 你可以按需修改.

**示例**

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer(optionsBuilder =>
    {
        optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    });
});
````

### 自定义批量操作

如果你有更好的逻辑或使用外部库实现批量操作, 你可以通过实现 `IEfCoreBulkOperationProvider` 覆写这个逻辑.

- 你可以使用下面的示例模板:

```csharp
public class MyCustomEfCoreBulkOperationProvider
    : IEfCoreBulkOperationProvider, ITransientDependency
{
    public async Task DeleteManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task InsertManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }

    public async Task UpdateManyAsync<TDbContext, TEntity>(
        IEfCoreRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken)
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        // Your logic here.
    }
}
```

## 另请参阅

* [实体](Entities.md)
* [仓储](Repositories.md)
