# 实体

实体是DDD(Domain Driven Design)中核心概念.Eric Evans是这样描述实体的 "一个没有从其属性,而是通过连续性和身份的线索来定义的对象"

实体通常映射到关系型数据库的表中.

## 实体类

实体都继承自`Entity<TKey>`类,如下所示:

```C#
public class Book : Entity<Guid>
{
    public string Name { get; set; }

    public float Price { get; set; }
}
```

> 如果你不想继承基类`Entity<TKey>`,也可以直接实现`IEntity<TKey>`接口

`Entity<TKey>`类只是用给定的主 **键类型** 定义了一个`Id`属性,在上面的示例中是`Guid`类型.可以是其他类型如`string`, `int`, `long`或其他你需要的类型.

### Guid主键的实体

如果你的实体Id类型为 `Guid`,有一些好的实践可以实现:

* 创建一个构造函数,获取ID作为参数传递给基类.
  * 如果没有为GUID Id赋值,**ABP框架会在保存时设置它**,但是在将实体保存到数据库之前最好在实体上有一个有效的Id.
* 如果使用带参数的构造函数创建实体,那么还要创建一个 `private` 或 `protected`  构造函数. 当数据库提供程序从数据库读取你的实体时(反序列化时)将使用它.
* 不要使用 `Guid.NewGuid()` 来设置Id! 在创建实体的代码中**使用[`IGuidGenerator`服务](Guid-Generation.md)**传递Id参数. `IGuidGenerator`经过优化可以产生连续的GUID.这对于关系数据库中的聚集索引非常重要.

示例实体:

````csharp
public class Book : Entity<Guid>
{
    public string Name { get; set; }
    public float Price { get; set; }
    protected Book()
    {
    }
    public Book(Guid id)
     : base(id)
    {
    }
}
````

在[应用服务](Application-Services.md)中使用示例:

````csharp
public class BookAppService : ApplicationService, IBookAppService
{
    private readonly IRepository<Book> _bookRepository;
    public BookAppService(IRepository<Book> bookRepository)
    {
        _bookRepository = bookRepository;
    }
    public async Task CreateAsync(CreateBookDto input)
    {
        await _bookRepository.InsertAsync(
            new Book(GuidGenerator.Create())
            {
                Name = input.Name,
                Price = input.Price
            }
        );
    }
}
````

* `BookAppService` 注入图书实体的默认[仓库](Repositories.md),使用`InsertAsync`方法插入 `Book` 到数据库中.
* `GuidGenerator`类型是 `IGuidGenerator`,它是在`ApplicationService`基类中定义的属性. ABP将这样常用属性预注入,所以不需要手动[注入](Dependency-Injection.md).
* 如果你想遵循DDD最佳实践,请参阅下面的*聚合示例*部分.

### 具有复合键的实体

有些实体可能需要 **复合键** .在这种情况下,可以从非泛型`Entity`类派生实体.如:

````C#
public class UserRole : Entity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    
    public DateTime CreationTime { get; set; }

    public UserRole()
    {
            
    }
    
    public override object[] GetKeys()
    {
        return new object[] { UserId, RoleId };
    }
}
````

上面的例子中,复合键由`UserId`和`RoleId`组成.在关系数据库中,它是相关表的复合主键. 具有复合键的实体应当实现上面代码中所示的`GetKeys()`方法.

> 你还需要在**对象关系映射**(ORM)中配置实体的键. 参阅[Entity Framework Core](Entity-Framework-Core.md)集成文档查看示例.

> 需要注意,复合主键实体不可以使用 `IRepository<TEntity, TKey>` 接口,因为它需要一个唯一的Id属性. 但你可以使用 `IRepository<TEntity>`.更多信息请参见[仓储](Repositories.md)的文档.

## 聚合根

"*聚合是域驱动设计中的一种模式.DDD的聚合是一组可以作为一个单元处理的域对象.例如,订单及订单系列的商品,这些是独立的对象,但将订单(连同订单系列的商品)视为一个聚合通常是很有用的*"( [查看详细介绍](http://martinfowler.com/bliki/DDD_Aggregate.html))

`AggregateRoot<TKey>`类继承自`Entity<TKey>`类,所以默认有`Id`这个属性

> 值得注意的是 ABP 会默认为聚合根创建仓储,当然,ABP也可以为所有的实体创建仓储,详情参见[仓储](Repositories.md).

ABP不强制你使用聚合根,实际上你可以使用上面定义的`Entity`类,当然,如果你想实现[领域驱动设计](Domain-Driven-Design.md)并且创建聚合根,这里有一些最佳实践仅供参考:

* 聚合根需要维护自身的完整性,所有的实体也是这样.但是聚合根也要维护子实体的完整性.所以,聚合根必须一直有效.
* 使用Id引用聚合根,而不使用导航属性
* 聚合根被视为一个单元.它是作为一个单元检索和更新的.它通常被认为是一个交易边界.
* 不单独修改聚合根中的子实体

如果你想在应用程序中实现DDD,请参阅[实体设计最佳实践指南](Best-Practices/Entities.md).

### 聚合根例子

这是一个具有子实体集合的聚合根例子:

````C#
public class Order : AggregateRoot<Guid>
{
    public virtual string ReferenceNo { get; protected set; }

    public virtual int TotalItemCount { get; protected set; }

    public virtual DateTime CreationTime { get; protected set; }

    public virtual List<OrderLine> OrderLines { get; protected set; }

    protected Order()
    {

    }

    public Order(Guid id, string referenceNo)
    {
        Check.NotNull(referenceNo, nameof(referenceNo));
        
        Id = id;
        ReferenceNo = referenceNo;
        
        OrderLines = new List<OrderLine>();
    }

    public void AddProduct(Guid productId, int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException(
                "You can not add zero or negative count of products!",
                nameof(count)
            );
        }

        var existingLine = OrderLines.FirstOrDefault(ol => ol.ProductId == productId);

        if (existingLine == null)
        {
            OrderLines.Add(new OrderLine(this.Id, productId, count));
        }
        else
        {
            existingLine.ChangeCount(existingLine.Count + count);
        }

        TotalItemCount += count;
    }
}

public class OrderLine : Entity
{
    public virtual Guid OrderId { get; protected set; }

    public virtual Guid ProductId { get; protected set; }

    public virtual int Count { get; protected set; }

    protected OrderLine()
    {

    }

    internal OrderLine(Guid orderId, Guid productId, int count)
    {
        OrderId = orderId;
        ProductId = productId;
        Count = count;
    }

    internal void ChangeCount(int newCount)
    {
        Count = newCount;
    }

    public override object[] GetKeys()
    {
        return new Object[] {OrderId, ProductId};
    }
}
````

> 如果你不想你的聚合根继承`AggregateRoot<TKey>`类,你可以直接实现`IAggregateRoot<TKey>`接口

`Order`是一个具有`Guid`类型`Id`属性的 **聚合根**.它有一个`OrderLine`实体集合.`OrderLine`是一个具有组合键(`OrderId`和 ` ProductId`)的实体.

虽然这个示例可能无法实现聚合根的所有最佳实践,但它仍然遵循良好的实践:

* `Order`有一个公共的构造函数,它需要 **minimal requirements** 来构造一个"订单"实例.因此,在没有`Id`和`referenceNo`的时候是无法创建订单的.**protected/private**的构造函数只有从数据库读取对象时 **反序列化** 才需要.
* `OrderLine`的构造函数是internal的,所以它只能由领域层来创建.在`Order.AddProduct`这个方法的内部被使用.
* `Order.AddProduct`实现了业务规则将商品添加到订单中
* 所有属性都有`protected`的set.这是为了防止实体在实体外部任意改变.因此,在没有向订单中添加新产品的情况下设置 `TotalItemCount`将是危险的.它的值由`AddProduct`方法维护.

ABP框架不强制你应用任何DDD规则或模式.但是,当你准备应用的DDD规则或模式时候,ABP会让这变的可能而且更简单.文档同样遵循这个原则.

### 带有组合键的聚合根

虽然这种聚合根并不常见(也不建议使用),但实际上可以按照与上面提到的跟实体相同的方式定义复合键.在这种情况下,要使用非泛型的`AggregateRoot`基类.

### BasicAggregateRoot类

`AggregateRoot` 类实现了 `IHasExtraProperties` 和 `IHasConcurrencyStamp` 接口,这为派生类带来了两个属性. `IHasExtraProperties` 使实体可扩展(请参见下面的 *额外的属性*部分) 和 `IHasConcurrencyStamp` 添加了由ABP框架管理的 `ConcurrencyStamp` 属性实现[乐观并发](https://docs.microsoft.com/zh-cn/ef/core/saving/concurrency). 在大多数情况下,这些是聚合根需要的功能.

但是,如果你不需要这些功能,你的聚合根可以继承 `BasicAggregateRoot<TKey>`(或`BasicAggregateRoot`).

## 基类和接口的审计属性

有一些属性,像`CreationTime`,`CreatorId`,`LastModificationTime`...在所有应用中都很常见. ABP框架提供了一些接口和基类来**标准化**这些属性,并**自动设置它们的值**.

### 审计接口

有很多的审计接口,你可以实现一个你需要的那个.

> 虽然可以手动实现这些接口,但是可以使用下一节中定义的**基类**简化代码.

* `IHasCreationTime` 定义了以下属性:
  * `CreationTime`
* `IMayHaveCreator` 定义了以下属性:
  * `CreatorId`
* `ICreationAuditedObject` 继承 `IHasCreationTime` 和 `IMayHaveCreator`, 所以它定义了以下属性:
  * `CreationTime`
  * `CreatorId`
* `IHasModificationTime` 定义了以下属性:
  * `LastModificationTime`
* `IModificationAuditedObject` 扩展 `IHasModificationTime` 并添加了 `LastModifierId` 属性. 所以它定义了以下属性:
  * `LastModificationTime`
  * `LastModifierId`
* `IAuditedObject` 扩展 `ICreationAuditedObject` 和 `IModificationAuditedObject`, 所以它定义了以下属性:
  * `CreationTime`
  * `CreatorId`
  * `LastModificationTime`
  * `LastModifierId`
* `ISoftDelete` (参阅 [数据过滤文档](Data-Filtering.md)) 定义了以下属性:
  * `IsDeleted`
* `IHasDeletionTime` 扩展 `ISoftDelete` 并添加了 `DeletionTime` 属性. 所以它定义了以下属性:
  * `IsDeleted`
  * `DeletionTime` 
* `IDeletionAuditedObject` 扩展 `IHasDeletionTime`  并添加了 `DeleterId` 属性. 所以它定义了以下属性:
  * `IsDeleted`
  * `DeletionTime` 
  * `DeleterId`
* `IFullAuditedObject` 继承 `IAuditedObject` 和 `IDeletionAuditedObject`, 所以它定义了以下属性:
  * `CreationTime`
  * `CreatorId`
  * `LastModificationTime`
  * `LastModifierId`
  * `IsDeleted`
  * `DeletionTime`
  * `DeleterId`

当你实现了任意接口,或者从下一节定义的类派生,ABP框架就会尽可能地自动管理这些属性.

> 实现 `ISoftDelete` , `IDeletionAuditedObject` 或 `IFullAuditedObject` 让你的实体**软删除**. 参阅[数据过滤文档](Data-Filtering.md),了解软删除模式.

### 审计基类

虽然可以手动实现以上定义的任何接口,但建议从这里定义的基类继承:

* `CreationAuditedEntity<TKey>` 和 `CreationAuditedAggregateRoot<TKey>` 实现了 `ICreationAuditedObject` 接口.
* `AuditedEntity<TKey>` 和 `AuditedAggregateRoot<TKey>` 实现了 `IAuditedObject` 接口.
* `FullAuditedEntity<TKey>` and `FullAuditedAggregateRoot<TKey>` 实现了 `IFullAuditedObject` 接口.

所有这些基类都有非泛型版本,可以使用 `AuditedEntity` 和 `FullAuditedAggregateRoot` 来支持复合主键;

所有这些基类也有 `... WithUser`,像 `FullAuditedAggregateRootWithUser<TUser>` 和 `FullAuditedAggregateRootWithUser<TKey, TUser>`. 这样就可以将导航属性添加到你的用户实体. 但在聚合根之间添加导航属性不是一个好做法,所以这种用法是不建议的(除非你使用EF Core之类的ORM可以很好地支持这种情况,并且你真的需要它. 请记住这种方法不适用于NoSQL数据库(如MongoDB),你必须真正实现聚合模式）.

## 额外的属性

ABP定义了 `IHasExtraProperties` 接口,可以由实体实现,以便能够动态地设置和获取的实体属性. `AggregateRoot` 基类已经实现了 `IHasExtraProperties` 接口. 如果你从这个类(或者上面定义的一个相关审计类)派生,那么你可以直接使用API​.

### GetProperty 和 SetProperty 扩展方法

这些扩展方法是获取和设置实体数据的推荐方法. 例:

````csharp
public class ExtraPropertiesDemoService : ITransientDependency
{
    private readonly IIdentityUserRepository _identityUserRepository;

    public ExtraPropertiesDemoService(IIdentityUserRepository identityUserRepository)
    {
        _identityUserRepository = identityUserRepository;
    }

    public async Task SetTitle(Guid userId, string title)
    {
        var user = await _identityUserRepository.GetAsync(userId);

        //SET A PROPERTY
        user.SetProperty("Title", title);

        await _identityUserRepository.UpdateAsync(user);
    }

    public async Task<string> GetTitle(Guid userId)
    {
        var user = await _identityUserRepository.GetAsync(userId);

        //GET A PROPERTY
        return user.GetProperty<string>("Title");
    }
}
````

* 属性的**值是object**,可以是任何类型的对象(string,int,bool...等).
* 如果给定的属性未设置值, `GetProperty` 方法会返回 `null`.
* 你可以使用不同的**属性名称**(如这里的`Title`)同时存储多个属性.

最好为属性名**定义一个常量**防止拼写错误. 最佳方式是定义**扩展方法**来利用智能感知. 例:

````csharp
public static class IdentityUserExtensions
{
    private const string TitlePropertyName = "Title";

    public static void SetTitle(this IdentityUser user, string title)
    {
        user.SetProperty(TitlePropertyName, title);
    }

    public static string GetTitle(this IdentityUser user)
    {
        return user.GetProperty<string>(TitlePropertyName);
    }
}
````

然后你可以直接使用 `IdentityUser` 对象的 `user.SetTitle("...")` 和 `user.GetTitle()`.

### HasProperty 和 RemoveProperty 扩展方法

* `HasProperty` 用于检查对象是否设置了属性.
* `RemoveProperty` 用于从对象中删除属性. 你可以使用它来替代设置 `null` 值.

### 它是如何实现的?

`IHasExtraProperties` 要求实现类定义一个名称为 `ExtraProperties` 的`Dictionary<string, object>` 属性.

所以,如果你需要你可以直接使用 `ExtraProperties` 属性来使用字典API,但是推荐使用 `SetProperty` 和 `GetProperty` 方法,因为它们会检查 `null` 值.

#### 它是如何存储的?

存储字典的方式取决于你使用的数据库提供程序.

* 对于 [Entity Framework Core](Entity-Framework-Core.md),这是两种类型的配置;
    * 默认它以 `JSON` 字符串形式存储在 `ExtraProperties` 字段中. 序列化到 `JSON` 和反序列化到 `JSON` 由ABP使用EF Core的[值转换](https://docs.microsoft.com/zh-cn/ef/core/modeling/value-conversions)系统自动完成.
    * 如果需要,你可以使用 `ObjectExtensionManager` 为所需的额外属性定义一个单独的数据库字段. 那些使用 `ObjectExtensionManager` 配置的属性继续使用单个 `JSON` 字段. 当你使用预构建的[应用模块](Modules/Index.md)并且想要[扩展模块的实体](Customizing-Application-Modules-Extending-Entities.md). 参阅[EF Core迁移文档](Entity-Framework-Core.md)了解如何使用 `ObjectExtensionManager`.
* 对于 [MongoDB](MongoDB.md), 它以 **常规字段** 存储, 因为 MongoDB 天生支持这种 [额外](https://mongodb.github.io/mongo-csharp-driver/1.11/serialization/#supporting-extra-elements) 系统.

### 讨论额外的属性

如果你使用**可重复使用的模块**,其中定义了一个实体,你想使用简单的方式get/set此实体相关的一些数据,那么额外的属性系统是非常有用的. 
你通常 **不需要** 为自己的实体使用这个系统,是因为它有以下缺点:

* 它不是**完全类型安全的**,因为它使用字符串用作属性名称.
* 这些属性**不容易[自动映射](Object-To-Object-Mapping.md)到其他对象**.
* 它**不会**为EF Core在数据库表中**创建字段**,因此在数据库中针对这个字段创建索引或搜索/排序并不容易.

### 额外属性背后的实体

`IHasExtraProperties` 不限于与实体一起使用. 你可以为任何类型的类实现这个接口,使用 `GetProperty`,`SetProperty` 和其他相关方法.

## 另请参阅

* [实体设计最佳实践指南](Best-Practices/Entities.md)
