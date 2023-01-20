## OpenIddictApplications

OpenIddictApplications Table is used to store OpenIddict applications.

### Description

This table stores the OpenID Connect applications in the application. Each record in the table represents an OpenID Connect application and allows to manage and track the applications effectively. This table can be used to store information about OpenID Connect applications, including the client id, client secret, redirect uri, and other relevant information. It can also be used to authenticate and authorize clients using OpenID Connect protocol.

### Module

[`Volo.Abp.OpenIddict`](../OpenIddict.md)

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

[`Volo.Abp.OpenIddict`](../OpenIddict.md)

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

[`Volo.Abp.OpenIddict`](../OpenIddict.md)

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

[`Volo.Abp.OpenIddict`](../OpenIddict.md)