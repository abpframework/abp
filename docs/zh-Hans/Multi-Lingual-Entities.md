# 多语言实体

ABP框架为多语言实体定义了两个基本接口用于翻译实体的标准模型.

## IHasMultiLingual

`IHasMultiLingual<TTranslation>` 接口用于标记多语言实体. 通过 `IHasMultiLingual<TTranslation>` 接口被标记为多语言的实体定义与语言无关的信息. 多语言实体包含翻译集合,其中包含与语言有关的信息.

示例:

```csharp
public class Product : Entity<Guid>, IMultiLingualEntity<ProductTranslation>
{
    public decimal Price { get; set; }

    public ICollection<ProductTranslation> Translations { get; set; }
}
```

## IMultiLingualTranslation

`IMultiLingualTranslation` 接口用于标记多语言实体的翻译. 通过 `IHasMultiLingual<TTranslation>` 接口被标记为的翻译实体定义与语言有关的信息. 翻译实体包含 `Language` 字段,用于翻译的语言代码.

示例:

```csharp
public class ProductTranslation : Entity<Guid>, IMultiLingualTranslation
{
    public string Name { get; set; }

    public string Language { get; set; }
}
```

## 映射为DTO对象

ABP提供了[对象到对象的映射](Object-To-Object-Mapping.md)系统,你可以通过实现 `IObjectMapper<TSource, TDestination>` 接口将多语言实体映射为DTO.

示例:

```csharp
public class MultiLingualProductObjectMapper : IObjectMapper<Product, ProductDto>, ITransientDependency
{
    private readonly IMultiLingualObjectManager _multiLingualObjectManager;

    public MultiLingualProductObjectMapper(IMultiLingualObjectManager multiLingualObjectManager)
    {
        _multiLingualObjectManager = multiLingualObjectManager;
    }

    public ProductDto Map(Product source)
    {
        var translation = _multiLingualObjectManager.GetTranslation<Product, ProductDto>(source);

        return new ProductDto
        {
            Price = source.Price,
            Id = source.Id,
            Name = translation?.Name
        };
    }

    public ProductDto Map(Product source, ProductDto destination)
    {
        return default;
    }
}

```

### AutoMapper集成

ABP提供了 `CreateMultiLingualMap` 扩展方法用于将多语言实体映射为DTO.

示例:

```csharp
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        var mapResult = this.CreateMultiLingualMap<Product, ProductTranslation, ProductDto>();
    }
}
```

`CreateMultiLingualMap` 扩展方法返回了一个类型为 `CreateMultiLingualMapResult` 的对象,它包含 `EntityMap` 和 `TranslationMap` 字段. 这些字段可以用于自定义多语言映射.

示例:

```csharp
this.CreateMultiLingualMap<Order, OrderTranslation, OrderListDto>(context)
    .EntityMap.ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
```

## IMultiLingualObjectManager

`IMultiLingualObjectManager` 接口定义了 `GetTranslation` 和 `GetTranslationAsync` 方法用于获取实体当前的翻译对象.

`IMultiLingualObjectManager` 的默认实现首先使用当前的UI语言寻找翻译, 如果当前的UI语言没有对应的翻译, 那么会搜索默认语言设置(参阅[设置](Settings.md))用于寻找翻译. 如果默认语言没有对应的翻译, 那么它会返回已存在翻译集合中的第一个翻译对象.
