## 实体

实体是DDD(Domain Driven Design)中核心概念.Eric Evans是这样描述实体的 "一个没有从其属性,而是通过连续性和身份的线索来定义的对象"

实体通常映射到关系型数据库的表中.

### 实体类

实体都继承自`Entity<TKey>`类,如下所示:

```C#
public class Person : Entity<int>
{
    public string Name { get; set; }

    public DateTime CreationTime { get; set; }

    public Person()
    {
        CreationTime = DateTime.Now;
    }
}
```

> 如果你不想继承基类`Entity<TKey>`,也可以直接实现`IEntity<TKey>`接口

`Entity<TKey>`类只是用给定的主 **键类型** 定义了一个`Id`属性,在上面的示例中是`int`类型.可以是其他类型如`string`, `Guid`, `long`或其他你需要的类型.

实体类还重写了 **equality** 运算符(==),以方便地检查两个实体是否相等(如果它们是相同的类型并且它们的Id相等,则它们是相等的).

#### 具有复合键的实体

有些实体可能需要 **复合键** .在这种情况下,可以从非泛型`Entity`类派生实体.如:

````C#
public class UserRole : Entity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    
    public DateTime CreationTime { get; set; }

    public Phone()
    {
            
    }
    
    public override object[] GetKeys()
    {
        return new object[] { UserId, RoleId };
    }
}
````

上面的例子中,复合键由`UserId`和`RoleId`组成.在关系数据库中,它是相关表的复合主键.

具有复合键的实体应当实现上面代码中所示的`GetKeys()`方法.

你还需要在 **object-to-relational mapping**(ORM)中配置实体的键.

> 复合主键在仓储中有限制.由于不知道Id属性,所以对于这些实体,不能使用`IRepository<TEntity, TKey>`.但是,可以使用`IRepository<TEntity>`.更多信息请参见[仓储](Repositories.md)的文档.

### 聚合根

"*聚合是域驱动设计中的一种模式.DDD的聚合是一组可以作为一个单元处理的域对象.例如,订单及订单系列的商品,这些是独立的对象,但将订单(连同订单系列的商品)视为一个聚合通常是很有用的*"( [查看详细介绍](http://martinfowler.com/bliki/DDD_Aggregate.html))

`AggregateRoot`类继承自`Entity`类,所以默认有`Id`这个属性

> 值得注意的是 ABP 会默认为聚合根创建仓储,当然,ABP也可以为所有的实体创建仓储,详情参见[仓储](Repositories.md).

ABP不强制你使用聚合根,实际上你可以使用上面定义的`Entity`类,当然,如果你想实现DDD并且创建聚合根,这里有一些最佳实践仅供参考:

* 聚合根需要维护自身的完整性,所有的实体也是这样.但是聚合根也要维护子实体的完整性.所以,聚合根必须一直有效.
* 使用Id引用聚合根,而不使用导航属性
* 聚合根被视为一个单元.它是作为一个单元检索和更新的.它通常被认为是一个交易边界.
* 不单独修改聚合根中的子实体

#### 聚合根例子

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
}
````

> 如果你不想你的聚合根继承`AggregateRoot<TKey>`类,你可以直接实现`IAggregateRoot<TKey>`接口

`Order`是一个具有`Guid`类型`Id`属性的 **聚合根**.它有一个`OrderLine`实体集合.`OrderLine`是一个具有组合键(`OrderLine`和 ` ProductId`)的实体.

虽然这个示例可能无法实现聚合根的所有最佳实践,但它仍然遵循良好的实践:

* `Order`有一个公共的构造函数,它需要 **minimal requirements** 来构造一个"订单"实例.因此,在没有`Id`和`referenceNo`的时候是无法创建订单的.**protected/private**的构造函数只有从数据库读取对象时 **反序列化** 才需要.
* `OrderLine`的构造函数是internal的,所以它只能由领域层来创建.在`Order.AddProduct`这个方法的内部被使用.
* `Order.AddProduct`实现了业务规则将商品添加到订单中
* 所有属性都有`protected`的set.这是为了防止实体在实体外部任意改变.因此,在没有向订单中添加新产品的情况下设置 `TotalItemCount`将是危险的.它的值由`AddProduct`方法维护.

ABP不强制你应用任何DDD规则或模式.但是,当你准备应用的DDD规则或模式时候,ABP会让这变的可能而且更简单.文档同样遵循这个原则.

#### 带有组合键的聚合根

虽然这种聚合根并不常见(也不建议使用),但实际上可以按照与上面提到的跟实体相同的方式定义复合键.在这种情况下,要使用非泛型的`AggregateRoot`基类.
