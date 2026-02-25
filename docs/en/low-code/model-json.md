```json
//[doc-seo]
{
    "Description": "Define dynamic entities using model.json in the ABP Low-Code System. Learn about entity properties, enums, foreign keys, validators, UI configuration, and migration requirements."
}
```

# model.json Structure

The `model.json` file defines all your dynamic entities, their properties, enums, relationships, interceptors, custom endpoints, and UI configurations. It is an alternative configuration source to [C# Attributes and Fluent API](fluent-api.md), ideal when you prefer a declarative JSON approach.

## File Location

Place your `model.json` in a `_Dynamic` folder inside your **Domain** project:

```
YourApp.Domain/
└── _Dynamic/
    └── model.json
```

The module automatically discovers and loads this file at application startup.

> A JSON Schema file (`model.schema.json`) is available in the module source for IDE IntelliSense. Reference it using the `$schema` property:

```json
{
  "$schema": "path/to/model.schema.json",
  "entities": []
}
```

## Top-Level Structure

The `model.json` file has three root sections:

```json
{
  "$schema": "...",
  "enums": [],
  "entities": [],
  "endpoints": []
}
```

| Section | Description |
|---------|-------------|
| `enums` | Enum type definitions |
| `entities` | Entity definitions with properties, foreign keys, interceptors, and UI |
| `endpoints` | Custom REST API endpoints with JavaScript handlers |

## Enum Definitions

Define enums that can be used as property types:

```json
{
  "enums": [
    {
      "name": "LowCodeDemo.Organizations.OrganizationType",
      "values": [
        { "name": "Corporate", "value": 0 },
        { "name": "Enterprise", "value": 1 },
        { "name": "Startup", "value": 2 },
        { "name": "Consulting", "value": 3 }
      ]
    }
  ]
}
```

Reference enums in entity properties using the `enumType` field:

```json
{
  "name": "OrganizationType",
  "enumType": "LowCodeDemo.Organizations.OrganizationType"
}
```

## Entity Definition

Each entity has the following structure:

```json
{
  "name": "LowCodeDemo.Products.Product",
  "displayProperty": "Name",
  "parent": null,
  "properties": [],
  "interceptors": [],
  "ui": {}
}
```

### Entity Attributes

| Attribute | Type | Description |
|-----------|------|-------------|
| `name` | string | **Required.** Full entity name with namespace (e.g., `"MyApp.Products.Product"`) |
| `displayProperty` | string | Property to display in lookups and foreign key dropdowns |
| `parent` | string | Parent entity name for parent-child (master-detail) relationships |
| `properties` | array | Property definitions |
| `interceptors` | array | CRUD lifecycle interceptors |
| `ui` | object | UI configuration |

### Parent-Child Relationships

Use the `parent` field to create nested entities. Children are managed through the parent entity's UI:

```json
{
  "name": "LowCodeDemo.Orders.OrderLine",
  "parent": "LowCodeDemo.Orders.Order",
  "properties": [
    {
      "name": "ProductId",
      "foreignKey": {
        "entityName": "LowCodeDemo.Products.Product"
      }
    },
    { "name": "Quantity", "type": "int" },
    { "name": "Amount", "type": "decimal" }
  ]
}
```

Multi-level nesting is supported (e.g., `Order > OrderLine > ShipmentItem > ShipmentTracking`).

## Property Definition

```json
{
  "name": "Price",
  "type": "decimal",
  "isRequired": true,
  "isUnique": false,
  "isMappedToDbField": true,
  "serverOnly": false,
  "allowSetByClients": true,
  "enumType": null,
  "foreignKey": null,
  "validators": [],
  "ui": {}
}
```

### Property Types

| Type | Description |
|------|-------------|
| `string` | Text (default if type is omitted) |
| `int` | 32-bit integer |
| `long` | 64-bit integer |
| `decimal` | Decimal number |
| `DateTime` | Date and time |
| `boolean` | True/false |
| `Guid` | GUID/UUID |
| `Enum` | Enum type (requires `enumType` field) |

### Property Flags

| Flag | Type | Default | Description |
|------|------|---------|-------------|
| `isRequired` | bool | `false` | Property must have a value |
| `isUnique` | bool | `false` | Value must be unique across all records |
| `isMappedToDbField` | bool | `true` | Property is stored in the database |
| `serverOnly` | bool | `false` | Property is hidden from API clients |
| `allowSetByClients` | bool | `true` | Whether clients can set this value |

### Foreign Key Properties

Define a foreign key relationship inline on a property:

```json
{
  "name": "CustomerId",
  "foreignKey": {
    "entityName": "LowCodeDemo.Customers.Customer",
    "displayPropertyName": "Name",
    "access": "edit"
  }
}
```

| Attribute | Description |
|-----------|-------------|
| `entityName` | **Required.** Full name of the target entity — can be a **dynamic entity** (e.g., `"LowCodeDemo.Customers.Customer"`) or a **[reference entity](reference-entities.md)** (e.g., `"Volo.Abp.Identity.IdentityUser"`) |
| `displayPropertyName` | Property to display in lookups (defaults to entity's `displayProperty`) |
| `access` | [Foreign access](foreign-access.md) level: `"none"`, `"view"`, or `"edit"` |

> **Note:** [Reference entities](reference-entities.md) are existing C# entities (like ABP's `IdentityUser`) that are registered for read-only access. Unlike dynamic entities, they don't get CRUD pages — they're used only for foreign key lookups and display values.

### Validators

Add validation rules to properties:

```json
{
  "name": "EmailAddress",
  "validators": [
    { "type": "required" },
    { "type": "emailAddress" },
    { "type": "minLength", "length": 5 },
    { "type": "maxLength", "length": 255 }
  ]
}
```

Additional validator examples:

```json
{
  "name": "Website",
  "validators": [
    { "type": "url", "message": "Please enter a valid URL" }
  ]
},
{
  "name": "PhoneNumber",
  "validators": [
    { "type": "phone" }
  ]
},
{
  "name": "ProductCode",
  "validators": [
    { "type": "regularExpression", "pattern": "^[A-Z]{3}-\\d{4}$", "message": "Code must be in format ABC-1234" }
  ]
},
{
  "name": "Price",
  "type": "decimal",
  "validators": [
    { "type": "range", "minimum": 0.01, "maximum": 99999.99 }
  ]
}
```

| Validator | Parameters | Applies To | Description |
|-----------|------------|------------|-------------|
| `required` | `allowEmptyStrings` (optional) | All types | Value is required |
| `minLength` | `length` | String | Minimum string length |
| `maxLength` | `length` | String | Maximum string length |
| `stringLength` | `minimumLength`, `maximumLength` | String | String length range (min and max together) |
| `emailAddress` | — | String | Must be a valid email |
| `phone` | — | String | Must be a valid phone number |
| `url` | — | String | Must be a valid URL |
| `creditCard` | — | String | Must be a valid credit card number |
| `regularExpression` | `pattern` | String | Must match the regex pattern |
| `range` | `minimum`, `maximum` | Numeric | Numeric range |
| `min` | `minimum` | Numeric | Minimum numeric value |
| `max` | `maximum` | Numeric | Maximum numeric value |

> All validators accept an optional `message` parameter for a custom error message. The `regularExpression` validator also accepts the alias `pattern`, and `emailAddress` also accepts `email`.

## UI Configuration

### Entity-Level UI

```json
{
  "ui": {
    "pageTitle": "Products"
  }
}
```

> Only entities with `ui.pageTitle` get a menu item and a dedicated page in the UI.

### Property-Level UI

```json
{
  "name": "RegistrationNumber",
  "ui": {
    "displayName": "Registration Number",
    "isAvailableOnDataTable": true,
    "isAvailableOnDataTableFiltering": true,
    "creationFormAvailability": "Hidden",
    "editingFormAvailability": "NotAvailable",
    "quickLookOrder": 100
  }
}
```

| Attribute | Type | Default | Description |
|-----------|------|---------|-------------|
| `displayName` | string | Property name | Display label in UI |
| `isAvailableOnDataTable` | bool | `true` | Show in data grid |
| `isAvailableOnDataTableFiltering` | bool | `true` | Show in filter panel |
| `creationFormAvailability` | string | `"Available"` | Visibility in create form |
| `editingFormAvailability` | string | `"Available"` | Visibility in edit form |
| `quickLookOrder` | int | -2 | Order in quick-look panel (-2 = not shown) |

#### Form Availability Values

| Value | Description |
|-------|-------------|
| `Available` | Visible and editable |
| `Hidden` | Not visible in the form |
| `NotAvailable` | Visible but disabled/read-only |

## Interceptors

Define JavaScript interceptors for CRUD lifecycle hooks:

```json
{
  "interceptors": [
    {
      "commandName": "Create",
      "type": "Pre",
      "javascript": "if(!context.commandArgs.data['Name']) { globalError = 'Name is required!'; }"
    }
  ]
}
```

See [Interceptors](interceptors.md) for details.

## Endpoints

Define custom REST endpoints with JavaScript handlers:

```json
{
  "endpoints": [
    {
      "name": "GetProductStats",
      "route": "/api/custom/products/stats",
      "method": "GET",
      "requireAuthentication": false,
      "javascript": "var count = await db.count('Products.Product'); return ok({ total: count });"
    }
  ]
}
```

See [Custom Endpoints](custom-endpoints.md) for details.

## Complete Example

```json
{
  "enums": [
    {
      "name": "ShipmentStatus",
      "values": [
        { "name": "Pending", "value": 0 },
        { "name": "Shipped", "value": 2 },
        { "name": "Delivered", "value": 4 }
      ]
    }
  ],
  "entities": [
    {
      "name": "LowCodeDemo.Products.Product",
      "displayProperty": "Name",
      "properties": [
        { "name": "Name", "isUnique": true, "isRequired": true },
        { "name": "Price", "type": "decimal" },
        { "name": "StockCount", "type": "int" },
        { "name": "ReleaseDate", "type": "DateTime" }
      ],
      "ui": { "pageTitle": "Products" }
    },
    {
      "name": "LowCodeDemo.Orders.Order",
      "displayProperty": "Id",
      "properties": [
        {
          "name": "CustomerId",
          "foreignKey": {
            "entityName": "LowCodeDemo.Customers.Customer",
            "access": "edit"
          }
        },
        { "name": "TotalAmount", "type": "decimal" },
        { "name": "IsDelivered", "type": "boolean" }
      ],
      "interceptors": [
        {
          "commandName": "Create",
          "type": "Post",
          "javascript": "context.log('Order created: ' + context.commandArgs.entityId);"
        }
      ],
      "ui": { "pageTitle": "Orders" }
    },
    {
      "name": "LowCodeDemo.Orders.OrderLine",
      "parent": "LowCodeDemo.Orders.Order",
      "properties": [
        {
          "name": "ProductId",
          "foreignKey": { "entityName": "LowCodeDemo.Products.Product" }
        },
        { "name": "Quantity", "type": "int" },
        { "name": "Amount", "type": "decimal" }
      ]
    }
  ]
}
```

## Migration Requirements

When you modify `model.json`, you need database migrations for the changes to take effect:

* **New entity**: `dotnet ef migrations add Added_{EntityName}`
* **New property**: `dotnet ef migrations add Added_{PropertyName}_To_{EntityName}`
* **Type change**: `dotnet ef migrations add Changed_{PropertyName}_In_{EntityName}`

> The same migration requirement applies when using [C# Attributes](fluent-api.md). Any change to entity structure requires an EF Core migration.

## See Also

* [Attributes & Fluent API](fluent-api.md)
* [Interceptors](interceptors.md)
* [Custom Endpoints](custom-endpoints.md)
* [Scripting API](scripting-api.md)
