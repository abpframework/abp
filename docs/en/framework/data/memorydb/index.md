```json
//[doc-seo]
{
    "Description": "Learn how to use ABP's in-memory database provider, register a MemoryDb context and repositories, and customize entity serialization."
}
```

# In-Memory Database Provider

The `Volo.Abp.MemoryDb` package implements ABP repositories with an in-process database. It is useful for tests and other non-durable scenarios. Data is kept in the application process and is lost when the process stops.

## Installation

Install the `Volo.Abp.MemoryDb` NuGet package in the data-access project:

````shell
abp add-package Volo.Abp.MemoryDb
````

The command adds the package and the `AbpMemoryDbModule` dependency to the module class. You can also configure the dependency manually as shown in the next section.

## Configure the Module

Add `AbpMemoryDbModule` as a dependency of your module:

````csharp
[DependsOn(typeof(AbpMemoryDbModule))]
public class MyDataModule : AbpModule
{
}
````

## Create a MemoryDb Context

Derive a class from `MemoryDbContext` and return the entity types managed by the context:

````csharp
public class MyMemoryDbContext : MemoryDbContext
{
    private static readonly Type[] EntityTypes =
    [
        typeof(Book),
        typeof(Author)
    ];

    public override IReadOnlyList<Type> GetEntityTypes()
    {
        return EntityTypes;
    }
}
````

Register the context in the `ConfigureServices` method of your module:

````csharp
context.Services.AddMemoryDbContext<MyMemoryDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

`AddDefaultRepositories()` registers default repositories for the aggregate roots returned by the context. Pass `includeAllEntities: true` when default repositories are also needed for other entity types.

MemoryDb repositories use ABP's unit-of-work-aware database provider. Repository operations require an active [unit of work](../../architecture/domain-driven-design/unit-of-work.md).

## JSON Serialization

MemoryDb stores serialized entity values. Configure `Utf8JsonMemoryDbSerializerOptions` to customize the underlying `System.Text.Json` options:

````csharp
Configure<Utf8JsonMemoryDbSerializerOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new MyEntityJsonConverter()
    );
});
````
