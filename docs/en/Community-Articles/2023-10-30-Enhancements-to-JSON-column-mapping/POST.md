# EF Core 8 - Enhancements to JSON column mapping

In this article, we will examine the enhancements introduced in EF Core 8 for the JSON column feature, building upon the foundation laid by [JSON columns in Entity Framework Core 7](https://community.abp.io/posts/json-columns-in-entity-framework-core-7-cjaom76j).

## The entity classes we will be using in the article

```csharp
public class Person
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public ContactDetails ContactDetails { get; set; }
}

public class ContactDetails
{
    public List<Address> Addresses { get; set; } = new();
    public string? Phone { get; set; }
}

public class Address
{
    public Address(string street, string city, string postcode, string country)
    {
        Street = street;
        City = city;
        Postcode = postcode;
        Country = country;
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
    public bool IsMainAddress { get; set; }
}
```

## The DbContext class we will be using in the article

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if SQLSERVER
        optionsBuilder.UseSqlServer("Server=localhost;Database=EfCore8Json;Trusted_Connection=True;TrustServerCertificate=True");
#elif SQLITE
        optionsBuilder.UseSqlite("Data Source=EfCore8Json.db");
#endif
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(b =>
        {
            b.ToTable("Persons");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired();
            b.OwnsOne(x => x.ContactDetails, cb =>
            {
                cb.ToJson();
                cb.Property(x => x.Phone);
                cb.OwnsMany(x => x.Addresses);
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}
```

## Translate element access into JSON arrays

EF Core 8 supports indexing in JSON arrays when executing queries. For example, the following query returns individuals whose first address is the main address in the database:

```csharp
var query = dbContext.Persons
    .Select(x => x.ContactDetails.Addresses[0])
    .Where(x => x.IsMainAddress == true)
    .ToListAsync();
```

The generated SQL query is as follows when using SQL Server:

```sql
SELECT JSON_QUERY([p].[ContactDetails], '$.Addresses[0]'), [p].[Id]
FROM [Persons] AS [p]
WHERE CAST(JSON_VALUE([p].[ContactDetails], '$.Addresses[0].IsMainAddress') AS bit) = CAST(1 AS bit)
```

> Note: If you attempt to access an index that is outside of the array, it will return null.

## JSON Columns for SQLite

In EF Core 7, JSON column mapping was supported for Azure SQL/SQL Server. In EF Core 8, this support has been extended to include SQLite as well.

### Queries into JSON columns

The following query returns individuals whose first address is the main address in the database:

```csharp
var query = dbContext.Persons
    .Select(x => x.ContactDetails.Addresses[0])
    .Where(x => x.IsMainAddress == true)
    .ToListAsync();
```

The generated SQL query is as follows when using SQLite:

```sql
SELECT "p"."ContactDetails" ->> '$.Addresses[0]', "p"."Id"
FROM "Persons" AS "p"
WHERE "p"."ContactDetails" ->> '$.Addresses[0].IsMainAddress' = 0
```

## References

- [EF Core 8 - Enhancements to JSON column mapping](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/whatsnew#enhancements-to-json-column-mapping)