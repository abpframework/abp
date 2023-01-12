## AbpEntityPropertyChanges

AbpEntityPropertyChanges is used to store entity property changes.

### Description

This table stores information about the property changes made to entities in the application. Each record in the table represents a change made to a property of an entity and allows to track the changes effectively. For example, you can use the `EntityChangeId`, `NewValue`, `OriginalValue`, `PropertyName` columns to filter the property changes by entity change id, new value, original value, and property name respectively, so that you can easily track the changes made to the properties of entities in the application.

## Module

[`Volo.Abp.AuditLogging`](../../Audit-Logging.md)

## Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityChanges](AbpEntityChanges.md) | Id | To match the entity change. |