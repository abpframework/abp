# Using Complex Types as Value Objects with Entity Framework Core 8.0

Entity Framework Core 8.0 is being shipped in a week as a part of .NET 8.0. In this article, I will introduce the new **[Complex Types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#value-objects-using-complex-types)** feature of EF Core 8, and show some examples of how you can use it in your projects built with ABP Framework.

## What is a Value Object?

A [Value Object](https://docs.abp.io/en/abp/latest/Value-Objects) is a simple object that has no conceptual identity (Id). A Value Object is typically owned by a parent [Entity](https://docs.abp.io/en/abp/latest/Entities) object. Using [Complex Types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#value-objects-using-complex-types) with EF Core 8 is the best way to create Value Objects that are stored as a part of your main entity.

## Creating an ABP Project

I will show code examples, so I am creating a new ABP project using the following [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) command:

````bash
abp new ComplexTypeDemo -t app-nolayers
````

> I prefer a non-layered project to keep the things simple. If you are new to ABP Framework, follow the [Getting Started](https://docs.abp.io/en/abp/latest/Getting-Started-Overall) tutorial to learn how to create a new project from scratch.

Once I created the solution, I am running the following command to execute the database migrations in order to create the initial database:

````bash
dotnet run --project ComplexTypeDemo --migrate-database
````

> We could also execute the `migrate-database.ps1` (that is coming as a part of the solution) as a shortcut.

## Creating a Complex Type

Assume that we have a `Customer` [entity](https://docs.abp.io/en/abp/latest/Entities) as shown below:

````csharp
public class Customer : AggregateRoot<Guid>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
    public Address BusinessAddress { get; set; }
}
````



Let's see the most popular example, an `Address` class:

````csharp
public class Address
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string PostCode { get; set; }
}
````

Once we define such an `Address` class, we can use it as a part of a `Customer` object:

````csharp
public class Order
{
    public int Id { get; set; }
    public required string Contents { get; set; }
    public required Address ShippingAddress { get; set; }
    public required Address BillingAddress { get; set; }
    public Customer Customer { get; set; } = null!;
}
````



d

