## AbpEntityChanges

AbpEntityChanges is used to store entity changes.

### Description

This table stores information about the entity changes in the application. Each record in the table represents a entity change and allows to track the actions performed in the application effectively. For example, you can use the `EntityId`, `EntityTenantId`, `EntityTypeFullName` columns to filter the entity changes by `EntityId`, `EntityTenantId`, `EntityTypeFullName` columns to filter the entity changes by entity id, entity tenant id, and entity type full name respectively, so that you can easily track the actions performed in the application.

### Module

[`Volo.Abp.AuditLogging`](../../Audit-Logging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](AbpAuditLogs.md) | Id | To match the audit log. |

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityPropertyChanges](AbpEntityPropertyChanges.md) | EntityChangeId | To match the entity property changes. |
