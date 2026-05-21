```json
//[doc-seo]
{
    "Description": "ABP Low-Code System: design dynamic entities, forms, pages, permissions, menus, filters, and React runtime pages with the Admin Console Low-Code Designer."
}
```

# Low-Code System

> You must have an ABP Team or a higher license to use this module.

> **Preview:** The Low-Code System is currently in preview. APIs, designer behavior, generated metadata, and React runtime details may change before general availability. Use it for evaluation and controlled projects, and review release notes before upgrading.

The ABP Low-Code System lets you build data-driven admin screens from metadata. The primary workflow is the **Low-Code Designer** in ABP Admin Console, backed by the **React runtime** in your application.

Use the designer to model entities, enums, properties, relations, pages, forms, filters, permissions, actions, and health checks. The runtime uses the same metadata to provide:

* CRUD REST APIs
* EF Core dynamic entity tables
* Permission definitions
* Dynamic menu items
* React data grid, kanban, calendar, gallery, form, and dashboard pages
* Create and edit forms
* Advanced filters
* Excel and CSV export

No DTO, repository, application service, controller, or React CRUD page is required for the standard flow.

![Low-Code Designer overview](images/designer-overview.png)

## Supported UI

Low-Code runtime UI is currently documented for **React**. The backend model, APIs, permissions, scripting, and custom endpoint infrastructure are shared by the module, but the UI walkthroughs in this section focus on Admin Console plus React.

## How to Enable

The Low-Code System is an optional startup template feature. When creating a new application with [ABP Studio](../studio/index.md), choose a modern React application template and enable **Low-Code System** in the project creation wizard.

ABP Studio creates the required backend module references, dynamic model initializer, EF Core configuration, Admin Console integration, and React runtime wiring.

The generated React project includes:

* `@volo/abp-react-lowcode`
* `configureLowCode`
* `LowCodeLocalizationProvider`
* `createDynamicRoutes`
* `useMenuItems`
* Page, form, dashboard, file, and attachment hooks

The host application wires the low-code modules, calls the generated `_Dynamic` initializer, configures EF Core dynamic entities, and seeds the required OpenIddict clients.

## Run the Application

After ABP Studio creates the solution, use **Solution Runner** to run the backend host and the React application. Run the database migration task before opening the runtime pages.

The generated solution README contains the exact command-line equivalents if you prefer to run the projects outside ABP Studio.

If you generate a solution inside another repository, make sure parent build files such as `Directory.Packages.props` are not inherited accidentally. Use an empty output folder outside another solution, or isolate the generated solution's MSBuild configuration before running `dotnet build`.

Open Admin Console and navigate to **Low-Code Designer** after the backend is running:

```text
https://localhost:<host-port>/admin-console/lowcode-designer
```

Open generated runtime pages after the React application is running:

```text
http://localhost:<react-port>/dynamic/<page-name>
```

## Designer Workflow

The designer is the day-to-day entry point.

1. Use **Data** to create entities, enums, properties, and relations.
2. Use **Pages** to choose a page type, menu placement, fields, default sorting, filters, dashboards, and linked forms.
3. Use **Forms** to arrange create and edit forms with tabs, groups, controls, validations, and actions.
4. Use **Permissions** to review generated permissions and control access.
5. Use **Actions** and **Interceptors** when the standard CRUD flow needs custom logic.
6. Use **Health** to review model issues before publishing changes.

![Entity properties in the designer](images/designer-properties.png)

![Form setup in the designer](images/designer-forms.png)

## React Runtime

React runtime pages are generated from the same metadata. The page below was produced from a low-code page definition and includes the grid, menu item, permissions, display values, export, create form, and filters. The same runtime can render kanban, calendar, gallery, standalone form, and dashboard page definitions.

![Generated React data grid](images/runtime-data-grid.png)

![Generated React advanced filters](images/runtime-filters.png)

![Generated React create form](images/runtime-create-form.png)

## Filters

React low-code filters are type-aware. The runtime shows only operators that make sense for the field type. For example:

* Text fields support contains, equals, starts with, ends with, and has value.
* Numeric fields support equals, comparison, between, and has value.
* Date fields use date-friendly labels such as on, after, before, and between.
* Boolean fields use an `All / Yes / No` value selector.
* File and image fields use `Has value` with an `All / Yes / No` value selector.

`All` means no filter is applied. `Yes` maps to non-empty values. `No` maps to empty values.

![Has value filter options](images/runtime-filters-has-value.png)

## Export

Every dynamic entity page can export filtered data to Excel or CSV. Export requests use the same search, sorting, and filter input as the list endpoint. Server-only fields are excluded and foreign key values are displayed through their configured display property.

| Endpoint | Description |
|----------|-------------|
| `GET /api/low-code/pages/{pageName}/download-token` | Gets a short-lived download token |
| `GET /api/low-code/pages/{pageName}/export/excel` | Exports filtered data as Excel |
| `GET /api/low-code/pages/{pageName}/export/csv` | Exports filtered data as CSV |

## Advanced Configuration

The designer stores and reads the same model metadata described in the reference pages below. Use these pages when you need source-controlled model files, custom startup wiring, script handlers, or low-level integration details.

| Topic | Use it for |
|-------|------------|
| [Designer](designer.md) | Admin Console tabs, entity/page/form setup, permissions, and health |
| [React Runtime](react-runtime.md) | React package wiring, routes, menu items, filters, forms, and export |
| [Attributes & Fluent API](fluent-api.md) | Source-controlled C# metadata and runtime overrides |
| [model.json Structure](model-json.md) | JSON descriptor format used by the designer and runtime |
| [Reference Entities](reference-entities.md) | Lookups to existing entities such as Identity users |
| [Foreign Access](foreign-access.md) | Access to related dynamic entities through relations |
| [Interceptors](interceptors.md) | JavaScript lifecycle logic for CRUD operations |
| [Custom Endpoints](custom-endpoints.md) | JavaScript-backed REST endpoints |
| [Scripting API](scripting-api.md) | Server-side script context and helpers |

## Runtime Internals

The generated pages are powered by these services:

* `DynamicEntityAppService` handles CRUD, list queries, filtering, sorting, and export.
* `DynamicPageAppService` exposes page-based CRUD, file, attachment, lookup, child, foreign-access, and export endpoints.
* `DynamicEntityUIAppService` returns page, form, dashboard, field, filter, and menu metadata.
* `DynamicPermissionDefinitionProvider` creates permissions for dynamic entities.
* `CustomEndpointExecutor` runs JavaScript-backed custom endpoints.
* EF Core maps dynamic entities as shared-type entities.

## See Also

* [Low-Code Designer](designer.md)
* [React Runtime](react-runtime.md)
* [model.json Structure](model-json.md)
