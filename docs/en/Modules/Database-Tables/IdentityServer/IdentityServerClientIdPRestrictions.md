## IdentityServerClientIdPRestrictions

IdentityServerClientIdPRestrictions Table is used to store identity provider restrictions of a client.

### Description

This table stores the identity provider restrictions of a client in the application. Each record in the table represents an identity provider restriction of a client and allows to manage and track the identity provider restrictions effectively. This table can be used to store information about identity provider restrictions of a client, including the identity provider and client id. It can also be used to manage and validate the identity provider restrictions of a client.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerClients` | `Id` | To match the identity provider restriction with the client. |