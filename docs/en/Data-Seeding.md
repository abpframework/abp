# Data Seeding

## Introduction

Some applications (or modules) using a database may need to have some **initial data** to be able to properly start and run. For example, an **admin user** & roles must be available at the beginning. Otherwise you can not **login** to the application to create new users and roles.

Data seeding is also useful for [testing](Testing.md) purpose, so your automatic tests can assume some initial data available in the database.

### Why a Data Seed System?

While EF Core Data Seeding system provides a way, it is very limited and doesn't cover production scenarios. Also, it is only for EF Core.

ABP Framework provides a data seed system that is;

* **Modular**: Any [module](Module-Development-Basics.md) can silently contribute to the data seeding process without knowing and effecting each other. In this way, a module seeds its own initial data.
* **Database Independent**: It is not only for EF Core, it also works for other database providers (like [MongoDB](MongoDB.md)).
* **Production Ready**: It solves the problems on production environments. See the "*On Production*" section below.
* **Dependency Injection**: It takes the full advantage of dependency injection, so you can use any internal or external service while seeding the initial data. Actually, you can do much more than data seeding.

## IDataSeeder

`IDataSeeder` is the main service that is used to seed initial data. It is pretty easy to use;

````csharp
public class MyService : ITransientDependency
{
    private readonly IDataSeeder _dataSeeder;

    public MyService(IDataSeeder dataSeeder)
    {
        _dataSeeder = dataSeeder;
    }

    public async Task FooAsync()
    {
        await _dataSeeder.SeedAsync();
    }
}
````

You can [inject](Dependency-Injection.md) the `IDataSeeder` and use it to seed the initial data. We will see the *IDataSeedContributor* section below to learn how to insert data. But before that, we should understand that: Where & how to execute the `IDataSeeder.SeedAsync()`?

### On Production

The [application startup template](Startup-Templates/Application.md) comes with a *YourProjectName***.DbMigrator** project (Acme.BookStore.DbMigrator on the picture below), which is a **console application** that is responsible to **migrate** the database schema (for relational databases) and **seed** the initial data:

![bookstore-visual-studio-solution-v3](images/bookstore-visual-studio-solution-v3.png)

This console application is properly configured for you. It even supports **multi-tenant** scenarios where each tenant has its own database (migrates & seeds all necessary databases).

It is expected to run this DbMigrator application whenever you **deploy a new version** of your solution to the server. It will migrate your **database schema** (create new tables/fields... etc.) and **seed new initial data** needed to properly run the new version of your solution. Then you can deploy/start your actual application.

Even if you are using MongoDB or another NoSQL database, it is recommended to use the DbMigrator application to seed your data or perform your data migration.

Having such a separate console application has several advantages;

* You can run it before updating your application, so your application will run on the ready database.
* Your application starts faster compared to if it seeds the initial data itself.
* Your application can properly run on a clustered environment (where multiple instances of your application run concurrently). If you seed data on application startup you would have conflicts in this case.

### On Development

We suggest the same way on development. Run the DbMigrator console application whenever you [create a database migration](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/) (using EF Core `Add-Migration` command, for example) or change the data seed code (will be explained later).

You can continue to use the standard `Update-Database` command for EF Core, but it will not seed if you've created a new seed data.

### On Testing

TODO

## IDataSeedContributor

TODO