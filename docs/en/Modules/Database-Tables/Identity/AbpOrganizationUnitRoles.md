## AbpOrganizationUnitRoles

AbpOrganizationUnitRoles is a table that stores the organization unit roles.

### Description

This table stores information about the roles assigned to organization units in the application. Each record in the table represents a role assigned to an organization unit and allows to manage and track the roles effectively. For example, you can use the `OrganizationUnitId`, `RoleId` columns to filter the roles by organization unit id and role id respectively, so that you can easily manage and track the roles assigned to organization units in the application. This table is useful for managing role-based access control at the level of organization units, allowing to assign different roles to different parts of the organization structure.

### Module

[`Volo.Abp.Identity`](../../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnits`](AbpOrganizationUnits.md) | `Id` | The organization unit id. |
| [`AbpRoles`](../AbpRoles.md) | `Id` | The role id. |