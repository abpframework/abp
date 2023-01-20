## AbpAuditLogs

AbpAuditLogs is used to store audit logs.

### Description

This table stores information about the audit logs in the application. Each record in the table represents a audit log and allows to track the actions performed in the application effectively. For example, you can use the ApplicationName, UserId, ExecutionTime columns to filter the audit logs by `ApplicationName`, `UserId`, `ExecutionTime` columns to filter the audit logs by application name, user id, and execution time respectively, so that you can easily track the actions performed in the application.

### Module

[`Volo.Abp.AuditLogging`](../Audit-Logging.md)

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogActions](#abpauditlogactions) | AuditLogId | To match the audit log actions. |
| [AbpEntityChanges](#abpentitychanges) | AuditLogId | To match the entity changes. |

---

## AbpAuditLogActions

AbpAuditLogActions is used to store audit log actions.

### Description

This table stores information about the actions performed in the application, which are logged for auditing purposes. Each record in the table represents a single action that has been performed and includes details such as the unique identifier of the audit log, the service and method name, any parameters passed to the method, the execution time and duration, and any additional properties. The Id column is used as the primary key for the table and the AuditLogId column is used to link the action to a specific audit log.

### Module

[`Volo.Abp.AuditLogging`](../Audit-Logging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | To match the audit log. |

---

## AbpEntityChanges

AbpEntityChanges is used to store entity changes.

### Description

This table stores information about the entity changes in the application. Each record in the table represents a entity change and allows to track the actions performed in the application effectively. For example, you can use the `EntityId`, `EntityTenantId`, `EntityTypeFullName` columns to filter the entity changes by `EntityId`, `EntityTenantId`, `EntityTypeFullName` columns to filter the entity changes by entity id, entity tenant id, and entity type full name respectively, so that you can easily track the actions performed in the application.

### Module

[`Volo.Abp.AuditLogging`](../Audit-Logging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | To match the audit log. |

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityPropertyChanges](#abpentitypropertychanges) | EntityChangeId | To match the entity property changes. |

---

## AbpEntityPropertyChanges

AbpEntityPropertyChanges is used to store entity property changes.

### Description

This table stores information about the property changes made to entities in the application. Each record in the table represents a change made to a property of an entity and allows to track the changes effectively. For example, you can use the `EntityChangeId`, `NewValue`, `OriginalValue`, `PropertyName` columns to filter the property changes by entity change id, new value, original value, and property name respectively, so that you can easily track the changes made to the properties of entities in the application.

## Module

[`Volo.Abp.AuditLogging`](../Audit-Logging.md)

## Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityChanges](#abpentitychanges) | Id | To match the entity change. |