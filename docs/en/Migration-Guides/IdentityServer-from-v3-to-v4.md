# Migration Guide for the Abp Identity Server from the v3 to the v4

Identity Server released the v4 version in June this year. This version brings a lot of features, enhancements and bug fixes. Unfortunately, it is also breaking changes.

I saw that it released several bug patches in a short time, so we did not upgrade to v4 in the first time.

We are now upgraded abp to net 5, we think it is time to upgrade Identity Server to v4.

There are a lot of code changes for the abp team, but fortunately for developers, you don't need to do that, just like the changes we made in the application template.

Breaking changes to the database are always tricky. In Identity Server v4 it uses `ApiScope` as an independent aggregate root. Previously it belonged to `ApiResource`. Some entities have also been changed(adding new properties, changing default values, etc.).

## The following are the specific changes:

`Client`:
* Added `RequireRequestObject(bool)` and `AllowedIdentityTokenSigningAlgorithms(string)` properties.
* Change `RequireConsent` from `true` to `false`.
* Change `RequirePkce` from `false` to `true`.

`DeviceFlowCodes`:
* Added `SessionId(string)` and `Description(string)` properties.

`PersistedGrant`:
* Added `SessionId(string)` and `ShowInDiscoveryDocument(bool,true)` properties

`ApiResource`:
* Added `AllowedAccessTokenSigningAlgorithms(string)` and `Description(string)` and `ConsumedTime(DateTime?)` properties

`ApiScope`:
* Before it was a property of `ApiResource`, now it becomes an independent aggregate root.
* Added `Enabled(string)` and `Description(bool,true)` properties

Before we used `Dictionary<string, string>` as aggregate root's `Properties`, now we use independent classes such as `ApiResourceProperty`, `ApiScopeProperty`, `IdentityResourceProperty`.

These are the changes you need to pay attention to.

## How to migrate to Identity Server v4?

> Please backup your database before migration!!!

* If you are using EF Core, you need to add a new migration for Identity Server v4 changes.
* Update the `IdentityServerDataSeedContributor` class in your project according to [the latest code](https://github.com/abpframework/abp/blob/dev/templates/app/aspnet-core/src/MyCompanyName.MyProjectName.Domain/IdentityServer/IdentityServerDataSeedContributor.cs).

If you have been using hard coding configuration to initialize the identity server just like we do in the application template project, a simple migration method is `clear all the tables related to the identity server in the database, and then execute the `DbMigrator` application.

If you customize the `IdentityServerDataSeedContributor` or the entities mentioned above, you may need to update your code according to the actual situation. You have already understood all the changes, I think it will not be too complicated.

If you have made some advanced changes to the Identity server, please check [the commit](https://github.com/abpframework/abp/commit/e118346f12b2fb146ab67f2655148916ba4a4518) for all our changes.

That's all.

## Related resources:
https://leastprivilege.com/2020/06/19/announcing-identityserver4-v4-0/

https://github.com/IdentityServer/IdentityServer4/issues/4592