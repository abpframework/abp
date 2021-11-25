# BLOB Storing Database Provider

BLOB Storing Database Storage Provider can store BLOBs in a relational or non-relational database.

There are two database providers implemented;

* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore) package implements for [EF Core](Entity-Framework-Core.md), so it can store BLOBs in [any DBMS supported](https://docs.microsoft.com/en-us/ef/core/providers/) by the EF Core.
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB) package implements for [MongoDB](MongoDB.md).

> Read the [BLOB Storing document](Blob-Storing.md) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a database as the storage provider.

## Installation

### Automatic Installation

If you've created your solution based on the [application startup template](Startup-Templates/Application.md), you can use the `abp add-module` [CLI](CLI.md) command to automatically add related packages to your solution.

Open a command prompt (terminal) in the folder containing your solution (`.sln`) file and run the following command:

````bash
abp add-module Volo.Abp.BlobStoring.Database
````

This command adds all the NuGet packages to corresponding layers of your solution. If you are using EF Core, it adds necessary configuration, adds a new database migration and updates the database.

### Manual Installation

Here, all the NuGet packages defined by this provider;

* [Volo.Abp.BlobStoring.Database.Domain.Shared](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain.Shared)
* [Volo.Abp.BlobStoring.Database.Domain](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain)
* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore)
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB)

You can only install Volo.Abp.BlobStoring.Database.EntityFrameworkCore or Volo.Abp.BlobStoring.Database.MongoDB (based on your preference) since they depends on the other packages.

After installation, add `DepenedsOn` attribute to your related [module](Module-Development-Basics.md). Here, the list of module classes defined by the related NuGet packages listed above:

* `BlobStoringDatabaseDomainModule`
* `BlobStoringDatabaseDomainSharedModule`
* `BlobStoringDatabaseEntityFrameworkCoreModule`
* `BlobStoringDatabaseMongoDbModule`

Whenever you add a NuGet package to a project, also add the module class dependency.

If you are using EF Core, you also need to configure your **Migration DbContext** to add BLOB storage tables to your database schema. Call `builder.ConfigureBlobStoring()` extension method inside the `OnModelCreating` method to include mappings to your DbContext. Then you can use the standard `Add-Migration` and `Update-Database` [commands](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/) to create necessary tables in your database.

## Configuration

### Connection String

If you will use your `Default` connection string, you don't need to any additional configuration.

If you want to use a separate database for BLOB storage, use the `AbpBlobStoring` as the [connection string](Connection-Strings.md) name in your configuration file (`appsettings.json`). In this case, also read the [EF Core Migrations](Entity-Framework-Core-Migrations.md) document to learn how to create and use a different database for a desired module.

### Configuring the Containers

If you are using only the database storage provider, you don't need to manually configure it, since it is automatically done. If you are using multiple storage providers, you may want to configure it.

Configuration is done in the `ConfigureServices` method of your [module](Module-Development-Basics.md) class, as explained in the [BLOB Storing document](Blob-Storing.md).

**Example: Configure to use the database storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseDatabase();
    });
});
````

> See the [BLOB Storing document](Blob-Storing.md) to learn how to configure this provider for a specific container.

## Additional Information

It is expected to use the [BLOB Storing services](Blob-Storing.md) to use the BLOB storing system. However, if you want to work on the database tables/entities, you can use the following information.

### Entities

Entities defined for this module:

* `DatabaseBlobContainer` (aggregate root) represents a container stored in the database.
* `DatabaseBlob` (aggregate root) represents a BLOB in the database.

See the [entities document](Entities.md) to learn what is an entity and aggregate root.

### Repositories

* `IDatabaseBlobContainerRepository`
* `IDatabaseBlobRepository`

You can also use `IRepository<DatabaseBlobContainer, Guid>` and `IRepository<DatabaseBlob, Guid>` to take the power of IQueryable. See the [repository document](Repositories.md) for more.

### Other Services

* `DatabaseBlobProvider` is the main service that implements the database BLOB storage provider, if you want to override/replace it via [dependency injection](Dependency-Injection.md) (don't replace `IBlobProvider` interface, but replace `DatabaseBlobProvider` class).