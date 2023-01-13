## AbpUserOrganizationUnits

The AbpUserOrganizationUnits table is used to store user organization units.

### Description

This table stores information about the organization units assigned to users in the application. Each record in the table represents a user and organization unit assignment and allows to manage and track the user-organization unit assignments effectively. This table can be used to manage user-organization unit relationships, and to group users based on the organization structure.

### Module

[`Volo.Abp.Identity`](../../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](AbpUsers.md) | Id | The user id. |
| [AbpOrganizationUnits](AbpOrganizationUnits.md) | Id | The organization unit id. |