```json
//[doc-seo]
{
    "Description": "Use ABP multi-lingual objects to select translations by culture, apply fallback rules, and process translations in bulk."
}
```

# Multi-Lingual Objects

The `Volo.Abp.MultiLingualObject` package provides a contract and a selection service for objects that store one translation per language. Persistence mapping is application-specific; the package does not create a database relationship for the translations.

## Installation

Install the package in the project that defines the consuming module:

```bash
abp add-package Volo.Abp.MultiLingualObject
```

Add `AbpMultiLingualObjectsModule` as a dependency of that module when the package is installed manually:

````csharp
[DependsOn(typeof(AbpMultiLingualObjectsModule))]
public class MyApplicationModule : AbpModule
{
}
````

## Define a Multi-Lingual Object

Implement `IMultiLingualObject<TTranslation>` on the object and `IObjectTranslation` on its translation type:

```csharp
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
