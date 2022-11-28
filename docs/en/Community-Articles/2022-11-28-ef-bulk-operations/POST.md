# Bulk Operations with Entity Framework Core 7.0
Entity Framework tracks all the entity changes and applies those changes to the database one by one when the `SaveChanges()` method is called. There was no way to execute bulk operations in Entity Framework Core without a dependency. 

As you know the [Entity Framework Extensions](https://entityframework-extensions.net/bulk-savechanges) library was doing it but it was not free.

There was no other solution until now. [Bulk Operations](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates) are now available in Entity Framework Core with .NET 7.

With .NET 7, there are two new methods such as `ExecuteUpdate` and `ExecuteDelete` available to execute bulk operations. It's a similar usage with the Entity Framework Core Extensions library if you're familiar with it.

You can visit the microsoft example [here](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#executeupdate-and-executedelete-bulk-updates) about how to use it.

ABP Framework supports Entity Framework Core as its ORM. So, you can use these new methods in your repository classes in your ABP applications without any limitation.

## Usage in Repositories

You can use these methods in your repositories as below:

```csharp
public class BookEntityFrameworkCoreRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
{
    public BookEntityFrameworkCoreRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task UpdateListingAsync()
    {
        var dbSet = await GetDbSetAsync();

        await dbSet
            .Where(x => x.IsListed && x.PublishedOn.Year <= 2022)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsListed, x => false));
    }

    public async Task DeleteOldBooksAsync()
    {
        var dbSet = await GetDbSetAsync();

        await dbSet
            .Where(x => x.PublishedOn.Year <= 2000)
            .ExecuteDeleteAsync();
    }
}
```

> There is no need to take an action for bulk inserting. You can use the `InsertManyAsync` method of the repository instead of creating a new method for it if you don't have custom logic. It'll use a new bulk inserting feature automatically since it's available in EF Core 7.0.

> If you use `ExecuteDeleteAsync` or `ExecuteUpdateAsync`, then ABP's soft delete and auditing features can not work. Because these ABP features work with EF Core's change tracking system and these new methods doesn't work with the change tracking system. So, use them carefully.