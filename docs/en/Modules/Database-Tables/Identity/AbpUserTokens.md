## AbpUserTokens

The AbpUserTokens table is used to store user tokens.

### Description

This table stores information about the tokens of the users in the application. Each record in the table represents a token for a user and allows to manage and track the user tokens effectively. This table can be used to store information about user's refresh tokens, access tokens and other tokens used in the application. It can also be used to invalidate or revoke user tokens.

### Module

[Volo.Abp.Identity](../../Identity.md)

### Uses

| Table | Column | Description |
| --- | --- | --- |
| [AbpUsers](AbpUsers.md) | Id | The user id. |