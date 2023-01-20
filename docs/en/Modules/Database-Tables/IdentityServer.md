## IdentityServerApiResources

IdentityServerApiResources Table is used to store API resources.

### Description

This table stores the IdentityServer API resources in the application. Each record in the table represents an API resource and allows to manage and track the resources effectively. This table can be used to store information about API resources, including the resource name, display name, description, and other relevant information. It can also be used to define the scopes, claims, and properties associated with the API resources, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

---

## IdentityServerIdentityResources

IdentityServerIdentityResources Table is used to store identity resources.

### Description

This table stores the identity resources in the application. Each record in the table represents an identity resource and allows to manage and track the identity resources effectively. This table can be used to store information about identity resources, including the name, display name, description, and enabled status. It can also be used to manage and validate the identity resources.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

---

## IdentityServerClients

IdentityServerClients Table is used to store clients.

### Description

This table stores the clients registered with IdentityServer in the application. Each record in the table represents a client and allows to manage and track the clients effectively. This table can be used to store information about clients, including the client id, client name, client uri and other relevant information. It can also be used to define the scopes, claims, and properties associated with the clients, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

---

## IdentityServerApiScopes

IdentityServerApiScopes Table is used to store scopes of an API resource.

### Description

This table stores the IdentityServer API scopes in the application. Each record in the table represents an API scope and allows to manage and track the scopes effectively. This table can be used to store information about API scopes, including the scope name, display name, description, and other relevant information. It can also be used to define the claims and properties associated with the API scopes, which are then used to control access to protected resources.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

---

## IdentityServerApiResourceClaims

IdentityServerApiResourceClaims Table is used to store claims of an API resource.

### Description

This table stores the claims of an API resource in the application. Each record in the table represents a claim of an API resource and allows to manage and track the claims effectively. This table can be used to store information about claims of an API resource, including the claim type and API resource id. It can also be used to manage and validate the claims of an API resource.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

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

[`Volo.Abp.IdentityServer`](../IdentityServer.md)

---

## IdentityServerPersistedGrants

IdentityServerPersistedGrants Table is used to store persisted grants.

### Description

This table stores the persisted grants in the application. Each record in the table represents a persisted grant and allows to manage and track the persisted grants effectively. This table can be used to store information about persisted grants, including the key, type, subject id, client id, creation time, expiration, and data. It can also be used to manage and validate the persisted grants.

### Module

[`Volo.Abp.IdentityServer`](../IdentityServer.md)