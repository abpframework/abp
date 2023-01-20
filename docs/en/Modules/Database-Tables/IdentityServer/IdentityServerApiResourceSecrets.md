## IdentityServerApiResourceSecrets Table

IdentityServerApiResourceSecrets Table is used to store secrets of an API resource.

### Description

This table stores the secrets of an API resource in the application. Each record in the table represents a secret of an API resource and allows to manage and track the secrets effectively. This table can be used to store information about secrets of an API resource, including the secret value, expiration date, and API resource id. It can also be used to manage and validate the secrets of an API resource.

### Module

[Volo.Abp.IdentityServer](../../IdentityServer.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| `IdentityServerApiResources` | `Id` | To match the secret with the API resource. |