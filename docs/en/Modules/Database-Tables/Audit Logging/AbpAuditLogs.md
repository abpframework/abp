## AbpAuditLogs

AbpAuditLogs is used to store audit logs.

### Description

This table stores information about the audit logs in the application. Each record in the table represents a audit log and allows to track the actions performed in the application effectively. For example, you can use the ApplicationName, UserId, ExecutionTime columns to filter the audit logs by `ApplicationName`, `UserId`, `ExecutionTime` columns to filter the audit logs by application name, user id, and execution time respectively, so that you can easily track the actions performed in the application.

### Module

[`Volo.Abp.AuditLogging`](../../Audit-Logging.md)

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogActions](AbpAuditLogActions.md) | AuditLogId | To match the audit log actions. |
| [AbpEntityChanges](AbpEntityChanges.md) | AuditLogId | To match the entity changes. |