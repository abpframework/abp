## Entity Framework Core 集成最佳实践

> 有关EF Core 集成的基础知识,请参阅[Entity Framework Core 集成文档](../Entity-Framework-Core.md).

- **推荐** 为每个模块定义单独的 `DbContext` 接口和类.
- **不推荐** 在应用程序开发中使用延迟加载.
- **不推荐** 为 `DbContext` 启用延迟加载.

### DbContext Interface

- **推荐** 继承自`IEfCoreDbContext` 的 `DbContext` 定义一个相应的 **interface**.
- **推荐** 添加 `ConnectionStringName` **attribute** 到 `DbContext` 接口.
- **推荐** 将 `DbSet<TEntity>`  **properties** 添加到 `DbContext` 接口中,注意: 仅适用于聚合根. 例如:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IIdentityDbContext : IEfCoreDbContext
{
    DbSet<IdentityUser> Users { get; set; }
    DbSet<IdentityRole> Roles { get; set; }
}
````

### DbContext class

* **推荐** `DbContext` 继承自 `AbpDbContext<TDbContext>` 类.
* **推荐** 添加 `ConnectionStringName` **attribute** 到 `DbContext` 类.
* **推荐** 实现 `DbContext` 类实现其相应的接口. 例如:

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

### 表前缀与架构

- **推荐** 添加静态 **properties** `TablePrefix` 与  `Schema` 到 `DbContext` 类. 使用常量为其设置一个默认值. 例如:

````C#
public static string TablePrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
public static string Schema { get; set; } = AbpIdentityConsts.DefaultDbSchema;
````

  - **推荐** 总是使用简短的 `TablePrefix` 值为模块在共享数据库中创建  **unique table names**. `Abp` 前缀是为ABP Core模块保留的.
  - **推荐** `Schema` 默认赋值为 `null`.

### Model Mapping

- **推荐** 重写 `DbContext` 的 `OnModelCreating` 方法显式 **配置所有实体**. 例如:

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

- **不推荐** 直接在 `OnModelCreating` 方法中配置model, 而是为 `ModelBuilder` 定义一个 **扩展方法**. 使用Configure*ModuleName*作为方法名称. 例如:

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
            b.ConfigureByConvention();
            //code omitted for brevity
        });

        builder.Entity<IdentityUserClaim>(b =>
        {
            b.ToTable(options.TablePrefix + "UserClaims", options.Schema);
            b.ConfigureByConvention();
            //code omitted for brevity
        });
        //code omitted for brevity
    }
}
````

* **推荐** 为每个Enttiy映射调用 `b.ConfigureByConvention();`(如上所示).
* **推荐** 通过继承 `AbpModelBuilderConfigurationOptions` 来创建 **configuration Options** 类. 例如:

````C#
public class IdentityModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
{
    public IdentityModelBuilderConfigurationOptions()
        : base(AbpIdentityConsts.DefaultDbTablePrefix, AbpIdentityConsts.DefaultDbSchema)
    {
    }
}
````

### 仓储实现

- **推荐** 从 `EfCoreRepository<TDbContext,TEntity,TKey>` 类 **继承** 仓储并实现相应的仓储接口. 例如:

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

* **推荐** 使用 `DbContext` 接口而不是类来作为泛型参数.
* **推荐** 使用 `GetCancellationToken` 帮助方法将 `cancellationToken` 传递给EF Core. 例如:

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

如果调用者代码中未提供取消令牌,则 `GetCancellationToken` 会从`ICancellationTokenProvider.Token` 获取取消令牌.

- **推荐** 为具有 **子集合** 的聚合根创建 `IQueryable<TEntity>` 返回类型的 `IncludeDetails` **扩展方法**. 例如:

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

* **推荐** 推荐在仓储其他方法中使用 `IncludeDetails` 扩展方法, 就像上面的示例代码一样(参阅 FindByNormalizedUserNameAsync).

- **推荐** 覆盖具有 **子集合** 的聚合根仓储中的 `WithDetails` 方法. 例如:

````C#
public override IQueryable<IdentityUser> WithDetails()
{
    return GetQueryable().IncludeDetails(); // Uses the extension method defined above
}
````

### Module Class

- **推荐** 为Entity Framework Core集成包定义一个Module类.
- **推荐** 使用 `AddAbpDbContext<TDbContext>` 方法将 `DbContext` 添加到 `IServiceCollection`.
- **推荐** 将已实现的仓储添加到 `AddAbpDbContext<TDbContext>` 方法的options中. 例如:

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
