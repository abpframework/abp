# Switch to EF Core MySQL Provider

This document explains how to switch to the **MySQL** database provider for **[the application startup template](Startup-Templates/Application.md)** which comes with SQL Server provider pre-configured.

## Replace the Volo.Abp.EntityFrameworkCore.SqlServer Package

`.EntityFrameworkCore` project in the solution depends on the [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet package. Remove this package and add the same version of the [Volo.Abp.EntityFrameworkCore.MySQL](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL) package.

## Replace the Module Dependency

Find ***YourProjectName*EntityFrameworkCoreModule** class inside the `.EntityFrameworkCore` project, remove `typeof(AbpEntityFrameworkCoreSqlServerModule)` from the `DependsOn` attribute, add `typeof(AbpEntityFrameworkCoreMySQLModule)` (also replace `using Volo.Abp.EntityFrameworkCore.SqlServer;` with `using Volo.Abp.EntityFrameworkCore.MySQL;`).

## UseMySQL()

Find `UseSqlServer()` calls in your solution, replace with `UseMySQL()`. Check the following files:

* *YourProjectName*EntityFrameworkCoreModule.cs inside the `.EntityFrameworkCore` project.
* *YourProjectName*MigrationsDbContextFactory.cs inside the `.EntityFrameworkCore.DbMigrations` project.

> Depending on your solution structure, you may find more code files need to be changed.

## Change the Connection Strings

MySQL connection strings are different than SQL Server connection strings. So, check all `appsettings.json` files in your solution and replace the connection strings inside them. See the [connectionstrings.com]( https://www.connectionstrings.com/mysql/ ) for details of MySQL connection string options.

You typically will change the `appsettings.json` inside the `.DbMigrator` and `.Web` projects, but it depends on your solution structure.

## Change the Migrations DbContext

MySQL DBMS has some slight differences than the SQL Server. Some module database mapping configuration (especially the field lengths) causes problems with MySQL. For example, some of the the [IdentityServer module](Modules/IdentityServer.md) tables has such problems and it provides an option to configure the fields based on your DBMS.

The startup template contains a *YourProjectName*MigrationsDbContext which is responsible to maintain and migrate the database schema. This DbContext basically calls extension methods of the depended modules to configure their database tables.

Open the *YourProjectName*MigrationsDbContext and change the `builder.ConfigureIdentityServer();` line as shown below:

````csharp
builder.ConfigureIdentityServer(options =>
{
    options.DatabaseProvider = EfCoreDatabaseProvider.MySql;
});
````

Then `ConfigureIdentityServer()` method will set the field lengths to not exceed the MySQL limits. Refer to related module documentation if you have any problem while creating or executing the database migrations.

## Re-Generate the Migrations

The startup template uses [Entity Framework Core's Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/). EF Core Migrations depend on the selected DBMS provider. So, changing the DBMS provider will cause the migration fails.

* Delete the Migrations folder under the `.EntityFrameworkCore.DbMigrations` project and re-build the solution.
* Run `Add-Migration "Initial"` on the Package Manager Console (select the `.DbMigrator`  (or `.Web`) project as the startup project in the Solution Explorer and select the `.EntityFrameworkCore.DbMigrations` project as the default project in the Package Manager Console).

This will create a database migration with all database objects (tables) configured.

Run the `.DbMigrator` project to create the database and seed the initial data.

## Run the Application

It is ready. Just run the application and enjoy coding.