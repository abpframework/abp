# Audit Logging Module

The Audit Logging Module basically implements the `IAuditingStore` to save the audit log objects to a database.

> This document covers only the audit logging module which persists audit logs to a database. See [the audit logging](../Audit-Logging.md) document for more about the audit logging system.

## How to Install

This module comes as pre-installed (as NuGet/NPM packages) when you [create a new solution](https://abp.io/get-started) with the ABP Framework. You can continue to use it as package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../CLI.md) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/audit-logging). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## Internals

### Domain Layer

#### Aggregates

- `AuditLog` (aggregate root): Represents an audit log record in the system.
  - `EntityChange` (collection): Changed entities of audit log.
  - `AuditLogAction` (collection): Executed actions of audit log.

#### Repositories

Following custom repositories are defined for this module:

- `IAuditLogRepository`

### Database providers

#### Common

##### Table / collection prefix & schema

All tables/collections use the `Abp` prefix by default. Set static properties on the `AbpAuditLoggingDbProperties` class if you need to change the table prefix or set a schema name (if supported by your database provider).

##### Connection string

This module uses `AbpAuditLogging` for the connection string name. If you don't define a connection string with this name, it fallbacks to the `Default` connection string. See the [connection strings](https://docs.abp.io/en/abp/latest/Connection-Strings) documentation for details.

#### Entity Framework Core

##### Tables

- **AbpAuditLogs**
  - AbpAuditLogActions
  - AbpEntityChanges
    - AbpEntityPropertyChanges

#### MongoDB

##### Collections

- **AbpAuditLogs**

## See Also

* [Audit logging system](../Audit-Logging.md)