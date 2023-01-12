## AbpAuditLogActions

AbpAuditLogActions is used to store audit log actions.

### Description

This table stores information about the actions performed in the application, which are logged for auditing purposes. Each record in the table represents a single action that has been performed and includes details such as the unique identifier of the audit log, the service and method name, any parameters passed to the method, the execution time and duration, and any additional properties. The Id column is used as the primary key for the table and the AuditLogId column is used to link the action to a specific audit log.

### Module

[`Volo.Abp.AuditLogging`](../../Audit-Logging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](AbpAuditLogs.md) | Id | To match the audit log. |
