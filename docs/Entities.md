## Entities

Entities are one of the core concepts of DDD (Domain Driven Design). Eric Evans describe it as "*An object that is not fundamentally defined by its attributes, but rather by a thread of continuity and identity*".

An entity is generally mapped to a table in a relational database.

### Entity Class

Entities are derived from `Entity<TKey>` class as shown below:

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

> If you do not want derive your entity from the base `Entity<TKey>` class, you can directly implement `IEntity<TKey>` interface.

`Entity<TKey>` class just defines an `Id` property with the given primary **key type**, which is `int` in the sample above. It can be other types like `string`, `Guid`, `long` or whatever you need.

Entity class also overrides the **equality** operator (==) to easily check if two entities are equal (they are equals if they are same entity type and their Ids are equals).

#### Entities with Composite Keys

Some entities may need to have **composite keys**. In that case, you can derive your entity from the non-generic `Entity` class. Example:

````C#
public class UserRole : Entity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    
    public DateTime CreationTime { get; set; }

    public Phone()
    {
            
    }
}
````

For the example above, the composite key is composed of `UserId` and `RoleId`. For a relational database, it is the composite primary key of the related table.

> Composite primary keys has some restriction with repositories. Since it has not known Id property, you can not use `IRepository<TEntity, TKey>` for these entities. However, you can always use `IRepository<TEntity>`. See repository documentation (TODO: link) for more.

### AggregateRoot Class

"*Aggregate is a pattern in Domain-Driven Design. A DDD aggregate is a cluster of domain objects that can be treated as a single unit. An example may be an order and its line-items, these will be separate objects, but it's useful to treat the order (together with its line items) as a single aggregate.*" (see the [full description](http://martinfowler.com/bliki/DDD_Aggregate.html))

`AggregateRoot` extends `Entity`. So, it also has an `Id` property by default. 

> Notice that ABP creates default repositories only for aggregate roots by default. However, it's possible to include all entities. See repository documentation (TODO: link) for more. 

ABP does not force you to use aggregate roots, you can only use the `Entity` class as defined before. However, if you want to implement DDD and want to create aggregate root classes, there are some best practices you can consider:

* An aggregate root is responsible to preserve it's own integrity. This is also true for all entities, but aggregate root has responsibility for it's sub entities too.
* An aggregate root can be referenced by it's Id. Do not reference it by navigation property.
* An aggregate root is treated as a single unit. It's retrieved and updated as a single unit. It's generally considered as a transaction boundary.
* Work with sub-entities over the aggregate root, do not modify them independently.

#### Aggregate Example

This is a full sample of an aggregate root with a related sub-entity collection:

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

In this example;

* `Order` is an **aggregate root entity** with `Guid` type `Id` property. It has a collection of `OrderLine` entities. `OrderLine` is another entity with a composite primary key (`OrderLine`  and ` ProductId`).
* ...

