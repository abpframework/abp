```json
//[doc-seo]
{
    "Description": "Learn how to implement the Audit Logging Module in ABP Framework to efficiently save and manage audit logs in your database."
}
```

# Audit Logging Module

The Audit Logging Module basically implements the `IAuditingStore` to save the audit log objects to a database.

> This document covers only the audit logging module which persists audit logs to a database. See [the audit logging](../framework/infrastructure/audit-logging.md) document for more about the audit logging system.

## How to Install

This module comes as pre-installed (as NuGet/NPM packages). You can continue to use it as package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../cli) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/audit-logging). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## Internals

### Domain Layer

#### Aggregates

- `AuditLog` (aggregate root): Represents an audit log record in the system.
  - `EntityChange` (collection): Changed entities of audit log.
  - `AuditLogAction` (collection): Executed actions of audit log.

#### Extending the Entities

The `AuditLog`, `AuditLogAction` and `EntityChange` entities support the [Module Entity Extensions](../framework/architecture/modularity/extending/module-entity-extensions.md) system. Configure them in the `Domain.Shared` project before the database model is created. The following example adds a property to `AuditLog`:

````csharp
ObjectExtensionManager.Instance.Modules()
    .ConfigureAuditLogging(auditLogging =>
    {
        auditLogging.ConfigureAuditLog(auditLog =>
        {
            auditLog.AddOrUpdateProperty<string>("ExternalId");
        });
    });
````

Use `ConfigureAuditLogAction` or `ConfigureEntityChange` in the same way to extend the other supported entities.

#### Repositories

Following custom repositories are defined for this module:

- `IAuditLogRepository`

#### Audit Log Conversion

The module uses `IAuditLogInfoToAuditLogConverter` to convert the `AuditLogInfo` collected by the auditing system into the persisted `AuditLog` aggregate. You can inject this service when you need the same conversion in a custom persistence flow, or replace its default implementation using the [dependency injection system](../framework/fundamentals/dependency-injection.md#replace-a-service) to customize the mapping.

#### Persistence Limits

Before saving an audit log, the module truncates fields that have maximum lengths defined by `AuditLogConsts`, `AuditLogActionConsts`, `EntityChangeConsts` and `EntityPropertyChangeConsts`. Action parameters are handled differently: if `AuditLogAction.Parameters` exceeds `AuditLogActionConsts.MaxParametersLength` (2,000 by default), it is persisted as an empty string instead of being truncated.

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Abp` prefix by default. Set static properties on the `AbpAuditLoggingDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `AbpAuditLogging` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string. See the [connection strings](../framework/fundamentals/connection-strings.md) documentation for details.

#### Entity Framework Core

##### Tables

- **AbpAuditLogs**
  - AbpAuditLogActions
  - AbpEntityChanges
    - AbpEntityPropertyChanges
- **AbpAuditLogExcelFiles**

#### MongoDB

##### Collections

- **AbpAuditLogs**
- **AbpAuditLogExcelFiles**

## See Also

* [Audit logging system](../framework/infrastructure/audit-logging.md)
