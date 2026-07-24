```json
//[doc-seo]
{
    "Description": "Use ABP multi-lingual objects to select translations by culture, apply fallback rules, and process translations in bulk."
}
```

# Multi-Lingual Objects

> The `Volo.Abp.MultiLingualObject` package is **not published on NuGet**. It was removed from the release pipeline in [#8271](https://github.com/abpframework/abp/pull/8271) and re-designing this feature is tracked in [#11698](https://github.com/abpframework/abp/issues/11698). The source code is still maintained in the framework repository, so you can copy the [Volo.Abp.MultiLingualObjects](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.MultiLingualObjects) project into your solution if you want to use the pattern described below.

The `Volo.Abp.MultiLingualObjects` project provides a contract and a selection service for objects that store one translation per language. Persistence mapping is application-specific; it does not create a database relationship for the translations.

## Getting the Code

Copy the [Volo.Abp.MultiLingualObjects](https://github.com/abpframework/abp/tree/dev/framework/src/Volo.Abp.MultiLingualObjects) project into your solution and reference it from the project that defines the consuming module.

Add `AbpMultiLingualObjectsModule` as a dependency of that module:

````csharp
using Volo.Abp.Modularity;
using Volo.Abp.MultiLingualObjects;

[DependsOn(typeof(AbpMultiLingualObjectsModule))]
public class MyApplicationModule : AbpModule
{
}
````

## Define a Multi-Lingual Object

Implement `IMultiLingualObject<TTranslation>` on the object and `IObjectTranslation` on its translation type:

```csharp
using System.Collections.Generic;
using Volo.Abp.MultiLingualObjects;

public class Product : IMultiLingualObject<ProductTranslation>
{
    public ICollection<ProductTranslation> Translations { get; set; } =
        new List<ProductTranslation>();
}

public class ProductTranslation : IObjectTranslation
{
    public string Language { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}
```

`Language` stores a culture name such as `en`, `en-US` or `tr`.

## Select a Translation

Inject `IMultiLingualObjectManager` and call `GetTranslationAsync`:

```csharp
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiLingualObjects;

public class ProductService
    : ITransientDependency
{
    private readonly IMultiLingualObjectManager _multiLingualObjectManager;

    public ProductService(
        IMultiLingualObjectManager multiLingualObjectManager)
    {
        _multiLingualObjectManager = multiLingualObjectManager;
    }

    public Task<ProductTranslation?> GetTranslationAsync(Product product)
    {
        return _multiLingualObjectManager
            .GetTranslationAsync<Product, ProductTranslation>(product);
    }
}
```

With the default arguments, the manager uses `CultureInfo.CurrentUICulture.Name`. It selects translations in this order:

1. An exact translation for the current UI culture.
2. A translation for a parent of the current UI culture when `fallbackToParentCultures` is `true`.
3. A translation for the language configured by `LocalizationSettingNames.DefaultLanguage`.
4. The first available translation.

The method returns `null` when the collection is null or empty. To disable only the parent-culture fallback, set `fallbackToParentCultures` to `false`. The default-language and first-available fallbacks still apply.

## Select Translations in Bulk

Use `GetBulkTranslationsAsync` to select translations for multiple objects with the same culture and fallback settings:

```csharp
var results = await _multiLingualObjectManager
    .GetBulkTranslationsAsync<Product, ProductTranslation>(products);

foreach (var (product, translation) in results)
{
    // Use product and its selected translation.
}
```

Each result keeps the source object together with its selected translation. An object with no translations gets a `null` translation.
