```json
//[doc-seo]
{
    "Description": "Connect ABP Low-Code entities with regular C# code. Register reference entities for low-code lookups and access low-code entities from application code via typed repositories, DynamicEntity, or IDynamicPageAppService."
}
```

# Code Integration

> **Preview:** Low-Code integration APIs are part of the preview Low-Code System. Registration details, repository behavior, and page service contracts may change before general availability.

Use the [Low-Code Designer](designer.md) and [React Runtime](react-runtime.md) for standard CRUD screens. This page covers the integration points for the cases where low-code models and regular application code need to reach each other directly.

## Choose the Right Integration Path

| Need | Recommended approach |
|------|----------------------|
| A low-code field should point to an existing C# entity such as `IdentityUser` | Register a [reference entity](reference-entities.md) and use its CLR full name in the foreign key definition |
| Application code should work with a low-code entity that has a real C# class | Inject `IRepository<TEntity, Guid>` for that `[DynamicEntity]` class |
| Application code should work with a descriptor-only or runtime-defined low-code entity | Inject `IRepository<DynamicEntity, Guid>` and call `SetEntityName(...)` |
| Application code should reuse runtime page behavior such as low-code permissions, foreign-access checks, files, attachments, or child-page rules | Inject `IDynamicPageAppService` and work by `pageName` |

## Low-Code to Existing C# Entities

Low-code entities can point to existing entities that are not managed by the Low-Code System itself. Register them once during startup:

````csharp
AbpDynamicEntityConfig.ReferencedEntityList.Add<IdentityUser>(
    "UserName",
    "UserName",
    "Email"
);
````

Then use the registered CLR type name in a foreign key:

````csharp
[DynamicForeignKey("Volo.Abp.Identity.IdentityUser", "UserName")]
public Guid? OwnerId { get; set; }
````

or in a descriptor file:

```json
{
  "name": "OwnerId",
  "foreignKey": {
    "entityName": "Volo.Abp.Identity.IdentityUser"
  }
}
```

Scripts can also query the same reference entity:

```javascript
var user = await db.get('Volo.Abp.Identity.IdentityUser', ownerId);
```

Reference entities are read-only from the low-code side. For the full registration model and metadata details, see [Reference Entities](reference-entities.md).

## C# Code to Low-Code Entities

### Option 1: Typed Repository for Code-Defined Dynamic Entities

If the low-code entity is declared as a normal C# class with `[DynamicEntity]` and `DynamicEntityBase`, use it like any other ABP entity. The low-code demo's `Customer` entity is a typical example:

````csharp
public class CustomerSyncService : ITransientDependency
{
    private readonly IRepository<Customer, Guid> _customerRepository;

    public CustomerSyncService(IRepository<Customer, Guid> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task UpdateAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(id);

        customer.SetData("Telephone", "5550001122");
        customer.SetData("CreditLimit", 119.99m);

        await _customerRepository.UpdateAsync(customer);
    }
}
````

This is the simplest path when a CLR type already exists. `GetData(...)` and `SetData(...)` still work for dynamic or extra properties on that typed entity.

### Option 2: Generic Repository for Descriptor-Only or Runtime-Defined Entities

If the low-code entity only exists in descriptor files or runtime metadata, inject `IRepository<DynamicEntity, Guid>`. Set the entity name before repository operations:

````csharp
public class ProductImportService : ITransientDependency
{
    private readonly IRepository<DynamicEntity, Guid> _dynamicEntityRepository;

    public ProductImportService(IRepository<DynamicEntity, Guid> dynamicEntityRepository)
    {
        _dynamicEntityRepository = dynamicEntityRepository;
    }

    public async Task<Guid> CreateAsync()
    {
        _dynamicEntityRepository.SetEntityName("LowCodeDemo.Products.Product");

        var product = new DynamicEntity("LowCodeDemo.Products.Product", Guid.NewGuid())
        {
            ["Name"] = "Road Helmet",
            Data =
            {
                ["Price"] = 125m,
                ["StockCount"] = 40
            }
        };

        await _dynamicEntityRepository.InsertAsync(product);
        return product.Id;
    }

    public async Task<decimal> GetPriceAsync(Guid id)
    {
        _dynamicEntityRepository.SetEntityName("LowCodeDemo.Products.Product");

        var product = await _dynamicEntityRepository.GetAsync(id);
        return product.GetData<decimal>("Price");
    }
}
````

Call `SetEntityName(...)` again whenever you switch to another low-code entity name. After that, standard repository operations such as `GetAsync`, `GetListAsync`, `InsertAsync`, `UpdateAsync`, `DeleteAsync`, and `GetQueryableAsync` work normally.

Use `GetData(...)` and `SetData(...)` for normal read/write code. Use the indexers only when you need to target the underlying storage explicitly, such as query expressions or manual `DynamicEntity` construction.

| API | Use it for |
|-----|------------|
| `entity.GetData<T>("FieldName")` | General read API; it resolves CLR properties, mapped fields, and extra properties through one call |
| `entity.SetData("FieldName", value)` | General write API; it chooses the mapped field or `Data` dictionary automatically |
| `entity["FieldName"]` | Direct access to a dynamic property with `isMappedToDbField: true` |
| `entity.Data["FieldName"]` | Direct access to a dynamic property without `isMappedToDbField: true` |

### Query Expressions: `entity["Field"]` vs `entity.Data["Field"]`

For materialized entities, `GetData(...)` is the safest read API because it hides the storage details. For `IQueryable` expressions, use the access form that matches the property's storage shape so EF Core and the low-code query layer can translate it correctly:

| Property shape | Query form |
|----------------|------------|
| Normal CLR property | `x.PropertyName` |
| Dynamic property with `isMappedToDbField: true` | `x["PropertyName"]` |
| Dynamic property without `isMappedToDbField: true` | `x.Data["PropertyName"]` |

The low-code demo uses both patterns in the same query. In `LowCodeDemo.Orders.OrderLine`, `ProductId` is mapped to a DB field, while `Amount` stays in the `Data` dictionary:

````csharp
var orderLineQuery = await _dynamicEntityRepository
    .SetEntityName("LowCodeDemo.Orders.OrderLine")
    .GetQueryableAsync();

var filtered = orderLineQuery
    .Where(line => line.Data["Amount"] != null && (decimal?)line.Data["Amount"] > 1000m)
    .Select(line => new
    {
        ProductId = (Guid?)line["ProductId"],
        Amount = (decimal?)line.Data["Amount"]
    });
````

The same rule applies to joins. `ProductId` is joined through the entity indexer because it is mapped, while `CustomerId` on `LowCodeDemo.Orders.Order` is joined through `Data` because it is not:

````csharp
var orderLineQuery = await _dynamicEntityRepository
    .SetEntityName("LowCodeDemo.Orders.OrderLine")
    .GetQueryableAsync();

var orderQuery = await _dynamicEntityRepository
    .SetEntityName("LowCodeDemo.Orders.Order")
    .GetQueryableAsync();

var query =
    from orderLine in orderLineQuery
    join order in orderQuery
        on (Guid?)orderLine["OrderId"] equals order.Id
    select new
    {
        OrderId = order.Id,
        CustomerId = (Guid?)order.Data["CustomerId"],
        Amount = (decimal?)orderLine.Data["Amount"]
    };
````

Use `GetData(...)` again after the query has been materialized into entity objects.

### Option 3: `IDynamicPageAppService` for Runtime-Like Behavior

Use `IDynamicPageAppService` when custom code should go through the same page-level behavior as the generated runtime: low-code permissions, foreign-access validation, lookup rules, file handling, attachments, export, and child records.

This API works with `pageName`, not `entityName`:

````csharp
public class ProductRuntimeService : ITransientDependency
{
    private readonly IDynamicPageAppService _dynamicPageAppService;

    public ProductRuntimeService(IDynamicPageAppService dynamicPageAppService)
    {
        _dynamicPageAppService = dynamicPageAppService;
    }

    public async Task<DynamicEntityDto> CreateAsync()
    {
        return await _dynamicPageAppService.CreateAsync(
            "products",
            new DynamicEntityCreateInput
            {
                Properties = new Dictionary<string, string?>
                {
                    ["Name"] = "Road Helmet",
                    ["Price"] = "125",
                    ["StockCount"] = "40"
                }
            }
        );
    }

    public async Task<PagedResultDto<DynamicEntityDto>> GetListAsync()
    {
        return await _dynamicPageAppService.GetDataAsync(
            "products",
            new DynamicEntityListRequestDto
            {
                MaxResultCount = 20
            }
        );
    }
}
````

Reach for this service when your custom code should behave like the runtime page rather than like a raw repository call.

## Joining Low-Code and Regular Entities in LINQ

Low-code entities can be composed with typed repositories in the same LINQ query. The low-code demo does this by joining dynamic order lines and orders with the typed `Customer` repository:

````csharp
var orderLineQuery = await _dynamicEntityRepository
    .SetEntityName("LowCodeDemo.Orders.OrderLine")
    .GetQueryableAsync();

var orderQuery = (await _dynamicEntityRepository
    .SetEntityName("LowCodeDemo.Orders.Order")
    .GetQueryableAsync())
    .As<IQueryable<DynamicEntity>>();

var customerQuery = (await _customerRepository.GetQueryableAsync())
    .As<IQueryable<IEntity<Guid>>>();

var query = orderLineQuery
    .Where(line => line.Data["Amount"] != null && (decimal?)line.Data["Amount"] > 1000m)
    .GroupJoin(
        orderQuery,
        orderLine => (Guid?)orderLine["OrderId"],
        order => order.Id,
        (orderLine, orders) => new { orderLine, orders }
    )
    .SelectMany(
        x => x.orders.DefaultIfEmpty(),
        (x, order) => new { x.orderLine, order }
    )
    .GroupJoin(
        customerQuery,
        x => (Guid?)x.order!.Data["CustomerId"],
        customer => customer.Id,
        (x, customers) => new { x.orderLine, x.order, customers }
    );
````

This is useful when a business rule or reporting service needs both low-code entities and typed C# entities in one application-layer query.

## See Also

* [Reference Entities](reference-entities.md)
* [Attributes & Fluent API](fluent-api.md)
* [Model Descriptor Files](model-json.md)
* [React Runtime](react-runtime.md)
* [Scripting API](scripting-api.md)
