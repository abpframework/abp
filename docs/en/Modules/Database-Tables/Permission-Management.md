## AbpPermissionGroups

AbpPermissionGroups is used to store permission groups.

### Description

This table stores information about the permission groups in the application. Each record in the table represents a permission group and allows to manage and track the permission groups effectively. This table is important for managing and organizing the permissions in the application, by grouping them into logical categories.

### Module

[`Volo.Abp.PermissionManagement`](../Permission-Management.md)

---

## AbpPermissions

The AbpPermissions table is used to store permission definitions.

### Description

This table stores information about the permissions in the application. Each record in the table represents a permission and allows to manage and track the permissions effectively. This table is important for managing and controlling access to different parts of the application and for defining the granular permissions that make up the larger permissions or roles.

### Module

[`Volo.Abp.PermissionManagement`](../Permission-Management.md)

---

## AbpPermissionGrants

AbpPermissionGrants is used to store granted permissions for a role or a user.

### Description

This table stores information about the granted permissions in the application. Each record in the table represents a granted permission and allows to manage and track the granted permissions effectively. The table is used to store and manage the permissions in the application and to keep track of permissions that are granted, to whom and when. The columns such as `Name`, `ProviderName`, `ProviderKey`, `TenantId` can be used to filter the granted permissions by name, provider name, provider key, and tenant id respectively, so that you can easily manage and track the granted permissions in the application. This table is important for managing and controlling the access to different parts of the application, and also for auditing purposes.

### Module

[`Volo.Abp.PermissionManagement`](../Permission-Management.md)