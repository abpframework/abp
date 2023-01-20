## IdentityServerClientSecrets

IdentityServerClientSecrets Table is used to store secrets of a client.

### Description

This table stores the secrets of a client in the application. Each record in the table represents a secret of a client and allows to manage and track the secrets effectively. This table can be used to store information about secrets of a client, including the secret and client id. It can also be used to manage and validate the secrets of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the secret with the client. |