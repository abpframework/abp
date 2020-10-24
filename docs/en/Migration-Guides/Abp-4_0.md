# ABP Framework 3.3 to 4.0 Migration Guide

## Auto API Controller Route Changes

The route calculation for the [Auto API Controllers](https://docs.abp.io/en/abp/latest/API/Auto-API-Controllers) is changing with the ABP Framework version 4.0 ([#5325](https://github.com/abpframework/abp/issues/5325)). Previously, **camelCase** route paths were being used. Beginning from the version 4.0, it uses **kebab-case** route paths where it is possible.

**A typical auto API before v4.0**

![route-before-4](images/route-before-4.png)

**camelCase route parts become kebab-case with 4.0**

![route-4](images/route-4.png)

If it is hard to change it for your application, you can continue to use the version 3.x route strategy, by following one of the approaches;

* Set `UseV3UrlStyle` to `true` in the options of the `options.ConventionalControllers.Create(...)` method. Example:

````csharp
options.ConventionalControllers
    .Create(typeof(BookStoreApplicationModule).Assembly, opts =>
        {
            opts.UseV3UrlStyle = true;
        });
````

This approach effects only the controllers for the `BookStoreApplicationModule`.

* Set `UseV3UrlStyle` to `true` for the `AbpConventionalControllerOptions` to set it globally. Example:

```csharp
Configure<AbpConventionalControllerOptions>(options =>
{
    options.UseV3UrlStyle = true;
});
```

Setting it globally effects all the modules in a modular application.

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
