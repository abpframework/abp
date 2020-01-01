# Entity Framework Core MySQL Database Provider

This document explains how to switch to the **MySQL** database provider for **[the application startup template](Startup-Templates/Application.md)** which comes with SQL Server provider pre-configured.

## Replace the Volo.Abp.EntityFrameworkCore.SqlServer Package

`.EntityFrameworkCore` project in the solution depends on the [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet package. Remove this package and add the same version of the [Volo.Abp.EntityFrameworkCore.MySQL](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.MySQL) package.

## Replace the Module Dependency

Find ***YourProjectName*EntityFrameworkCoreModule** class inside the `.EntityFrameworkCore` project, remove `typeof(AbpEntityFrameworkCoreSqlServerModule)` from the `DependsOn` attribute, add `typeof(AbpEntityFrameworkCoreMySQLModule)` (also replace `using Volo.Abp.EntityFrameworkCore.SqlServer;` with `using Volo.Abp.EntityFrameworkCore.MySQL;`).

## Call UseMySQL

Find `UseSqlServer()` calls in your solution, replace with `UseMySQL()`

TODO