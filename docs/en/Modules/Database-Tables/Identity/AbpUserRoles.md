## AbpUserRoles

The AbpUserRoles table is used to store user roles.

### Description

This table stores information about the roles assigned to users in the application. Each record in the table represents a role assigned to a user and allows to manage and track the role assignments effectively. This table can be used to manage user-based access control by allowing to assign roles to users, which describe the access rights of the individual user.

### Module

[`Volo.Abp.Identity`](../../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](AbpUsers.md) | Id | The user id. |
| [AbpRoles](AbpRoles.md) | Id | The role id. |
