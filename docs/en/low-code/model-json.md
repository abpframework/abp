```json
//[doc-seo]
{
    "Description": "Define ABP Low-Code model.json metadata for dynamic entities, pages, forms, filters, permissions, script endpoints, event handlers, background jobs, and workers."
}
```

# model.json Structure

> **Preview:** The Low-Code System is currently in preview. The descriptor format is stable enough for evaluation and source control, but fields may change before general availability.

`model.json` is the source-controlled descriptor format used by the Low-Code Designer and React runtime. Use the [Low-Code Designer](designer.md) for normal editing. Use this page when you need to review, generate, merge, or source-control the JSON metadata directly.

## File Location

Generated low-code applications keep the model file in a `_Dynamic` folder under the application domain project:

```text
YourApp.Domain/
`-- _Dynamic/
    `-- model.json
```

The low-code module discovers this file during application startup. A JSON Schema (`model.schema.json`) is available in the module source and can be referenced with `$schema` for IDE IntelliSense.

```json
{
  "$schema": "path/to/model.schema.json",
  "entities": []
}
```

## Top-Level Sections

The current model format is page/form centered. Entities define data shape; pages and forms define the React runtime UI.

```json
{
  "$schema": "...",
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
      "name": "Acme.Campaigns.CampaignStatus",
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
  "enumType": "Acme.Campaigns.CampaignStatus",
  "defaultValue": "0"
}
```

## Entities

Entities describe the persisted data model. UI is not configured with legacy property `ui` objects. Use page `columns` and `filters`, and named `forms`, for runtime UI behavior.

```json
{
  "name": "Acme.Campaigns.Campaign",
  "displayName": "Campaigns",
  "displayProperty": "Name",
  "properties": [],
  "crossFieldValidations": [],
  "interceptors": []
}
```

| Field | Description |
|-------|-------------|
| `name` | Required stable full entity name, for example `Acme.Campaigns.Campaign` |
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
  "name": "Budget",
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

Use entity `attachments` when each record can have multiple arbitrary files:

```json
{
  "name": "Acme.Campaigns.Campaign",
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
  "name": "campaigns",
  "title": "Campaigns",
  "icon": "fa-solid fa-bullhorn",
  "type": "dataGrid",
  "entityName": "Acme.Campaigns.Campaign",
  "group": "marketing",
  "columns": [
    { "propertyName": "Name", "order": 0 },
    { "propertyName": "Status", "order": 1 },
    { "propertyName": "Budget", "order": 2 }
  ],
  "filters": [
    { "propertyName": "Name", "control": "text", "defaultOperator": "contains" },
    { "propertyName": "Status", "control": "select", "defaultOperator": "equal" }
  ],
  "createFormName": "campaign-form",
  "editFormName": "campaign-form"
}
```

| Page type | Required fields | Purpose |
|-----------|-----------------|---------|
| `dataGrid` | `entityName` | Searchable, sortable CRUD grid |
| `kanban` | `entityName`, `groupByProperty` | Cards grouped by an enum/status-like property |
| `calendar` | `entityName`, `calendarStartProperty` | Records shown on a calendar |
| `gallery` | `entityName` | Visual/card list, optionally using `galleryImageProperty` |
| `form` | `entityName`, `formName` | Standalone form page |
| `dashboard` | `dashboard` | Dashboard visualizations |

Runtime routes use the page name:

```text
/dynamic/<page-name>
/dynamic/<page-name>/create
/dynamic/<page-name>/edit/<record-id>
/dynamic/<page-name>/<record-id>
```

## Forms

Forms are named definitions referenced by pages through `formName`, `createFormName`, or `editFormName`.

```json
{
  "name": "campaign-form",
  "entityName": "Acme.Campaigns.Campaign",
  "enableSaveAndNew": true,
  "fields": [
    { "id": "name", "label": "Name", "type": "text", "binding": "Name" },
    { "id": "status", "label": "Status", "type": "select", "binding": "Status", "enumType": "Acme.Campaigns.CampaignStatus" },
    { "id": "ownerId", "label": "Owner", "type": "lookup", "binding": "OwnerId" }
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
            "rows": [
              { "cells": [{ "fieldId": "name", "colSpan": 4 }] },
              { "cells": [{ "fieldId": "status", "colSpan": 2 }, { "fieldId": "ownerId", "colSpan": 2 }] }
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
    "create": "Acme.Campaigns.Create",
    "update": "Acme.Campaigns.Update",
    "delete": "Acme.Campaigns.Delete"
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
      "name": "GetCampaignStats",
      "route": "/api/custom/campaigns/stats",
      "method": "GET",
      "requireAuthentication": true,
      "requiredPermissions": ["Acme.Campaigns"],
      "javascript": "var count = await db.count('Acme.Campaigns.Campaign'); return ok({ total: count });"
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
      "name": "NotifyCampaignCompleted",
      "eventName": "Acme.Campaigns.CampaignCompleted",
      "javascript": "log('Campaign completed: ' + eventData.id);"
    }
  ],
  "backgroundJobs": [
    {
      "name": "SendCampaignSummary",
      "javascript": "log('Sending summary for ' + jobData.campaignId);"
    }
  ],
  "backgroundWorkers": [
    {
      "name": "CampaignCleanup",
      "period": 3600000,
      "javascript": "log('Cleaning campaign data.');"
    }
  ]
}
```

Background workers require either `period` in milliseconds or `cronExpression`.

## Complete Example

```json
{
  "enums": [
    {
      "name": "Acme.Campaigns.CampaignStatus",
      "values": [
        { "name": "Draft", "value": 0 },
        { "name": "Active", "value": 1 },
        { "name": "Completed", "value": 2 }
      ]
    }
  ],
  "entities": [
    {
      "name": "Acme.Campaigns.Campaign",
      "displayName": "Campaigns",
      "displayProperty": "Name",
      "properties": [
        { "name": "Name", "type": "string", "isRequired": true, "validators": [{ "type": "maxLength", "length": 128 }] },
        { "name": "Status", "type": "enum", "enumType": "Acme.Campaigns.CampaignStatus", "defaultValue": "0" },
        { "name": "Budget", "type": "money" },
        { "name": "StartDate", "type": "date" },
        { "name": "CoverImage", "type": "image", "fileAllowedContentTypes": ["image/*"] }
      ]
    }
  ],
  "forms": [
    {
      "name": "campaign-form",
      "entityName": "Acme.Campaigns.Campaign",
      "fields": [
        { "id": "name", "label": "Name", "type": "text", "binding": "Name" },
        { "id": "status", "label": "Status", "type": "select", "binding": "Status", "enumType": "Acme.Campaigns.CampaignStatus" },
        { "id": "budget", "label": "Budget", "type": "money", "binding": "Budget" },
        { "id": "startDate", "label": "Start Date", "type": "date", "binding": "StartDate" },
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
                "rows": [
                  { "cells": [{ "fieldId": "name", "colSpan": 4 }] },
                  { "cells": [{ "fieldId": "status", "colSpan": 2 }, { "fieldId": "budget", "colSpan": 2 }] },
                  { "cells": [{ "fieldId": "startDate", "colSpan": 2 }, { "fieldId": "coverImage", "colSpan": 2 }] }
                ]
              }
            ]
          }
        ]
      }
    }
  ],
  "pageGroups": [
    { "name": "marketing", "title": "Marketing", "icon": "fa-solid fa-bullhorn", "order": 10 }
  ],
  "pages": [
    {
      "name": "campaigns",
      "title": "Campaigns",
      "type": "dataGrid",
      "entityName": "Acme.Campaigns.Campaign",
      "group": "marketing",
      "columns": [
        { "propertyName": "Name", "order": 0 },
        { "propertyName": "Status", "order": 1 },
        { "propertyName": "Budget", "order": 2 }
      ],
      "filters": [
        { "propertyName": "Name", "control": "text", "defaultOperator": "contains" },
        { "propertyName": "Status", "control": "select", "defaultOperator": "equal" },
        { "propertyName": "CoverImage", "control": "exists", "defaultOperator": "hasValue" }
      ],
      "createFormName": "campaign-form",
      "editFormName": "campaign-form"
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
* [Attributes & Fluent API](fluent-api.md)
* [Interceptors](interceptors.md)
* [Custom Endpoints](custom-endpoints.md)
* [Scripting API](scripting-api.md)
