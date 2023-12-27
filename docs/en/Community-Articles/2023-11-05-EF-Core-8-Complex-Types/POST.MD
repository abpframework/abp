# Using Complex Types as Value Objects with Entity Framework Core 8.0

Entity Framework Core 8.0 is being shipped in a week as a part of .NET 8.0. In this article, I will introduce the new **[Complex Types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#value-objects-using-complex-types)** feature of EF Core 8 and show some examples of how you can use it in your projects built with ABP Framework.

## What is a Value Object?

A [Value Object](https://docs.abp.io/en/abp/latest/Value-Objects) is a simple object that has no conceptual identity (Id). Instead, a Value Object is identified by its properties.

A Value Object is typically owned by a parent [Entity](https://docs.abp.io/en/abp/latest/Entities) object. Using [Complex Types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#value-objects-using-complex-types) with EF Core 8 is the best way to create Value Objects that are stored as a part of your main entity.

## Creating an ABP Project

> **WARNING**: ABP Framework has not a .NET 8.0 compatible version yet. It is planned to be released on November 15, 2023. I created this project with ABP 7.4.1 (based on .NET 7.0), then manually changed the `TargetFramework` (in the `csproj`) to `net8.0` and added the `Microsoft.EntityFrameworkCore.SqlServer` package with version `8.0.0-rc.2.23480.1`. After that, the project is being compiled, but some parts of this article may not work as expected until ABP Framework 8.0-RC.1 is released.
>
> **I will update the article once ABP Framework 8.0-RC.1 is released.**

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
public class Customer : BasicAggregateRoot<Guid>
{
    public string Name { get; set; }
    public Address HomeAddress { get; set; }
    public Address BusinessAddress { get; set; }
}
````

Here, we have two address properties, one for home and the other one for business. Since an address is a multi-values property, we can define it as a separate object:

````csharp
public class Address
{
    public string City { get; set; }
    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string PostCode { get; set; }
}
````

`Address` is a typical complex object. It is actually a part of the `Customer` object, but we wanted to collect its parts into a dedicated class to make it a domain concept and easily manage its properties together.

## Configure EF Core Mappings

We should set the `Address` class as a Complex Type in our EF Core mapping configuration. There are two ways of it.

As the first way, we can add the `ComplexType` attribute on top of the `Address` class:

````csharp
[ComplexType] // Added this line
public class Address
{
    ...
}
````

As an alternative, we can configure the mapping using the fluent mapping API. You can write the following code into the `OnModelCreating` method of your `DbContext` class:

````csharp
builder.Entity<Customer>(b =>
{
    b.ToTable("Customers");
    b.ComplexProperty(x => x.HomeAddress);     // Mapping a Complex Type
    b.ComplexProperty(x => x.BusinessAddress); // Mapping another Complex Type
    //... configure other properties
});

````

You can further configure the properties of the `Address` class:

````csharp
b.ComplexProperty(x => x.HomeAddress, a =>
{
    a.Property(x => x.City).HasMaxLength(50).IsRequired();
});
````

Once you configure the mappings, you can use the [EF Core command-line tool](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) to create a database migration:

````bash
dotnet ef migrations add "Added_Customer_And_Address"
````

And update the database:

````bash
dotnet ef database update
````

If you check the fields of the `Customers` table in your database, you will see the following fields:

* `Id`
* `Name`
* `HomeAddress_City`
* `HomeAddress_Line1`
* `HomeAddress_Line2`
* `HomeAddress_PostCode`
* `BusinessAddress_City`
* `BusinessAddress_Line1`
* `BusinessAddress_Line2`
* `BusinessAddress_PostCode`

As you see, EF Core stores the `Address` properties as a part of your main entity.

## Querying Objects

You can query entities from database using the properties of a complex type as same as you query by the properties of the main entity.

**Example: Find customers by `BusinessAddress.PostCode`:**

````csharp
public class MyService : ITransientDependency
{
    private readonly IRepository<Customer, Guid> _customerRepository;

    public MyService(IRepository<Customer, Guid> customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task DemoAsync()
    {
        var customers = await _customerRepository.GetListAsync(
            c => c.BusinessAddress.PostCode == "12345"
        );
        
        //...
    }
}
````

## Mutable vs Immutable

Entity Framework Core Complex Types can work with mutable and immutable types. In the `Address` example above, I've shown a simple mutable class - that means you can change an individual property of an `Address` object after creating it (or after querying from database). However, designing Value Objects as immutable is a highly common approach.

For example, you can use C#'s `struct` type to define an immutable `Address` type:

````csharp
public readonly struct Address(string line1, string? line2, string city, string postCode)
{
    public string City { get; } = city;
    public string Line1 { get; } = line1;
    public string? Line2 { get; } = line2;
    public string PostCode { get; } = postCode;
}
````

See the [Microsoft's documentation](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#mutability) for more examples and different usages.

## Final Notes

There are more details about using Complex Types in your applications. I want to mention some of them here:

* You can have nested complex types. For example, `Address` may have one or more `PhoneNumber` objects as its properties. Everything will work seamlessly.
* A single instance of a Complex Type can be set to multiple properties (of the same or different entities). In that case, changing the Complex object's properties will affect all of the properties. However, try to avoid that since it may create unnecessary complexities in your code that is hard to understand.
* You can manipulate mutable complex object properties just as another property in your entity. EF Core change tracking system will track them as you expect.
* Currently, EF Core doesn't support to have a collection of complex objects in an entity. It works only for properties.

For more details and examples, see the Microsoft's document in the *References* section.

## Source Code

You can find the sample project here:

[https://github.com/hikalkan/samples/tree/master/EfCoreComplexTypeDemo](https://github.com/hikalkan/samples/tree/master/EfCoreComplexTypeDemo)

## References

* [Value objects using Complex Types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#value-objects-using-complex-types) in [What's new with EF Core 8.0](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew) document by Microsoft.
* [ABP Entity Framework Core integration document](https://docs.abp.io/en/abp/latest/Entity-Framework-Core)

