## MongoDB Integration

* Do define a separated `MongoDbContext` interface and class for each module.

### MongoDbContext Interface

- **Do** define an **interface** for the `MongoDbContext` that inherits from `IAbpMongoDbContext`.
- **Do** add a `ConnectionStringName` **attribute** to the `MongoDbContext` interface.
- **Do** add `IMongoCollection<TEntity>` **properties** to the `MongoDbContext` interface only for the aggregate roots. Example:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IAbpIdentityMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<IdentityUser> Users { get; }
    IMongoCollection<IdentityRole> Roles { get; }
}
````

### MongoDbContext class

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

### Collection Prefix

- **Do** add static `CollectionPrefix` **property** to the `DbContext` class. Set default value from a constant. Example:

```c#
public static string CollectionPrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
```

Used the same constant defined for the EF Core integration table prefix in this example.

- **Do** always use a short `CollectionPrefix` value for a module to create **unique collection names** in a shared database. `Abp` collection prefix is reserved for ABP core modules.

### Collection Mapping

- **Do** explicitly **configure all aggregate roots** by overriding the `CreateModel` method of the `MongoDbContext`. Example:

```c#
protected override void CreateModel(IMongoModelBuilder modelBuilder)
{
    base.CreateModel(modelBuilder);

    modelBuilder.ConfigureIdentity();
}
```

- **Do not** configure model directly in the  `CreateModel` method. Instead, create an **extension method** for the `IMongoModelBuilder`. Use Configure*ModuleName* as the method name. Example:

```c#
public static class AbpIdentityMongoDbContextExtensions
{
    public static void ConfigureIdentity(
        this IMongoModelBuilder builder,
        Action<IdentityMongoModelBuilderConfigurationOptions> optionsAction = null)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUser>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "Users";
        });

        builder.Entity<IdentityRole>(b =>
        {
            b.CollectionName = AbpIdentityDbProperties.DbTablePrefix + "Roles";
        });
    }
}
```

### Repository Implementation

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
    return await (await GetMongoQueryableAsync())
        .FirstOrDefaultAsync(
            u => u.NormalizedUserName == normalizedUserName,
            GetCancellationToken(cancellationToken)
        );
}
```

`GetCancellationToken` fallbacks to the `ICancellationTokenProvider.Token` to obtain the cancellation token if it is not provided by the caller code.

* **Do** ignore the `includeDetails` parameters for the repository implementation since MongoDB loads the aggregate root as a whole (including sub collections) by default.
* **Do** use the `GetMongoQueryableAsync()` method to obtain an `IQueryable<TEntity>` to perform queries  wherever possible. Because;
  *  `GetMongoQueryableAsync()` method automatically uses the `ApplyDataFilters` method to filter the data based on the current data filters (like soft delete and multi-tenancy).
  * Using `IQueryable<TEntity>` makes the code as much as similar to the EF Core repository implementation and easy to write and read.
* **Do** implement data filtering if it is not possible to use the `GetMongoQueryable()` method.

### Module Class

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
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<AbpIdentityMongoDbContext>(options =>
        {
            options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
            options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
        });
    }
}
```

Notice that this module class also calls the static `BsonClassMap` configuration method defined above.