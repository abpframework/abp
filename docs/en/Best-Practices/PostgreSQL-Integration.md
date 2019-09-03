## Entity Framework Core PostgreSQL Integration

> See [Entity Framework Core Integration document](../Entity-Framework-Core.md) for the basics of the EF Core integration.

### EntityFrameworkCore Project Update

- In `Acme.BookStore.EntityFrameworkCore` project replace package `Volo.Abp.EntityFrameworkCore.SqlServer` with `Volo.Abp.EntityFrameworkCore.PostgreSql` 
- Update to use PostgreSQL in `BookStoreEntityFrameworkCoreModule`
  - Replace the `AbpEntityFrameworkCoreSqlServerModule` with the `AbpEntityFrameworkCorePostgreSqlModule`
  - Replace the `options.UseSqlServer()` with the `options.UsePostgreSql()`
- In other projects update the PostgreSQL connection string in necessary `appsettings.json` files

#### Delete Existing Migrations

Delete all existing migration files (including `DbContextModelSnapshot`)

![postgresql-delete-initial-migrations](images/postgresql-delete-initial-migrations.png)

#### Regenerate Initial Migration & Update the Database

Set the correct startup project (usually a web project),
Open the **Package Manager Console** (Tools -> Nuget Package Manager -> Package Manager Console), select the `Acme.BookStore.EntityFrameworkCore.DbMigrations` as the **Default project** and execute the following command:

Run `Add-Migration` command.
````
PM> Add-Migration Initial
````

Then execute the `Update-Database` command to update the database schema:

````
PM> Update-Database
````

![postgresql-update-database](images/postgresql-update-database.png)
