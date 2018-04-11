## Module Development

### Introduction

This document describes the best practices for who want to develop modules that satisfies the following specifications:

* Develop the module that applies the **Domain Driven Design** patterns & best practices.
* Develop the module as DBMS and **ORM independent**.
* Develop the module that can be used as a remote service / **microservice** as well as can be integrated to a **monolithic** application.

This guide is also usable for application development best practices, mostly.

### Data Access Layer

The module should be completely independent of any DBMS and and ORM.

- **Do not** use `IQueryable<TEntity>` features in the application code (domain, application... layers) except the data access layer.
- **Do** always use the specifically created repository interface (like `IIdentityUserRepository`) from the application code (as developed specified below). **Do not** use generic repository interfaces (like `IRepository<IdentityUser, Guid>`).

#### Repositories

* **Do** define a repository interface (and create its corresponding implementations) for each aggregate root.

For the example aggregate root:

````C#
public class IdentityUser : AggregateRoot<Guid>
{
    //...
}
````

Define the repository interface as below:

````C#
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    //...
}
````

* **Do** define repository interfaces in the **domain layer**.
* **Do not** inherit the repository interface from `IRepository` interfaces. Because it inherits `IQueryable` and the repository should not expose `IQueryable` to the application.
* **Do** inherit the repository interface from `IBasicRepository<TEntity, TKey>` (as normally) or a lower-featured interface, like `IReadOnlyRepository<TEntity, TKey>` (if it's needed).
* **Do not** define repositories for entities those are **not aggregate roots**.
* **Do** define all repository methods as **asynchronous**.
* **Do** add an **optional** `cancellationToken` parameter to every method of the repository. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    CancellationToken cancellationToken = default
);
````

* **Do** create a **synchronous extension** method for each asynchronous repository method. Example:

````C#
public static class IdentityUserRepositoryExtensions
{
    public static IdentityUser FindByNormalizedUserName(
        this IIdentityUserRepository repository,
        [NotNull] string normalizedUserName)
    {
        return AsyncHelper.RunSync(
            () => repository.FindByNormalizedUserNameAsync(normalizedUserName)
        );
    }
}
````

This will allow synchronous code to use the repository methods easier.

* **Do** add an optional `bool includeDetails = true` parameter (default value is `true`) for every repository method which returns a **single entity**. Example:

````C#
Task<IdentityUser> FindByNormalizedUserNameAsync(
    [NotNull] string normalizedUserName,
    bool includeDetails = true,
    CancellationToken cancellationToken = default
);
````

This parameter will be implemented for ORMs to eager load sub collections of the entity.

* **Do** add an optional `bool includeDetails = false` parameter (default value is `false`) for every repository method which returns a **list of entities**. Example:

````C#
Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
    string normalizedRoleName, 
    bool includeDetails = false,
    CancellationToken cancellationToken = default
);
````

* **Do not** create composite classes to combine entities to get from repository with a single method call. Examples: *UserWithRoles*, *UserWithTokens*, *UserWithRolesAndTokens*. Instead, properly use `includeDetails` option to add all details of the entity when needed.
* **Avoid** to create projection classes for entities to get less property of an entity from the repository. Example: Avoid to create BasicUserView class to select a few properties needed for the use case needs. Instead, directly use the aggregate root class. However, there may be some exceptions for this rule, where:
  * Performance is so critical for the use case and getting the whole aggregate root highly impacts the performance.

#### Integrations

* **Do** create separated package/module for each ORM integration (like *Company.Module.EntityFrameworkCore* and *Company.Module.MongoDB*).

##### Entity Framework Core

- Do define a separated `DbContext` interface and class for each module.

###### DbContext Interface

- **Do** define an **interface** for the `DbContext` that inherits from `IEfCoreDbContext`.
- **Do** add a `ConnectionStringName` **attribute** to the `DbContext` interface.
- **Do** add `DbSet<TEntity>` **properties** to the `DbContext` interface for only aggregate roots. Example:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IIdentityDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; set; }
    DbSet<IdentityRole> Roles { get; set; }
}
````

###### DbContext class

* **Do** inherit the `DbContext` from the `AbpDbContext<TDbContext>` class.
* **Do** add a `ConnectionStringName` attribute to the `DbContext` class.
* **Do** implement the corresponding `interface` for the `DbContext` class. Example:

````C#
[ConnectionStringName("AbpIdentity")]
public class IdentityDbContext : AbpDbContext<IdentityDbContext>, IIdentityDbContext
{
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {

    }

    //code omitted for brevity
}
````

###### Table Prefix and Schema

- **Do** add static `TablePrefix` and `Schema` **properties** to the `DbContext` class. Set default value from a constant. Example:

````C#
public static string TablePrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
public static string Schema { get; set; } = AbpIdentityConsts.DefaultDbSchema;
````

  - **Do** always use a short `TablePrefix` value for a module to create **unique table names** in a shared database. `Abp` table prefix is reserved for ABP core modules.
  - **Do** set `Schema` to `null` as default.

###### Model Mapping

- **Do** explicitly **configure all entities** by overriding the `OnModelCreating` method of the `DbContext`. Example:

````C#
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.ConfigureIdentity(options =>
    {
        options.TablePrefix = TablePrefix;
        options.Schema = Schema;
    });
}
````

- **Do not** configure model directly in the  `OnModelCreating` method. Instead, create an **extension method** for `ModelBuilder`. Use Configure*ModuleName* as the method name. Example:

````C#
public static class IdentityDbContextModelBuilderExtensions
{
    public static void ConfigureIdentity(
        [NotNull] this ModelBuilder builder,
        Action<IdentityModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new IdentityModelBuilderConfigurationOptions();
        optionsAction?.Invoke(options);

        builder.Entity<IdentityUser>(b =>
        {
            b.ToTable(options.TablePrefix + "Users", options.Schema);            
            //code omitted for brevity
        });

        builder.Entity<IdentityUserClaim>(b =>
        {
            b.ToTable(options.TablePrefix + "UserClaims", options.Schema);
            //code omitted for brevity
        });
        
        //code omitted for brevity
    }
}
````

* **Do** create a **configuration options** class by inheriting from the `ModelBuilderConfigurationOptions`. Example:

````C#
public class IdentityModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
{
    public IdentityModelBuilderConfigurationOptions()
        : base(AbpIdentityConsts.DefaultDbTablePrefix, AbpIdentityConsts.DefaultDbSchema)
    {
    }
}
````

###### Repository Implementation

- **Do** **inherit** the repository from the `EfCoreRepository<TDbContext, TEntity, TKey>` class and implement the corresponding repository interface. Example:

````C#
public class EfCoreIdentityUserRepository
    : EfCoreRepository<IIdentityDbContext, IdentityUser, Guid>, IIdentityUserRepository
{
    public EfCoreIdentityUserRepository(
        IDbContextProvider<IIdentityDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
}
````

* **Do** use the `DbContext` interface as the generic parameter, not the class.
* **Do** pass the `cancellationToken` to EF Core using the `GetCancellationToken` helper method. Example:

````C#
public virtual async Task<IdentityUser> FindByNormalizedUserNameAsync(
    string normalizedUserName, 
    bool includeDetails = true,
    CancellationToken cancellationToken = default)
{
    return await DbSet
        .IncludeDetails(includeDetails)
        .FirstOrDefaultAsync(
            u => u.NormalizedUserName == normalizedUserName,
            GetCancellationToken(cancellationToken)
        );
}
````

`GetCancellationToken` fallbacks to the `ICancellationTokenProvider.Token` to obtain the cancellation token if it is not provided by the caller code.

- **Do** create a `IncludeDetails` **extension method** for the `IQueryable<TEntity>` for each aggregate root which has **sub collections**. Example:

````C#
public static IQueryable<IdentityUser> IncludeDetails(
    this IQueryable<IdentityUser> queryable,
    bool include = true)
{
    if (!include)
    {
        return queryable;
    }

    return queryable
        .Include(x => x.Roles)
        .Include(x => x.Logins)
        .Include(x => x.Claims)
        .Include(x => x.Tokens);
}
````

* **Do** use the `IncludeDetails` extension method in the repository methods just like used in the example code above (see FindByNormalizedUserNameAsync).

- **Do** override `IncludeDetails` method of the repository for aggregates root which have **sub collections**. Example:

````C#
protected override IQueryable<IdentityUser> IncludeDetails(IQueryable<IdentityUser> queryable)
{
    return queryable.IncludeDetails(); //uses the extension method defined above
}
````

###### Module Class

- **Do** define a module class for the Entity Framework Core integration package.
- **Do** add `DbContext` to the `IServiceCollection` using the `AddAbpDbContext<TDbContext>` method.
- **Do** add implemented repositories to the options for the `AddAbpDbContext<TDbContext>` method. Example:

````C#
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
    )]
public class AbpIdentityEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddAbpDbContext<IdentityDbContext>(options =>
        {
            options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
            options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
        });

        services.AddAssemblyOf<AbpIdentityEntityFrameworkCoreModule>();
    }
}
````

##### MongoDB

* Do define a separated `DbContext` interface and class for each module.

###### MongoDbContext Interface

- **Do** define an **interface** for the `MongoDbContext` that inherits from `IAbpMongoDbContext`.
- **Do** add a `ConnectionStringName` **attribute** to the `MongoDbContext` interface.
- **Do** add `IMongoCollection<TEntity>` **properties** to the `MongoDbContext` interface for only aggregate roots. Example:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IAbpIdentityMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<IdentityUser> Users { get; }
    IMongoCollection<IdentityRole> Roles { get; }
}
````

###### MongoDbContext class

- **Do** inherit the `MongoDbContext` from the `AbpMongoDbContext` class.
- **Do** add a `ConnectionStringName` attribute to the `MongoDbContext` class.
- **Do** implement the corresponding `interface` for the `MongoDbContext` class. Example:

```c#
[ConnectionStringName("AbpIdentity")]
public class AbpIdentityMongoDbContext : AbpMongoDbContext, IAbpIdentityMongoDbContext
{
    public IMongoCollection<IdentityUser> Users => Collection<IdentityUser>();
    public IMongoCollection<IdentityRole> Roles => Collection<IdentityRole>();

    //code omitted for brevity
}
```

###### Collection Prefix

- **Do** add static `CollectionPrefix` **property** to the `DbContext` class. Set default value from a constant. Example:

```c#
public static string CollectionPrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
```

Used the same constant defined for the EF Core integration table prefix in this example.

- **Do** always use a short `CollectionPrefix` value for a module to create **unique collection names** in a shared database. `Abp` collection prefix is reserved for ABP core modules.

###### Collection Mapping

- **Do** explicitly **configure all entities** by overriding the `CreateModel` method of the `MongoDbContext`. Example:

```c#
protected override void CreateModel(IMongoModelBuilder modelBuilder)
{
    base.CreateModel(modelBuilder);

    modelBuilder.ConfigureIdentity(options =>
    {
        options.CollectionPrefix = CollectionPrefix;
    });
}
```

- **Do not** configure model directly in the  `CreateModel` method. Instead, create an **extension method** for `IMongoModelBuilder`. Use Configure*ModuleName* as the method name. Example:

```c#
public static class AbpIdentityMongoDbContextExtensions
{
    public static void ConfigureIdentity(
        this IMongoModelBuilder builder,
        Action<IdentityMongoModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        var options = new IdentityMongoModelBuilderConfigurationOptions();

        optionsAction?.Invoke(options);

        builder.Entity<IdentityUser>(b =>
        {
            b.CollectionName = options.CollectionPrefix + "Users";
        });

        builder.Entity<IdentityRole>(b =>
        {
            b.CollectionName = options.CollectionPrefix + "Roles";
        });
    }
}
```

- **Do** create a **configuration options** class by inheriting from the `MongoModelBuilderConfigurationOptions`. Example:

```c#
public class IdentityMongoModelBuilderConfigurationOptions
    : MongoModelBuilderConfigurationOptions
{
    public IdentityMongoModelBuilderConfigurationOptions()
        : base(AbpIdentityConsts.DefaultDbTablePrefix)
    {
    }
}
```

* **Do** explicitly configure `BsonClassMap` for all entities. Create a static method for this purpose. Example:

````C#
public static class AbpIdentityBsonClassMap
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            BsonClassMap.RegisterClassMap<IdentityUser>(map =>
            {
                map.AutoMap();
                map.ConfigureExtraProperties();
            });

            BsonClassMap.RegisterClassMap<IdentityRole>(map =>
            {
                map.AutoMap();
            });
        });
    }
}
````

`BsonClassMap` works with static methods. So, it is only needed to configure entities once in an application. `OneTimeRunner` guarantees it in a thread safe manner. Such a mapping above ensures that unit test properly run. This code will be called by the **module class** below.

###### Repository Implementation

- **Do** **inherit** the repository from the `MongoDbRepository<TMongoDbContext, TEntity, TKey>` class and implement the corresponding repository interface. Example:

```c#
public class MongoIdentityUserRepository
    : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityUser, Guid>,
      IIdentityUserRepository
{
    public MongoIdentityUserRepository(
        IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
```

- **Do** pass the `cancellationToken` to the MongoDB Driver using the `GetCancellationToken` helper method. Example:

```c#
public async Task<IdentityUser> FindByNormalizedUserNameAsync(
    string normalizedUserName, 
    bool includeDetails = true,
    CancellationToken cancellationToken = default)
{
    return await GetMongoQueryable()
        .FirstOrDefaultAsync(
            u => u.NormalizedUserName == normalizedUserName,
            GetCancellationToken(cancellationToken)
        );
}
```

`GetCancellationToken` fallbacks to the `ICancellationTokenProvider.Token` to obtain the cancellation token if it is not provided by the caller code.

* **Do** ignore the `includeDetails` parameters for the repository implementation since MongoDB loads the aggregate root as a whole (including sub collections) by default.
* **Do** use the `GetMongoQueryable()` method to obtain an `IQueryable<TEntity>` to perform queries  wherever possible. Because;
  *  `GetMongoQueryable()` method automatically uses the `ApplyDataFilters` method to filter the data based on the current data filters (like soft delete and multi-tenancy).
  * Using `IQueryable<TEntity>` makes the code as much as similar to the EF Core repository implementation and easy to write and read.
* **Do** implement data filtering if the `GetMongoQueryable()` method is not possible to use.

###### Module Class

- **Do** define a module class for the MongoDB integration package.
- **Do** add `MongoDbContext` to the `IServiceCollection` using the `AddMongoDbContext<TMongoDbContext>` method.
- **Do** add implemented repositories to the options for the `AddMongoDbContext<TMongoDbContext>` method. Example:

```c#
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpUsersMongoDbModule)
    )]
public class AbpIdentityMongoDbModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        AbpIdentityBsonClassMap.Configure();

        services.AddMongoDbContext<AbpIdentityMongoDbContext>(options =>
        {
            options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
            options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
        });

        services.AddAssemblyOf<AbpIdentityMongoDbModule>();
    }
}
```

Notice that this module class also calls the static `BsonClassMap` configuration method defined above.