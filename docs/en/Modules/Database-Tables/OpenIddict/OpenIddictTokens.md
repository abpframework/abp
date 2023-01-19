## OpenIddictTokens

OpenIddictTokens Table is used to store tokens.

### Description

This table stores the OpenID Connect tokens in the application. Each record in the table represents an OpenID Connect token and allows to manage and track the tokens effectively. This table can be used to store information about OpenID Connect tokens, including the token payload, expiration, type, and other relevant information. It can also be used to manage and validate the tokens issued to clients and users, such as access tokens and refresh tokens, and to control access to protected resources.

### Module

[`Volo.Abp.OpenIddict`](../../OpenIddict.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `OpenIddictApplications` | `Id` | To match the token with the application. |
| `OpenIddictAuthorizations` | `Id` | To match the token with the authorization. |