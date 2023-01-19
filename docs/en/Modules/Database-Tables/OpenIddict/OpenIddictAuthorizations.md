## OpenIddictAuthorizations

OpenIddictAuthorizations Table is used to store authorizations.

### Description

This table stores the OpenID Connect authorization data in the application. Each record in the table represents an OpenID Connect authorization and allows to manage and track the authorizations effectively. It can also be used to manage and validate the authorization grants issued to clients and users.

### Module

[`Volo.Abp.OpenIddict`](../../OpenIddict.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `OpenIddictApplications` | `Id` | To match the authorization with the application. |