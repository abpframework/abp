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

## IDataSeedContributor

`IDataSeedContributor` is the interface that should be implemented in order to seed data to the database.

**Example: Seed one initial book to the database if there is no book**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Acme.BookStore
{
    public class BookStoreDataSeedContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Book, Guid> _bookRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public BookStoreDataSeedContributor(
            IRepository<Book, Guid> bookRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _bookRepository = bookRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }
        
        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _bookRepository.GetCountAsync() > 0)
                {
                    return;
                }

                var book = new Book(
                    id: _guidGenerator.Create(),
                    name: "The Hitchhiker's Guide to the Galaxy",
                    type: BookType.ScienceFiction,
                    publishDate: new DateTime(1979, 10, 12),
                    price: 42
                );

                await _bookRepository.InsertAsync(book);
            }
        }
    }
}
````

* `IDataSeedContributor` defines the `SeedAsync` method to execute the **data seed logic**.
* It is typical to **check database** if the seeding data is already present.
* You can **inject** service and perform any logic needed to seed the data.

> Data seed contributors are automatically discovered by the ABP Framework and executed as a part of the data seed process.

### DataSeedContext

`DataSeedContext` contains `TenantId` if your application is [multi-tenant](Multi-Tenancy.md), so you can use this value while inserting data or performing custom logic based on the tenant.

`DataSeedContext` also contains name-value style configuration parameters for passing to the seeder contributors from the `IDataSeeder`.

## Modularity

An application can have multiple data seed contributor (`IDataSeedContributor`) class. So, any reusable module can also implement this interface to seed its own initial data.

For example, the [Identity Module](Modules/Identity.md) has a data seed contributor that creates an admin role and admin user and assign all the permissions.

## IDataSeeder

> You typically never need to directly use the `IDataSeeder` service since it is already done if you've started with the [application startup template](Startup-Templates/Application.md). But its suggested to read it to understand the design behind the data seed system.

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

You can [inject](Dependency-Injection.md) the `IDataSeeder` and use it to seed the initial data when you need. It internally calls all the `IDataSeedContributor` implementations to complete the data seeding.

It is possible to send named configuration parameters to the `SeedAsync` method as shown below:

````csharp
await _dataSeeder.SeedAsync(
    new DataSeedContext()
        .WithProperty("MyProperty1", "MyValue1")
        .WithProperty("MyProperty2", 42)
);
````

Then the data seed contributors can access to these properties via the `DataSeedContext` explained before. 

If a module needs to a parameter, it should be declared on the [module documentation](Modules/Index.md). For example, the [Identity Module](Modules/Identity.md) can use `AdminEmail` and `AdminPassword` parameters if you provide (otherwise uses the default values).

### Where & How to Seed Data?

It is important to understand where & how to execute the `IDataSeeder.SeedAsync()`?

#### On Production

The [application startup template](Startup-Templates/Application.md) comes with a *YourProjectName***.DbMigrator** project (Acme.BookStore.DbMigrator on the picture below), which is a **console application** that is responsible to **migrate** the database schema (for relational databases) and **seed** the initial data:

![bookstore-visual-studio-solution-v3](images/bookstore-visual-studio-solution-v3.png)

This console application is properly configured for you. It even supports **multi-tenant** scenarios where each tenant has its own database (migrates & seeds all necessary databases).

It is expected to run this DbMigrator application whenever you **deploy a new version** of your solution to the server. It will migrate your **database schema** (create new tables/fields... etc.) and **seed new initial data** needed to properly run the new version of your solution. Then you can deploy/start your actual application.

Even if you are using MongoDB or another NoSQL database (that doesn't need to schema migrations), it is recommended to use the DbMigrator application to seed your data or perform your data migration.

Having such a separate console application has several advantages;

* You can **run it before** updating your application, so your application will run on the ready database.
* Your application **starts faster** compared to if it seeds the initial data itself.
* Your application can properly run on a **clustered environment** (where multiple instances of your application run concurrently). If you seed data on application startup you would have conflicts in this case.

#### On Development

We suggest the same way on development. Run the DbMigrator console application whenever you [create a database migration](https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/migrations/) (using EF Core `Add-Migration` command, for example) or change the data seed code (will be explained later).

> You can continue to use the standard `Update-Database` command for EF Core, but it will not seed if you've created a new seed data.

#### On Testing

You probably want to seed the data also for automated [testing](Testing.md), so want to use the `IDataSeeder.SeedAsync()`. In the [application startup template](Startup-Templates/Application.md), it is done in the [OnApplicationInitialization](Module-Development-Basics.md) method of the *YourProjectName*TestBaseModule class of the TestBase project.

In addition to the standard seed data (that is also used on production), you may want to seed additional data unique to the automated tests. If so, you can create a new data seed contributor in the test project to have more data to work on.