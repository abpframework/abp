```json
//[doc-seo]
{
    "Description": "Control access to related entities through foreign key relationships using Foreign Access in the ABP Low-Code System."
}
```

# Foreign Access

> **Preview:** Foreign access metadata is part of the preview Low-Code System. Relation behavior and designer options may change before general availability.

Use the [Low-Code Designer](designer.md) to review relation metadata visually. This page explains the advanced configuration values that control how related dynamic entities can be reached from runtime pages.

Foreign Access controls how related **dynamic entities** can be accessed through foreign key relationships. It determines whether users can view or manage related data directly from the **target entity's** UI.

The foreign key defines only the schema relation. Reverse access is configured per page: the target entity's page defines a `relationships[]` entry that selects the access level, generated or named related pages/forms, and optional separate authorization. See [Data Modeling and Page Behavior](data-modeling.md#reverse-relationships).

> **Important:** Foreign Access only works between **dynamic entities**. It does not apply to [reference entities](reference-entities.md) because they are read-only and don't have UI pages.

## Access Levels

The `ForeignAccess` enum defines three levels. A page relationship uses `View` or `Edit`; a foreign key without a page relationship behaves like `None`.

| Level | Value | Description |
|-------|-------|-------------|
| `None` | 0 | No access from the target entity side. The relationship exists only for lookups. This is the behavior of a foreign key that no page relationship references. |
| `View` | 1 | Read-only access. Users can view related records from the target entity's page. |
| `Edit` | 2 | Full CRUD access. Users can create, update, and delete related records from the target entity's page. |

## Defining the Foreign Key

The foreign key only declares the schema direction. With attributes:

````csharp
[DynamicEntity]
public class Order
{
    [DynamicForeignKey("MyApp.Customers.Customer", "Name")]
    public Guid CustomerId { get; set; }
}
````

With the Fluent API:

````csharp
AbpDynamicEntityConfig.EntityConfigurations.Configure(
    "MyApp.Orders.Order",
    entity =>
    {
        entity.AddOrGetProperty("CustomerId")
            .WithForeignKey("MyApp.Customers.Customer", "Name");
    }
);
````

In JSON descriptors:

```json
{
  "name": "CustomerId",
  "foreignKey": {
    "entityName": "LowCodeDemo.Customers.Customer",
    "displayPropertyName": "Name"
  }
}
```

## Configuring Access in Page Descriptors

Reverse access is defined on the **target entity's page** with a `relationships[]` entry. `sourceEntityName` and `sourcePropertyName` identify the foreign key that points back to the page entity:

```json
{
  "name": "customers",
  "title": "Customers",
  "type": "dataGrid",
  "entityName": "LowCodeDemo.Customers.Customer",
  "relationships": [
    {
      "id": "orders-customer",
      "sourceEntityName": "LowCodeDemo.Orders.Order",
      "sourcePropertyName": "CustomerId",
      "access": "edit",
      "relatedPageMode": "page",
      "relatedPageName": "orders",
      "createFormMode": "generated",
      "editFormMode": "generated"
    }
  ]
}
```

| Field | Description |
|-------|-------------|
| `id` | Stable identifier unique within the page; used by relationship permissions and routes |
| `access` | `view` or `edit` |
| `relatedPageMode` | `generated` builds the related list from the source entity; `page` reuses `relatedPageName` |
| `createFormMode` / `editFormMode` | `generated`, `inherit` (reuse the related page's forms), or `form` with `createFormName` / `editFormName` |
| `useSeparatePermission` | Resolve view/create/update/delete for this relationship separately from the host page |
| `permissionConfig` | Optional permission overrides used with `useSeparatePermission` |

### Examples from the Demo Application

**Edit access** — Orders can be managed from the Customer page (`pages/customers.json`):

```json
{
  "id": "orders-customer",
  "sourceEntityName": "LowCodeDemo.Orders.Order",
  "sourcePropertyName": "CustomerId",
  "access": "edit",
  "relatedPageMode": "page",
  "relatedPageName": "orders",
  "createFormMode": "generated",
  "editFormMode": "generated"
}
```

**View access** — Warehouses managed by a customer are viewable from the Customer page:

```json
{
  "id": "warehouses-manager-customer",
  "sourceEntityName": "LowCodeDemo.Inventory.Warehouse",
  "sourcePropertyName": "ManagerCustomerId",
  "access": "view",
  "relatedPageMode": "page",
  "relatedPageName": "warehouses",
  "createFormMode": "generated",
  "editFormMode": "generated"
}
```

## UI Behavior

When foreign access is configured between two **dynamic entities**:

![Relation overview in the Low-Code Designer](images/designer-relations.png)

### `ForeignAccess.View`

An **action menu item** appears on the target entity's data grid row (e.g., a "Visited Countries" item on the Country row). Clicking it opens a read-only modal showing related records.

### `ForeignAccess.Edit`

An **action menu item** appears on the target entity's data grid row (e.g., an "Orders" item on the Customer row). Clicking it opens a fully functional CRUD modal where users can create, edit, and delete related records.

### `ForeignAccess.None`

No action menu item is added. The foreign key exists only for data integrity and lookup display.

Self-relations are supported. For example, an `Employee.ManagerId` foreign key can expose direct reports on the employee page, while page columns and filters can follow paths such as `ManagerId.ManagerId.Name` within the configured query-depth limit.

## Permission Control

Foreign access actions respect the **entity permissions** of the source entity (the entity with the foreign key). For example, if a user does not have the `Delete` permission for `Order`, the delete button will not appear in the foreign access modal, even if the access level is `Edit`.

A generated page relationship can set `useSeparatePermission: true`. In that mode, view/create/update/delete access is resolved for the host page and relationship ID instead of relying only on the source entity permission. Relationship create, update, and delete also require relationship view access.

## How It Works

The `PageRelationshipDescriptor` on the target entity's page stores the relationship metadata:

* **Source entity** — the dynamic entity with the foreign key (e.g., `Order`), from `sourceEntityName`
* **Target entity** — the dynamic entity being referenced (e.g., `Customer`), the page `entityName`
* **Foreign key property** — the property name (e.g., `CustomerId`), from `sourcePropertyName`
* **Access level** — `View` or `Edit`

`DynamicForeignAccessService` resolves these relationships when building entity actions and filtering related data.

> **Terminology:** In foreign access context, "target entity" refers to the entity whose UI shows the action menu (the entity being pointed to by the foreign key). This is different from "reference entity" which specifically means an existing C# entity registered for read-only access.

## See Also

* [Model Descriptor Files](model-json.md)
* [Data Modeling and Page Behavior](data-modeling.md)
* [Reference Entities](reference-entities.md)
* [Attributes & Fluent API](fluent-api.md)
