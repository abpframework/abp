```json
//[doc-seo]
{
    "Description": "Model Low-Code property storage, primitive collections, related fields, presentations, backend filters, and page or relationship permissions."
}
```

# Data Modeling and Page Behavior

The Low-Code Designer can model more than scalar fields and basic CRUD pages. This page covers the data and page features that affect storage, queries, presentation, and authorization.

## Property Storage

Scalar properties use one of two storage shapes:

* `isMappedToDbField: true` maps the property to its own physical column.
* An omitted or `false` `isMappedToDbField` stores the property through the entity's dynamic data mapping.

By default, dynamic data mapping uses the entity's JSON `Data` column. Applications that need individual columns for those properties can disable JSON data storage while configuring EF Core:

```csharp
builder.ConfigureDynamicEntities(useJsonDataStorage: false);
```

The equivalent module option is `AbpLowCodeEntityFrameworkCoreOptions.UseJsonDataStorage`. Its verified default is `true`.

Changing the storage mode or `isMappedToDbField` affects the physical schema. Decide the storage strategy before creating production tables, then use the normal migration or runtime schema workflow for later changes. Formulas and rollups are virtual and do not create physical scalar columns.

Source-model and runtime-model dynamic tables can use separate prefixes:

```csharp
LowCodeDbProperties.JsonModelTablePrefix = "Src_";
LowCodeDbProperties.RuntimeTablePrefix = "Runtime_";
```

Configure prefixes before the dynamic model is initialized. Changing a prefix after tables exist requires renaming or migrating those tables.

## Primitive Collections

A primitive collection keeps an ordered list of values on one property. Supported element types are `string`, `int`, `long`, `decimal`, `dateTime`, `boolean`, `guid`, `enum`, `date`, `time`, `money`, `file`, and `image`.

```json
{
  "name": "Tags",
  "type": "string",
  "collection": {
    "maxCount": 25,
    "uniqueItems": true,
    "storageKey": "b7db3ad2-3452-511b-b9b7-cc11d2db6dcb"
  }
}
```

Collection rules:

* `maxCount` is optional, but must be greater than zero when supplied.
* `uniqueItems` is required and controls duplicate-value validation.
* The effective item limit is the lower of `maxCount` and `LowCode:PrimitiveCollections:MaximumItemsPerProperty`. The verified global default is `1000`.
* `storageKey` is a stable internal identity stored in the model for the normalized collection table. When adding a collection through the Designer or MCP, omit it from the mutation payload; the server generates it. Do not change it after data exists.
* A collection property cannot also be a foreign key, formula, or rollup.

Collections are stored in normalized rows rather than inside the owner JSON payload. The React runtime returns them as ordered arrays and uses collection-aware controls for scalar, enum, file, and image values.

## Related Fields and Self-Relations

Page columns and filters can follow foreign keys by using dot-separated property paths:

```json
{
  "columns": [
    { "propertyName": "CustomerId.Name", "label": "Customer" },
    { "propertyName": "CustomerId.CountryId.Name", "label": "Country" }
  ],
  "filters": [
    { "propertyName": "CustomerId.CountryId.RegionId.Name" }
  ]
}
```

Only requested related fields are projected into the response. The same paths can be used by page filtering and export, including registered reference entities.

Self-relations are supported. For example, an employee page can use `ManagerId.ManagerId.Name` to follow the same relation more than once. Every path is still limited by the configured maximum foreign-key depth exposed by the Low-Code query capabilities.

## Reverse Relationships

A foreign key defines the schema direction. A page relationship defines how records that point back to the host record are shown and edited:

```json
{
  "name": "Authors",
  "entityName": "Acme.Authors.Author",
  "relationships": [
    {
      "id": "author-books",
      "sourceEntityName": "Acme.Books.Book",
      "sourcePropertyName": "AuthorId",
      "access": "edit",
      "relatedPageMode": "page",
      "relatedPageName": "Books",
      "createFormMode": "generated",
      "editFormMode": "form",
      "editFormName": "BookEditForAuthor"
    }
  ]
}
```

`access` can be `none`, `view`, or `edit`. The generated modes build the related page or form from the source entity; the explicit modes reuse named page and form descriptors.

See [Foreign Access](foreign-access.md) for the runtime APIs and UI behavior used by these relationships.

## Enum and Boolean Presentation

Enum values can define reusable display metadata:

```json
{
  "name": "Acme.Orders.OrderStatus",
  "values": [
    {
      "name": "Pending",
      "value": 10,
      "displayName": "Waiting",
      "presentation": "badge",
      "color": "#F59E0B"
    }
  ]
}
```

Enum presentation supports `text`, `badge`, and `iconOnly`. Pages can override the display name, presentation, color, or icon for one property without changing the shared enum:

```json
{
  "enumPresentations": [
    {
      "propertyName": "Status",
      "values": [
        { "value": 10, "displayName": "Awaiting review", "presentation": "badge", "color": "#F59E0B" }
      ]
    }
  ]
}
```

Boolean columns support `text`, `checkbox`, `badge`, and `iconOnly`, with separate metadata for `true`, `false`, and `null`:

```json
{
  "propertyName": "IsActive",
  "booleanPresentation": "badge",
  "booleanValues": {
    "true": { "displayName": "Active", "color": "#16A34A" },
    "false": { "displayName": "Inactive", "color": "#DC2626" },
    "null": { "displayName": "Not set", "color": "#6B7280" }
  }
}
```

Icons can reference a CSS class, stored blob, data URL, or application path. Runtime-layer writes apply stricter icon validation than source-controlled descriptors.

## Backend Filters

Visible page filters are controlled by the user. A backend filter is always applied by the server and is useful for tenant, ownership, role, or workflow scoping.

```json
{
  "backendFilter": {
    "items": [
      {
        "propertyName": "Status",
        "operator": "equal",
        "value": "Active"
      },
      {
        "logic": "or",
        "items": [
          {
            "propertyName": "CreatorId",
            "operator": "equal",
            "valueProvider": "CurrentUserId"
          },
          {
            "logic": "and",
            "propertyName": "AllowedRole",
            "operator": "in",
            "valueProvider": "CurrentUserRoles"
          }
        ]
      }
    ]
  }
}
```

A filter value can be:

* Static through `value`.
* Resolved by JavaScript through `javaScript`.
* Resolved by a registered provider through `valueProvider`.

Built-in providers cover the current user ID, username, first name, surname, email, email verification, phone number, phone verification, roles, and current tenant ID. Applications can register additional typed providers with `AbpLowCodePageBackendFilterOptions`.

Backend filters are combined with search and user-selected filters. They are not sent as editable client state, so do not replace them with a hidden React filter when the rule is security-sensitive.

## Page and Relationship Permissions

Pages use resource-based authorization by default. `permissionConfig` can keep that generated default, require a named permission, allow any authenticated user, or make an operation public:

```json
{
  "permissionConfig": {
    "view": "default",
    "create": "Acme.Orders.Create",
    "update": "authenticated",
    "delete": "Acme.Orders.Delete"
  }
}
```

For generated reverse relationships, enable separate authorization when child access must not inherit the host page decision:

```json
{
  "id": "author-books",
  "sourceEntityName": "Acme.Books.Book",
  "sourcePropertyName": "AuthorId",
  "access": "edit",
  "useSeparatePermission": true,
  "permissionConfig": {
    "view": "default",
    "create": "Acme.Books.Create",
    "update": "Acme.Books.Update",
    "delete": "Acme.Books.Delete"
  }
}
```

When `useSeparatePermission` is `true`, generated relationship permissions are scoped to the host page and relationship ID. Create, update, and delete also require relationship view access.

## See Also

* [Low-Code Designer](designer.md)
* [Model Descriptor Files](model-json.md)
* [Calculated and Rollup Properties](formula-properties.md)
* [Data Import](data-import.md)
* [Foreign Access](foreign-access.md)
* [React Runtime](react-runtime.md)
