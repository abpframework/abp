# EF Core 9 LINQ & SQL translation

EF Core improves the translation of LINQ queries to SQL with every release. EF Core 9 is no exception. This article will show you some of the improvements in EF Core 9.

EF Core 9 includes a lot of improvements in LINQ to SQL translation. we don't cover all of them in this article. You can find more information in the [official release notes](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-9.0/whatsnew#linq-and-sql-translation).

## Support for complex types

### GroupBy

EF Core now supports grouping by complex type instance. For example:

```csharp
var groupedAddress = await context.Customers
    .GroupBy(c => new { c.Address })
    .Select(g => new { g.Key, Count = g.Count() })
    .ToListAsync();
```

Address is a complex type as a value object here.

### ExecuteUpdate

EF Core now supports updating a complex type. For example:

```csharp
var newAddress = new Address("New Street", "New City", "New Country");

await context.Customers
    .Where(e => e.Region == "Turkey")
    .ExecuteUpdateAsync(s => s.SetProperty(b => b.Address, newAddress));
```

EF Core updates each column of the complex type.

## Prune unneeded elements from SQL

Ef Core now translates LINQ queries to SQL more efficiently. It will remove unneeded elements from the SQL query and bring better performance.

### Table pruning

When you use table-per-hierarchy (TPH) inheritance, previously EF Core generated SQL queries that included JIONs to tables that were not needed.

For example:

```csharp
public class Order
{
    public int Id { get; set; }
    ...

    public Customer Customer { get; set; }
}

public class DiscountedOrder : Order
{
    public double Discount { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    ...

    public List<Order> Orders { get; set; }
}

public class AppContext : DbContext
{
    ...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>().UseTptMappingStrategy();
    }
}
```

Consider the following query to get all customers with at least one order:

```csharp
var customers = await context.Customers.Where(o => o.Orders.Any()).ToListAsync();
```

Previously, EF Core generated the following SQL query:

```sql
SELECT [c].[Id], [c].[Name]
FROM [Customers] AS [c]
WHERE EXISTS (
    SELECT 1
    FROM [Orders] AS [o]
    LEFT JOIN [DiscountedOrders] AS [d] ON [o].[Id] = [d].[Id]
    WHERE [c].[Id] = [o].[CustomerId])
```

It included a JOIN to the `DiscountedOrders` table, which was not needed. In EF Core 9, the generated SQL query is:

```sql
SELECT [c].[Id], [c].[Name]
FROM [Customers] AS [c]
WHERE EXISTS (
    SELECT 1
    FROM [Orders] AS [o]
    WHERE [c].[Id] = [o].[CustomerId])
```

## EF Core in ABP

ABP Framework is built on top of the latest technologies. It will support EF Core 9 as soon as it is released. You can use the latest features of EF Core in your ABP applications.

For example, you can use the `ExecuteUpdateAsync` method in your ABP application:

```csharp
public class Book : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public float Price { get; set; }

    public string Author { get; set; }
}

public class AppContext : AbpDbContext<AppContext>
{
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Author).IsRequired().HasMaxLength(64);
        });
    }
}

public class BookRepository : EfCoreRepository<AppContext, Book, Guid>, IBookRepository
{
    public BookRepository(IDbContextProvider<AppContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task UpdatePriceByAuthorAsync(string author, float price)
    {
        await (await GetDbSetAsync())
            .Where(b => b.Author == author)
            .ExecuteUpdateAsync(b => b.SetProperty(x => x.Price, price));
    }
}
```

* `FullAuditedAggregateRoot` is an aggregate root base class with auditing properties provided by ABP Framework.
* `IRepository` is a generic repository interface provided by ABP Framework that provides CRUD operations and you can use EF Core's API in your entity repository implementation.

## References

* [LINQ and SQL translation](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-9.0/whatsnew#linq-and-sql-translation)
* [ABP Entity Framework Core Integration](https://abp.io/docs/latest/framework/data/entity-framework-core)
* [ABP Entities](https://abp.io/docs/latest/framework/architecture/domain-driven-design/entities)
