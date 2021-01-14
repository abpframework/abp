# ABP Framework 4.2 RC Has Been Published

Today, we have released the [ABP Framework](https://abp.io/) and the [ABP Commercial](https://commercial.abp.io/) 4.2.0 RC (Release Candidate). This blog post introduces the new features and important changes in this new version.

> **The planned release date for the [4.2.0 final](https://github.com/abpframework/abp/milestone/48) version is January 28, 2021**.

## Get Started with the 4.2 RC

If you want to try the version `4.2.0` today, follow the steps below;

1) **Upgrade** the ABP CLI to the version `4.2.0-rc.1` using a command line terminal:

````bash
dotnet tool update Volo.Abp.Cli -g --version 4.2.0-rc.1
````

**or install** if you haven't installed before:

````bash
dotnet tool install Volo.Abp.Cli -g --version 4.2.0-rc.1
````

2) Create a **new application** with the `--preview` option:

````bash
abp new BookStore --preview
````

See the [ABP CLI documentation](https://docs.abp.io/en/abp/latest/CLI) for all the available options.

> You can also use the *Direct Download* tab on the [Get Started](https://abp.io/get-started) page by selecting the **Preview checkbox**.

## What's new with the ABP Framework 4.2

## IRepository.GetQueryableAsync()

> **This version comes with an important change about using `IQueryable` features over the [repositories](https://docs.abp.io/en/abp/4.2/Repositories). It is suggested to read this section carefully and apply in your applications.**

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

Beginning from the version 4.2, the recommended way is using `IRepository.GetQueryableAsync()` to obtain an `IQueryable`, then use the LINQ extension methods over it.

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

> **The new way has a significant performance and scalability gain. The old usage (directly using LINQ over the repositories) will be removed in the next major version.** You have a lot of time for the change, but we recommend to immediately take the action since the old usage has a big scalability problem.

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

### Repository Bulk Operations

This version adds the following methods to the repositories:

* `InsertManyAsync`
* `UpdateManyAsync`
* `DeleteManyAsync`

The purpose of these methods to insert, update or delete many entities in one call with a better performance.

Currently, **MongoDB** provider implements these methods as a single bulk operation since MongoDB API natively supports. But current **Entity Framework Core** implementation is not a real bulk operation. Instead, it does its best with the native API of the EF Core. If you want to implement in a more performant way, you can [customize the bulk operations](https://docs.abp.io/en/abp/4.2/Entity-Framework-Core#customize-bulk-operations) with your own implementation or by using a library. We could find a good open source library for EF Core 5.0 to implement it.

### Selecting DBMS on Template Creation

And DbMigrator creates the initial migration if no migration exists.

**TODO**

### Angular Unit Testing

We've improved the modules and the startup template to setup and write unit tests easier with the Angular UI. See the [Angular Unit Testing document](https://docs.abp.io/en/abp/4.2/UI/Angular/Testing) for details.

### Other News

* Improved HTTP **request-response performance** by resolving dependencies in a deferred way in the action/page filters, interceptors and some other services.
* Removed `MultipleActiveResultSets` from connection strings for new templates for SQL Server, since the new EF Core gives a warning when using it. If you want to use it, you need to change the connection string yourself.
* Added `HardDeleteAsync` extension method that takes a predicate to delete multiple entities. This extension method is available if the entity [Soft Delete](https://docs.abp.io/en/abp/latest/Data-Filtering).
* Implemented the [Page Alerts](https://docs.abp.io/en/abp/4.2/UI/Angular/Page-Alerts) for Angular UI.
* Implemented [Page Progressbar](https://github.com/abpframework/abp/blob/dev/docs/en/UI/Blazor/Page-Progress.md). It automatically shows an undetermined progress bar on top of the page while performing an AJAX request. It also proves an API to you if you need to show/hide the progress bar in your code.

## What's new with the ABP Commercial 4.2

TODO

## ABP Commercial

TODO

## About the Next Release(s)

TODO

## Feedback

Please check out the ABP Framework 4.2.0 RC and [provide feedback](https://github.com/abpframework/abp/issues/new) to help us to release a more stable version. **The planned release date for the [4.2.0 final](https://github.com/abpframework/abp/milestone/48) version is January 28, 2021**.
