```json
//[doc-seo]
{
    "Description": "Learn how to switch your ABP Framework application to the MySQL database provider, replacing the default SQL Server setup effortlessly."
}
```

# Switch to EF Core MySQL Provider

> [ABP CLI](../../../cli/index.md) and the [Get Started](https://abp.io/get-started) page already provides an option to create a new solution with MySQL. See [that document](./other-dbms.md) to learn how to use. This document provides guidance for who wants to manually switch to MySQL after creating the solution.

This document explains how to switch to the **MySQL** database provider for **[the application startup template](../../../solution-templates/layered-web-application/index.md)** which comes with SQL Server provider pre-configured.

## Replace the Volo.Abp.EntityFrameworkCore.SqlServer Package

`.EntityFrameworkCore` project in the solution depends on the [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet package. Remove this package and add the same version of the [Volo.Abp.EntityFrameworkCore.MySQL](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL) package.

## Replace the Module Dependency

Find ***YourProjectName*EntityFrameworkCoreModule** class inside the `.EntityFrameworkCore` project, remove `typeof(AbpEntityFrameworkCoreSqlServerModule)` from the `DependsOn` attribute, add `typeof(AbpEntityFrameworkCoreMySQLModule)` (also replace `using Volo.Abp.EntityFrameworkCore.SqlServer;` with `using Volo.Abp.EntityFrameworkCore.MySQL;`).

## UseMySQL()

Find `UseSqlServer()` calls in your solution. Check the following files:

* *YourProjectName*EntityFrameworkCoreModule.cs inside the `.EntityFrameworkCore` project. Replace `UseSqlServer()` with `UseMySQL()`.
* *YourProjectName*DbContextFactory.cs inside the `.EntityFrameworkCore` project. Replace `UseSqlServer()` with `UseMySQL()`.

> Depending on your solution structure, you may find more code files need to be changed.

## Use Pomelo Provider

Alternatively, you can use the [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql) provider. Replace the [Volo.Abp.EntityFrameworkCore.MySQL](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL) package with the [Volo.Abp.EntityFrameworkCore.MySQL.Pomelo](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL.Pomelo) package in your `.EntityFrameworkCore` project.

Find ***YourProjectName*EntityFrameworkCoreModule** class inside the `.EntityFrameworkCore` project, replace `typeof(AbpEntityFrameworkCoreMySQLModule)` with `typeof(AbpEntityFrameworkCoreMySQLPomeloModule)` in the `DependsOn` attribute.

> Depending on your solution structure, you may find more code files need to be changed.

The `UseMySQL()` method calls remain the same, no changes needed.

## Change the Connection Strings

MySQL connection strings are different than SQL Server connection strings. So, check all `appsettings.json` files in your solution and replace the connection strings inside them. See the [connectionstrings.com](https://www.connectionstrings.com/mysql) for details of MySQL connection string options.

You typically will change the `appsettings.json` inside the `.DbMigrator` and `.Web` projects, but it depends on your solution structure.

## Re-Generate the Migrations

The startup template uses [Entity Framework Core's Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/). EF Core Migrations depend on the selected DBMS provider. So, changing the DBMS provider will cause the migration fails.

* Delete the Migrations folder under the `.EntityFrameworkCore` project and re-build the solution.
* Run `Add-Migration "Initial"` on the Package Manager Console (select the `.DbMigrator`  (or `.Web`) project as the startup project in the Solution Explorer and select the `.EntityFrameworkCore` project as the default project in the Package Manager Console).

This will create a database migration with all database objects (tables) configured.

Run the `.DbMigrator` project to create the database and seed the initial data.

## Run the Application

It is ready. Just run the application and enjoy coding.
