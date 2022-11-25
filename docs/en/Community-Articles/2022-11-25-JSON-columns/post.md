# The new EF Core JSON Columns

## JSON Columns

Most relational databases support columns that contain JSON documents. The JSON in these columns can be drilled into with queries. This allows, for example, filtering and sorting by the elements of the documents, as well as projection of elements out of the documents into results. JSON columns allow relational databases to take on some of the characteristics of document databases, creating a useful hybrid between the two.

EF7 contains provider-agnostic support for JSON columns, with an implementation for SQL Server. This support allows mapping of aggregates built from .NET types to JSON documents. Normal LINQ queries can be used on the aggregates, and these will be translated to the appropriate query constructs needed to drill into the JSON. EF7 also supports updating and saving changes to the JSON documents.

### Mapping JSON Columns AbpDbContext

```csharp	
public class ContactDetails
{
    public Address Address { get; set; }
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
}

public class Person : AggregateRoot<int>
{
    public string Name { get; set; } = null!;
    public ContactDetails ContactDetails { get; set; } = null!;
}

public class MyDbContext : AbpDbContext<MyDbContext>
{
    public DbSet<Person> Persons { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Person>(b =>
        {
            b.ToTable(abpEf7JsonColumnConsts.DbTablePrefix + "Persons", abpEf7JsonColumnConsts.DbSchema);
            b.ConfigureByConvention();
            b.OwnsOne(x=>x.ContactDetails, c =>
            {
                c.ToJson();
                c.OwnsOne(cd => cd.Address);
            });
        });
    }
}
```

### Querying JSON Columns

```csharp
var persons = await (await GetDbSetAsync()).ToListAsync();

var contacts = await (await GetDbSetAsync()).Select(person => new
{
    person,
    person.ContactDetails.Phone,
    Addresses = person.ContactDetails.Address
}).ToListAsync();

var addresses = await (await GetDbSetAsync()).Select(person => new
{
    person,
    Addresses = person.ContactDetails.Address
}).ToListAsync();
```

### Updating JSON Columns

```csharp
var person = await (await GetDbSetAsync()).FirstAsync();

person.ContactDetails.Phone = "123456789";
person.ContactDetails.Address = new Address("Street", "City", "Postcode", "Country");
await UpdateAsync(person, true);
```

### Database View

![image](./Database.png)
