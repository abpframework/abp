# Connection Strings

> Connection string system is especially needed when you want to create or use a modular system. If you have a monolithic application with a single database, you can go with the [ABP startup solution template](Startup-Templates/Application.md), which is properly configured for you.

ABP Framework is designed to be [modular](Module-Development-Basics.md) and [multi-tenancy](Multi-Tenancy.md) aware. Connection string management is also designed to support these scenarios;

* Allows to set separate connection strings for every module, so every module can have its own physical database. Modules even might be configured to use different database providers.
* Allows to set separate connection string and use a separate database per tenant (in a SaaS application).

It also supports hybrid scenarios;

* Allows to group modules into databases (e.g., all modules into a single shared database or two modules to database A, three modules to database B, one module to database C and rest of the modules to database D)
* Allows to group tenants into databases, just like the modules.
* Allows to separate databases per tenant per module (which might be hard to maintain for you because of too many databases, but the ABP framework supports it).

All the [pre-built application modules](Modules/Index.md) are designed to be compatible these scenarios.

## Configure the Connection Strings

See the following configuration:

````json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=MyMainDb;Trusted_Connection=True;",
  "AbpIdentityServer": "Server=localhost;Database=MyIdsDb;Trusted_Connection=True;",
  "AbpPermissionManagement": "Server=localhost;Database=MyPermissionDb;Trusted_Connection=True;"
}
````

> ABP uses the `IConfiguration` service to get the application configuration. While the simplest way to write configuration into the `appsettings.json` file, it is not limited to this file. You can use environment variables, user secrets, Azure Key Vault... etc. See the [configuration](Configuration.md) document for more.

This configuration defines three different connection strings:

* `MyMainDb` (the `Default` connection string) is the main connection string of the application. If you don't specify a connection string for a module, it fallbacks to the `Default` connection string. The [application startup template](Startup-Templates/Application.md) is configured to use a single connection string, so all the modules uses a single, shared database.
* `MyIdsDb` (the `AbpIdentityServer` connection string) is used by the [IdentityServer](Modules/IdentityServer.md) module.
* `MyPermissionDb` (the `AbpPermissionManagement` connection string) is used by the [Permission Management](Modules/Permission-Management.md) module.

[Pre-built application modules](Modules/Index.md) define constants for the connection string names. For example, the [IdentityServer module](Modules/IdentityServer.md) defines a ` ConnectionStringName ` constant in the ` AbpIdentityServerDbProperties ` class (located in the ` Volo.Abp.IdentityServer ` namespace). Other modules similarly define constants, so you can investigate the connection string name.

### AbpDbConnectionOptions

ABP uses the `AbpDbConnectionOptions` to get the connection strings. If you configure the connection strings as explained above, `AbpDbConnectionOptions` is automatically filled. However, you can set or override the connection strings using [the options pattern](Options.md). You can configure the `AbpDbConnectionOptions` in the `ConfigureServices` method of your [module](Module-Development-Basics.md) as shown below:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpDbConnectionOptions>(options =>
    {
        options.ConnectionStrings.Default = "...";
        options.ConnectionStrings["AbpPermissionManagement"] = "...";
    });
}
````

## Set the Connection String Name

A module typically has a unique connection string name associated to its `DbContext` class using the `ConnectionStringName` attribute. Example:

````csharp
[ConnectionStringName("AbpIdentityServer")]
public class IdentityServerDbContext
    : AbpDbContext<IdentityServerDbContext>, IIdentityServerDbContext
{
}
````

For [Entity Framework Core](Entity-Framework-Core.md) and [MongoDB](MongoDB.md), write this to your `DbContext` class (and the interface if it has).

## Database Migrations for the Entity Framework Core

Relational databases require to create the database and the database schema (tables, views... etc.) before using it.

The startup template (with EF Core ORM) comes with a single database and a `.EntityFrameworkCore` project that contains related classes and the migration files for that database. This project mainly defines a `YourProjectNameDbContext` class that calls the `Configure...()` methods of the used modules, like `builder.ConfigurePermissionManagement()`.

Once you want to separate a module's database, you typically will need to create a second migration path. See the [EF Core Migrations](Entity-Framework-Core-Migrations.md) document to learn how to create and use a different database for a desired module.

## Multi-Tenancy

See [the multi-tenancy document](Multi-Tenancy.md) to learn how to use separate databases for tenants.

## Replace the Connection String Resolver

ABP defines the `IConnectionStringResolver` and uses it whenever it needs a connection string. It has two pre-built implementations:

* `DefaultConnectionStringResolver` uses the `AbpDbConnectionOptions` to select the connection string based on the rules defined in the "Configure the Connection Strings" section above.
* `MultiTenantConnectionStringResolver` used for multi-tenant applications and tries to get the configured connection string for the current tenant if available. It uses the `ITenantStore` to find the connection strings. It inherits from the `DefaultConnectionStringResolver` and fallbacks to the base logic if no connection string specified for the current tenant.

If you need a custom logic to determine the connection string, implement the `IConnectionStringResolver` interface (optionally derive from the existing implementations) and replace the existing implementation using the [dependency injection](Dependency-Injection.md) system.