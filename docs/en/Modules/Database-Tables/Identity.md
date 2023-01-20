## AbpUsers

The AbpUsers table is used to store users.

### Description

This table stores information about the users in the application. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application, and for controlling access to different parts of the application based on user information.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [AbpUserClaims](#abpuserclaims) | UserId | To match the user with its claims. |
| [AbpUserLogins](#abpuserlogins) | UserId | To match the user with its logins. |
| [AbpUserRoles](#abpuserroles) | UserId | To match the user with its roles. |
| [AbpUserTokens](#abpusertokens) | UserId | To match the user with its tokens. |
| [AbpUserOrganizationUnits](#abpuserorganizationunits) | UserId | To match the user with its organization units. |

---

## AbpRoles

AbpRoles is a table that stores the roles.

### Description

This table stores information about the roles in the application. Each record in the table represents a role and allows to manage and track the roles effectively. Roles are used to manage and control access to different parts of the application by assigning permissions and claims to roles and then assigning those roles to users. This table is important for managing and organizing the roles in the application, and for defining the access rights of the users.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [AbpRoleClaims](#abproleclaims) | RoleId | To match the role with its claims. |
| [AbpUserRoles](#abpuserroles) | RoleId | To match the role with its users. |
| [AbpOrganizationUnitRoles](#abporganizationunitroles) | RoleId | To match the role with its organization units. |

---

## AbpClaimTypes

AbpClaimTypes is a table that stores the information of the claim types.

### Description

This table stores information about the claim types used in the application. Each record in the table represents a claim type and allows to manage and track the claim types effectively. For example, you can use the `Name`, `Regex` columns to filter the claim types by name, and regex pattern respectively, so that you can easily manage and track the claim types in the application. This table is important for managing and controlling the user claims, which are used to describe the user's identity and access rights.

### Module

[`Volo.Abp.Identity`](../Identity.md)

---

## AbpLinkUsers

AbpLinkUsers is a table that stores the link users.

### Description

This table stores information about the linked users in the application. Each record in the table represents a linked user and allows to manage and track the linked users effectively. For example, you can use the `SourceUserId`, `SourceTenantId`, `TargetUserId`, `TargetTenantId` columns to filter the linked users by source user id, source tenant id, target user id, and target tenant id respectively, so that you can easily manage and track the linked users in the application. This table is useful for linking multiple user accounts across different tenants or applications to a single user, allowing them to easily switch between their accounts.

### Module

[`Volo.Abp.Identity`](../Identity.md)

---

## AbpUserClaims

The AbpUserClaims table is used to store user claims.

### Description

This table stores information about the claims assigned to users in the application. Each record in the table represents a claim assigned to a user and allows to manage and track the claims effectively. This table can be used to manage user-based access control by allowing to assign claims to users, which describe the access rights of the individual user.


### Module

[`Volo.Abp.Identity`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | To match the user with its claims. |

---

## AbpUserLogins

The AbpUserLogins table is used to store user logins.

### Description

This table stores information about the logins of the users in the application. Each record in the table represents a user login and allows to manage and track the user logins effectively. This table can be used to store information about user's external logins such as login with facebook, google, etc and also it can be used to track login history of users.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | To match the user with its logins. |

---

## AbpUserRoles

The AbpUserRoles table is used to store user roles.

### Description

This table stores information about the roles assigned to users in the application. Each record in the table represents a role assigned to a user and allows to manage and track the role assignments effectively. This table can be used to manage user-based access control by allowing to assign roles to users, which describe the access rights of the individual user.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | To match the user with its roles. |
| [AbpRoles](#abproles) | Id | To match the role with its users. |

---

## AbpUserTokens

The AbpUserTokens table is used to store user tokens.

### Description

This table stores information about the tokens of the users in the application. Each record in the table represents a token for a user and allows to manage and track the user tokens effectively. This table can be used to store information about user's refresh tokens, access tokens and other tokens used in the application. It can also be used to invalidate or revoke user tokens.

### Module

[Volo.Abp.Identity](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | To match the user with its tokens. |

---

## AbpOrganizationUnits

AbpOrganizationUnits is a table that stores the organization units.

### Description

This table stores information about the organization units in the application. Each record in the table represents an organization unit and allows to manage and track the organization units effectively. For example, you can use the `Code`, `ParentId` columns to filter the organization units by code and parent id respectively, so that you can easily manage and track the organization units in the application. This table is useful for creating and managing a hierarchical structure of the organization, allowing to group users and assign roles based on the organization structure.

### Module

[`Volo.Abp.Identity`](../Identity.md)


### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnits`](#abporganizationunits) | `ParentId` | The parent organization unit id. |

### Used by

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnitRoles`](#abporganizationunitroles) | `OrganizationUnitId` | To match the organization unit with its roles. |
| [`AbpUserOrganizationUnits`](#abpuserorganizationunits) | `OrganizationUnitId` | To match the organization unit with its users. |
---

## AbpOrganizationUnitRoles

AbpOrganizationUnitRoles is a table that stores the organization unit roles.

### Description

This table stores information about the roles assigned to organization units in the application. Each record in the table represents a role assigned to an organization unit and allows to manage and track the roles effectively. For example, you can use the `OrganizationUnitId`, `RoleId` columns to filter the roles by organization unit id and role id respectively, so that you can easily manage and track the roles assigned to organization units in the application. This table is useful for managing role-based access control at the level of organization units, allowing to assign different roles to different parts of the organization structure.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`AbpOrganizationUnits`](#abporganizationunits) | `Id` | To match the organization unit with its roles. |
| [`AbpRoles`](#abproles) | `Id` | To match the role with its organization units. |

---

## AbpUserOrganizationUnits

The AbpUserOrganizationUnits table is used to store user organization units.

### Description

This table stores information about the organization units assigned to users in the application. Each record in the table represents a user and organization unit assignment and allows to manage and track the user-organization unit assignments effectively. This table can be used to manage user-organization unit relationships, and to group users based on the organization structure.

### Module

[`Volo.Abp.Identity`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | To match the user with its organization units. |
| [AbpOrganizationUnits](#abporganizationunits) | Id | To match the organization unit with its users. |

---

## AbpRoleClaims

AbpRoleClaims is used to store role claims.

### Description

This table stores information about the claims assigned to roles in the application. Each record in the table represents a claim assigned to a role and allows to manage and track the claims effectively. This table is useful for managing role-based access control by allowing to assign claims to roles, which describe the access rights of the users that belong to that role.

### Module

[`Volo.Abp.PermissionManagement`](../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpRoles](#abproles) | Id | To match the role with its claims. |

---

## AbpSecurityLogs

The AbpSecurityLogs table is used to store security logs.

### Description

This table stores security logs of the application. Each record in the table represents a security event, and the table allows the management and tracking of security events effectively. This table is useful for auditing and troubleshooting the application, by providing a history of who did what and when.

### Module

[`Volo.Abp.Security`](../Identity.md)