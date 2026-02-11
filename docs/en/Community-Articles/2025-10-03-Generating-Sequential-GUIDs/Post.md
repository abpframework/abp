# You May Have Trouble with GUIDs: Generating Sequential GUIDs in .NET


If you’ve ever shoved a bunch of `Guid.NewGuid()` values into a SQL Server table with a clustered index on the PK, you’ve probably felt the pain: **Index fragmentation so bad you could use it as modern art.** Inserts slow down, page splits go wild, and your DBA starts sending you passive-aggressive Slack messages.

And yet… we keep doing it. Why? Because GUIDs are _easy_. They’re globally unique, they don’t need a round trip to the DB, and they make distributed systems happy. But here’s the catch: **random GUIDs are absolute chaos for ordered indexes**.

## The Problem with Vanilla GUIDs

* **Randomness kills order** — clustered indexes thrive on sequential inserts; random GUIDs force constant reordering.

* **Performance hit** — every insert can trigger page splits and index reshuffling.

* **Storage bloat** — fragmentation means wasted space and slower reads.

Sure, you could switch to int or long identity columns, but then you lose the distributed generation magic and security benefits (predictable IDs are guessable).

## Sequential GUIDs to the Rescue

Sequential GUIDs keep the uniqueness but add a predictable ordering component, usually by embedding a timestamp in part of the GUID. This means:

* Inserts happen at the “end” of the index, not all over the place.

* Fragmentation drops dramatically.

* You still get globally unique IDs without DB trips.

Think of it as **GUIDs with manners**.

## ABP Framework’s Secret Sauce


Here’s where ABP Framework flexes: it **uses sequential GUIDs by default** for entity IDs. No ceremony, no “remember to call this helper method”, it’s baked in.

Under the hood:

* ABP ships with IGuidGenerator (default: SequentialGuidGenerator).

* It picks the right sequential strategy for your DB provider:
  
  * **SequentialAtEnd** → SQL Server
  
  * **SequentialAsString** → MySQL/PostgreSQL
  
  * **SequentialAsBinary** → Oracle

* EF Core integration packages auto-configure this, so you rarely need to touch it.

Example in ABP:

```csharp
public class MyProductService : ITransientDependency
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IGuidGenerator _guidGenerator;


    public MyProductService(
        IRepository<Product, Guid> productRepository,
        IGuidGenerator guidGenerator)
    {
        _productRepository = productRepository;
        _guidGenerator = guidGenerator;
    }


    public async Task CreateAsync(string productName)
    {
        var product = new Product(_guidGenerator.Create(), productName);
        await _productRepository.InsertAsync(product);
    }
}
```

No `Guid.NewGuid()` here, `_guidGenerator.Create()` gives you a sequential GUID every time.

## Benefits of Sequential GUIDs

Let’s say you’re inserting 1M rows into a table with a clustered primary key:

* **Random GUIDs** → fragmentation ~99%, insert throughput tanks.

* **Sequential GUIDs** → fragmentation stays low, inserts fly.

In high-volume systems, this difference is **not** academic, it’s the difference between smooth scaling and spending weekends rebuilding indexes.

## When to Use Sequential GUIDs

* **Distributed systems** that still want DB-friendly inserts.

* **High-write workloads** with clustered indexes on GUID PKs.

* **Multi-tenant apps** where IDs need to be unique across tenants.

## When Random GUIDs Still Make Sense

* Security through obscurity, if you don’t want IDs to hint at creation order.

* Non-indexed identifiers, fragmentation isn’t a concern.

## The Final Take

ABP’s default sequential GUID generation is one of those “**small but huge**” features. It’s the kind of thing you don’t notice until you benchmark, and then you wonder why you ever lived without it.

## Links
You may want to check the following references to learn more about sequential GUIDs:

- [ABP Framework Documentation: Sequential GUIDs](https://docs.abp.io/en/abp/latest/Guid-Generation)