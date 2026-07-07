```json
//[doc-seo]
{
    "Description": "Define ABP Low-Code entities in C# with attributes and refine them with the Fluent API. This page documents the developer-facing code-first surface, including entity/property attributes, validation behavior, attachments, file/image options, enums, and fluent configuration helpers."
}
```

# Attributes & Fluent API

> **Preview:** Attributes and Fluent API configuration for the Low-Code System are preview APIs. Prefer the designer for day-to-day modeling work, and review release notes before relying on these APIs in long-lived integrations.

Use the [Low-Code Designer](designer.md) for runtime pages, forms, filters, dashboards, menus, and page groups. The code-first surface documented here is for source-controlled model metadata: entity definitions, property metadata, enums, validation, attachments, relationships, and runtime overrides.

## What This Surface Configures

Attributes and Fluent API can define or override:

* entity registration and display names
* parent-child entity structure
* default display properties for lookups
* property types, defaults, uniqueness, required state, and storage shape
* foreign keys to dynamic or reference entities
* file and image upload constraints
* entity attachments
* enum registration
* command interceptors
* cross-field validations

They do **not** define page titles, field placements, tabs, form layouts, filters, dashboards, menus, or page groups. Configure those in the designer or in [Model Descriptor Files](model-json.md).

## Quick Start

### Step 1: Define a Dynamic Enum and Entity

````csharp
[DynamicEnum]
public enum ProductStatus
{
    Draft = 0,
    Active = 1,
    Discontinued = 2
}

[DynamicEntity(DefaultDisplayPropertyName = nameof(Name))]
[DynamicEntityUI("Products")]
[DynamicEntityAttachments(
    "application/pdf",
    "image/png",
    "image/jpeg",
    MaxFileCount = 5,
    MaxFileSizeBytes = 5 * 1024 * 1024
)]
public class Product : DynamicEntityBase
{
    [Required]
    [StringLength(128)]
    [DynamicPropertyUnique]
    public string Name { get; set; } = null!;

    [Display(Name = "Unit Price")]
    [DynamicPropertyType(EntityPropertyType.Money)]
    [DynamicPropertyDefaultValue("0")]
    public decimal Price { get; set; }

    public ProductStatus Status { get; set; }

    [DynamicForeignKey("Catalog.Categories.Category", "Name", ForeignAccess.View)]
    public Guid? CategoryId { get; set; }

    [DynamicPropertyFileOptions(
        "application/pdf",
        MaxSizeBytes = 5 * 1024 * 1024
    )]
    public string? SpecSheet { get; set; }

    [DynamicPropertyImageOptions(
        "image/png",
        "image/jpeg",
        MaxWidth = 1024,
        MaxHeight = 1024,
        MaxSizeBytes = 2 * 1024 * 1024,
        ResizeMode = "fit"
    )]
    public string? HeroImage { get; set; }

    [DynamicPropertyNotMapped]
    public string? SearchSummary { get; set; }
}
````

### Step 2: Register the Assembly and Initialize the Model

````csharp
AbpDynamicEntityConfig.SourceAssemblies.Add(
    new DynamicEntityAssemblyInfo(typeof(MyDomainModule).Assembly)
);

await DynamicModelManager.Instance.InitializeAsync();
````

### Step 3: Apply Fluent Overrides When Needed

````csharp
AbpDynamicEntityConfig.EntityConfigurations.Configure<Product>(entity =>
{
    entity.WithDisplayName("Catalog Products");
    entity.EnableAttachments(
        10,
        5 * 1024 * 1024,
        20 * 1024 * 1024,
        "application/pdf",
        "image/png",
        "image/jpeg"
    );

    entity.AddOrGetProperty("DiscountedPrice")
        .AsMoney()
        .WithDefaultValue("0");

    entity.AddCrossFieldValidation(
        "DiscountedPrice",
        "Price",
        EntityCrossFieldValidationOperators.LessThanOrEqual,
        "Discounted price cannot exceed unit price."
    );

    entity.GetProperty("HeroImage").AsImage(
        2 * 1024 * 1024,
        1024,
        1024,
        "fit",
        "image/png",
        "image/jpeg"
    );
});
````

## Three-Layer Configuration System

From lowest to highest priority:

1. **Code layer**: C# classes with `[DynamicEntity]`, property attributes, and CLR validation attributes
2. **JSON descriptor layer**: source-controlled descriptor files under `_Dynamic`
3. **Fluent layer**: `AbpDynamicEntityConfig.EntityConfigurations`

A `DefaultLayer` runs last to fill in conventions such as required validation for non-nullable properties.

> When the same value is set in multiple layers, the higher-priority layer wins.

## C# Attributes Reference

### Entity-Level Attributes

| Attribute | Use it for | Key members |
|-----------|------------|-------------|
| `[DynamicEntity]` | Marks a CLR class as a low-code entity | `Parent`, `DefaultDisplayPropertyName` |
| `[DynamicEntityUI]` | Sets the entity display name used by the model | `DisplayName` |
| `[DynamicEntityAttachments]` | Enables and limits entity attachments | `IsEnabled`, `MaxFileCount`, `MaxFileSizeBytes`, `MaxTotalSizeBytes`, `AllowedContentTypes` |
| `[DynamicEntityCommandInterceptor]` | Adds `Create` / `Update` / `Delete` JavaScript interceptors | `Name`, `Type`, `Javascript` |
| `[DynamicEnum]` | Registers a CLR enum for low-code enum usage | No properties |

#### `[DynamicEntity]`

Use `[DynamicEntity]` on any class that should participate in the low-code model.

````csharp
[DynamicEntity(DefaultDisplayPropertyName = nameof(Name))]
public class Product : DynamicEntityBase
{
    public string Name { get; set; } = null!;
}
````

`DefaultDisplayPropertyName` defaults to `"Id"`. Set it when lookups and relation labels should show another field such as `Name` or `Code`.

For parent-child structures, set `Parent` by full entity name. The attribute also has a `Type` constructor overload when the parent CLR type is available:

````csharp
[DynamicEntity("Catalog.Orders.Order")]
public class OrderLine : DynamicEntityBase
{
    [DynamicForeignKey("Catalog.Products.Product", "Name")]
    public Guid ProductId { get; set; }
}
````

When `Parent` is set and the child class does not already declare `<ParentName>Id`, the code layer adds that mapped `Guid` foreign-key property automatically.

#### `[DynamicEntityUI]`

`DynamicEntityUIAttribute` only sets the entity `DisplayName`:

````csharp
[DynamicEntity]
[DynamicEntityUI("Products")]
public class Product : DynamicEntityBase
{
}
````

It does **not** configure page titles, form layouts, menu placement, dashboards, or other screen metadata.

#### `[DynamicEntityAttachments]`

Use this when the entity itself should support attachments:

````csharp
[DynamicEntity]
[DynamicEntityAttachments(
    "application/pdf",
    "image/png",
    MaxFileCount = 3,
    MaxFileSizeBytes = 5 * 1024 * 1024,
    MaxTotalSizeBytes = 15 * 1024 * 1024
)]
public class Product : DynamicEntityBase
{
}
````

Set `IsEnabled = false` to disable entity attachments explicitly in code.

#### `[DynamicEntityCommandInterceptor]`

This attribute can be applied multiple times to the same entity:

````csharp
[DynamicEntity]
[DynamicEntityCommandInterceptor(
    "Create",
    InterceptorType.Pre,
    "if (!context.commandArgs.data['Name']) { globalError = 'Name is required.'; }"
)]
[DynamicEntityCommandInterceptor(
    "Delete",
    InterceptorType.Post,
    "context.log('Deleted: ' + context.commandArgs.entityId);"
)]
public class Product : DynamicEntityBase
{
    public string Name { get; set; } = null!;
}
````

Use `Create`, `Update`, or `Delete` for `Name`, and `Pre`, `Post`, or `Replace` for `Type`. See [Interceptors](interceptors.md) for the full scripting context and replace-mode behavior.

#### `[DynamicEnum]`

Decorate CLR enums that should be visible to the low-code model:

````csharp
[DynamicEnum]
public enum ProductStatus
{
    Draft = 0,
    Active = 1,
    Discontinued = 2
}
````

Then use the enum directly on a dynamic entity property:

````csharp
[DynamicEntity]
public class Product : DynamicEntityBase
{
    public ProductStatus Status { get; set; }
}
````

Enum localization follows the usual ABP pattern:

```json
{
  "culture": "en",
  "texts": {
    "Enum:ProductStatus.Draft": "Draft",
    "Enum:ProductStatus.Active": "Active",
    "Enum:ProductStatus.Discontinued": "Discontinued"
  }
}
```

### Property-Level Attributes

| Attribute | Use it for | Key members |
|-----------|------------|-------------|
| `[DynamicPropertyUI]` | Sets the property display name | `DisplayName` |
| `[Display(Name = ...)]` | Sets the property display name with standard .NET metadata | `Name` |
| `[DynamicPropertyType]` | Overrides the inferred low-code type | `Type` |
| `[DynamicPropertyDefaultValue]` | Sets the descriptor default value | `DefaultValue` |
| `[DynamicPropertyNotMapped]` | Stores the property in the data dictionary instead of a dedicated DB field | No properties |
| `[DynamicForeignKey]` | Configures a lookup relation | `EntityName`, `DisplayPropertyName`, `Access`, `DependsOnPropertyName`, `DependsOnFilterPropertyName` |
| `[DynamicPropertySetByClients]` | Controls whether clients may write the field | `Allow` |
| `[DynamicPropertyServerOnly]` | Hides the field from clients entirely | No properties |
| `[DynamicPropertyUnique]` | Marks the field as unique | No properties |
| `[DynamicPropertyFileOptions]` | Marks the field as a file and applies upload constraints | `MaxSizeBytes`, `AllowedContentTypes` |
| `[DynamicPropertyImageOptions]` | Marks the field as an image and applies upload and resize constraints | `MaxSizeBytes`, `MaxWidth`, `MaxHeight`, `ResizeMode`, `AllowedContentTypes` |

#### Display Names: `[DynamicPropertyUI]` and `[Display]`

Both attributes write to the same `DisplayName` field in the descriptor:

````csharp
[DynamicPropertyUI(DisplayName = "SKU")]
public string Code { get; set; } = null!;

[Display(Name = "Unit Price")]
public decimal Price { get; set; }
````

If both are present on the same property, `[Display(Name = ...)]` takes precedence.

Like `DynamicEntityUI`, `DynamicPropertyUI` does **not** control list visibility, form placement, filters, quick-look order, tabs, or other runtime screen layout details. Those belong to page and form descriptors.

#### Type, Storage, Relations, and Upload Options

````csharp
public class Product : DynamicEntityBase
{
    [DynamicPropertyType(EntityPropertyType.Money)]
    [DynamicPropertyDefaultValue("0")]
    public decimal Price { get; set; }

    [DynamicForeignKey(
        "Catalog.Categories.Category",
        "Name",
        ForeignAccess.View
    )]
    public Guid? CategoryId { get; set; }

    [DynamicForeignKey(
        "Catalog.Products.Product",
        "Name",
        ForeignAccess.View,
        DependsOnPropertyName = nameof(CategoryId),
        DependsOnFilterPropertyName = "CategoryId"
    )]
    public Guid? RelatedProductId { get; set; }

    [DynamicPropertyNotMapped]
    public string? SearchSummary { get; set; }

    [DynamicPropertySetByClients(false)]
    public string? GeneratedCode { get; set; }

    [DynamicPropertyServerOnly]
    public string? InternalNotes { get; set; }

    [DynamicPropertyUnique]
    public string Code { get; set; } = null!;

    [DynamicPropertyFileOptions(
        "application/pdf",
        MaxSizeBytes = 5 * 1024 * 1024
    )]
    public string? SpecSheet { get; set; }

    [DynamicPropertyImageOptions(
        "image/png",
        "image/jpeg",
        MaxWidth = 1024,
        MaxHeight = 1024,
        MaxSizeBytes = 2 * 1024 * 1024,
        ResizeMode = "fit"
    )]
    public string? HeroImage { get; set; }
}
````

`DynamicPropertyFileOptions` sets `Type = File`. `DynamicPropertyImageOptions` sets `Type = Image`. Use them instead of setting file or image types manually when you also need upload constraints.

For image fields, `ResizeMode` currently supports these values:

* `fit`: preserves aspect ratio and scales the image down so it fits within `MaxWidth` and `MaxHeight`
* `fill`: crops from the center and scales the image to fill the target box; use it when both `MaxWidth` and `MaxHeight` are set

If `ResizeMode = "fill"` is configured without both `MaxWidth` and `MaxHeight`, the current React runtime falls back to `fit`.

Resize is currently applied client-side by the React low-code runtime form before upload. Backend upload endpoints validate file size and content type, then store the stream as-is. If you upload images through scripting or direct API/backend calls instead of the React runtime form, no automatic resize is applied on the server.

## Standard .NET Attributes and Required Inference

Code-defined low-code properties also consume standard .NET metadata:

* `[Display(Name = ...)]` sets the descriptor `DisplayName`
* any `ValidationAttribute` on the CLR property is copied into the descriptor
* `[Required]` and non-nullable types contribute to `IsRequired`

The required-state rules are:

* `[Required]` always makes the property required
* non-nullable value types such as `int`, `Guid`, `DateTime`, `decimal`, and `bool` are treated as required
* nullable value types such as `int?` and `Guid?` are optional
* nullable reference types such as `string?` are optional
* non-nullable reference types without an explicit `[Required]` stay optional for backward compatibility

For source-controlled low-code models, the built-in validation set that round-trips cleanly through the descriptor pipeline is:

* `[Required]`
* `[StringLength]`
* `[MaxLength]`
* `[MinLength]`
* `[Range]`
* `[EmailAddress]`
* `[Phone]`
* `[Url]`
* `[CreditCard]`
* `[RegularExpression]`

Example:

````csharp
public class Product : DynamicEntityBase
{
    [Required]
    [StringLength(128, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Range(0, 100000)]
    public decimal Price { get; set; }

    [EmailAddress]
    public string? ContactEmail { get; set; }
}
````

For advanced validation scenarios, JSON `validators` and cross-field validations are still the better fit than custom CLR validation attributes.

## Fluent API

The Fluent API is the highest-priority model layer. Use it when you need runtime overrides, source-controlled patches over descriptor files, or descriptor-only properties without adding CLR members.

### Registration and Initialization

Register source assemblies before initialization:

````csharp
AbpDynamicEntityConfig.SourceAssemblies.Add(
    new DynamicEntityAssemblyInfo(
        typeof(MyDomainModule).Assembly,
        rootNamespace: "Catalog",
        projectRootPath: sourcePath
    )
);

await DynamicModelManager.Instance.InitializeAsync();
````

You can also register specific types directly:

````csharp
AbpDynamicEntityConfig.DynamicEntityTypes.Add(typeof(Product));
AbpDynamicEntityConfig.DynamicEnumTypes.Add(typeof(ProductStatus));
````

Use the generic overload when a CLR type exists, or the string overload for descriptor-only entities:

````csharp
AbpDynamicEntityConfig.EntityConfigurations.Configure<Product>(entity =>
{
    entity.WithDisplayName("Products");
});

AbpDynamicEntityConfig.EntityConfigurations.Configure(
    "Catalog.Products.Product",
    entity =>
    {
        entity.WithDisplayName("Products");
    }
);
````

### `EntityDescriptor` Surface

The `Configure(...)` callback gives you an `EntityDescriptor`.

| Member | What it controls |
|--------|-------------------|
| `DisplayName` | Entity display name |
| `DefaultDisplayPropertyName` | Field used for lookups and relation labels |
| `Parent` | Parent entity name for child entities |
| `Attachments` | Raw `EntityAttachmentDescriptor` access |
| `CrossFieldValidations` | Raw cross-field validation list |
| `Interceptors` | Raw interceptor list |
| `Properties` | Full property descriptor list |
| `AddOrGetProperty(name)` | Returns an existing property or creates a new descriptor |
| `FindProperty(name)` | Returns the property or `null` |
| `GetProperty(name)` | Returns the property or throws |
| `WithDisplayName(string)` | Fluent helper for `DisplayName` |
| `EnableAttachments(...)` | Fluent helper for attachment configuration |
| `DisableAttachments()` | Explicitly disables attachments |
| `AddCrossFieldValidation(...)` | Adds an entity-level cross-field validation |

### `EntityPropertyDescriptor` Surface

`AddOrGetProperty(...)`, `FindProperty(...)`, and `GetProperty(...)` work with `EntityPropertyDescriptor`.

#### Direct members

| Member | What it controls |
|--------|-------------------|
| `DisplayName` | Property display label |
| `Type` | Low-code property type |
| `IsRequired` | Required state |
| `IsMappedToDbField` | Dedicated DB field vs data dictionary storage |
| `DefaultValue` | Default value stored in the descriptor |
| `IsUnique` | Uniqueness |
| `AllowSetByClients` | Client write access |
| `ServerOnly` | Hide from clients entirely |
| `ForeignKey` | Foreign key descriptor |
| `EnumType` | Enum CLR type name for descriptor-only enum fields |
| `FileMaxSizeBytes` | File/image max size |
| `FileAllowedContentTypes` | Allowed MIME types for file/image fields |
| `ImageMaxWidth` | Image width constraint |
| `ImageMaxHeight` | Image height constraint |
| `ImageResizeMode` | Resize strategy |
| `ValidationAttributes` | Raw validation descriptors |

#### Extension methods

| Method | What it does |
|--------|---------------|
| `MapToDbField(bool isMappedToDbField = true)` | Sets `IsMappedToDbField` |
| `AsType(EntityPropertyType type)` | Sets `Type` |
| `AsMoney()` | Sets `Type = Money` |
| `AsFile(long? maxSizeBytes = null, params string[] allowedContentTypes)` | Sets `Type = File` and file constraints |
| `AsImage(long? maxSizeBytes = null, int? maxWidth = null, int? maxHeight = null, string? resizeMode = null, params string[] allowedContentTypes)` | Sets `Type = Image` and image constraints |
| `WithDefaultValue(string? defaultValue)` | Sets `DefaultValue` |
| `WithEnumType(string enumType)` | Sets `Type = Enum` and `EnumType` |
| `WithForeignKey(string entityName, string? displayPropertyName = null, ForeignAccess access = ForeignAccess.None, string? dependsOnPropertyName = null, string? dependsOnFilterPropertyName = null)` | Configures the relation in one call |
| `AsServerOnly(bool serverOnly = true)` | Sets `ServerOnly` |
| `AsRequired(bool isRequired = true)` | Sets `IsRequired` |

### Fluent API Example

````csharp
AbpDynamicEntityConfig.EntityConfigurations.Configure(
    "Catalog.Products.Product",
    entity =>
    {
        entity.WithDisplayName("Products");
        entity.DefaultDisplayPropertyName = "Name";
        entity.EnableAttachments(
            5,
            5 * 1024 * 1024,
            20 * 1024 * 1024,
            "application/pdf",
            "image/png"
        );

        entity.AddOrGetProperty("Code")
            .AsRequired()
            .WithDefaultValue("P-");

        entity.GetProperty("Code").DisplayName = "SKU";

        entity.AddOrGetProperty("Price")
            .AsMoney()
            .AsRequired();

        entity.GetProperty("Price").DisplayName = "Unit Price";

        entity.AddOrGetProperty("Status")
            .WithEnumType("Catalog.Products.ProductStatus");

        entity.AddOrGetProperty("CategoryId")
            .WithForeignKey("Catalog.Categories.Category", "Name", ForeignAccess.View);

        entity.AddOrGetProperty("HeroImage")
            .AsImage(
                2 * 1024 * 1024,
                1024,
                1024,
                "fit",
                "image/png",
                "image/jpeg"
            );

        entity.AddOrGetProperty("InternalNotes")
            .AsServerOnly();

        entity.AddOrGetProperty("DiscountedPrice")
            .AsMoney()
            .WithDefaultValue("0");

        entity.AddCrossFieldValidation(
            "DiscountedPrice",
            "Price",
            EntityCrossFieldValidationOperators.LessThanOrEqual,
            "Discounted price cannot exceed unit price."
        );

        entity.Interceptors.Add(new CommandInterceptorDescriptor("Create")
        {
            Type = InterceptorType.Pre,
            Javascript = "if (!context.commandArgs.data['Name']) { globalError = 'Name is required.'; }"
        });
    }
);
````

### Fluent API Notes

* Use attributes when the field already exists as a CLR property.
* Use Fluent API when you need to override descriptor values without touching the CLR class.
* Use Fluent API when you need descriptor-only properties or cross-field validations.
* Use JSON descriptors or the designer for page/form/filter/dashboard/menu/page-group metadata.

## EF Core Setup

Your DbContext should implement `IDbContextWithDynamicEntities` and call `ConfigureDynamicEntities()`:

````csharp
public class CatalogDbContext : AbpDbContext<CatalogDbContext>, IDbContextWithDynamicEntities
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ConfigureDynamicEntities();
    }
}
````

Run low-code initialization before EF Core design-time migrations so the model is complete when migrations are generated.

## Layer Precedence

The final model is merged in this order:

```text
Fluent API > JSON descriptors > Code attributes and CLR metadata > Defaults
```

For example:

* if `[DynamicPropertyUnique]` marks a field as unique but a descriptor sets `"isUnique": false`, the descriptor wins
* if JSON sets a display name and Fluent API later changes `DisplayName`, the Fluent API wins

## See Also

* [Model Descriptor Files](model-json.md)
* [Code Integration](code-integration.md)
* [Reference Entities](reference-entities.md)
* [Foreign Access](foreign-access.md)
* [Interceptors](interceptors.md)
