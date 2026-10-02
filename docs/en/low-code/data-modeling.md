```json
//[doc-seo]
{
    "Description": "Model Low-Code property storage, primitive collections, related fields, presentations, backend filters, and page or relationship permissions."
}
```

# Data Modeling and Page Behavior

The Low-Code Designer can model more than scalar fields and basic CRUD pages. This page covers the data and page features that affect storage, queries, presentation, and authorization.

## Property Storage

Scalar properties keep their values in one of two places: a physical column of their own, or the entity's JSON `Data` column. `isMappedToDbField` on the property decides which:

| `isMappedToDbField` | Where the values are stored |
|---------------------|-----------------------------|
| `true` | A physical column of its own |
| `false` | The entity's `Data` JSON column |
| Omitted | Follows `UseJsonDataStorage`: the `Data` JSON column when it is `true`, a column of its own when it is `false` |

The storage of a new property comes from the module option `AbpLowCodeEntityFrameworkCoreOptions.UseJsonDataStorage`. It is `true`, which stores new properties in the `Data` column. Set it to `false` when the database provider does not support the JSON column mapping, queries, and updates that Low-Code needs, so that new properties get columns of their own:

```csharp
Configure<AbpLowCodeEntityFrameworkCoreOptions>(options =>
{
    options.UseJsonDataStorage = false;
});
```

`builder.ConfigureDynamicEntities(useJsonDataStorage: false)` sets the same option while configuring EF Core. Low-Code does not infer the option from the provider.

A property created in the Designer, through MCP, or through any other model change records the storage chosen at that moment: its `isMappedToDbField` is set from the default. Changing `UseJsonDataStorage` later therefore affects only new properties, and existing entities keep working. A property whose descriptor omits `isMappedToDbField`, such as one written by hand in a model JSON file or defined in code, follows the option. Low-Code does not move existing values between the `Data` column and individual columns when the option changes. If you change `UseJsonDataStorage` while entities already hold data, first set `isMappedToDbField` explicitly on the existing descriptors that omit it (in model JSON files, in code, and on properties created before the Designer recorded the flag), so they keep their current storage. Otherwise they move to the other storage and their existing values are no longer read.

While `UseJsonDataStorage` is `false`, a model change cannot give an entity that keeps no `Data` document a property stored there; it is refused with `LowCode:JsonDataStorageDisabled`. An entity that already keeps a `Data` document keeps working as before.

A dynamic entity's table has the `Data` column only while at least one of its properties is stored there. An entity whose properties all have their own columns has no `Data` column, so it works on databases without JSON column support. The column is added when a property first needs it and is not removed afterwards.

Formulas, rollups, and primitive collections are not stored in either place: formulas and rollups are virtual, and collections have their own tables.

### Physical Name Length

Runtime entities and mapped properties become tables and columns named after them, and a non-default app adds its name to the table name. Databases limit how long such names can be; PostgreSQL, for example, accepts 63 bytes. A model change that would create a longer table or column name is refused before the schema is touched, with `LowCode:PhysicalTableNameTooLong` or `LowCode:PhysicalColumnNameTooLong`. The error names the entity or property; use a shorter entity, app, or property name. The limit comes from the configured EF Core provider, and a table name must leave room for its primary key name (`PK_` followed by the table name).

Names that Low-Code derives for primitive collection tables and their keys and indexes are shortened with a stable suffix instead.

### Default Values

A property's `defaultValue` must fit the property type. A value that cannot be converted, such as `"not-a-number"` on an `int` property, is refused together with the model change that sets it.

### Adding a Required Property to Existing Data

When you add a required property to an entity that already has records, choose how the existing rows get a value. In the Designer, this is the **Existing Records** setting on the property dialog's **Advanced** tab. A model change sets it through the operation's options:

| Option | Effect |
|--------|--------|
| `existingDataMode: "none"` | Existing rows are not filled |
| `existingDataMode: "fixed"` with `existingDataValue` | Every existing row gets the same value |
| `existingDataMode: "formula"` with `existingDataExpression` | Each existing row gets the result of an [expression](expression-language.md) |

Fixed values and formulas work for properties stored in the `Data` column and for properties mapped to their own column. The fill is part of the model change, not of the property: `backfillValue` is not a property attribute, and a model change that sets it on a property is refused with the existing-data options to use instead.

### Table Prefixes

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
    "uniqueItems": true
  }
}
```

Collection rules:

* `maxCount` is optional, but must be greater than zero when supplied.
* `uniqueItems` is required and controls duplicate-value validation.
* The effective item limit is the lower of `maxCount` and `LowCode:PrimitiveCollections:MaximumItemsPerProperty`. The verified global default is `1000`.
* Collection table names are derived from the entity and property names. Developer JSON, Designer, MCP, and code-layer definitions do not require an internal collection identifier.
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

`access` can be `view` or `edit`; omit the relationship entirely when the foreign key should stay lookup-only. `relatedPageMode`, `createFormMode`, and `editFormMode` are required. The generated modes build the related page or form from the source entity; the explicit modes reuse named page and form descriptors, and `inherit` reuses the related page's own forms.

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

### Filters on List Fields

A property that stores multiple values (a [primitive collection](#primitive-collections)) keeps its values in rows of its own table. A backend filter on such a property matches over those rows, and it works on every EF Core provider. The operator depends on both the property and the value source:

| Property | Value | Operators |
|----------|-------|-----------|
| Single value | One value | Every operator the property type supports |
| Single value | Multiple values | `in` (is any of), `notIn` (is none of) |
| List | One value | `equal` (contains), `notEqual` (does not contain), `in`, `notIn` |
| List | Multiple values | `in` (contains any of), `notIn` (contains none of) |
| List | None | `hasValue` (`true`: has items, `false`: is empty) |

A multiple-value source is a static array, JavaScript that returns an array, or a value provider registered with `cardinality: PageBackendFilterValueCardinality.Multiple`. A multiple-value provider can be used only with `in` and `notIn`. Other operators on a list field, such as `contains`, `startsWith`, `greaterThan`, or `between`, are refused when the model is saved, and the error names the allowed operators. The Designer offers only the valid operators and sources for the selected field.

An empty value list never removes the condition: `in` with no values matches no records, and `notIn` with no values excludes none.

The following filter shows records shared with any of the current user's teams. `AudienceTeamIds` is a list field, and `CurrentUserTeamIds` is an application-registered provider that returns several IDs:

```json
{
  "backendFilter": {
    "propertyName": "AudienceTeamIds",
    "operator": "in",
    "valueProvider": "CurrentUserTeamIds"
  }
}
```

```csharp
Configure<AbpLowCodePageBackendFilterOptions>(options =>
{
    options.AddOrReplaceValueProvider(
        "CurrentUserTeamIds",
        LocalizableString.Create<MyResource>("BackendFilter:CurrentUserTeamIds"),
        typeof(CurrentUserTeamIdsBackendFilterValueProvider),
        [EntityPropertyType.Guid],
        groupPath: ["Current user"],
        cardinality: PageBackendFilterValueCardinality.Multiple,
        supportedOperators: ["in", "notIn"]);
});
```

`AddOrReplaceValueProvider` accepts either a plain display name or an `ILocalizableString`; the Designer lists sources by their localized names.

### What a Backend Filter Scopes

A backend filter limits the existing records that the page reads, including lists, single-record reads, and exports, and the records that update and delete can find. A record outside the filter cannot be read, updated, or deleted through the page.

A backend filter does not check the payload of a create request and never assigns a value. To set a scoped field such as an owner on new records, leave it out of the create form and set it in a Create `Pre` [interceptor](interceptors.md), for example with `context.commandArgs.setValue('OwnerId', context.currentUser.id);`.

If a JavaScript filter value fails, including a script that calls an [error helper](scripting-api.md#error-helpers), no filter value is produced and the query fails instead of returning unfiltered data. The error is answered like any other script error.

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
