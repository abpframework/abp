## AbpLinkUsers

AbpLinkUsers is a table that stores the link users.

### Description

This table stores information about the linked users in the application. Each record in the table represents a linked user and allows to manage and track the linked users effectively. For example, you can use the `SourceUserId`, `SourceTenantId`, `TargetUserId`, `TargetTenantId` columns to filter the linked users by source user id, source tenant id, target user id, and target tenant id respectively, so that you can easily manage and track the linked users in the application. This table is useful for linking multiple user accounts across different tenants or applications to a single user, allowing them to easily switch between their accounts.

### Module

[`Volo.Abp.Identity`](../../Identity.md)