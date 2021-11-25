## Entity Framework Core Integration Best Practices

> See [Entity Framework Core Integration document](../Entity-Framework-Core.md) for the basics of the EF Core integration.

- **Do** define a separated `DbContext` interface and class for each module.
- **Do not** rely on lazy loading on the application development.
- **Do not** enable lazy loading for the `DbContext`.

### DbContext Interface

- **Do** define an **interface** for the `DbContext` that inherits from `IEfCoreDbContext`.
- **Do** add a `ConnectionStringName` **attribute** to the `DbContext` interface.
- **Do** add `DbSet<TEntity>` **properties** to the `DbContext` interface for only aggregate roots. Example:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IIdentityDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; }
    DbSet<IdentityRole> Roles { get; }
}
````

* **Do not** define `set;` for the properties in this interface.

### DbContext class

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

### Table Prefix and Schema

- **Do** add static `TablePrefix` and `Schema` **properties** to the `DbContext` class. Set default value from a constant. Example:

````C#
public static string TablePrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
public static string Schema { get; set; } = AbpIdentityConsts.DefaultDbSchema;
````

  - **Do** always use a short `TablePrefix` value for a module to create **unique table names** in a shared database. `Abp` table prefix is reserved for ABP core modules.
  - **Do** set `Schema` to `null` as default.

### Model Mapping

- **Do** explicitly **configure all entities** by overriding the `OnModelCreating` method of the `DbContext`. Example:

````C#
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);
    builder.ConfigureIdentity();
}
````

- **Do not** configure model directly in the  `OnModelCreating` method. Instead, create an **extension method** for `ModelBuilder`. Use Configure*ModuleName* as the method name. Example:

````C#
public static class IdentityDbContextModelBuilderExtensions
{
    public static void ConfigureIdentity([NotNull] this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<IdentityUser>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users", AbpIdentityDbProperties.DbSchema);
            b.ConfigureByConvention();
            //code omitted for brevity
        });

        builder.Entity<IdentityUserClaim>(b =>
        {
            b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "UserClaims", AbpIdentityDbProperties.DbSchema);
            b.ConfigureByConvention();
            //code omitted for brevity
        });
        
        //code omitted for brevity
    }
}
````

* **Do** call `b.ConfigureByConvention();` for each entity mapping (as shown above).

### Repository Implementation

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
    return await (await GetDbSetAsync())
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

* **Do** use the `IncludeDetails` extension method in the repository methods just like used in the example code above (see `FindByNormalizedUserNameAsync`).

- **Do** override `WithDetails` method of the repository for aggregates root which have **sub collections**. Example:

````C#
public override async Task<IQueryable<IdentityUser>> WithDetailsAsync()
{
    // Uses the extension method defined above
    return (await GetQueryableAsync()).IncludeDetails();
}
````

### Module Class

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
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityDbContext>(options =>
        {
            options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
            options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
        });
    }
}
````
