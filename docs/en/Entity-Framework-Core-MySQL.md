# Switch to EF Core MySQL Provider

> [ABP CLI](CLI.md) and the [Get Started](https://abp.io/get-started) page already provides an option to create a new solution with MySQL. See [that document](Entity-Framework-Core-Other-DBMS.md) to learn how to use. This document provides guidance for who wants to manually switch to MySQL after creating the solution.

This document explains how to switch to the **MySQL** database provider for **[the application startup template](Startup-Templates/Application.md)** which comes with SQL Server provider pre-configured.

## Replace the Volo.Abp.EntityFrameworkCore.SqlServer Package

`.EntityFrameworkCore` project in the solution depends on the [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet package. Remove this package and add the same version of the [Volo.Abp.EntityFrameworkCore.MySQL](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL) package.

## Replace the Module Dependency

Find ***YourProjectName*EntityFrameworkCoreModule** class inside the `.EntityFrameworkCore` project, remove `typeof(AbpEntityFrameworkCoreSqlServerModule)` from the `DependsOn` attribute, add `typeof(AbpEntityFrameworkCoreMySQLModule)` (also replace `using Volo.Abp.EntityFrameworkCore.SqlServer;` with `using Volo.Abp.EntityFrameworkCore.MySQL;`).

## UseMySQL()

Find `UseSqlServer()` calls in your solution. Check the following files:

* *YourProjectName*EntityFrameworkCoreModule.cs inside the `.EntityFrameworkCore` project. Replace `UseSqlServer()` with `UseMySQL()`.
* *YourProjectName*DbContextFactory.cs inside the `.EntityFrameworkCore` project. Replace `UseSqlServer()` with `UseMySql()`. Then add a new parameter (`ServerVersion`) to `UseMySql()` method. Example: `.UseMySql(configuration.GetConnectionString("Default"), ServerVersion.Parse("8.0.21-mysql"))`. See [this issue](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/pull/1233) for more information about `ServerVersion`)

> Depending on your solution structure, you may find more code files need to be changed.

## Change the Connection Strings

MySQL connection strings are different than SQL Server connection strings. So, check all `appsettings.json` files in your solution and replace the connection strings inside them. See the [connectionstrings.com]( https://www.connectionstrings.com/mysql/ ) for details of MySQL connection string options.

You typically will change the `appsettings.json` inside the `.DbMigrator` and `.Web` projects, but it depends on your solution structure.

## Re-Generate the Migrations

The startup template uses [Entity Framework Core's Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/). EF Core Migrations depend on the selected DBMS provider. So, changing the DBMS provider will cause the migration fails.

* Delete the Migrations folder under the `.EntityFrameworkCore` project and re-build the solution.
* Run `Add-Migration "Initial"` on the Package Manager Console (select the `.DbMigrator`  (or `.Web`) project as the startup project in the Solution Explorer and select the `.EntityFrameworkCore` project as the default project in the Package Manager Console).

This will create a database migration with all database objects (tables) configured.

Run the `.DbMigrator` project to create the database and seed the initial data.

## Run the Application

It is ready. Just run the application and enjoy coding.
