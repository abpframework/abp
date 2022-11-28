# Entity Framework Core Bulk Operations
Entity Framework tracks all the entity changes and applies those changes to database one by one when `SaveChanges()` method called. There was no way to execute bulk operations in Entity Framework Core without a dependency. 

As you know [Entity Framework Extensions](https://entityframework-extensions.net/bulk-savechanges) library were doing it but it was not free.

There was no other solution until now. [Bulk Operations](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates) are now available in Entity Framework Core with .NET 7.

With .NET 7, there are two new methods such as `ExecuteUpdate` and `ExecuteDelete` available to execute bulk operations. It's a similar usage with Entity Framework Core Extensions library if you're familiar with it.

You can visit the microsoft example [here](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates) about how to use it.


## Bulk Operations in ABP Framework

ABP Framework supports Entity Framework Core as its ORM. So, you can use these new methods in your repository classes in your ABP applications. But there is another feature that ABP Framework provides: `IEfCoreBulkOperationProvider`. You can visit the [documentation](https://docs.abp.io/en/abp/latest/Entity-Framework-Core#customize-bulk-operations) to learn more about it. You always have full control of execution of bulk operations in your ABP applications. When you call `InsertMany`, `DeleteMany` or `UpdateMany` methods of repository, that provider will be invoked and you can customize all the queries from one point. Mostly you don't need to implement & customize that interface, but it's good to know that you can do it if you need. 

## Usage in Repositories

Most of real world case scenarios, you need custom queries to execute bulk operations. You can't manage all the cases with overriding `IEfCoreBulkOperationProvider` even you can't identify entities in this provider because it's not aware of the properties of entities. Best way to use this new feature is inside the repositories.

Let me show an example below:

```csharp
public class BookEntityFrameworkCoreRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookEntityFrameworkCoreRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task UpdateListingAsync()
    {
        var dbSet = await GetDbSetAsync();

        dbSet
            .Where(x => x.IsListed && x.PublishedOn.Year <= 2022)
            .ExecuteUpdate(s => s.SetProperty(x => x.IsListed, x => false));
    }

    public async Task DeleteOldBooksAsync()
    {
        var dbSet = await GetDbSetAsync();

        dbSet.Where(x => x.PublishedOn.Year <= 2000)
            .ExecuteDelete();
    }

    public async Task ImportAsync(List<Book> books)
    {
        var dbSet = await GetDbSetAsync();

        // No need an action for bulk inserting.
        await base.InsertManyAsync(books); 
    }
}
```

> There is no need to take an action for bulk inserting. You can use `InsertManyAsync` method of repository instead of creating a new method for it if you don't have a custom logic. It'll use new bulk inserting feature automatically since it's available in EF Core 7.0.