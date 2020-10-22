# ABP Framework 3.3 to 4.0 Migration Guide

## Identity Server Changes

ABP Framework upgrades the [IdentityServer4](https://www.nuget.org/packages/IdentityServer4) library from 3.x to 4.x with the ABP Framework version 4.0. IdentityServer 4.x has a lot of changes, some of them are **breaking changes in the data structure**.

### Database Changes

**So, if you are upgrading from 3.x, then there are some change should be done in your database.**

#### ApiScope

As the **most important breaking change**, Identity Server 4.x places the `ApiScope` as an independent aggregate root. Previously it was a part of the to `ApiResource` aggregate. This requires manual operation. See the *Database Migration* section.

Also, added `Enabled(string)` and `Description(bool,true)` properties.

#### ApiResource

* Added `AllowedAccessTokenSigningAlgorithms (string)` and `ShowInDiscoveryDocument(bool, default: true)` properties

#### Client

* Added `RequireRequestObject (bool)` and `AllowedIdentityTokenSigningAlgorithms (string)` properties.
* Changed default value of `RequireConsent` from `true` to `false`.
* Changed default value of `RequirePkce` from `false` to `true`.

#### DeviceFlowCodes

* Added `SessionId (string)` and `Description (string)` properties.

#### PersistedGrant

* Added `SessionId (string)` and `Description(string)` and `ConsumedTime (DateTime?)` properties

## Migrating the Database

> Attention: **Please backup your database** before the migration!

If you are using **Entity Framework Core**, you need to add a new database migration, using the `Add-Migration` command, and apply changes to the database. Please **review the migration** script to understand if it effects your existing data. Otherwise, you may **loose some of your configuration**.

If you haven't customize the `IdentityServerDataSeedContributor` and haven't customized the initial data inside the `IdentityServer*` tables;

1. Update `IdentityServerDataSeedContributor` class by comparing to [the latest code](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Domain/IdentityServer/IdentityServerDataSeedContributor.cs). You probably only need to add the `CreateApiScopesAsync` method and the code related to it.
2. Then you can simply clear all the **table data** in these tables then execute the `DbMigrator` application again to fill it with the new configuration.

If you've customize your IdentityServer configuration in the database or in the seed data, you should understand the changes and upgrade your code/data accordingly.

### Related Resources
* https://leastprivilege.com/2020/06/19/announcing-identityserver4-v4-0/
* https://github.com/IdentityServer/IdentityServer4/issues/4592
