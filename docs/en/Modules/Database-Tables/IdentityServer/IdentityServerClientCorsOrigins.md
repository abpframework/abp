## IdentityServerClientCorsOrigins

IdentityServerClientCorsOrigins Table is used to store CORS origins of a client.

### Description

This table stores the CORS origins of a client in the application. Each record in the table represents a CORS origin of a client and allows to manage and track the CORS origins effectively. This table can be used to store information about CORS origins of a client, including the origin and client id. It can also be used to manage and validate the CORS origins of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the CORS origin with the client. |