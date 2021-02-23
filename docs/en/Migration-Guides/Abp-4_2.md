# ABP version 4.2 Migration Guide

This version has no breaking changes but there is an important change on the repositories that should be applied for your application for an important performance and scalability gain.

## IRepository.GetQueryableAsync

`IRepository` interface inherits `IQueryable`, so you can directly use the standard LINQ extension methods, like `Where`, `OrderBy`, `First`, `Sum`... etc.

**Example: Using LINQ directly over the repository object**

````csharp
public class BookAppService : ApplicationService, IBookAppService
{
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookAppService(IRepository<Book, Guid> bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task DoItInOldWayAsync()
    {
        //Apply any standard LINQ extension method
        var query = _bookRepository
            .Where(x => x.Price > 10)
            .OrderBy(x => x.Name);

        //Execute the query asynchronously
        var books = await AsyncExecuter.ToListAsync(query);
    }
}
````

*See [the documentation](https://docs.abp.io/en/abp/4.2/Repositories#iqueryable-async-operations) if you wonder what is the `AsyncExecuter`.*

**Beginning from the version 4.2, the recommended way is using `IRepository.GetQueryableAsync()` to obtain an `IQueryable`, then use the LINQ extension methods over it.**

**Example: Using the new GetQueryableAsync method**

````csharp
public async Task DoItInNewWayAsync()
{
    //Use GetQueryableAsync to obtain the IQueryable<Book> first
    var queryable = await _bookRepository.GetQueryableAsync();

    //Then apply any standard LINQ extension method
    var query = queryable
        .Where(x => x.Price > 10)
        .OrderBy(x => x.Name);

    //Finally, execute the query asynchronously
    var books = await AsyncExecuter.ToListAsync(query);
}
````

ABP may start a database transaction when you get an `IQueryable` (If current [Unit Of Work](https://docs.abp.io/en/abp/latest/Unit-Of-Work) is transactional). In this new way, it is possible to **start the database transaction in an asynchronous way**. Previously, we could not get the advantage of asynchronous while starting the transactions.

> **The new way has a significant performance and scalability gain. The old usage (directly using LINQ over the repositories) will be removed in the next major version (5.0).** You have a lot of time for the change, but we recommend to immediately take the action since the old usage has a big **scalability problem**.

### Actions to Take

* Use the repository's queryable feature as explained before.
* If you've overridden `CreateFilteredQuery` in a class derived from `CrudAppService`, you should override the `CreateFilteredQueryAsync` instead and remove the `CreateFilteredQuery` in your class.
* If you've overridden `WithDetails` in your custom repositories, remove it and override `WithDetailsAsync` instead.
* If you've used `DbContext` or `DbSet` properties in your custom repositories, use `GetDbContextAsync()` and `GetDbSetAsync()` methods instead of them.

You can re-build your solution and check the `Obsolete` warnings to find some of the usages need to change.

#### About IRepository Async Extension Methods

Using IRepository Async Extension Methods has no such a problem. The examples below are pretty fine:

````csharp
var countAll = await _personRepository
    .CountAsync();

var count = await _personRepository
    .CountAsync(x => x.Name.StartsWith("A"));

var book1984 = await _bookRepository
    .FirstOrDefaultAsync(x => x.Name == "John");   
````

See the [repository documentation](https://docs.abp.io/en/abp/4.2/Repositories#iqueryable-async-operations) to understand the relation between `IQueryable` and asynchronous operations.

## .NET Package Upgrades

ABP uses the latest 5.0.* .NET packages. If your application is using 5.0.0 packages, you may get an error on build. We recommend to depend on the .NET packages like `5.0.*` in the `.csproj` files to use the latest patch versions.

Example:

````xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="5.0.*" />
````

## Blazorise Library Upgrade

If you are upgrading to 4.2, you also need also upgrade the following packages in your Blazor application;

* `Blazorise.Bootstrap` to `0.9.3-preview6`
* `Blazorise.Icons.FontAwesome` to `0.9.3-preview6`