## AbpAuditLogs

AbpAuditLogs is used to store audit logs.

### Description

This table stores information about the audit logs in the application. Each record in the table represents a audit log and allows to track the actions performed in the application effectively. For example, you can use the ApplicationName, UserId, ExecutionTime columns to filter the audit logs by `ApplicationName`, `UserId`, `ExecutionTime` columns to filter the audit logs by application name, user id, and execution time respectively, so that you can easily track the actions performed in the application.

### Module

[`Volo.Abp.AuditLogging`](Audit-Logging.md)

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

[`Volo.Abp.AuditLogging`](Audit-Logging.md)

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

[`Volo.Abp.AuditLogging`](Audit-Logging.md)

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

[`Volo.Abp.AuditLogging`](Audit-Logging.md)

## Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityChanges](#abpentitychanges) | Id | To match the entity change. |

---

## AbpBackgroundJobs

AbpBackgroundJobs is used to store background jobs.

### Description

This table stores information about the background jobs in the application. Each record in the table represents a background job and allows to manage and track the background jobs effectively. For example, you can use the `JobName`, `JobArgs`, `TryCount`, `NextTryTime`, `LastTryTime`, `IsAbandoned`, `Priority` columns to filter the background jobs by job name, job arguments, try count, next try time, last try time, abandoned status, and priority respectively, so that you can easily manage and track the background jobs in the application.

### Module

[`Volo.Abp.BackgroundJobs`](Background-Jobs.md)

---

## AbpBlobContainers

AbpBlobContainers is a table that stores the information of the blob container.

### Description

This table stores information about the BLOB (binary large object) containers in the application. Each record in the table represents a blob container and allows to manage and organize the blobs effectively. For example, you can use the `ContainerId` column to link each blob with its corresponding container in the [`AbpBlobs`](#abpblobs) table, so that you can easily manage and organize the blobs.

## Module

`Volo.Abp.BlobStoring`

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobs](#abpblobs) | ContainerId | To match the blob. |

---

## AbpBlobs

AbpBlobs is a table to store blobs.

### Description

This table stores the binary data of BLOBs (binary large objects) in the application. Each record in the table represents a BLOB and allows to manage and track the BLOBs effectively. Each BLOB is related to a container in the "AbpBlobContainers" table, where the container name, tenant id and other properties of the container can be found.

### Module

`Volo.Abp.BlobStoring`

### Uses 

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobContainers](#abpblobcontainers) | Id | To match the blob container. |

---

## AbpTenants

AbpTenants Table is used to store tenants.

### Description

This table stores information about the tenants. Each record in the table represents a tenant and allows to manage and track the tenants effectively. This table is important for managing and tracking the tenants of the application.

### Module

[`Volo.Abp.TenantManagement`](Tenant-Management.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`AbpTenantConnectionStrings`](#AbpTenantconnectionstrings) | TenantId | To match the tenant connection string with the tenant. |

---

## AbpTenantConnectionStrings

AbpTenantConnectionStrings Table is used to store tenant connection strings.

### Description

This table stores information about the tenant connection strings. Each record in the table represents a tenant connection string and allows to manage and track the tenant connection strings effectively. This table is important for managing and tracking the tenant connection strings of the application.

### Module

[`Volo.Abp.TenantManagement`](Tenant-Management.md)

## Uses

| Table | Column | Description |
| --- | --- | --- |
| [`AbpTenants`](#AbpTenants) | Id | To match the tenant connection string with the tenant. |

---

## BlgBlogs

BlgBlogs Table is used to store blogs.

### Description

This table stores information about the blogs. Each record in the table represents a blog and allows to manage and track the blogs effectively. This table is important for managing and tracking the blogs of the application.

### Module

`Volo.Blogging`

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPosts`](#blgposts) | BlogId | To match the post with the blog. |

---

## BlgPosts

BlgPosts Table is used to store posts.

### Description

This table stores information about the posts. Each record in the table represents a post and allows to manage and track the posts effectively. This table is important for managing and tracking the posts of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgBlogs`](#blgblogs) | Id | To match the post with the blog. |

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgComments`](#blgcomments) | PostId | To match the comment with the post. |
| [`BlgPostTags`](#blgposttags) | PostId | To match the post tag with the post. |

---

## BlgComments

BlgComments Table is used to store comments.

### Description

This table stores information about the comments. Each record in the table represents a comment and allows to manage and track the comments effectively. This table is important for managing and tracking the comments of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPosts`](#blgposts) | Id | To match the comment with the post. |
| [`BlgComments`](#blgcomments) | Id | To match the comment with the parent comment. |

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgComments`](#blgcomments) | RepliedCommentId | To match the comment with the parent comment. |

---

## BlgTags

BlgTags Table is used to store tags.

### Description

This table stores information about the tags. Each record in the table represents a tag and allows to manage and track the tags effectively. This table is important for managing and tracking the tags of the application.

### Module

`Volo.Blogging`

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`BlgPostTags`](#blgposttags) | TagId | To match the post tag with the tag. |

---

## BlgPostTags

BlgPostTags Table is used to store post tags.

### Description

This table stores information about the post tags. Each record in the table represents a post tag and allows to manage and track the post tags effectively. This table is important for managing and tracking the post tags of the application.

### Module

`Volo.Blogging`

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`BlgTags`](#blgtags) | Id | To match the post tag with the tag. |
| [`BlgPosts`](#blgposts) | Id | To match the post tag with the post. |

---

## BlgUsers

BlgUsers Table is used to store users.

### Description

This table stores information about the users. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application.

### Module

`Volo.Blogging`

---

## CmsUsers

CmsUsers Table is used to store users.

### Description

This table stores information about the users. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Index.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [CmsBlogPosts](#cmsblogposts) | AuthorId | To match the author. |

---

## CmsBlogs

CmsBlogs table is used to store blogs.

### Description

This table stores information about the blogs. Each record in the table represents a blog and allows to manage and track the blogs effectively. This table is important for managing and tracking the blogs of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Blogging.md)

---

## CmsBlogPosts

CmsBlogPosts table is used to store blog posts.

### Description

This table stores information about the blog posts. Each record in the table represents a blog post and allows to manage and track the blog posts effectively. This table is important for managing and tracking the blog posts of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Blogging.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [CmsUsers](#cmsusers) | Id | To match the author. |

---

## CmsBlogFeatures

The CmsBlogFeatures table is used to store blog features.

### Description

This table stores information about the blog features. Each record in the table represents a blog feature and allows to manage and track the blog features effectively. This table is important for managing and tracking the blog features of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Blogging.md)

---

## CmsComments

The CmsComments table is used to store comments for CMS pages. The table is used by the CmsComments component.

### Description

This table stores information about the comments. Each record in the table represents a comment and allows to manage and track the comments effectively. This table is important for managing and tracking the comments of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Comments.md)

---

## CmsTags

CmsTags Table is used to store tags.

### Description

This table stores information about the tags. Each record in the table represents a tag and allows to manage and track the tags effectively. This table is important for managing and tracking the tags of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Tags.md)

---

## CmsEntityTags

The CmsEntityTags table is used to store the tags and their relations with entities. The table is used by the CmsTags component.

### Description

This table stores information about the tags associated with a entity. Each record in the table represents a tag associated with a entities and allows to manage and track the tags associated with a entity effectively. This table is important for managing and tracking the tags associated with a entity of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Tags.md)

---

## CmsGlobalResources

CmsGlobalResources Table is used to store global resources.

### Description

This table stores information about the global resources. Each record in the table represents a global resource and allows to manage and track the global resources effectively. This table is important for managing and tracking the global resources of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Global-Resources.md)

---

## CmsMediaDescriptors

CmsMediaDescriptors Table is used to store media descriptors.

### Description

This table stores information about the media descriptors. Each record in the table represents a media descriptor and allows to manage and track the media descriptors effectively. This table is important for managing and tracking the media descriptors of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Index.md)

---

## CmsMenuItems

CmsMenuItems Table is used to store menu items.

### Description

This table stores information about the menu items. Each record in the table represents a menu item and allows to manage and track the menu items effectively. This table is important for managing and tracking the menu items of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Menus.md)

---

## CmsPages

CmsPages Table is used to store pages.

### Description

This table stores the pages in the application. Each record in the table represents a page and allows to manage and track the pages effectively. This table can be used to store information about pages, including the page title, content, URL, creation date, and other relevant information.

### Module

[`Volo.CmsKit`](Cms-Kit/Pages.md)

---

## CmsRatings

CmsRatings Table is used to store ratings.

### Description

This table stores information about the ratings. Each record in the table represents a rating and allows to manage and track the ratings effectively. This table is important for managing and tracking the ratings of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Ratings.md)

---

## CmsUserReactions

CmsUserReactions Table is used to store user reactions.

### Description

This table stores information about the user reactions. Each record in the table represents a user reaction and allows to manage and track the user reactions effectively. This table is important for managing and tracking the user reactions of the application.

### Module

[`Volo.CmsKit`](Cms-Kit/Reactions.md)

---

## DocsProjects

DocsProjects Table is used to store documentation projects.

### Description

This table stores information about the documentation projects. Each record in the table represents a documentation project and allows to manage and track the documentation projects effectively. This table is important for managing and tracking the documentation projects of the application.

### Module

[`Volo.Docs`](Docs.md)

---

## DocsDocuments

DocsDocuments Table is used to store documents.

### Description

This table stores information about the documents. Each record in the table represents a document and allows to manage and track the documents effectively. This table is important for managing and tracking the documents of the application.

### Module

[`Volo.Docs`](Docs.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [DocsDocumentContributors](#docsdocumentcontributors) | DocumentId | To match the document. |

---

## DocsDocumentContributors

DocsDocumentContributors Table is used to store document contributors.

### Description

This table stores information about the document contributors. Each record in the table represents a document contributor and allows to manage and track the document contributors effectively. This table is important for managing and tracking the document contributors of the application.

### Module

[`Volo.Docs`](Docs.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [DocsDocuments](#docsdocuments) | Id | To match the document. |

---

## AbpFeatureGroups

AbpFeatureGroups is used to store feature groups.

### Description

This table stores information about the feature groups in the application. Each record in the table represents a feature group and allows to manage and organize the features effectively. For example, you can group all the features in the [`AbpFeatures`](#abpfeatures) table related to the `Identity` module under the `Identity` group.

### Module

[`Volo.Abp.Features`](Feature-Management.md)

---

## AbpFeatures

AbpFeatures is used to store features.

### Description

This table stores information about the features in the application. Each record in the table represents a feature and allows to manage and organize the features effectively. For example, you can use the `Name` column to link each feature with its corresponding feature value in the [`AbpFeatureValues`](#abpfeaturevalues) table, so that you can easily manage and organize the features.

### Module

[`Volo.Abp.FeatureManagement`](Feature-Management.md)

---

## AbpFeatureValues

AbpFeatureValues is used to store feature values for providers.

### Description

This table stores the values of the features for different providers. Each record in the table represents a feature value and allows to manage the application features values effectively. For example, you can use the `Name` column to link each feature value with its corresponding feature in the [`AbpFeatures`](#abpfeatures) table, so that you can easily manage and organize the features.

### Module

[`Volo.Abp.FeatureManagement`](Feature-Management.md)

---

## AbpUsers

The AbpUsers table is used to store users.

### Description

This table stores information about the users in the application. Each record in the table represents a user and allows to manage and track the users effectively. This table is important for managing and tracking the users of the application, and for controlling access to different parts of the application based on user information.

### Module

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

---

## AbpLinkUsers

AbpLinkUsers is a table that stores the link users.

### Description

This table stores information about the linked users in the application. Each record in the table represents a linked user and allows to manage and track the linked users effectively. For example, you can use the `SourceUserId`, `SourceTenantId`, `TargetUserId`, `TargetTenantId` columns to filter the linked users by source user id, source tenant id, target user id, and target tenant id respectively, so that you can easily manage and track the linked users in the application. This table is useful for linking multiple user accounts across different tenants or applications to a single user, allowing them to easily switch between their accounts.

### Module

[`Volo.Abp.Identity`](Identity.md)

---

## AbpUserClaims

The AbpUserClaims table is used to store user claims.

### Description

This table stores information about the claims assigned to users in the application. Each record in the table represents a claim assigned to a user and allows to manage and track the claims effectively. This table can be used to manage user-based access control by allowing to assign claims to users, which describe the access rights of the individual user.


### Module

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

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

[Volo.Abp.Identity](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)


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

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

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

[`Volo.Abp.PermissionManagement`](Identity.md)

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

[`Volo.Abp.Identity`](Identity.md)

---

## AbpLanguages

AbpLanguages is a table to store languages.

### Description

This table stores information about the languages supported in the application. Each record in the table represents a language and allows to manage and track the languages effectively. This table is important for supporting multiple languages in the application and for providing a better user experience by allowing users to switch between different languages.

### Module
    
`Volo.Abp.Localization`

---

## AbpLanguageTexts

AbpLanguageTexts is a table that stores the localization texts.

### Description

This table stores the localized text for different languages in the application. Each record in the table represents a localized text and allows to manage and track the localized texts effectively. This table is important for providing a better user experience by allowing the application to display text in the user's preferred language.

### Module

`Volo.Abp.Localization`

---

## AbpLocalizationResources

AbpLocalizationResources is a table that stores the localization resources.

### Description

This table stores the localization resources for the application. Each record in the table represents a localization resource and allows to manage and track the resources effectively. This table is important for providing a better user experience by allowing the application to support multiple resources and providing localized text and other localization-specific features.

### Module

`Volo.Abp.Localization`

---

## AbpLocalizationTexts

AbpLocalizationTexts is a table that stores the localization texts.

### Description

This table stores the localization texts in the application. Each record in the table represents a localization text for a specific resource, culture and language. The table contains the resource name, culture name, and a json encoded value which holds the key-value pair of localization text. It allows for efficient storage and management of localization texts and allows for easy update or addition of new translations for specific resources and cultures.

### Module

`Volo.Abp.Localization`

---

## AbpPermissionGroups

AbpPermissionGroups is used to store permission groups.

### Description

This table stores information about the permission groups in the application. Each record in the table represents a permission group and allows to manage and track the permission groups effectively. This table is important for managing and organizing the permissions in the application, by grouping them into logical categories.

### Module

[`Volo.Abp.PermissionManagement`](Permission-Management.md)

---

## AbpPermissions

The AbpPermissions table is used to store permission definitions.

### Description

This table stores information about the permissions in the application. Each record in the table represents a permission and allows to manage and track the permissions effectively. This table is important for managing and controlling access to different parts of the application and for defining the granular permissions that make up the larger permissions or roles.

### Module

[`Volo.Abp.PermissionManagement`](Permission-Management.md)

---

## AbpPermissionGrants

AbpPermissionGrants is used to store granted permissions for a role or a user.

### Description

This table stores information about the granted permissions in the application. Each record in the table represents a granted permission and allows to manage and track the granted permissions effectively. The table is used to store and manage the permissions in the application and to keep track of permissions that are granted, to whom and when. The columns such as `Name`, `ProviderName`, `ProviderKey`, `TenantId` can be used to filter the granted permissions by name, provider name, provider key, and tenant id respectively, so that you can easily manage and track the granted permissions in the application. This table is important for managing and controlling the access to different parts of the application, and also for auditing purposes.

### Module

[`Volo.Abp.PermissionManagement`](Permission-Management.md)

---

## AbpSettings

The AbpSettings table is used to store the application settings.

### Description

This table stores the application settings. Each record in the table represents a setting and allows to manage and track the settings effectively. The table is used to store key-value pairs of settings for the application, and it allows for the dynamic configuration of the application without the need for recompilation.

### Module

[`Volo.Abp.SettingManagement`](Setting-Management.md)

---

## AbpTextTemplateContents

The AbpTextTemplateContents table is used to store the content of the text templates.

### Description

This table stores the text template contents for the application. Each record in the table represents a text template content and allows to manage and track the text template contents effectively. This table can be used to store reusable text templates that can be easily referenced and rendered by the application at runtime.

### Module

`Volo.Abp.TextTemplating`

---

## OpenIddictApplications

OpenIddictApplications Table is used to store OpenIddict applications.

### Description

This table stores the OpenID Connect applications in the application. Each record in the table represents an OpenID Connect application and allows to manage and track the applications effectively. This table can be used to store information about OpenID Connect applications, including the client id, client secret, redirect uri, and other relevant information. It can also be used to authenticate and authorize clients using OpenID Connect protocol.

### Module

[`Volo.Abp.OpenIddict`](OpenIddict.md)

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`OpenIddictAuthorizations`](#openiddictauthorizations) | `ApplicationId` | To match the authorization with the application. |
| [`OpenIddictTokens`](#openiddicttokens) | `ApplicationId` | To match the token with the application. |

---

## OpenIddictAuthorizations

OpenIddictAuthorizations Table is used to store authorizations.

### Description

This table stores the OpenID Connect authorization data in the application. Each record in the table represents an OpenID Connect authorization and allows to manage and track the authorizations effectively. It can also be used to manage and validate the authorization grants issued to clients and users.

### Module

[`Volo.Abp.OpenIddict`](OpenIddict.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`OpenIddictApplications`](#openiddictapplications) | `Id` | To match the authorization with the application. |

### Used By

| Table | Column | Description |
| --- | --- | --- |
| [`OpenIddictTokens`](#openiddicttokens) | `AuthorizationId` | To match the token with the authorization. |

---

## OpenIddictTokens

OpenIddictTokens Table is used to store tokens.

### Description

This table stores the OpenID Connect tokens in the application. Each record in the table represents an OpenID Connect token and allows to manage and track the tokens effectively. This table can be used to store information about OpenID Connect tokens, including the token payload, expiration, type, and other relevant information. It can also be used to manage and validate the tokens issued to clients and users, such as access tokens and refresh tokens, and to control access to protected resources.

### Module

[`Volo.Abp.OpenIddict`](OpenIddict.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [`OpenIddictApplications`](#openiddictapplications) | `Id` | To match the token with the application. |
| [`OpenIddictAuthorizations`](#openiddictauthorizations) | `Id` | To match the token with the authorization. |

---

## OpenIddictScopes

OpenIddictScopes Table is used to store scopes.

### Description

This table stores the OpenID Connect scopes in the application. Each record in the table represents an OpenID Connect scope and allows to manage and track the scopes effectively. This table can be used to store information about OpenID Connect scopes, including the name and description of the scope. It can also be used to define the permissions or access rights associated with the scopes, which are then used to control access to protected resources.

### Module

[`Volo.Abp.OpenIddict`](OpenIddict.md)

---

## IdentityServerApiResources

IdentityServerApiResources Table is used to store API resources.

### Description

This table stores the IdentityServer API resources in the application. Each record in the table represents an API resource and allows to manage and track the resources effectively. This table can be used to store information about API resources, including the resource name, display name, description, and other relevant information. It can also be used to define the scopes, claims, and properties associated with the API resources, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

---

## IdentityServerIdentityResources

IdentityServerIdentityResources Table is used to store identity resources.

### Description

This table stores the identity resources in the application. Each record in the table represents an identity resource and allows to manage and track the identity resources effectively. This table can be used to store information about identity resources, including the name, display name, description, and enabled status. It can also be used to manage and validate the identity resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

---

## IdentityServerClients

IdentityServerClients Table is used to store clients.

### Description

This table stores the clients registered with IdentityServer in the application. Each record in the table represents a client and allows to manage and track the clients effectively. This table can be used to store information about clients, including the client id, client name, client uri and other relevant information. It can also be used to define the scopes, claims, and properties associated with the clients, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

---

## IdentityServerApiScopes

IdentityServerApiScopes Table is used to store scopes of an API resource.

### Description

This table stores the IdentityServer API scopes in the application. Each record in the table represents an API scope and allows to manage and track the scopes effectively. This table can be used to store information about API scopes, including the scope name, display name, description, and other relevant information. It can also be used to define the claims and properties associated with the API scopes, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

---

## IdentityServerApiResourceClaims

IdentityServerApiResourceClaims Table is used to store claims of an API resource.

### Description

This table stores the claims of an API resource in the application. Each record in the table represents a claim of an API resource and allows to manage and track the claims effectively. This table can be used to store information about claims of an API resource, including the claim type and API resource id. It can also be used to manage and validate the claims of an API resource.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiResources` | `Id` | To match the claim with the API resource. |

---

## IdentityServerIdentityResourceClaims

IdentityServerIdentityResourceClaims Table is used to store claims of an identity resource.

### Description

This table stores the claims of an identity resource in the application. Each record in the table represents a claim of an identity resource and allows to manage and track the claims effectively. This table can be used to store information about claims of an identity resource, including the claim type and identity resource id. It can also be used to manage and validate the claims of an identity resource.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerIdentityResources` | `Id` | To match the claim with the identity resource. |

---

## IdentityServerClientClaims

IdentityServerClientClaims Table is used to store claims of a client.

### Description

This table stores the claims of a client in the application. Each record in the table represents a claim of a client and allows to manage and track the claims effectively. This table can be used to store information about claims of a client, including the claim type, claim value and client id. It can also be used to manage and validate the claims of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the claim with the client. |

---

## IdentityServerApiScopeClaims

IdentityServerApiScopeClaims Table is used to store claims of an API scope.

### Description

This table stores the claims of an API scope in the application. Each record in the table represents a claim of an API scope and allows to manage and track the claims effectively. This table can be used to store information about claims of an API scope, including the claim type and API scope id. It can also be used to manage and validate the claims of an API scope.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiScopes` | `Id` | To match the claim with the API scope. |

---

## IdentityServerApiResourceProperties

IdentityServerApiResourceProperties Table is used to store properties of an API resource.

### Description

This table stores the properties associated with IdentityServer API resources. Each record in the table represents a property and allows to manage and track the properties effectively. This table can be used to store information about properties, including the property key and value, and the associated API resource. These properties can be used to store additional metadata or configuration information related to the API resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiResources` | `Id` | To match the property with the API resource. |

---

## IdentityServerIdentityResourceProperties

IdentityServerIdentityResourceProperties Table is used to store properties of an identity resource.

### Description

This table stores the properties associated with IdentityServer identity resources. Each record in the table represents a property and allows to manage and track the properties effectively. This table can be used to store information about properties, including the property key and value, and the associated identity resource. These properties can be used to store additional metadata or configuration information related to the identity resources.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerIdentityResources` | `Id` | To match the property with the identity resource. |

---

## IdentityServerClientProperties

IdentityServerClientProperties Table is used to store properties of a client.

### Description

This table stores the properties of a client in the application. Each record in the table represents a property of a client and allows to manage and track the properties effectively. This table can be used to store information about properties of a client, including the key, value and client id. It can also be used to manage and validate the properties of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the property with the client. |

---

## IdentityServerApiScopeProperties

IdentityServerApiScopeProperties Table is used to store properties of an API scope.

### Description

This table stores the properties associated with IdentityServer API scopes. Each record in the table represents a property and allows to manage and track the properties effectively. This table can be used to store information about properties, including the property key and value, and the associated API scope. These properties can be used to store additional metadata or configuration information related to the API scopes.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiScopes` | `Id` | To match the property with the API scope. |

---

## IdentityServerApiResourceScopes

IdentityServerApiResourceScopes Table is used to store scopes of an API resource.

### Description

This table stores the scopes of an API resource in the application. Each record in the table represents a scope of an API resource and allows to manage and track the scopes effectively. This table can be used to store information about scopes of an API resource, including the scope name and API resource id. It can also be used to manage and validate the scopes of an API resource.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiResources` | `Id` | To match the scope with the API resource. |

---

## IdentityServerClientScopes

IdentityServerClientScopes Table is used to store scopes of a client.

### Description

This table stores the scopes of a client in the application. Each record in the table represents a scope of a client and allows to manage and track the scopes effectively. This table can be used to store information about scopes of a client, including the scope and client id. It can also be used to manage and validate the scopes of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the scope with the client. |

---

## IdentityServerApiResourceSecrets

IdentityServerApiResourceSecrets Table is used to store secrets of an API resource.

### Description

This table stores the secrets of an API resource in the application. Each record in the table represents a secret of an API resource and allows to manage and track the secrets effectively. This table can be used to store information about secrets of an API resource, including the secret value, expiration date, and API resource id. It can also be used to manage and validate the secrets of an API resource.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiResources` | `Id` | To match the secret with the API resource. |

---

## IdentityServerClientSecrets

IdentityServerClientSecrets Table is used to store secrets of a client.

### Description

This table stores the secrets of a client in the application. Each record in the table represents a secret of a client and allows to manage and track the secrets effectively. This table can be used to store information about secrets of a client, including the secret and client id. It can also be used to manage and validate the secrets of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the secret with the client. |

---

## IdentityServerClientCorsOrigins

IdentityServerClientCorsOrigins Table is used to store CORS origins of a client.

### Description

This table stores the CORS origins of a client in the application. Each record in the table represents a CORS origin of a client and allows to manage and track the CORS origins effectively. This table can be used to store information about CORS origins of a client, including the origin and client id. It can also be used to manage and validate the CORS origins of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the CORS origin with the client. |

---

## IdentityServerClientGrantTypes

IdentityServerClientGrantTypes Table is used to store grant types of a client.

### Description

This table stores the grant types of a client in the application. Each record in the table represents a grant type of a client and allows to manage and track the grant types effectively. This table can be used to store information about grant types of a client, including the grant type and client id. It can also be used to manage and validate the grant types of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the grant type with the client. |

---

## IdentityServerClientIdPRestrictions

IdentityServerClientIdPRestrictions Table is used to store identity provider restrictions of a client.

### Description

This table stores the identity provider restrictions of a client in the application. Each record in the table represents an identity provider restriction of a client and allows to manage and track the identity provider restrictions effectively. This table can be used to store information about identity provider restrictions of a client, including the identity provider and client id. It can also be used to manage and validate the identity provider restrictions of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the identity provider restriction with the client. |

---

## IdentityServerClientPostLogoutRedirectUris

IdentityServerClientPostLogoutRedirectUris Table is used to store post logout redirect URIs of a client.

### Description

This table stores the post logout redirect URIs of a client in the application. Each record in the table represents a post logout redirect URI of a client and allows to manage and track the post logout redirect URIs effectively. This table can be used to store information about post logout redirect URIs of a client, including the post logout redirect URI and client id. It can also be used to manage and validate the post logout redirect URIs of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the post logout redirect URI with the client. |

---

## IdentityServerClientRedirectUris

IdentityServerClientRedirectUris Table is used to store redirect URIs of a client.

### Description

This table stores the redirect URIs of a client in the application. Each record in the table represents a redirect URI of a client and allows to manage and track the redirect URIs effectively. This table can be used to store information about redirect URIs of a client, including the redirect URI and client id. It can also be used to manage and validate the redirect URIs of a client.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the redirect URI with the client. |

---

## IdentityServerDeviceFlowCodes

IdentityServerDeviceFlowCodes Table is used to store device flow codes.

### Description

This table stores the device flow codes in the application. Each record in the table represents a device flow code and allows to manage and track the device flow codes effectively. This table can be used to store information about device flow codes, including the user code, device code, subject id, client id, creation time, expiration, data and session id. It can also be used to manage and validate the device flow codes.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)

---

## IdentityServerPersistedGrants

IdentityServerPersistedGrants Table is used to store persisted grants.

### Description

This table stores the persisted grants in the application. Each record in the table represents a persisted grant and allows to manage and track the persisted grants effectively. This table can be used to store information about persisted grants, including the key, type, subject id, client id, creation time, expiration, and data. It can also be used to manage and validate the persisted grants.

### Module

[`Volo.Abp.IdentityServer`](IdentityServer.md)