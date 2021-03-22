# Multi Lingual Entities

ABP Framework defines two basic interfaces for Multi-Lingual entity definitions to provide a standard model for translating entities.

## IHasMultiLingual

`IHasMultiLingual<TTranslation>` interface is used to mark multi lingual entities. The entities marked with `IHasMultiLingual<TTranslation>` interface must define language-neutral information. The entities marked with `IHasMultiLingual<TTranslation>` contains a collection of Translations which contains language-dependent information.

Example:

```csharp
public class Product : Entity, IMultiLingualEntity<ProductTranslation>
{
    public decimal Price { get; set; }

    public ICollection<ProductTranslation> Translations { get; set; }
}
```

## IMultiLingualTranslation

`IMultiLingualTranslation` interface is used to mark translation of a Multi-Lingual entity. The entities marked with `IMultiLingualTranslation` interface must define language dependent information. The entities marked with `IMultiLingualTranslation` contains Language field which contains a language code for the translation.

Example:

```csharp
public class ProductTranslation : Entity, IMultiLingualTranslation
{
    public string Name { get; set; }

    public string Language { get; set; }
}
```

## Map to DTO object

ABP provdies the [Object To Object Mapping](Object-To-Object-Mapping.md) system, you can implement the `IObjectMapper<TSource, TDestination>` interface to map multi lingual entities to DTOs.

Example:

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

### AutoMapper integration

ABP provides the `CreateMultiLingualMap` extension method for mapping multilingual entities to DTOs.

Example:

```csharp
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        var mapResult = this.CreateMultiLingualMap<Product, ProductTranslation, ProductDto>();
    }
}
```

`CreateMultiLingualMap` extension method returns an object of type `CreateMultiLingualMapResult` which contains `EntityMap` and `TranslationMap` fields. These fields can be used to customize multi lingual mapping.

Example:

```csharp
this.CreateMultiLingualMap<Order, OrderTranslation, OrderListDto>(context)
    .EntityMap.ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));
```

## IMultiLingualObjectManager

`IMultiLingualObjectManager` interface defines `GetTranslation` and `GetTranslationAsync` method to get the translation object of the entity.

The default implementation of the `IMultiLingualObjectManager` interface finds the translation with selected UI language first. If there is no translation with selected UI language, then extension method searches for the default language setting (see [Setting](Settings.md)) and uses the translation in default language. If extension method couldn't find any translation in current UI language or default language, it uses one of the existing translations.