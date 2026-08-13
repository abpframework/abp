```json
//[doc-seo]
{
    "Description": "Define ABP Low-Code descriptor metadata and descriptor schemas for dynamic entities, pages, forms, filters, permissions, script endpoints, event handlers, background jobs, and workers."
}
```

# Model Descriptor Files

> **Preview:** The Low-Code System is currently in preview. The descriptor format is stable enough for evaluation and source control, but fields may change before general availability.

Low-code metadata is source-controlled as JSON descriptor files used by the Low-Code Designer and React runtime. Current generated projects keep descriptors as split files under `_Dynamic/model`; older projects may still contain an aggregate `_Dynamic/model.json` document. Use the [Low-Code Designer](designer.md) for normal editing. Use this page when you need to review, generate, merge, or source-control the JSON metadata directly.

## File Location

Generated low-code applications keep descriptor files under `_Dynamic/model/` plus a generated initializer in the same `_Dynamic` folder. A typical layered application stores one JSON file per descriptor and keeps starter examples separately:

```text
YourApp.Domain/
`-- _Dynamic/
    |-- YourAppLowCodeInitializer.cs
    |-- model/
    |   |-- entities/
    |   |   `-- Acme/Catalog/Product.json
    |   |-- pages/
    |   |   `-- products.json
    |   |-- forms/
    |   |   `-- product-form.json
    |   `-- permissions/
    |       `-- Acme.Catalog.json
    `-- model-examples/
        |-- product.entity.json
        |-- product-form.form.json
        `-- products-page.page.json
```

Single-layer applications use the same `_Dynamic` layout under the host project instead of the domain project.

Keep the whole `_Dynamic` folder and the generated initializer in source control. The runtime scans `_Dynamic/model/**/*.json`; files in `model-examples/` are ignored until you copy them into the matching category folder under `model/`.

Layout conventions:

* `entities/` and `enums/` usually follow namespace-like folders such as `entities/Acme/Catalog/Product.json`.
* `pageGroups/` usually stays flat as `pageGroups/{groupName}.json`.
* `pages/` stays flat as `pages/{pageName}.json`, even when the page belongs to a page group or is a dashboard.
* Dashboard pages live in `pages/`; there is no separate dashboard descriptor folder.
* The runtime scans the directory tree directly. Do not add a combined index file next to `model/`.

## Example Files and Validation

ABP Studio-generated solutions include a `model-examples/` folder with starter descriptors that are safe to keep in source control but are not loaded by the runtime until copied into `model/`.

Typical example copies:

* `product.entity.json` -> `_Dynamic/model/entities/YourCompany/YourApp/Catalog/Product.json`
* `product-form.form.json` -> `_Dynamic/model/forms/product-form.json`
* `products-page.page.json` -> `_Dynamic/model/pages/products.json`

Copying example files into `model/` activates the metadata, but entity and mapped-property changes still require the generated migration workflow before runtime CRUD pages can query the backing table safely.

After manual JSON edits, validate the descriptor files with the generated startup project before running migrations or shipping changes:

```bash
dotnet run --project <startup-project> -- --check-lowcode-model-files
```

To validate a specific folder instead of the configured source assemblies, pass `--model-directory`:

```bash
dotnet run --project <startup-project> -- --check-lowcode-model-files --model-directory "<path-to-_Dynamic/model>"
```

The file checker reports invalid JSON, wrong category placement, duplicate descriptor names, and model materialization errors before the runtime loads the descriptors.

It does not catch every runtime-semantic UI problem. For example, a form file can still pass the checker while rendering an empty group at runtime if the placement `fieldId` values do not match the form field definitions or if the expected group placements are missing. Use [Health](health.md), the Designer preview, or the runtime page itself to catch those issues.

## JSON Schemas

ABP publishes JSON Schema definitions for the descriptor objects that make up low-code metadata. These schemas are useful when you generate descriptors, review changes in source control, or want IDE validation for split descriptor files.

The schema files live in the ABP repository under `schemas/low-code`. Use the branch or tag that matches your ABP version.

The schema manifest is published at:

```text
https://raw.githubusercontent.com/abpframework/abp/rel-10.5/schemas/low-code/manifest.json
```

For another version, replace `rel-10.5` with the matching ABP branch or tag.

The manifest maps each descriptor collection to its descriptor schema:

| Descriptor collection | Descriptor schema |
|-----------------------|-------------------|
| `enums` | `definitions/enum-descriptor.schema.json` |
| `entities` | `definitions/entity-descriptor.schema.json` |
| `endpoints` | `definitions/endpoint-descriptor.schema.json` |
| `eventHandlers` | `definitions/script-event-handler-descriptor.schema.json` |
| `backgroundJobs` | `definitions/script-background-job-descriptor.schema.json` |
| `backgroundWorkers` | `definitions/script-background-worker-descriptor.schema.json` |
| `pageGroups` | `definitions/page-group-descriptor.schema.json` |
| `pages` | `definitions/page-descriptor.schema.json` |
| `forms` | `definitions/form-descriptor.schema.json` |
| `permissions` | `definitions/permission-descriptor.schema.json` |

### Split Descriptor Files

Use the descriptor schema directly when a descriptor is stored as its own JSON file. The descriptor object is the same shape used for one item inside the related descriptor collection.

```json
{
  "$schema": "https://raw.githubusercontent.com/abpframework/abp/rel-10.5/schemas/low-code/definitions/entity-descriptor.schema.json",
  "name": "Acme.Catalog.Product",
  "displayName": "Products",
  "properties": []
}
```

For example, a file that stores one page descriptor can reference `page-descriptor.schema.json`, and a file that stores one form descriptor can reference `form-descriptor.schema.json`.

The published schemas validate individual descriptor objects, not an aggregate descriptor document. If you still maintain an aggregate `_Dynamic/model.json`, do not point it at `entity-descriptor.schema.json`, `page-descriptor.schema.json`, or another descriptor schema; those schemas expect one descriptor object, not the top-level arrays. For aggregate metadata, use the manifest table above as the section-level reference and validate each array item with its matching descriptor schema.

## Top-Level Sections

When descriptor metadata is viewed as an aggregate document, the logical sections are page/form centered. Entities define data shape; pages and forms define the React runtime UI.

```json
{
  "enums": [],
  "entities": [],
  "endpoints": [],
  "eventHandlers": [],
  "backgroundJobs": [],
  "backgroundWorkers": [],
  "pageGroups": [],
  "pages": [],
  "forms": [],
  "permissions": []
}
```

| Section | Description |
|---------|-------------|
| `enums` | Reusable enum definitions |
| `entities` | Dynamic entities, properties, relations, attachments, validations, and interceptors |
| `endpoints` | JavaScript-backed custom HTTP endpoints |
| `eventHandlers` | JavaScript handlers for distributed events |
| `backgroundJobs` | Named JavaScript background job handlers |
| `backgroundWorkers` | Scheduled JavaScript workers |
| `pageGroups` | Menu folders used by runtime pages |
| `pages` | React runtime page definitions, including data grids, kanban, calendar, gallery, form pages, and dashboards |
| `forms` | Named form definitions referenced by pages |
| `permissions` | Custom permission definitions referenced by pages and endpoints |

## Enums

Define enums before properties that reference them:

```json
{
  "enums": [
    {
      "name": "Acme.Catalog.ProductStatus",
      "values": [
        { "name": "Draft", "value": 0 },
        { "name": "Active", "value": 1 },
        { "name": "Paused", "value": 2 },
        { "name": "Completed", "value": 3 }
      ]
    }
  ]
}
```

Use the enum from a property with `type: "enum"` and `enumType`:

```json
{
  "name": "Status",
  "type": "enum",
  "enumType": "Acme.Catalog.ProductStatus",
  "defaultValue": "0"
}
```

## Entities

Entities describe the persisted data model. UI is not configured with legacy property `ui` objects. Use page `columns` and `filters`, and named `forms`, for runtime UI behavior.

```json
{
  "name": "Acme.Catalog.Product",
  "displayName": "Products",
  "displayProperty": "Name",
  "properties": [],
  "crossFieldValidations": [],
  "interceptors": []
}
```

| Field | Description |
|-------|-------------|
| `name` | Required stable full entity name, for example `Acme.Catalog.Product` |
| `displayName` | Default plural/screen label |
| `displayProperty` | Property shown in lookups and foreign key display values |
| `parent` | Parent entity name for child/detail entities |
| `attachments` | Record-level attachment settings |
| `properties` | Entity property definitions |
| `crossFieldValidations` | Validation rules comparing two properties |
| `interceptors` | Create, update, and delete lifecycle scripts |

### Properties

```json
{
  "name": "Price",
  "type": "money",
  "isRequired": true,
  "isUnique": false,
  "allowSetByClients": true,
  "serverOnly": false,
  "isMappedToDbField": true,
  "validators": [
    { "type": "range", "minimum": 0, "maximum": 1000000 }
  ]
}
```

| Field | Description |
|-------|-------------|
| `name` | Required PascalCase property name |
| `type` | Property type; omitted means `string` |
| `displayName` | Default field label; pages/forms can override it |
| `enumType` | Enum name when `type` is `enum` |
| `defaultValue` | Default value for new records, stored as a string and converted at runtime |
| `isRequired` | Required/not nullable backend and UI validation |
| `isUnique` | Unique value validation |
| `serverOnly` | Hidden from clients, API responses, and UI metadata |
| `allowSetByClients` | Whether create/update clients may set this value |
| `isMappedToDbField` | Whether the property is stored in the database |
| `foreignKey` | Lookup relation metadata |
| `validators` | Backend/UI validation rules |

For virtual calculated fields and related-record aggregates, see [Calculated and Rollup Properties](formula-properties.md). The [Low-Code Expression Language](expression-language.md) reference documents the scalar syntax used by calculated properties and formula backfills.

### Property Types

| Type | Description |
|------|-------------|
| `string` | Text |
| `int`, `long` | Whole numbers |
| `decimal`, `money` | Decimal numbers and money values |
| `dateTime`, `date`, `time` | Date/time values |
| `boolean` | True/false |
| `guid` | GUID value |
| `enum` | Integer-backed enum; requires `enumType` |
| `file`, `image` | Upload metadata handled by the low-code file pipeline |

### File, Image, and Attachments

Use `file` or `image` properties for first-class upload fields:

```json
{
  "name": "CoverImage",
  "type": "image",
  "fileAllowedContentTypes": ["image/*"],
  "fileMaxSizeBytes": 5242880,
  "imageMaxWidth": 1600,
  "imageMaxHeight": 900,
  "imageResizeMode": "fit"
}
```

`imageResizeMode` currently supports:

* `fit`: preserve aspect ratio and scale down to fit within `imageMaxWidth` and `imageMaxHeight`
* `fill`: crop from the center and scale to fill the target box

When `imageResizeMode` is `fill`, set both `imageMaxWidth` and `imageMaxHeight`. If either dimension is missing, the current React runtime falls back to `fit`.

Image resizing is currently performed client-side by the React low-code runtime before upload. Backend upload endpoints still validate size and content type, but they store the uploaded bytes as-is. Direct API uploads and scripting uploads do not get automatic server-side resizing.

Use entity `attachments` when each record can have multiple arbitrary files:

```json
{
  "name": "Acme.Catalog.Product",
  "attachments": {
    "isEnabled": true,
    "maxFileCount": 10,
    "maxFileSizeBytes": 5242880,
    "allowedContentTypes": ["application/pdf", "image/*"]
  }
}
```

### Foreign Keys

```json
{
  "name": "OwnerId",
  "type": "guid",
  "foreignKey": {
    "entityName": "Volo.Abp.Identity.IdentityUser",
    "displayPropertyName": "UserName",
    "access": "none"
  }
}
```

`entityName` can point to another dynamic entity or a registered [reference entity](reference-entities.md). `access` controls [Foreign Access](foreign-access.md) behavior for dynamic entity relations.

### Validators

```json
{
  "name": "EmailAddress",
  "type": "string",
  "validators": [
    { "type": "required" },
    { "type": "email" },
    { "type": "maxLength", "length": 255 }
  ]
}
```

Common validators include `required`, `minLength`, `maxLength`, `stringLength`, `email`, `emailAddress`, `phone`, `url`, `creditCard`, `regularExpression`, `range`, `min`, and `max`. Validators can include a custom `message`.

## Pages

Pages create runtime routes and menu entries. They also choose how entity data is rendered in React.

```json
{
  "name": "products",
  "title": "Products",
  "icon": "fa-solid fa-box",
  "type": "dataGrid",
  "entityName": "Acme.Catalog.Product",
  "group": "catalog",
  "defaultFileExportMode": 0,
  "allowFileBundleExport": true,
  "columns": [
    { "propertyName": "Name", "order": 0, "exportOrder": 0 },
    { "propertyName": "Status", "order": 1, "exportOrder": 1 },
    { "propertyName": "Price", "order": 2, "exportOrder": 2, "exportable": false }
  ],
  "filters": [
    { "propertyName": "Name", "control": "text", "defaultOperator": "contains" },
    { "propertyName": "Status", "control": "select", "defaultOperator": "equal" }
  ],
  "createFormName": "product-form",
  "editFormName": "product-form"
}
```

Page columns support two independent flags:

| Field | Default | Purpose |
|-------|---------|---------|
| `visible` | `true` | Renders the field in the React page view |
| `exportOrder` | `order` | Optional page-level export order. Lower values are exported first |
| `exportable` | `true` | Page-level export flag managed by **Export Fields**. Allows the field to be included in Excel, CSV, download-link columns, and file bundle export |

If `columns` is present, export uses this list as the page-level export policy. `exportable: false` prevents the field from being exported even if a caller sends the field name manually. `exportOrder` controls default export order without changing display order. Server-only entity properties are never exportable.

Page export settings:

| Field | Default | Purpose |
|-------|---------|---------|
| `defaultFileExportMode` | `0` | Default spreadsheet output for file/image fields. `0` = file name, `1` = metadata columns, `2` = temporary download-link columns |
| `allowFileBundleExport` | `true` | Allows **Files (.zip)** export for exportable file/image columns on the page |

ZIP file bundle export only includes selected page columns that are file or image fields and are exportable. The ZIP contains `manifest.csv` plus files under `files/{recordId}/{fieldName}/{safeFileName}`.

| Page type | Required fields | Purpose |
|-----------|-----------------|---------|
| `dataGrid` | `entityName` | Searchable, sortable CRUD grid |
| `kanban` | `entityName`, `groupByProperty` | Cards grouped by an enum/status-like property |
| `calendar` | `entityName`, `calendarStartProperty` | Records shown on a calendar |
| `gallery` | `entityName` | Visual/card list, optionally using `galleryImageProperty` |
| `form` | `entityName`, `formName` | Standalone form page |
| `dashboard` | `dashboard` | Dashboard visualizations |

Use [Page Groups](page-groups.md) for the `group` reference and menu nesting rules. Use [Dashboards](dashboards.md) for the nested `dashboard` payload, visualization types, and runtime row grouping model.

Runtime routes use the page name:

```text
/dynamic/<page-name>
/dynamic/<page-name>/create
/dynamic/<page-name>/edit/<record-id>
/dynamic/<page-name>/<record-id>
```

## Forms

Forms are named definitions referenced by pages through `formName`, `createFormName`, or `editFormName`.

Use flat field placements inside each group. The current runtime and designer read `layout.tabs[].groups[].fields[]`, where each item references a `fieldId` and assigns `row`, `colSpan`, and optional `colStart`. If those placements are missing or reference the wrong field IDs, the form can still define fields while a group renders empty.

```json
{
  "name": "product-form",
  "entityName": "Acme.Catalog.Product",
  "enableSaveAndNew": true,
  "fields": [
    { "id": "name", "label": "Name", "type": "text", "binding": "Name" },
    { "id": "status", "label": "Status", "type": "select", "binding": "Status", "enumType": "Acme.Catalog.ProductStatus" },
    { "id": "price", "label": "Price", "type": "money", "binding": "Price" }
  ],
  "layout": {
    "tabs": [
      {
        "id": "main",
        "title": "Main",
        "isDefault": true,
        "groups": [
          {
            "id": "details",
            "title": "Details",
            "isDefault": true,
            "fields": [
              { "fieldId": "name", "row": 0, "colSpan": 4 },
              { "fieldId": "status", "row": 1, "colSpan": 2 },
              { "fieldId": "price", "row": 1, "colSpan": 2 }
            ]
          }
        ]
      }
    ]
  }
}
```

Form fields can be `text`, `textarea`, `number`, `checkbox`, `date`, `datetime`, `time`, `file`, `image`, `money`, `select`, `lookup`, `guid`, or `computed`. Form rules can hide, show, disable, enable, or set values for fields/groups.

## Filters

Filters are page-owned. Use `control: "auto"` unless you need a specific control.

| Property type | Typical operators |
|---------------|-------------------|
| `string` | `contains`, `equal`, `notEqual`, `startsWith`, `endsWith`, `notContains`, `hasValue` |
| `int`, `long`, `decimal`, `money` | `between`, `equal`, `notEqual`, `greaterThan`, `greaterThanOrEqual`, `lessThan`, `lessThanOrEqual`, `hasValue` |
| `date`, `dateTime`, `time` | `between`, `equal`, `greaterThan`, `greaterThanOrEqual`, `lessThan`, `lessThanOrEqual`, `hasValue` |
| `boolean` | `All / Yes / No` value selector |
| `enum`, lookup, `guid` | `equal`, `notEqual`, `in`, `notIn`, `hasValue` |
| `file`, `image` | `hasValue` with `All / Yes / No` |

`hasValue` is a UI alias. At runtime, `Yes` maps to `IsNotNull`, `No` maps to `IsNull`, and `All` does not add a filter.

## Permissions

Pages can use generated defaults or explicit permission configuration:

```json
{
  "permissionConfig": {
    "view": "authenticated",
    "create": "Acme.Catalog.Create",
    "update": "Acme.Catalog.Update",
    "delete": "Acme.Catalog.Delete"
  }
}
```

Custom permission definitions live in the top-level `permissions` section and can be granted through the normal ABP permission management UI.

## Scripts

### Interceptors

```json
{
  "interceptors": [
    {
      "commandName": "Create",
      "type": "Pre",
      "javascript": "if (!args.getValue('Name')) { globalError = 'Name is required.'; }"
    }
  ]
}
```

See [Interceptors](interceptors.md) and [Scripting API](scripting-api.md).

### Custom Endpoints

```json
{
  "endpoints": [
    {
      "name": "GetProductStats",
      "route": "/api/custom/products/stats",
      "method": "GET",
      "requireAuthentication": true,
      "requiredPermissions": ["Acme.Catalog"],
      "javascript": "var count = await db.count('Acme.Catalog.Product'); return ok({ total: count });"
    }
  ]
}
```

See [Custom Endpoints](custom-endpoints.md).

### Event Handlers, Jobs, and Workers

```json
{
  "eventHandlers": [
    {
      "name": "NotifyProductPublished",
      "eventName": "Acme.Catalog.ProductPublished",
      "javascript": "context.log('Product published: ' + eventData.id);"
    }
  ],
  "backgroundJobs": [
    {
      "name": "SendProductSummary",
      "javascript": "context.log('Sending summary for ' + jobData.productId);"
    }
  ],
  "backgroundWorkers": [
    {
      "name": "ProductCleanup",
      "period": 3600000,
      "javascript": "context.log('Cleaning product data.');"
    }
  ]
}
```

Background workers require either `period` in milliseconds or `cronExpression`. See [Script Actions](script-actions.md) for event handler, background job, background worker, code editor, and dry-run testing details.

## Complete Example

The complete example below shows the logical aggregate shape. Split projects store each descriptor as its own file, but field shapes are the same.

```json
{
  "enums": [
    {
      "name": "Acme.Catalog.ProductStatus",
      "values": [
        { "name": "Draft", "value": 0 },
        { "name": "Active", "value": 1 },
        { "name": "Archived", "value": 2 }
      ]
    }
  ],
  "entities": [
    {
      "name": "Acme.Catalog.Product",
      "displayName": "Products",
      "displayProperty": "Name",
      "properties": [
        { "name": "Name", "type": "string", "isRequired": true, "validators": [{ "type": "maxLength", "length": 128 }] },
        { "name": "Status", "type": "enum", "enumType": "Acme.Catalog.ProductStatus", "defaultValue": "0" },
        { "name": "Price", "type": "money" },
        { "name": "ReleaseDate", "type": "date" },
        { "name": "CoverImage", "type": "image", "fileAllowedContentTypes": ["image/*"] }
      ]
    }
  ],
  "forms": [
    {
      "name": "product-form",
      "entityName": "Acme.Catalog.Product",
      "fields": [
        { "id": "name", "label": "Name", "type": "text", "binding": "Name" },
        { "id": "status", "label": "Status", "type": "select", "binding": "Status", "enumType": "Acme.Catalog.ProductStatus" },
        { "id": "price", "label": "Price", "type": "money", "binding": "Price" },
        { "id": "releaseDate", "label": "Release Date", "type": "date", "binding": "ReleaseDate" },
        { "id": "coverImage", "label": "Cover Image", "type": "image", "binding": "CoverImage" }
      ],
      "layout": {
        "tabs": [
          {
            "id": "main",
            "title": "Main",
            "isDefault": true,
            "groups": [
              {
                "id": "details",
                "title": "Details",
                "isDefault": true,
                "fields": [
                  { "fieldId": "name", "row": 0, "colSpan": 4 },
                  { "fieldId": "status", "row": 1, "colSpan": 2 },
                  { "fieldId": "price", "row": 1, "colSpan": 2 },
                  { "fieldId": "releaseDate", "row": 2, "colSpan": 2 },
                  { "fieldId": "coverImage", "row": 2, "colSpan": 2 }
                ]
              }
            ]
          }
        ]
      }
    }
  ],
  "pageGroups": [
    { "name": "catalog", "title": "Catalog", "icon": "fa-solid fa-boxes-stacked", "order": 10 }
  ],
  "pages": [
    {
      "name": "products",
      "title": "Products",
      "type": "dataGrid",
      "entityName": "Acme.Catalog.Product",
      "group": "catalog",
      "columns": [
        { "propertyName": "Name", "order": 0, "exportOrder": 0 },
        { "propertyName": "Status", "order": 1, "exportOrder": 1 },
        { "propertyName": "Price", "order": 2, "exportOrder": 2, "exportable": false }
      ],
      "filters": [
        { "propertyName": "Name", "control": "text", "defaultOperator": "contains" },
        { "propertyName": "Status", "control": "select", "defaultOperator": "equal" },
        { "propertyName": "CoverImage", "control": "exists", "defaultOperator": "hasValue" }
      ],
      "createFormName": "product-form",
      "editFormName": "product-form"
    }
  ]
}
```

## Migration Requirements

Entity shape changes require database migrations before they can be used safely:

* New entity
* New persisted property
* Property type change
* Required/nullability change
* Unique index change

In ABP Studio, run the generated migration task for the solution. If you run the application from the command line, use the migration workflow generated by the startup template.

## See Also

* [Low-Code Designer](designer.md)
* [React Runtime](react-runtime.md)
* [Health](health.md)
* [Dashboards](dashboards.md)
* [Page Groups](page-groups.md)
* [MCP Integration](mcp.md)
* [Attributes & Fluent API](fluent-api.md)
* [Interceptors](interceptors.md)
* [Custom Endpoints](custom-endpoints.md)
* [Script Actions](script-actions.md)
* [Scripting API](scripting-api.md)
