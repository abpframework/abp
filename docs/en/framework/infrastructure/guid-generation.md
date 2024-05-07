# GUID Generation

GUID is a common **primary key type** that is used in database management systems. ABP prefers GUID as the primary for pre-built [application modules](../../modules). Also, `ICurrentUser.Id` property ([see](./current-user.md)) is type of GUID, that means the ABP assumes that the User Id is always GUID.

## Why Prefer GUID?

GUID has advantages and disadvantages. You can find many articles on the web related to this topic, so we will not discuss all again, but will list the most fundamental advantages:

* It is **usable** in all database providers.
* It allows to **determine the primary key** on the client side, without needing a **database round trip** to generate the Id value. This can be more performant while inserting new records to the database and allows us to know the PK before interacting to the database.
* GUIDs are **naturally unique** which has some advantages in the following situations if;
  * You need to integrate to **external** systems.
  * You need to **split or merge** different tables.
  * You are creating **distributed systems**.
* GUIDs are impossible to guess, so they can be **more secure** compared to auto-increment Id values in some cases.

While there are some disadvantages (just search it on the web), we found these advantages much more important while designing the ABP.

## IGuidGenerator

The most important problem with GUID is that it is **not sequential by default**. When you use the GUID as the primary key and set it as the **clustered index** (which is default) for your table, it brings a significant **performance problem on insert** (because inserting new record may need to re-order the existing records).

So, **never use `Guid.NewGuid()` to create Ids** for your entities!

One good solution to this problem is to generate **sequential GUIDs**, which is provided by the ABP out of the box. `IGuidGenerator` service creates sequential GUIDs (implemented by the `SequentialGuidGenerator` by default). Use `IGuidGenerator.Create()` when you need to manually set Id of an [entity](../architecture/domain-driven-design/entities.md).

**Example: An entity with GUID primary key and creating the entity**

Assume that you've a `Product` [entity](../architecture/domain-driven-design/entities.md) that has a `Guid` key:

````csharp
using System;
using Volo.Abp.Domain.Entities;

namespace AbpDemo
{
    public class Product : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        private Product() { /* This constructor is used by the ORM/database provider */ }

        public Product(Guid id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}
````

And you want to create a new product:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace AbpDemo
{
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
}
````

This service injects the `IGuidGenerator` in the constructor. If your class is an [application service](../architecture/domain-driven-design/application-services.md) or deriving from one of the other base classes, you can directly use the `GuidGenerator` base property which is a pre-injected `IGuidGenerator` instance.

## Options

### AbpSequentialGuidGeneratorOptions

`AbpSequentialGuidGeneratorOptions` is the [option class](../fundamentals/options.md) that is used to configure the sequential GUID generation. It has a single property:

* `DefaultSequentialGuidType` (`enum` of type `SequentialGuidType`): The strategy used while generating GUID values.

Database providers behaves differently while processing GUIDs, so you should set it based on your database provider. `SequentialGuidType` has the following `enum` members:

* `SequentialAtEnd` (**default**) works well with the [SQL Server](../data/entity-framework-core).
* `SequentialAsString` is used by [MySQL](../data/entity-framework-core/mysql.md) and [PostgreSQL](../data/entity-framework-core/postgresql.md).
* `SequentialAsBinary` is used by [Oracle](../data/entity-framework-core/oracle.md).

Configure this option in the `ConfigureServices` method of your [module](../architecture/modularity/basics.md), as shown below:

````csharp
Configure<AbpSequentialGuidGeneratorOptions>(options =>
{
    options.DefaultSequentialGuidType = SequentialGuidType.SequentialAsBinary;
});
````

> EF Core [integration packages](../data/entity-framework-core/other-dbms.md) sets this option to a proper value for the related DBMS. So, most of the times, you don't need to set this option if you are using these integration packages.