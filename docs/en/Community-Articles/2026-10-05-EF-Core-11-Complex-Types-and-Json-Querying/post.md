# EF Core 11: Complex Types, JSON Queries, and JSON Indexes

> **Release status.** At the time of writing, EF Core 11 and .NET 11 are not final releases. EF Core 11 requires the .NET 11 SDK to build and the .NET 11 runtime to run. The APIs and SQL shown in this article should therefore be treated as release-candidate behavior until .NET 11 and EF Core 11 reach GA.
>
> There is another important distinction: **complex types mapped to JSON columns were introduced in EF Core 10**. EF Core 11 builds on that foundation with better querying, JSON indexing support, and support for complex types and JSON columns in TPT/TPC inheritance hierarchies.

Relational databases are very good at normalized data.

But not every part of an application's domain model naturally wants to become another table.

Consider customer contact information:

```csharp
public class Customer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required Contact Contact { get; set; }
}

public record Contact
{
    public required string Email { get; init; }

    public required Address Address { get; init; }

    public List<string> Tags { get; init; } = [];
}

public record Address
{
    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }

    public required string PostalCode { get; init; }
}
```

Traditionally, we have three obvious choices:

1. Flatten the properties into the `Customers` table.
2. Normalize them into additional tables.
3. Serialize the whole object ourselves into a string column.

The first option can create very wide tables. The second introduces joins and additional relational identity for data that may not actually have independent identity. The third keeps the schema simple, but normally gives up strongly typed querying.

JSON columns give us another option.

With modern EF Core, we can keep `Contact` as a proper part of our .NET domain model, persist it as one JSON document, and still write LINQ queries against properties inside that document.

EF Core 11 makes this model considerably more interesting.

In this article, we will look at:

- Mapping complex types to JSON columns
- Querying JSON from LINQ
- `JSON_PATH_EXISTS` and `JSON_CONTAINS`
- JSON indexes and why they matter
- Complex types with TPT and TPC inheritance
- Provider and database requirements
- JSON versus normalized relational modeling
- A small realistic sample
- Production-readiness considerations
- An adoption checklist

The SQL Server examples target **SQL Server 2025**, because some of the most interesting EF Core 11 improvements depend on SQL Server's newer native JSON capabilities.

---

## Why Complex Types Fit JSON So Well

Before talking about JSON, it is worth understanding what EF Core means by a **complex type**.

A complex type is part of an entity's value.

It does not have its own identity or primary key.

That distinction matters.

Consider this:

```csharp
public class Customer
{
    public int Id { get; set; }

    public required Contact Contact { get; set; }
}
```

`Customer` is an entity.

`Contact` is not.

There is no meaningful:

```text
ContactId = 42
```

in this model.

The contact information exists because the customer exists.

Conceptually:

```text
Customer
│
├── Id
├── Name
│
└── Contact
    ├── Email
    ├── Tags[]
    │
    └── Address
        ├── Street
        ├── City
        ├── Country
        └── PostalCode
```

That structure maps naturally to a JSON document:

```json
{
  "Email": "berkan@example.com",
  "Address": {
    "Street": "Istiklal Street",
    "City": "Istanbul",
    "Country": "TR",
    "PostalCode": "34430"
  },
  "Tags": [
    "premium",
    "newsletter"
  ]
}
```

The important point is that we are not treating JSON as an unstructured `string`.

EF still understands the shape of `Contact`.

That means we can query:

```csharp
customer.Contact.Address.City
```

rather than manually writing JSON paths everywhere.

---

## Mapping a Complex Type to a JSON Column

Let's build a small customer application.

Our entity is:

```csharp
public class Customer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required Contact Contact { get; set; }
}

public record Contact
{
    public required string Email { get; init; }

    public required Address Address { get; init; }

    public List<string> Tags { get; init; } = [];
}

public record Address
{
    public required string Street { get; init; }

    public required string City { get; init; }

    public required string Country { get; init; }

    public required string PostalCode { get; init; }
}
```

Then configure the complex property:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Customer>(builder =>
    {
        builder.ComplexProperty(
            customer => customer.Contact,
            contact => contact.ToJson());
    });
}
```

The important call is:

```csharp
contact.ToJson()
```

Without `ToJson()`, a relational provider can map the individual complex-type properties into normal columns.

With `ToJson()`, the complex object is stored inside a single JSON column.

On SQL Server 2025, we can make the native JSON type explicit:

```csharp
builder.ComplexProperty(
    customer => customer.Contact,
    contact => contact
        .ToJson()
        .HasColumnType("json"));
```

Conceptually, our table now looks like this:

```text
Customers
┌────┬──────────────┬─────────────────────────────────────────────┐
│ Id │ Name         │ Contact                                     │
├────┼──────────────┼─────────────────────────────────────────────┤
│ 1  │ Alice Smith  │ {"Email":"alice@...","Address":{...},...}   │
│ 2  │ Bob Jones    │ {"Email":"bob@...","Address":{...},...}     │
└────┴──────────────┴─────────────────────────────────────────────┘
```

Instead of this:

```text
Customers
├── Id
├── Name
├── Contact_Email
├── Contact_Address_Street
├── Contact_Address_City
├── Contact_Address_Country
├── Contact_Address_PostalCode
└── ...
```

or a multi-table structure such as:

```text
Customers
    │
    └── CustomerAddresses
            │
            └── CustomerTags
```

This is one of the most useful characteristics of complex types: **your object model does not have to mirror your physical relational layout**.

---

## Complex Collections

Collections are even more interesting.

Suppose an order contains a snapshot of the purchased items:

```csharp
public class Order
{
    public int Id { get; set; }

    public required string Number { get; set; }

    public List<OrderItem> Items { get; set; } = [];
}

public record OrderItem
{
    public required string Sku { get; init; }

    public required string Name { get; init; }

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }
}
```

A complex collection on a relational provider is mapped to JSON:

```csharp
modelBuilder.Entity<Order>()
    .ComplexCollection(
        order => order.Items,
        items => items.ToJson());
```

The column contains something similar to:

```json
[
  {
    "Sku": "MONITOR-27",
    "Name": "27-inch Monitor",
    "Quantity": 2,
    "UnitPrice": 399.90
  },
  {
    "Sku": "USB-C-CABLE",
    "Name": "USB-C Cable",
    "Quantity": 3,
    "UnitPrice": 19.90
  }
]
```

This can be a good model for **snapshot data**.

The order does not necessarily need a relational relationship to the current product name or current price. In fact, for historical correctness, it often should not.

The order should remember what was purchased at that moment.

That is a production use case where JSON can be a better fit than blindly normalizing every object.

---

# Querying JSON with LINQ

Persisting JSON is only half of the story.

The real benefit comes when we can still query it.

Suppose we want all customers from Istanbul:

```csharp
var customers = await context.Customers
    .Where(customer =>
        customer.Contact.Address.City == "Istanbul")
    .ToListAsync();
```

From the application side, this looks like an ordinary strongly typed LINQ query.

EF knows that:

```csharp
customer.Contact.Address.City
```

lives inside the JSON document and translates the expression to the provider's JSON capabilities.

This is fundamentally different from doing:

```csharp
JsonSerializer.Deserialize<Contact>(customer.ContactJson)
```

after loading every row into memory.

The filtering happens in the database.

That difference becomes critical when the table contains millions of rows.

---

# Checking Whether a JSON Path Exists

Real JSON data is not always perfectly uniform.

Sometimes we care whether a property exists at all.

EF Core 11 introduces:

```csharp
EF.Functions.JsonPathExists(...)
```

For example:

```csharp
var blogs = await context.Blogs
    .Where(blog =>
        EF.Functions.JsonPathExists(
            blog.JsonData,
            "$.OptionalInt"))
    .ToListAsync();
```

On SQL Server this translates to `JSON_PATH_EXISTS`:

```sql
SELECT [b].[Id], [b].[Name], [b].[JsonData]
FROM [Blogs] AS [b]
WHERE JSON_PATH_EXISTS([b].[JsonData], N'$.OptionalInt') = 1;
```

`JSON_PATH_EXISTS` has been available since SQL Server 2022, so this particular API does not require SQL Server 2025.

This is useful for:

- Gradually evolving JSON documents
- Optional metadata
- Imported external documents
- Feature-specific properties
- Backward-compatible document schemas

It can also be used with complex types and owned entities mapped to JSON.

---

# `Contains()` Gets Much Better on SQL Server 2025

Now consider a primitive JSON collection:

```csharp
public class Blog
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public List<string> Tags { get; set; } = [];
}
```

We want blogs tagged with `ef-core`:

```csharp
var blogs = await context.Blogs
    .Where(blog => blog.Tags.Contains("ef-core"))
    .ToListAsync();
```

The LINQ is not new.

The interesting part is the SQL.

Before EF Core 11's SQL Server 2025 translation, SQL Server commonly needed an `OPENJSON` operation:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE N'ef-core' IN
(
    SELECT [t].[value]
    FROM OPENJSON([b].[Tags])
         WITH ([value] nvarchar(max) '$') AS [t]
);
```

`OPENJSON` expands the JSON array into a relational rowset and then searches it.

That works.

But SQL Server 2025 has a better primitive for this particular job:

```sql
JSON_CONTAINS
```

Configure EF to target compatibility level 170:

```csharp
services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sql => sql.UseCompatibilityLevel(170));
});
```

The same LINQ query:

```csharp
blog.Tags.Contains("ef-core")
```

can now become:

```sql
SELECT [b].[Id], [b].[Name], [b].[Tags]
FROM [Blogs] AS [b]
WHERE JSON_CONTAINS([b].[Tags], 'ef-core') = 1;
```

That is important for two reasons.

First, the generated SQL is closer to what we actually mean.

Second, `JSON_CONTAINS` can take advantage of SQL Server's JSON indexes.

That changes the performance discussion significantly.

---

# Calling `JSON_CONTAINS` Directly

Sometimes collection `Contains()` is not enough.

We may want to search at a particular JSON path.

EF Core 11 therefore also exposes:

```csharp
EF.Functions.JsonContains(...)
```

For example:

```csharp
var blogs = await context.Blogs
    .Where(blog =>
        EF.Functions.JsonContains(
            blog.JsonData,
            8,
            "$.Rating") == 1)
    .ToListAsync();
```

SQL Server 2025 can translate this to:

```sql
SELECT [b].[Id], [b].[Name], [b].[JsonData]
FROM [Blogs] AS [b]
WHERE JSON_CONTAINS(
    [b].[JsonData],
    8,
    N'$.Rating') = 1;
```

The direct API is useful when you need more control than normal LINQ collection operations provide.

One constraint is easy to miss: SQL Server's `JSON_CONTAINS` does not support searching for `null`.

Because of that, EF Core only uses this translation when it can establish appropriate nullability. Otherwise, it can fall back to the older `OPENJSON` strategy.

So do not assume:

```text
Contains() == JSON_CONTAINS()
```

for every query.

Inspect the generated SQL for performance-sensitive queries.

---

# JSON Indexes: Where This Becomes Production-Interesting

Putting JSON into a database is easy.

Querying a large amount of JSON efficiently is the difficult part.

Consider:

```csharp
var customers = await context.Customers
    .Where(customer =>
        customer.Contact.Address.City == "Istanbul")
    .ToListAsync();
```

If SQL Server has to inspect the JSON document of every customer, the model may perform well with 5,000 customers and badly with 5,000,000.

EF Core 11 can configure indexes over paths inside JSON-mapped complex types.

For example:

```csharp
modelBuilder.Entity<Customer>(builder =>
{
    builder.ComplexProperty(
        customer => customer.Contact,
        contact => contact
            .ToJson()
            .HasColumnType("json"));

    builder.HasIndex("Contact.Address.City");
});
```

On SQL Server 2025, this can generate a JSON index similar to:

```sql
CREATE JSON INDEX [IX_Customers_Contact_Address_City]
ON [Customers]([Contact])
FOR (N'$.Address.City');
```

Notice what happened.

Our EF model talks about:

```text
Contact.Address.City
```

while the database index talks about:

```text
Contact column
    +
$.Address.City JSON path
```

EF connects the two.

---

## Indexing JSON Collections

EF Core 11's index path syntax also understands collections.

Suppose:

```csharp
modelBuilder.Entity<Order>()
    .ComplexCollection(
        order => order.Items,
        items => items.ToJson());
```

We can describe an index over every item's SKU:

```csharp
modelBuilder.Entity<Order>()
    .HasIndex("Items[].Sku");
```

The `[]` means:

```text
every element of the JSON collection
```

We can also target a particular position when the model requires it.

This is a powerful capability, but it should not encourage us to index every possible JSON property.

Indexes are not free.

They:

- Consume storage
- Increase write cost
- Add maintenance work
- Increase migration complexity

The right question is not:

> Can this JSON path be indexed?

It is:

> Is this JSON path part of an important and selective production query?

---

# What a JSON Index Changes

Imagine one million customers.

Without an appropriate index:

```text
Customers
    │
    ├── read Contact JSON
    ├── evaluate $.Address.City
    ├── compare
    └── repeat
         × 1,000,000
```

Conceptually, the plan tends toward:

```text
Table/Index Scan
    ↓
JSON expression evaluation
    ↓
Filter City = Istanbul
```

With a usable JSON index:

```text
JSON Index
    │
    └── $.Address.City = Istanbul
             ↓
         matching rows
```

Conceptually:

```text
JSON Index Seek/Search
    ↓
Matching Customers
```

The exact execution plan depends on the SQL Server build, statistics, query shape, data distribution, and optimizer decisions.

So the rule is the same as with ordinary relational indexes:

**measure the actual execution plan.**

For SQL Server, useful tools include:

```sql
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
```

Then execute the query with and without the index and compare:

- Logical reads
- CPU time
- Elapsed time
- Scan versus index access
- Estimated versus actual rows

Do not benchmark only application-level wall-clock time.

A 15 ms query on your laptop tells you almost nothing about its behavior at production scale.

---

# A Small Realistic Model

Let's combine the ideas into a simple commerce example.

```text
Customer
│
├── Id
├── Name
└── Contact                    JSON
    ├── Email
    ├── Address
    │   ├── Street
    │   ├── City
    │   ├── Country
    │   └── PostalCode
    └── Tags[]

Order
│
├── Id
├── CustomerId
├── Number
└── Items[]                    JSON
    ├── Sku
    ├── Name
    ├── Quantity
    └── UnitPrice
```

The `DbContext` can look like this:

```csharp
public sealed class CommerceDbContext(
    DbContextOptions<CommerceDbContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(builder =>
        {
            builder.HasKey(customer => customer.Id);

            builder.ComplexProperty(
                customer => customer.Contact,
                contact => contact
                    .ToJson()
                    .HasColumnType("json"));

            builder.HasIndex("Contact.Address.City");
        });

        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(order => order.Id);

            builder.ComplexCollection(
                order => order.Items,
                items => items
                    .ToJson()
                    .HasColumnType("json"));

            builder.HasIndex("Items[].Sku");
        });
    }
}
```

SQL Server configuration:

```csharp
var connectionString =
    builder.Configuration.GetConnectionString("Default")!;

builder.Services.AddDbContext<CommerceDbContext>(options =>
{
    options.UseSqlServer(
        connectionString,
        sqlServer =>
            sqlServer.UseCompatibilityLevel(170));
});
```

Seed a customer:

```csharp
context.Customers.Add(
    new Customer
    {
        Name = "Alice",
        Contact = new Contact
        {
            Email = "alice@example.com",
            Address = new Address
            {
                Street = "Istiklal Street",
                City = "Istanbul",
                Country = "TR",
                PostalCode = "34430"
            },
            Tags =
            [
                "premium",
                "newsletter"
            ]
        }
    });

await context.SaveChangesAsync();
```

Then query the nested object:

```csharp
var istanbulCustomers = await context.Customers
    .Where(customer =>
        customer.Contact.Address.City == "Istanbul")
    .ToListAsync();
```

And query a primitive JSON collection:

```csharp
var premiumCustomers = await context.Customers
    .Where(customer =>
        customer.Contact.Tags.Contains("premium"))
    .ToListAsync();
```

To inspect what EF generates without executing the query:

```csharp
var query = context.Customers
    .Where(customer =>
        customer.Contact.Address.City == "Istanbul");

Console.WriteLine(query.ToQueryString());
```

I recommend doing this for every JSON query that matters operationally.

LINQ can look innocent while producing very different SQL depending on:

- EF Core version
- Provider version
- Database version
- Compatibility level
- Nullability
- Index availability

---

# Complex Types with TPT and TPC

EF Core 11 also removes an important modeling limitation.

Complex types and JSON columns can now be used with entities mapped using **TPT (table-per-type)** and **TPC (table-per-concrete-type)** inheritance.

Consider:

```csharp
public abstract class Animal
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required AnimalDetails Details { get; set; }
}

public class Dog : Animal
{
    public bool IsTrained { get; set; }
}

public class Cat : Animal
{
    public int LivesRemaining { get; set; }
}

public record AnimalDetails
{
    public DateOnly BirthDate { get; init; }

    public string? Veterinarian { get; init; }
}
```

For TPT:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Animal>()
        .UseTptMappingStrategy();

    modelBuilder.Entity<Animal>()
        .ComplexProperty(
            animal => animal.Details,
            details => details.ToJson());
}
```

Conceptually:

```text
Animals
├── Id
├── Name
└── Details JSON

Dogs
├── Id
└── IsTrained

Cats
├── Id
└── LivesRemaining
```

The complex value belongs to the base `Animal`, while derived entity data follows the TPT strategy.

TPC is also supported:

```csharp
modelBuilder.Entity<Animal>()
    .UseTpcMappingStrategy();

modelBuilder.Entity<Animal>()
    .ComplexProperty(
        animal => animal.Details,
        details => details.ToJson());
```

Conceptually:

```text
Dogs
├── Id
├── Name
├── Details JSON
└── IsTrained

Cats
├── Id
├── Name
├── Details JSON
└── LivesRemaining
```

This matters for domain models where inheritance and value objects appear together.

Before EF Core 11, choosing TPT or TPC could prevent you from using complex types or JSON in scenarios that otherwise made sense.

That restriction is removed.

It does not mean TPT or TPC suddenly becomes the right inheritance strategy for every application.

TPT can introduce joins.

TPC duplicates base columns across concrete tables.

JSON solves neither of those costs.

Choose the inheritance strategy independently, then use complex types where they accurately represent value semantics.

---

# JSON Versus Normalized Relational Modeling

JSON support is not an argument for turning SQL Server into MongoDB.

The relational model is still usually better for data with independent identity and relationships.

Consider order items.

We could store them as JSON:

```text
Orders
├── Id
├── CustomerId
└── Items JSON
```

or normalize them:

```text
Orders
    │
    └── OrderItems
            │
            └── Product
```

Neither is universally correct.

## JSON is attractive when

The data:

- Belongs strongly to one aggregate
- Has no useful independent identity
- Is normally loaded with its owner
- Is updated together with its owner
- Has a nested/document-like structure
- Is primarily queried through a small number of known paths
- Represents a historical snapshot
- Would otherwise create many low-value tables or columns

Examples include:

- Shipping snapshots
- Contact information
- Product configuration
- Integration metadata
- User preferences
- Order snapshots
- Structured audit metadata

## Normalization is usually better when

The nested data:

- Has its own lifecycle
- Is referenced by other entities
- Participates heavily in joins
- Requires foreign keys
- Needs uniqueness constraints
- Is independently updated
- Is frequently aggregated across rows
- Has many independently queried attributes
- Requires strong relational integrity

For example, this is suspicious:

```json
{
  "ProductId": 42,
  "SupplierId": 18,
  "WarehouseId": 7
}
```

if all three IDs represent real relational entities and your application constantly joins them.

You may be hiding a relational model inside a JSON document.

---

# The Trade-Off in One Picture

```text
NORMALIZED MODEL

Customer
   │
   ├──────── Address
   │
   └──────── Tags
              │
              └── Tag

Strengths:
✓ constraints
✓ joins
✓ independent updates
✓ mature indexing
✓ relational integrity

Costs:
• more tables
• more joins
• more mapping


JSON MODEL

Customer
   │
   └── Contact JSON
        ├── Address
        └── Tags[]

Strengths:
✓ aggregate locality
✓ natural nested model
✓ fewer tables
✓ flexible document shape
✓ value-object semantics

Costs:
• provider-specific querying
• specialized indexing
• weaker relational constraints
• document update costs
• easier to misuse
```

The decision should follow the domain and query patterns, not the novelty of the feature.

---

# JSON Does Not Remove Schema

A common misconception is:

> If we store it as JSON, we no longer have a schema.

That is not true here.

Your C# model is still a schema.

```csharp
public record Address
{
    public required string City { get; init; }

    public required string Country { get; init; }
}
```

Changing it can still require:

- Data migration
- Compatibility handling
- Deployment coordination
- Index changes
- Query changes

JSON moves part of the schema **inside a column**.

It does not eliminate schema evolution.

That distinction is especially important in long-lived ABP applications.

---

# Provider Requirements Matter

The EF model is provider-independent at the conceptual level.

JSON capabilities are not.

This is one of the most important production constraints in this article.

## SQL Server

For the SQL Server examples:

### `JSON_PATH_EXISTS`

Requires SQL Server 2022 or later.

EF Core 11 exposes it through:

```csharp
EF.Functions.JsonPathExists(...)
```

### `JSON_CONTAINS`

Requires SQL Server 2025.

To allow EF to generate SQL Server 2025-specific SQL, configure:

```csharp
sqlServer.UseCompatibilityLevel(170);
```

### Native `json` columns and JSON indexes

The examples in this article use SQL Server 2025's native JSON capabilities.

For example:

```csharp
.HasColumnType("json")
```

and:

```csharp
builder.HasIndex("Contact.Address.City");
```

can result in a native JSON index.

Older SQL Server versions may store JSON in textual columns and do not provide the same indexing story.

## Other providers

Do not assume that:

```csharp
EF.Functions.JsonContains(...)
```

means every EF Core provider automatically supports equivalent SQL.

Providers must implement translation for their database.

PostgreSQL, MySQL, SQLite, Oracle, and other databases all have different JSON:

- Types
- Operators
- Functions
- Indexes
- Path syntax
- Performance characteristics

Before choosing JSON as an architectural dependency, verify your **specific provider version**.

This is especially important for applications designed to support multiple database providers.

---

# Query Performance: Measure the Model You Actually Deploy

Suppose this query is important:

```csharp
var customers = await context.Customers
    .Where(customer =>
        customer.Contact.Address.City == city)
    .ToListAsync();
```

There are four separate questions:

```text
1. Can EF translate it?
       ↓
2. What SQL does the provider generate?
       ↓
3. Can the database use an index?
       ↓
4. Does the execution plan actually use it?
```

Passing step 1 does not guarantee step 4.

For production systems, inspect:

```csharp
query.ToQueryString()
```

and then the database execution plan.

A practical workflow is:

```text
LINQ
 ↓
ToQueryString()
 ↓
Generated SQL
 ↓
Actual execution plan
 ↓
STATISTICS IO / TIME
 ↓
Load test
```

That is particularly important for JSON because a small change in translation can turn an indexed lookup into document parsing across a large table.

---

# Running the Sample

A minimal project can be created with:

```bash
dotnet new console -n EfCore11JsonDemo
cd EfCore11JsonDemo
```

Then add the EF Core 11 SQL Server packages matching the .NET 11 release candidate you are using:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --prerelease
dotnet add package Microsoft.EntityFrameworkCore.Design --prerelease
```

Install or update the EF tool if necessary:

```bash
dotnet tool update --global dotnet-ef --prerelease
```

The project should target:

```xml
<TargetFramework>net11.0</TargetFramework>
```

Create the migration:

```bash
dotnet ef migrations add InitialCreate
```

Inspect it before applying it:

```bash
dotnet ef migrations script
```

Then:

```bash
dotnet ef database update
```

For SQL Server 2025, verify the database compatibility level as part of the environment setup.

Then print the generated SQL:

```csharp
var query = context.Customers
    .Where(customer =>
        customer.Contact.Tags.Contains("premium"));

Console.WriteLine(query.ToQueryString());
```

For performance testing, run the equivalent SQL with:

```sql
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
```

and capture the actual execution plan.

> **Testing note:** I have intentionally not included fabricated benchmark numbers here. JSON-index performance is highly dependent on row count, document size, selectivity, SQL Server build, statistics, hardware, and workload. The meaningful test is an indexed versus non-indexed comparison against a representative dataset on the exact database version you plan to deploy.

---

# A Note for ABP Applications

These features fit naturally into an ABP domain model.

For example:

```csharp
public class Customer : AggregateRoot<Guid>
{
    public string Name { get; private set; }

    public Contact Contact { get; private set; }

    protected Customer()
    {
    }

    public Customer(
        Guid id,
        string name,
        Contact contact)
        : base(id)
    {
        Name = name;
        Contact = contact;
    }
}
```

`Contact` is conceptually a value, not another aggregate.

That makes a complex type a good match.

Your EF Core configuration can then keep persistence concerns in the Entity Framework Core layer:

```csharp
builder.Entity<Customer>(b =>
{
    b.ConfigureByConvention();

    b.ComplexProperty(
        customer => customer.Contact,
        contact => contact
            .ToJson()
            .HasColumnType("json"));

    b.HasIndex("Contact.Address.City");
});
```

The domain model does not need to know:

```text
Contact is JSON.
```

That is exactly where an ORM abstraction is useful.

The domain expresses:

```text
Customer has Contact.
```

The persistence layer decides:

```text
Contact is persisted as JSON.
```

For ABP applications, I would especially consider this model for value-object-like structures and aggregate snapshots.

I would be much more cautious about using it to hide entities, relationships, or extension points that should remain relational.

---

# When I Would Not Use JSON

I would avoid JSON when I find myself doing things like this:

```text
JSON column
├── CustomerId
├── ProductId
├── SupplierId
├── CategoryId
├── WarehouseId
└── ...
```

and then repeatedly querying and joining those values.

At that point JSON is probably fighting the database.

I would also avoid it when:

- The document is updated extremely frequently
- Individual nested values need independent concurrency
- Referential integrity is important
- Reporting requires heavy cross-document aggregation
- Most JSON paths require indexes
- The structure is highly relational
- Multiple providers must behave identically
- The team cannot easily inspect generated SQL and execution plans

A useful heuristic is:

> If most of the document needs independent relational behavior, it probably wants to be relational.

# Conclusion

JSON support in EF Core is becoming much more than "serialize an object into a column."

With complex types, EF understands the structure of the value.

With `ToJson()`, we can choose document-style persistence without giving up our strongly typed domain model.

With EF Core 11, that model becomes considerably more practical: TPT and TPC inheritance can participate, nested JSON paths can be indexed, `JSON_PATH_EXISTS` becomes accessible from LINQ, and SQL Server 2025 can translate suitable collection searches to `JSON_CONTAINS`.

But the most important architectural point is what has **not** changed.

JSON is not a replacement for relational modeling.

It is another persistence shape.

Use normalized tables when data has independent identity, relationships, constraints, and cross-aggregate query requirements.

Use JSON when a structured value naturally belongs to one aggregate and is usually read and written as part of that aggregate.

And once JSON becomes part of an important query path, treat it exactly as you would any other database design:

**inspect the SQL, design the indexes, inspect the execution plan, and measure it under realistic load.**

That is where EF Core 11's JSON improvements become genuinely useful.

Thanks for reading, see you in the next one!
