## AbpOrganizationUnits

AbpOrganizationUnits is a table that stores the organization units.

### Description

This table stores information about the organization units in the application. Each record in the table represents an organization unit and allows to manage and track the organization units effectively. For example, you can use the `Code`, `ParentId` columns to filter the organization units by code and parent id respectively, so that you can easily manage and track the organization units in the application. This table is useful for creating and managing a hierarchical structure of the organization, allowing to group users and assign roles based on the organization structure.

### Module

[`Volo.Abp.Identity`](../../Identity.md)


### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnits`](AbpOrganizationUnits.md) | `ParentId` | The parent organization unit id. |

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnitRoles`](AbpOrganizationUnitRoles.md) | `OrganizationUnitId` | The organization unit id. |