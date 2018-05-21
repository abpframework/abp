## MongoDB Integration

This document explains how to integrate MongoDB as a database provider to ABP based applications and how to configure it.

### Installation

`Volo.Abp.MongoDB` is the main nuget package for the MongoDB integration. Install it to your project (for a layered application, to your data/infrastructure layer):

```
Install-Package Volo.Abp.MongoDB
```

Then add `AbpMongoDbModule` module dependency to your [module](Module-Development-Basics.md):

```c#
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

### Creating a Mongo Db Context

ABP introduces **Mongo Db Context** concept (which is similar to Entity Framework Core's DbContext) to make it easier to use collections and configure them. An example is shown below:

```c#
public class MyDbContext : AbpMongoDbContext
{
    public IMongoCollection<Question> Questions => Collection<Question>();

    public IMongoCollection<Category> Categories => Collection<Category>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Question>(b =>
        {
            b.CollectionName = "Questions";
        });
    }
}
```

* It's derived from `AbpMongoDbContext` class.
* Adds a public `IMongoCollection<TEntity>` property for each mongo collection. ABP uses these properties to create default repositories by default.
* Overriding `CreateModel` method allows to configure collections (like their collection name in the database).

### Registering DbContext To Dependency Injection

Use `AddAbpDbContext` method in your module to register your DbContext class for [dependency injection](Dependency-Injection.md) system.

```c#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDbContext<MyDbContext>();

            //...
        }
    }
}
```

#### Add Default Repositories

ABP can automatically create [repositories](Repositories.md) for the entities in your Db Context. Just use `AddDefaultRepositories()` option on registration:

````C#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories();
});
````

This will create a repository for each aggreate root entity (classes derived from AggregateRoot) by default. If you want to create repositories for other entities too, then set `includeAllEntities` to `true`:

```c#
services.AddMongoDbContext<MyDbContext>(options =>
{
    options.AddDefaultRepositories(includeAllEntities: true);
});
```

Then you can inject and use `IRepository<TEntity>` or `IQueryableRepository<TEntity>` in your services.

