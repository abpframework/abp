# The new EF Core JSON Columns

In this article, we will see how to use the new JSON columns in EF Core 7.0.

## JSON Columns

Most relational databases support columns that contain JSON documents. The JSON in these columns can be drilled into with queries. This allows, for example, filtering and sorting by the elements of the documents, as well as projection of elements out of the documents into results. JSON columns allow relational databases to take on some of the characteristics of document databases, creating a useful hybrid between the two.

EF7 contains provider-agnostic support for JSON columns, with an implementation for SQL Server. This support allows mapping of aggregates built from .NET types to JSON documents. Normal LINQ queries can be used on the aggregates, and these will be translated to the appropriate query constructs needed to drill into the JSON. EF7 also supports updating and saving changes to the JSON documents.

You can find more information about JSON columns in EF Core 7.0 in the [documentation](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns).

### Mapping JSON Columns

In EF Core, aggregate types are defined using `OwnsOne` and `OwnsMany`. 
`OwnsOne` is used to map a single aggregate, and `OwnsMany` is used to map a collection of aggregates.

`ToJson` is used to map a property to a JSON column. The property can be of any type that can be serialized to JSON.

These aggregates can be mapped to JSON columns using the `ToJson` method. The following example shows how to map a JSON column to an aggregate type:

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
```

Above, we have defined an aggregate type `ContactDetails` that contains an `Address` and a `Phone` number. The aggregate type is configured in `OnModelCreating` using `OwnsOne` and `ToJson`. The `Address` property is mapped to a JSON column using `ToJson`, and the `Phone` property is mapped to a regular column. This requires just one call to ToJson() when configuring the aggregate type:

```csharp	

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
            b.ToTable(MyProjectConsts.DbTablePrefix + "Persons", MyProjectConsts.DbSchema);
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

Queries into JSON columns work just the same as querying into any other aggregate type in EF Core. That is, just use LINQ! Here are some examples:

```csharp
var persons = await (await GetDbSetAsync()).ToListAsync();
```


```csharp
var contacts = await (await GetDbSetAsync()).Select(person => new
{
    person,
    person.ContactDetails.Phone,
    Addresses = person.ContactDetails.Address
}).ToListAsync();
```


```csharp

var addresses = await (await GetDbSetAsync()).Select(person => new
{
    person,
    Addresses = person.ContactDetails.Address
}).ToListAsync();
```

### Updating JSON Columns

You can update JSON columns using the `Update` method. The following example shows how to update a JSON column:

```csharp
var person = await (await GetDbSetAsync()).FirstAsync();

person.ContactDetails.Phone = "123456789";
person.ContactDetails.Address = new Address("Street", "City", "Postcode", "Country");
await UpdateAsync(person, true);
```

### Database View

![image](./Database.png)


### The Source Code
* You can find the full source code of the example application [here](https://github.com/abpframework/abp-samples/tree/master/EfCoreJSONColumnDemo).
* You can see this pull [request](https://github.com/abpframework/abp-samples/pull/210) for the changes I've done after creating the application.

### References

* [https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns)

* [https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities](https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities)