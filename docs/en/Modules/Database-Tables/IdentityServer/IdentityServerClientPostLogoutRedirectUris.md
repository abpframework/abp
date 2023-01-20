## IdentityServerClientPostLogoutRedirectUris

IdentityServerClientPostLogoutRedirectUris Table is used to store post logout redirect URIs of a client.

### Description

This table stores the post logout redirect URIs of a client in the application. Each record in the table represents a post logout redirect URI of a client and allows to manage and track the post logout redirect URIs effectively. This table can be used to store information about post logout redirect URIs of a client, including the post logout redirect URI and client id. It can also be used to manage and validate the post logout redirect URIs of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the post logout redirect URI with the client. |