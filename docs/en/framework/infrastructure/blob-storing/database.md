```json
//[doc-seo]
{
    "Description": "Learn how to configure BLOB storage in relational and non-relational databases using ABP Framework's database providers for seamless data management."
}
```

# BLOB Storing Database Provider

BLOB Storing Database Storage Provider can store BLOBs in a relational or non-relational database.

The database provider reads the complete input stream into memory before saving a BLOB and returns BLOB content from an in-memory buffer. Database and driver value-size limits still apply. Consider an external object-storage provider for very large BLOBs or workloads that require end-to-end streaming.

There are two database providers implemented;

* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore) package implements for [EF Core](../../data/entity-framework-core), so it can store BLOBs in [any DBMS supported](https://docs.microsoft.com/en-us/ef/core/providers/) by the EF Core.
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB) package implements for [MongoDB](../../data/mongodb).

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a database as the storage provider.

## Installation

### Automatic Installation

If you've created your solution based on the [application startup template](../../../solution-templates/layered-web-application), you can use the `abp add-module` [CLI](../../../cli) command to automatically add related packages to your solution.

Open a command prompt (terminal) in the folder containing your solution (`.sln`) file and run the following command:

````bash
abp add-module Volo.Abp.BlobStoring.Database
````

This command adds all the NuGet packages to corresponding layers of your solution. If you are using EF Core, it adds necessary configuration, adds a new database migration and updates the database.

### Manual Installation

The following NuGet packages are defined by this provider:

* [Volo.Abp.BlobStoring.Database.Domain.Shared](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain.Shared)
* [Volo.Abp.BlobStoring.Database.Domain](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain)
* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore)
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB)

You only need to install Volo.Abp.BlobStoring.Database.EntityFrameworkCore or Volo.Abp.BlobStoring.Database.MongoDB (based on your preference), since they depend on the other packages.

After installation, add the `[DependsOn]` attribute to your related [module](../../architecture/modularity/basics.md). Here is the list of module classes defined by the related NuGet packages listed above:

* `BlobStoringDatabaseDomainModule`
* `BlobStoringDatabaseDomainSharedModule`
* `BlobStoringDatabaseEntityFrameworkCoreModule`
* `BlobStoringDatabaseMongoDbModule`

Whenever you add a NuGet package to a project, also add the module class dependency.

If you are using EF Core, you also need to configure your **Migration DbContext** to add BLOB storage tables to your database schema. Call `builder.ConfigureBlobStoring()` extension method inside the `OnModelCreating` method to include mappings to your DbContext. Then you can use the standard `Add-Migration` and `Update-Database` [commands](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/) to create necessary tables in your database.

If you are using MongoDB and combine module collections in a custom `AbpMongoDbContext`, call `modelBuilder.ConfigureBlobStoring()` inside the `CreateModel` method:

````csharp
protected override void CreateModel(IMongoModelBuilder modelBuilder)
{
    base.CreateModel(modelBuilder);

    modelBuilder.ConfigureBlobStoring();
}
````

## Configuration

### Connection String

If you will use your `Default` connection string, you don't need to any additional configuration.

If you want to use a separate database for BLOB storage, use the `AbpBlobStoring` as the [connection string](../../fundamentals/connection-strings.md) name in your configuration file (`appsettings.json`). In this case, also read the [EF Core Migrations](../../data/entity-framework-core/migrations.md) document to learn how to create and use a different database for a desired module.

### Common Database Properties

The `AbpBlobStoringDatabaseDbProperties` class defines the following database settings. Set `DbTablePrefix` and `DbSchema` at application startup, before the database model is created:

* `DbTablePrefix` (`Abp` by default) is the prefix for table and collection names.
* `DbSchema` (`null` by default) is the database schema used by EF Core. MongoDB does not use this property.
* `ConnectionStringName` (`AbpBlobStoring`) is the connection-string name used by both database providers.

### Configuring the Containers

The database module selects `DatabaseBlobProvider` for the default container when no provider has already been selected. It does not replace an explicitly configured provider. If you use multiple storage providers, configure the database provider for the required default, typed or named containers.

Configuration is done in the `ConfigureServices` method of your [module](../../architecture/modularity/basics.md) class, as explained in the [BLOB Storing document](../blob-storing).

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

> See the [BLOB Storing document](../blob-storing) to learn how to configure this provider for a specific container.

## Additional Information

It is expected to use the [BLOB Storing services](../blob-storing) to use the BLOB storing system. However, if you want to work on the database tables/entities, you can use the following information.

### Database Tables and Collections

With the default `Abp` prefix, EF Core maps the entities to the `AbpBlobContainers` and `AbpBlobs` tables. MongoDB uses collections with the same names. Changing `DbTablePrefix` changes both table and collection names, while `DbSchema` only changes the EF Core schema.

### Entities

Entities defined for this module:

* `DatabaseBlobContainer` (aggregate root) represents a container stored in the database. It stores the tenant identifier and the container name. Persisted container names have a maximum length of 128 characters.
* `DatabaseBlob` (aggregate root) represents a BLOB in the database. It stores the container identifier, tenant identifier, BLOB name and content. Persisted BLOB names have a maximum length of 256 characters.

The provider creates a container record lazily when the first BLOB is saved to that container. Read, existence-check and delete operations do not create container records, and deleting the last BLOB does not delete its container record.

See the [entities document](../../architecture/domain-driven-design/entities.md) to learn what is an entity and aggregate root.

### Repositories

* `IDatabaseBlobContainerRepository`
* `IDatabaseBlobRepository`

You can also use `IRepository<DatabaseBlobContainer, Guid>` and `IRepository<DatabaseBlob, Guid>` to take the power of IQueryable. See the [repository document](../../architecture/domain-driven-design/repositories.md) for more.

### Other Services

* `DatabaseBlobProvider` is the main service that implements the database BLOB storage provider, if you want to override/replace it via [dependency injection](../../fundamentals/dependency-injection.md) (don't replace `IBlobProvider` interface, but replace `DatabaseBlobProvider` class).
