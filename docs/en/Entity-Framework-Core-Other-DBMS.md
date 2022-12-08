# Switch to Another DBMS for Entity Framework Core

[ABP CLI](CLI.md) provides a `-dbms` option to allow you to choose your Database Management System (DBMS) while creating a new solution. It accepts the following values:

- `SqlServer` (default)
- `MySQL`
- `SQLite`
- `Oracle`
- `Oracle-Devart`
- `PostgreSQL`

So, if you want to use MySQL for your solution, you can use the `-dbms MySQL` option while using the `abp new` command. Example:

````bash
abp new BookStore -dbms MySQL
````

Also, the [Get Started page](https://abp.io/get-started) on the ABP website allows you to select one of the providers.

> **This document provides guidance for who wants to manually change their DBMS after creating the solution.**

You can use the following documents to learn how to **switch to your favorite DBMS**:

* [MySQL](Entity-Framework-Core-MySQL.md)
* [PostgreSQL](Entity-Framework-Core-PostgreSQL.md)
* [Oracle](Entity-Framework-Core-Oracle.md)
* [SQLite](Entity-Framework-Core-SQLite.md)

You can also configure your DBMS provider **without** these integration packages. While using the integration package is always recommended (it also makes standard for the depended version across different modules), you can do it yourself if there is no integration package for your DBMS provider.

For an example, this document explains how to switch to MySQL without using [the MySQL integration package](Entity-Framework-Core-MySQL.md).

## Replace the SQL Server Dependency

* Remove the [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet package dependency from the `.EntityFrameworkCore` project.
* Add the [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/) NuGet package dependency to your `.EntityFrameworkCore` project.

## Remove the Module Dependency 

Remove the `AbpEntityFrameworkCoreSqlServerModule` from the dependency list of your ***YourProjectName*EntityFrameworkCoreModule** class.

## Change the UseSqlServer() Calls

Find the following code part inside the *YourProjectName*EntityFrameworkCoreModule class:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer();
});
````

Replace it with the following code part:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(ctx =>
    {
        if (ctx.ExistingConnection != null)
        {
            ctx.DbContextOptions.UseMySql(ctx.ExistingConnection);
        }
        else
        {
            ctx.DbContextOptions.UseMySql(ctx.ConnectionString);
        }
    });
});
````

* `UseMySql` calls in this code is defined by the Pomelo.EntityFrameworkCore.MySql package and you can use its additional options if you need.
* This code first checks if there is an existing (active) connection to the same database in the current request and reuses it if possible. This allows to share a single transaction among different DbContext types. ABP handles the rest of the things.
* It uses `ctx.ConnectionString` and passes to the `UseMySql` if there is no active connection (which will cause to create a new database connection). Using the `ctx.ConnectionString` is important here. Don't pass a static connection string (or a connection string from a configuration). Because ABP [dynamically determines the correct connection string](Connection-Strings.md) in a multi-database or [multi-tenant](Multi-Tenancy.md) environment.

## Change the Connection Strings

MySQL connection strings are different than SQL Server connection strings. So, check all `appsettings.json` files in your solution and replace the connection strings inside them. See the [connectionstrings.com]( https://www.connectionstrings.com/mysql/ ) for details of MySQL connection string options.

You typically will change the `appsettings.json` inside the `.DbMigrator` and `.Web` projects, but it depends on your solution structure.

## DBMS restrictions

MySQL DBMS has some slight differences than the SQL Server. Some module database mapping configuration (especially the field lengths) causes problems with MySQL. For example, some of the the [IdentityServer module](Modules/IdentityServer.md) tables has such problems and it provides an option to configure the fields based on your DBMS.

The module may provide some built-in solutions. You can configure it via `ModelBuilder`. eg: `Identity Server` module.

```csharp
builder.ConfigureIdentityServer(options =>
{
    options.DatabaseProvider = EfCoreDatabaseProvider.MySql;
});
```

Then `ConfigureIdentityServer()` method will set the field lengths to not exceed the MySQL limits. Refer to related module documentation if you have any problem while creating or executing the database migrations.

## Re-Generate the Migrations

The startup template uses [Entity Framework Core's Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/). EF Core Migrations depend on the selected DBMS provider. So, changing the DBMS provider will cause the migration fails.

* Delete the Migrations folder under the `.EntityFrameworkCore` project and re-build the solution.
* Run `Add-Migration "Initial"` on the Package Manager Console (select the `.DbMigrator`  (or `.Web`) project as the startup project in the Solution Explorer and select the `.EntityFrameworkCore` project as the default project in the Package Manager Console).

This will create a database migration with all database objects (tables) configured.

Run the `.DbMigrator` project to create the database and seed the initial data.

## Run the Application

It is ready. Just run the application and enjoy coding.

Related discussions: https://github.com/abpframework/abp/issues/1920