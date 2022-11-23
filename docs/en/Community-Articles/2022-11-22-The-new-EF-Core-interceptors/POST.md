# The new EF Core interceptors

## Interceptors

EF Core 7 has made a lot of enhancements to interceptors, You can see the list from [EF Core improved interceptors](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#new-and-improved-interceptors-and-events).

* Interception for creating and populating new entity instances (aka "materialization")
* Interception to modify the LINQ expression tree before a query is compiled
* Interception for optimistic concurrency handling (DbUpdateConcurrencyException)
* Interception for connections before checking if the connection string has been set
* Interception for when EF Core has finished consuming a result set, but before that result set is closed
* Interception for the creation of a DbConnection by EF Core
* Interception for DbCommand after it has been initialized

## Lazy initialization of `connection string`

You generally don't need to use [this](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#lazy-initialization-of-a-connection-string) feature, ABP has its own [connection string feature](https://docs.abp.io/en/abp/latest/Connection-Strings).  

The framework will automatically handle the module or multi-tenant connection string

## Add interceptors in `AbpDbContext`

[Add interceptors](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors#registering-interceptors) is very simple, Add your `interceptors` in the `OnConfiguring` method of `DbContext`

````csharp
public class BookStoreDbContext : AbpDbContext<BookStoreDbContext>,
{

    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new MyEfCorenterceptor());

        base.OnConfiguring(optionsBuilder);
    }
}
````

> Some interceptors may be [Singleton](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0#service-lifetimes) services. This means a single instance is used by many `DbContext` instances. The implementation must be thread-safe.


See the [EF Core Interceptors documentation](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors) for more information.
