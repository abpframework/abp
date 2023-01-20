## IdentityServerClientRedirectUris

IdentityServerClientRedirectUris Table is used to store redirect URIs of a client.

### Description

This table stores the redirect URIs of a client in the application. Each record in the table represents a redirect URI of a client and allows to manage and track the redirect URIs effectively. This table can be used to store information about redirect URIs of a client, including the redirect URI and client id. It can also be used to manage and validate the redirect URIs of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the redirect URI with the client. |