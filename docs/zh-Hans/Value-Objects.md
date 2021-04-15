## 值对象

> 一个对象,表示领域的描述方面,没有概念上的身份被称为 值对象.
>
> (Eric Evans)

属性相同但`Id`不同的两个[实体](https://docs.abp.io/zh-Hans/abp/latest/Entities) 被视为不同的实体.但是,值对象没有`Id`

## 值对象的类

值对象是一个抽象类,可以继承它来创建值对象类

**示例: An Address class**

```csharp
public class Address : ValueObject
{
    public Guid CityId { get; private set; }

    public string Street { get; private set; }

    public int Number { get; private set; }

    private Address()
    {
        
    }
    
    public Address(
        Guid cityId,
        string street,
        int number)
    {
        CityId = cityId;
        Street = street;
        Number = number;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Street;
        yield return CityId;
        yield return Number;
    }
}
```

- 值对象类必须实现 `GetAtomicValues()`方法来返回原始值

### ValueEquals

`ValueObject.ValueEquals(...)` 用于检测两个值是否相等

**示例: Check if two addresses are equals**

```csharp
Address address1 = ...
Address address2 = ...

if (address1.ValueEquals(address2)) //Check equality
{
    ...
}
```

## 最佳实践

以下是使用值对象时的一些最佳实践:

-  如果没有充分的理由将值对象设计为可变的,则将其设计为**不可变**（如上面的地址）.
- 构成一个值对象的属性应该形成一个概念整体.例如：CityId,Street和Number不应是个人实体的单独属性.这也使Person实体更简单.

## 另请参阅

- [实体](Entities.md)
