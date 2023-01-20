## IdentityServerClientClaims

IdentityServerClientClaims Table is used to store claims of a client.

### Description

This table stores the claims of a client in the application. Each record in the table represents a claim of a client and allows to manage and track the claims effectively. This table can be used to store information about claims of a client, including the claim type, claim value and client id. It can also be used to manage and validate the claims of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the claim with the client. |