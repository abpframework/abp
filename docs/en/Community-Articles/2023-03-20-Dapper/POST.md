# Using Dapper with the ABP Framework

[Dapper](https://github.com/DapperLib/Dapper) is a simple and lightweight object mapper for .NET. A key feature of Dapper is its [high performance](https://github.com/DapperLib/Dapper#performance) compared to other ORMs. In this article, I will show how to use it in your ABP projects. But, first look at when to use it.

### Source Code

You can find the [full source code of the demo application here](https://github.com/abpframework/abp-samples/tree/master/Dapper).

## When to use Dapper?

In ABP Framework, we suggest to use Dapper in a combination with Entity Framework Core (EF Core) for the following reasons:

* EF Core is much easier to use (you don't need to manually write SQL queries and work with low level database objects).
* EF Core abstracts different DBMS dialects, so it will be easier to change your DBMS later.
* EF Core's change tracking system automatically update changes in the database.
* EF Core is better compatible with Object Oriented Programming (OOP) practices and is more type safe to work with. So, EF Core code is more understandable and maintainable.

In most of your use cases, you typically work with one or a few entities and a maintainable codebase can be chosen instead of slight performance difference. However, there may be certain places in your application where it matters:

* You may work with a lot of entities, so want to query faster (Indeed, EF Core's [AsNoTracking()](https://learn.microsoft.com/en-us/ef/core/querying/tracking) extension can help in most cases).
* You may be performing too many database operations in a single request.
* EF Core may not create an optimized SQL query and you may want to manually write it for a better performance.

For such cases, Dapper can be a good choice. You can easily write SQL queries and bind the result to your objects.

## Setting Up the Entity Framework Core Part

We will use EF Core with Dapper, so we need to configure the EF Core first. I will use the following `Book` entity as an example:

````csharp
public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public DateTime PublishDate { get; set; }
    public float Price { get; set; }
}
````

Once I created the `Book` entity, I should add it to my `DbContext` class.

````csharp
public class DapperDemoDbContext : AbpDbContext<DapperDemoDbContext>
{
    // 1 - ADD A DBSET PROPERTY
    public DbSet<Book> Books { get; set; }

    public DapperDemoDbContext(DbContextOptions<DapperDemoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //...other code parts
        
        // 1 - MAP YOUR ENTITY TO THE DATABASE TABLE
        builder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });
    }
}

````

TODO: database migrations...

## Using Dapper Without the Integration Package

ABP provides an integration package for Dapper. However, first I want to show using Dapper without the integration package.

### Installing the Dapper Package

First, install the [Dapper](https://www.nuget.org/packages/Dapper) package to your project. You can use a command-line terminal, locate the root path of your project (`.csproj` file that you want to install it) and run the following command:

````bash
dotnet add package Dapper
````

> If your application is layered, then we suggest to add the `Dapper` package to your `EntityFrameworkCore` integration project in your solution.



s

