# Database Tables

This documentation describes all database tables and their purposes. You can read this documentation to get general knowledge of the database tables that come from each module.

## [Audit Logging Module](Audit-Logging.md)

### AbpAuditLogs

This table stores information about the audit logs in the application. Each record represents an audit log and tracks the actions performed in the application.

### AbpAuditLogActions

This table stores information about the actions performed in the application, which are logged for auditing purposes.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | Links each action to a specific audit log. |

### AbpEntityChanges

This table stores information about entity changes in the application, which are logged for auditing purposes.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpAuditLogs](#abpauditlogs) | Id | Links each entity change to a specific audit log. |

### AbpEntityPropertyChanges

This table stores information about property changes to entities in the application, which are logged for auditing purposes.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpEntityChanges](#abpentitychanges) | Id | 	Links each property change to a specific entity change. |

## [Background Jobs Module](Background-Jobs.md)

### AbpBackgroundJobs

This table stores information about the background jobs in the application and facilitates their efficient management and tracking. Each entry in the table contains details of a background job, including the job name, arguments, try count, next try time, last try time, abandoned status, and priority.

## [Tenant Management Module](Tenant-Management.md)

### AbpTenants

This table stores information about the tenants. Each record represents a tenant and contains information about the tenant, such as name and other details.

### AbpTenantConnectionStrings

This table stores information about the tenant database connection strings. When you define a connection string for a tenant, a new record will be added to this table. You can query this database to get connection strings by tenants.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpTenants](#abptenants) | Id | The `Id` column in the `AbpTenants` table is used to associate the tenant connection string with the corresponding tenant. |

## Blogging Module

### BlgUsers

This table stores information about the blog users. When a new identity user is created, a new record will be added to this table.

### BlgBlogs

This table serves to store blog information and semantically separates the posts of each blog.

### BlgPosts

This table stores information about the blog posts. You can query this table to get blog posts by blogs.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [BlgBlogs](#blgblogs) | Id | To associate the blog post with the corresponding blog. |
### BlgComments

This table stores information about the comments made on blog posts. You can query this table to get comments by posts.
#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [BlgPosts](#blgposts) | Id | Links the comment to the corresponding blog post. |
| [BlgComments](#blgcomments) | Id | Links the comment to the parent comment. |

### BlgTags

This table stores information about the tags. When a new tag is used, a new record will be added to this table. You can query this table to get tags by blogs.

### BlgPostTags

This table is used to associate tags with blog posts in order to categorize and organize the content. You can query this table to get post tags by posts.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [BlgTags](#blgtags) | Id | Links the post tag to the corresponding tag. |
| [BlgPosts](#blgposts) | Id | Links the post tag to the corresponding blog post. |

## [CMS Kit Module](Cms-Kit/Index.md)

### CmsUsers

This table stores information about the CMS Kit module users. When a new identity user is created, a new record will be added to this table.

### CmsBlogs

This table serves to store blog information and semantically separates the posts of each blog.

### CmsBlogPosts

This table stores information about the blog posts. You can query this table to get blog posts by blogs.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [CmsUsers](#cmsusers) | Id | Links the blog post to the corresponding author. |

### CmsBlogFeatures

This table stores information about the blog features. You can query this table to get blog features by blogs.

### CmsComments

This table is utilized by the [CMS Kit Comment system](Cms-Kit/Comments.md) to store comments made on the blog posts. You can query this table to get comments by posts.

### CmsTags

This table stores information about the tags. When a new tag is used, a new record will be added to this table. You can query this table to get tags by blogs.

### CmsEntityTags

This table is utilized by the [Tag Management system](Cms-Kit/Tags.md) to store tags and their relationship with various entities, thus enabling efficient categorization and organization of content. You can query this table to get entity tags by entities.

### CmsGlobalResources

This table is a database table for the [CMS Kit Global Resources system](Cms-Kit/Global-Resources.md), allowing dynamic addition of global styles and scripts.

### CmsMediaDescriptors

This table is utilized by the CMS kit module to manage media files by using the [BlobStoring](../Blob-Storing.md) module.

### CmsMenuItems

This table is used by the [CMS Kit Menu system](Cms-Kit/Menus.md) to manage and store information about dynamic public menus, including details such as menu item display names, URLs, and hierarchical relationships.

### CmsPages

This table is utilized by the [CMS Kit Page system](Cms-Kit/Pages.md) to store dynamic pages within the application, including information such as page URLs, titles, and content.

### CmsRatings

This table is utilized by the [CMS Kit Rating system](Cms-Kit/Ratings.md) to store ratings made on blog posts. You can query this table to get ratings by posts.

### CmsUserReactions

This table is utilized by the [CMS Kit Reaction system](Cms-Kit/Reactions.md) to store reactions made on blog posts. You can query this table to get reactions by posts.

## [Docs Module](Docs.md)

### DocsProjects

This table stores project information to categorize documents according to different projects.

### DocsDocuments

This table retrieves the document if it's not found in the cache. The documentation is being updated when the content is retrieved from the database.

### DocsDocumentContributors

This table stores information about the contributors of the documents. You can query this table to get document contributors by documents.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [DocsDocuments](#docsdocuments) | Id | Links the document contributor to the corresponding document. |

## [Feature Management Module](Feature-Management.md)

### AbpFeatureGroups

This table stores information about the feature groups in the application. For example, you can group all the features in the [`AbpFeatures`](#abpfeatures) table related to the `Identity` module under the `Identity` group.

### AbpFeatures

This table stores information about the features in the application. You can use the `Name` column to link each feature with its corresponding feature value in the [`AbpFeatureValues`](#abpfeaturevalues) table, so that you can easily manage and organize the features.

### AbpFeatureValues

This table stores the values of the features for different providers. You can use the `Name` column to link each feature value with its corresponding feature in the [`AbpFeatures`](#abpfeatures) table, so that you can easily manage and organize the features.

## [Identity Module](Identity.md)

### AbpUsers

This table stores information about the identity users in the application.

### AbpRoles

This table stores information about the roles in the application. Roles are used to manage and control access to different parts of the application by assigning permissions and claims to roles and then assigning those roles to users. This table is important for managing and organizing the roles in the application, and for defining the access rights of the users.

### AbpClaimTypes

This table stores information about the claim types used in the application. You can use the `Name`, `Regex` columns to filter the claim types by name, and regex pattern respectively, so that you can easily manage and track the claim types in the application.

### AbpLinkUsers

This table is useful for linking multiple user accounts across different tenants or applications to a single user, allowing them to easily switch between their accounts.

### AbpUserClaims

This table can manage user-based access control by allowing to assign claims to users, which describes the access rights of the individual user.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Links the user claim to the corresponding user. |

### AbpUserLogins

This table can store information about the user's external logins such as login with Facebook, Google, etc. and it can also be used to track the login history of users.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Links the user login to the corresponding user. |

### AbpUserRoles

This table can manage user-based access control by allowing to assign roles to users, which describe the access rights of the individual user.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Links the user role to the corresponding user. |
| [AbpRoles](#abproles) | Id | Links the user role to the corresponding role. |

### AbpUserTokens

This table can store information about user's refresh tokens, access tokens and other tokens used in the application. It can also be used to invalidate or revoke user tokens.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Links the user token to the corresponding user. |

### AbpOrganizationUnits

This table is useful for creating and managing a hierarchical structure of the organization, allowing to group users and assign roles based on the organization structure. You can use the `Code`, `ParentId` columns to filter the organization units by code and parent id respectively, so that you can easily manage and track the organization units in the application.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpOrganizationUnits](#abporganizationunits) | ParentId | Links the organization unit to its parent organization unit. |

### AbpOrganizationUnitRoles

This table is useful for managing role-based access control at the level of organization units, allowing to assign different roles to different parts of the organization structure. You can use the `OrganizationUnitId`, `RoleId` columns to filter the roles by organization unit id and role id respectively, so that you can easily manage and track the roles assigned to organization units in the application.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpOrganizationUnits](#abporganizationunits) | Id | Links the organization unit role to the corresponding organization unit. |
| [AbpRoles](#abproles) | Id | Links the organization unit role to the corresponding role. |

### AbpUserOrganizationUnits

This table stores information about the organization units assigned to the users in the application. This table can manage user-organization unit relationships, and to group users based on the organization structure.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](#abpusers) | Id | Links the user organization unit to the corresponding user. |
| [AbpOrganizationUnits](#abporganizationunits) | Id | Links the user organization unit to the corresponding organization unit. |

### AbpRoleClaims

This table is useful for managing role-based access control by allowing to assign claims to roles, which describes the access rights of the users that belong to that role.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [AbpRoles](#abproles) | Id | Links the role claim to the corresponding role. |

### AbpSecurityLogs

This table logs important operations and changes related to user accounts, allowing users to save the security logs for future reference.

## [Permission Management](Permission-Management.md)

### AbpPermissionGroups

This table is important for managing and organizing the permissions in the application, by grouping them into logical categories.

### AbpPermissions

This table is important for managing and controlling access to different parts of the application and for defining the granular permissions that make up the larger permissions or roles.

### AbpPermissionGrants

The table stores and manage the permissions in the application and to keep track of permissions that are granted, to whom and when. Columns such as `Name`, `ProviderName`, `ProviderKey`, `TenantId` can be used to filter the granted permissions by name, provider name, provider key, and tenant id respectively, so that you can easily manage and track the granted permissions in the application. 

## [Setting Management](Setting-Management.md)

### AbpSettings

This table stores key-value pairs of settings for the application, and it allows dynamic configuration of the application without the need for recompilation.

## [OpenIddict](OpenIddict.md)

### OpenIddictApplications

This table can store information about the OpenID Connect applications, including the client id, client secret, redirect URI, and other relevant information. It can also be used to authenticate and authorize clients using OpenID Connect protocol.

### OpenIddictAuthorizations

This table stores the OpenID Connect authorization data in the application. It can also be used to manage and validate the authorization grants issued to clients and users.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [OpenIddictApplications](#openiddictapplications) | Id | Links the authorization to the corresponding application. |

### OpenIddictTokens

This table can store information about the OpenID Connect tokens, including the token payload, expiration, type, and other relevant information. It can also be used to manage and validate the tokens issued to clients and users, such as access tokens and refresh tokens, and to control access to protected resources.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [OpenIddictApplications](#openiddictapplications) | Id | Links the token to the corresponding application. |
| [OpenIddictAuthorizations](#openiddictauthorizations) | Id | Links the token to the corresponding authorization. |

### OpenIddictScopes

This table can store information about the OpenID Connect scopes, including the name and description of the scope. It can also be used to define the permissions or access rights associated with the scopes, which are then used to control access to protected resources.

## [IdentityServer](IdentityServer.md)

### IdentityServerApiResources

This table can store information about the API resources, including the resource name, display name, description, and other relevant information. It can also be used to define the scopes, claims, and properties associated with the API resources, which are then used to control access to protected resources.

### IdentityServerIdentityResources

This table can store information about the identity resources, including the name, display name, description, and enabled status.

### IdentityServerClients

This table can store information about the clients, including the client id, client name, client URI and other relevant information. It can also be used to define the scopes, claims, and properties associated with the clients, which are then used to control access to protected resources.

### IdentityServerApiScopes

This table can store information about the API scopes, including the scope name, display name, description, and other relevant information. It can also be used to define the claims and properties associated with the API scopes, which are then used to control access to protected resources.

### IdentityServerApiResourceClaims

This table can store information about the claims of an API resource, including the claim type and API resource id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Links the claim to the corresponding API resource. |

### IdentityServerIdentityResourceClaims

This table can store information about the claims of an identity resource, including the claim type and identity resource id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerIdentityResources](#identityserveridentityresources) | Id | Links the claim to the corresponding identity resource. |

### IdentityServerClientClaims

This table can store information about the claims of a client, including the claim type, claim value and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the claim to the corresponding client. |

### IdentityServerApiScopeClaims

This table can store information about the claims of an API scope, including the claim type and API scope id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiScopes](#identityserverapiscopes) | Id | Links the claim to the corresponding API scope. |

### IdentityServerApiResourceProperties

This table can store information about properties, including the property key and value, and the associated API resource. These properties can store additional metadata or configuration information related to the API resources.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Links the property to the corresponding API resource. |

### IdentityServerIdentityResourceProperties

This table can store information about properties, including the property key and value, and the associated identity resource. These properties can store additional metadata or configuration information related to the identity resources.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerIdentityResources](#identityserveridentityresources) | Id | Links the property to the corresponding identity resource. |

### IdentityServerClientProperties

This table can be store information about the properties of a client, including the key, value and client id. These properties can store additional metadata or configuration information related to the clients.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the property to the corresponding client. |

### IdentityServerApiScopeProperties

This table can store information about the properties of an API scope, including the key, value and API scope id. These properties can store additional metadata or configuration information related to the API scopes.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiScopes](#identityserverapiscopes) | Id | Links the property to the corresponding API scope. |

### IdentityServerApiResourceScopes

This table can store information about the scopes of an API resource, including the scope name and API resource id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Links the scope to the corresponding API resource. |

### IdentityServerClientScopes

 This table can store information about the scopes of a client, including the scope and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the scope to the corresponding client. |

### IdentityServerApiResourceSecrets

This table can store information about the secrets of an API resource, including the secret value, expiration date, and API resource id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerApiResources](#identityserverapiresources) | Id | Links the secret to the corresponding API resource. |

### IdentityServerClientSecrets

This table can store information about the secrets of a client, including the secret value, expiration date, and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the secret to the corresponding client. |

### IdentityServerClientCorsOrigins

This table can store information about the CORS origins of a client, including the origin and client id. It can also be used to manage and validate the CORS origins of a client.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the CORS origin to the corresponding client. |

### IdentityServerClientGrantTypes

This table can store information about the grant types of a client, including the grant type and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the grant type to the corresponding client. |

### IdentityServerClientIdPRestrictions

This table can store information about the identity provider restrictions of a client, including the identity provider and client id. 

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the identity provider restriction to the corresponding client. |

### IdentityServerClientPostLogoutRedirectUris

This table can store information about the post logout redirect URIs of a client, including the post logout redirect URI and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the post logout redirect URI to the corresponding client. |

### IdentityServerClientRedirectUris

This table can store information about the redirect URIs of a client, including the redirect URI and client id.

#### Foreign Keys

| Table | Column | Description |
| --- | --- | --- |
| [IdentityServerClients](#identityserverclients) | Id | Links the redirect URI to the corresponding client. |

### IdentityServerDeviceFlowCodes

This table can store information about the device flow codes, including the user code, device code, subject id, client id, creation time, expiration, data and session id.

### IdentityServerPersistedGrants

This table can store information about the persisted grants, including the key, type, subject id, client id, creation time, expiration, and data.

## Others

### AbpBlobContainers

This table is important for providing a better user experience by allowing the application to support multiple containers and providing BLOB-specific features.

### AbpBlobs

This table stores the binary data of BLOBs (binary large objects) in the application. Each BLOB is related to a container in the [AbpBlobContainers](#abpblobcontainers) table, where the container name, tenant id and other properties of the container can be found.

#### Foreign Keys 

| Table | Column | Description |
| --- | --- | --- |
| [AbpBlobContainers](#abpblobcontainers) | Id | Links the BLOB to the corresponding container. |

### AbpLocalizationResources

This table stores the localization resources for the application. This table is important for providing a better user experience by allowing the application to support multiple resources and providing localized text and other localization-specific features.

### AbpLocalizationTexts

The table contains the resource name, culture name, and a json encoded value which holds the key-value pair of localization text. It allows for efficient storage and management of localization texts and allows for easy update or addition of new translations for specific resources and cultures.
