# New Raw SQL queries for unmapped types feature with EF Core 8.0

## Introduction

I would love to talk about the new feature in EF Core 8.0, specifically the `raw SQL queries for unmapped types`. 
This feature was recently introduced by Microsoft and is aimed at providing more flexibility and customization in database queries.

## What is the raw SQL queries for the unmapped types feature?

To give you a better understanding, let's look at a sample repository method with the ABP framework. 
Here is an example of a raw SQL query using the new feature:

````csharp
public interface IAuthorRepository : IRepository<Author, Guid>
{
    Task<List<AuthorIdWithNames>> GetAllAuthorNamesAsync();
}

public class AuthorIdWithNames
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}

public class EfCoreAuthorRepository : EfCoreRepository<BookStoreDbContext, Author, Guid>, IAuthorRepository
{
    public EfCoreAuthorRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<AuthorIdWithNames>> GetAllAuthorNamesAsync()
    {
        return await (await GetDbContextAsync()).Database.SqlQuery<AuthorIdWithNames>(@$"SELECT Id, Name FROM Authors").ToListAsync();
    }
}
````

In this code, we can see that we are using the `SqlQuery` method to execute a raw SQL query on a custom type, `AuthorIdWithNames` in this case. This allows us to retrieve data that may not be mapped to any of our entity classes in the context.

## In summary

This feature can be particularly useful in scenarios where we need to access data from tables or views that are not directly mapped to our entities. It also provides an alternative to using stored procedures for querying data.

However, it's important to note that using raw SQL queries can increase the risk of SQL injection attacks. So, it's recommended to use parameterized queries to prevent this. Additionally, this feature may not work with certain database providers, so it's important to check for compatibility before implementing it.

In conclusion, the raw SQL queries for unmapped types feature in EF Core 8.0 is a great addition for developers looking for more flexibility in database queries. It allows us to work with data that may not be directly mapped to our entities and can be a useful tool in certain scenarios. Just remember to use parameterized queries and check for compatibility before implementing it. 

## References

- [Raw SQL queries for unmapped types](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#raw-sql-queries-for-unmapped-types)
- [SQL Queries](https://learn.microsoft.com/en-us/ef/core/querying/sql-queries#querying-scalar-(non-entity)-types)
