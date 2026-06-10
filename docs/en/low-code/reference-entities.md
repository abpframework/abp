```json
//[doc-seo]
{
    "Description": "Link dynamic entities to existing C# entities like IdentityUser using Reference Entities in the ABP Low-Code System."
}
```

# Reference Entities

Reference Entities allow you to create foreign key relationships from **dynamic entities** to **existing C# entities** that live outside the Low-Code System.

## Dynamic Entities vs Reference Entities

| | Dynamic Entities | Reference Entities |
|---|-----------------|-------------------|
| **Definition** | Defined via `[DynamicEntity]` attribute or `model.json` | Existing C# classes (e.g., `IdentityUser`, `Tenant`) |
| **CRUD Operations** | Full CRUD (Create, Read, Update, Delete) | **Read-only** — no create/update/delete |
| **UI Pages** | Auto-generated pages with data grids and forms | No UI pages |
| **Permissions** | Auto-generated permissions | No permissions |
| **Purpose** | Primary data management | Foreign key lookups and display values |
| **Registration** | `AbpDynamicEntityConfig.SourceAssemblies` | `AbpDynamicEntityConfig.ReferencedEntityList` |

## Overview

Dynamic entities defined via [Attributes](fluent-api.md) or [model.json](model-json.md) can reference **other dynamic entities** using foreign keys. However, you may also need to link to entities that exist **outside** the Low-Code System — such as ABP's `IdentityUser`, `Tenant`, or your own C# entity classes.

**Reference entities** make this possible by exposing existing entities for:

* **Foreign key lookups** — dropdown selection in UI forms
* **Display values** — showing the entity's display property in grids instead of raw GUIDs
* **Read-only queries** — querying via the [Scripting API](scripting-api.md)

> **Key distinction:** When you define a foreign key with `entityName`, the system checks if it's a registered **reference entity** first. If not found, it assumes it's a **dynamic entity**.

## Registering Reference Entities

Register reference entities in your [Low-Code Initializer](index.md#1-create-a-low-code-initializer) using `AbpDynamicEntityConfig.ReferencedEntityList`:

````csharp
public static async Task InitializeAsync()
{
    await Runner.RunAsync(async () =>
    {
        // Register reference entity with default display property only
        AbpDynamicEntityConfig.ReferencedEntityList.Add<IdentityUser>(
            "UserName"
        );

        // Register reference entity with additional exposed properties
        AbpDynamicEntityConfig.ReferencedEntityList.Add<IdentityUser>(
            "UserName",       // Default display property
            "UserName",       // Exposed properties (for queries and display)
            "Email",
            "PhoneNumber"
        );
        
        // ... rest of initialization
        await DynamicModelManager.Instance.InitializeAsync();
    });
}
````

### `Add<TEntity>` Method

````csharp
public void Add<TEntity>(
    string defaultDisplayProperty,
    params string[] properties
) where TEntity : class, IEntity<Guid>
````

| Parameter | Description |
|-----------|-------------|
| `defaultDisplayProperty` | Property name used as display value in lookups |
| `properties` | Additional properties to expose (optional) |

> The entity type must implement `IEntity<Guid>`.

## Using Reference Entities in model.json

Reference a registered entity in a foreign key definition:

```json
{
  "name": "UserId",
  "foreignKey": {
    "entityName": "Volo.Abp.Identity.IdentityUser"
  }
}
```

The entity name must match the CLR type's full name. The module automatically detects that this is a reference entity and uses the registered `ReferenceEntityDescriptor`.

## Using Reference Entities with Attributes

Use the `[DynamicForeignKey]` attribute on a Guid property:

````csharp
[DynamicEntity]
public class Customer
{
    [DynamicForeignKey("Volo.Abp.Identity.IdentityUser", "UserName")]
    public Guid? UserId { get; set; }
}
````

## How It Works

The `ReferenceEntityDescriptor` class stores metadata about the reference entity:

* `Name` — Full CLR type name
* `Type` — The actual CLR type
* `DefaultDisplayPropertyName` — Display property for lookups
* `Properties` — List of `ReferenceEntityPropertyDescriptor` entries

When a foreign key points to a reference entity, the `ForeignKeyDescriptor` populates its `ReferencedEntityDescriptor` and `ReferencedDisplayPropertyDescriptor` instead of the standard `EntityDescriptor` fields.

## Querying Reference Entities in Scripts

Reference entities can be queried via the [Scripting API](scripting-api.md):

```javascript
// Query reference entity in interceptor or custom endpoint
var user = await db.get('Volo.Abp.Identity.IdentityUser', userId);
if (user) {
    context.log('User: ' + user.UserName);
}
```

## Limitations

* **Read-only**: Reference entities do not get CRUD operations, permissions, or UI pages.
* **No child entities**: You cannot define a reference entity as a parent in parent-child relationships.
* **Guid keys only**: Reference entities must have `Guid` primary keys (`IEntity<Guid>`).
* **Explicit registration required**: Each reference entity must be registered in code before use.

## Common Reference Entities

| Entity | Name for `entityName` | Typical Display Property |
|--------|----------------------|--------------------------|
| ABP Identity User | `Volo.Abp.Identity.IdentityUser` | `UserName` |

## See Also

* [model.json Structure](model-json.md)
* [Foreign Access](foreign-access.md)
* [Attributes & Fluent API](fluent-api.md)
