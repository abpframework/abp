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
- **Do** add `DbSet<T>` **properties** to the `DbContext` interface for only aggregate roots. Example:

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
* **Do** implement the repository `interface` for the `DbContext` class. Example:

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

- **Do** add static `TablePrefix` and `Schema` properties to the `DbContext` class. Set default value from an constant. Example:

````C#
public static string TablePrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;

public static string Schema { get; set; } = AbpIdentityConsts.DefaultDbSchema;
````

  - **Do** always use a short `TablePrefix` value for a module to create unique table names in a shared database. `Abp` table prefix is reserved for ABP core modules.
  - **Do** set `Schema` to `null` as default.

###### Model Mapping

- **Do** explicitly configure all entities by overriding the `OnModelCreating` method of the `DbContext`. Example:

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

- **Do not** configure model directly in the  `OnModelCreating` method. Instead, create an extension method for `ModelBuilder`. Use Configure*ModuleName* as the method name. Example:

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

* Do create a configuration options class by inheriting from the `ModelBuilderConfigurationOptions`. Example:

````C#
public class IdentityModelBuilderConfigurationOptions : ModelBuilderConfigurationOptions
{
    public IdentityModelBuilderConfigurationOptions()
        : base(AbpIdentityConsts.DefaultDbTablePrefix, AbpIdentityConsts.DefaultDbSchema)
    {

    }
}
````

###### Module Class

Define

###### Repository Implementation

- ​

##### MongoDB