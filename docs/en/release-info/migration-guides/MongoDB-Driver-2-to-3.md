# Migrating from MongoDB Driver 2 to 3

## Introduction

The release of MongoDB Driver 3 includes numerous user-requested fixes and improvements that were deferred in previous versions due to backward compatibility concerns. It also features internal improvements to reduce technical debt and enhance maintainability. One major update is the removal of a significant portion of the public API (primarily from `MongoDB.Driver.Core`), which was not intended for public use. The removed APIs were marked as deprecated in version 2.30.0.

Please refer to the [upgrade guide](https://www.mongodb.com/docs/drivers/csharp/current/upgrade/v3/) for a complete list of breaking changes and upgrade guidelines.

## Repository Changes

Some method signatures in the `MongoDbRepository` class have been updated because the `IMongoQueryable` has been removed.  The specific changes are as follows:

- The new `GetQueryableAsync` method has been added to return `IQueryable<TEntity>`.
- The `GetMongoQueryable` and `GetMongoQueryableAsync` methods return `IQueryable<TEntity>` instead of `IMongoQueryable<TEntity>`, 
- The `GetMongoQueryable` and `GetMongoQueryableAsync` methods are marked as obsolete, You should use the new `GetQueryableAsync` method instead.

Please update your application by searching for and replacing these method calls.

> The return value of the `GetQueryableAsync` method is `IQueryable<TEntity>`, which can be used directly to perform queries, similar to EF Core. Remove all instances of `IMongoQueryable` in your project and replace them with `IQueryable`.

**Previous code example:**

```csharp
var myEntity = await (await GetMongoQueryableAsync()).As<IMongoQueryable<MyEntity>>().FirstOrDefaultAsync(x => x.Id == id);
```

**Updated code example:**

```csharp
var myEntity = await GetQueryableAsync().FirstOrDefaultAsync(x => x.Id == id);
```

## Unit Test Changes

Previously, we used the [EphemeralMongo](https://github.com/asimmon/ephemeral-mongo) library for unit testing. However, it does not support the latest version of [MongoDB.Driver 3.x](https://github.com/mongodb/mongo-go-driver). You should replace it with [MongoSandbox](https://github.com/wassim-k/MongoSandbox).

In your unit test project files, replace the following:

```xml
<PackageReference Include="EphemeralMongo.Core" Version="1.1.3" />
<PackageReference Include="EphemeralMongo6.runtime.linux-x64" Version="1.1.3" Condition="$([MSBuild]::IsOSPlatform('Linux'))" />
<PackageReference Include="EphemeralMongo6.runtime.osx-x64" Version="1.1.3" Condition="$([MSBuild]::IsOSPlatform('OSX'))" />
<PackageReference Include="EphemeralMongo6.runtime.win-x64" Version="1.1.3" Condition="$([MSBuild]::IsOSPlatform('Windows'))" />
```

With:

```xml
<PackageReference Include="MongoSandbox.Core" Version="1.0.1" />
<PackageReference Include="MongoSandbox6.runtime.linux-x64" Version="1.0.1" Condition="$([MSBuild]::IsOSPlatform('Linux'))" />
<PackageReference Include="MongoSandbox6.runtime.osx-x64" Version="1.0.1" Condition="$([MSBuild]::IsOSPlatform('OSX'))" />
<PackageReference Include="MongoSandbox6.runtime.win-x64" Version="1.0.1" Condition="$([MSBuild]::IsOSPlatform('Windows'))" />
```

In your unit test classes, replace `using EphemeralMongo` with `using MongoSandbox`.

## Official Upgrade Guide

We recommend reviewing the [upgrade guide](https://www.mongodb.com/docs/drivers/csharp/current/upgrade/v3/) for MongoDB Driver 3 to ensure a smooth migration process.
