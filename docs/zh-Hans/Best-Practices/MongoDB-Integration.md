## MongoDB 集成

* **推荐** 为每个模块定义一个独立的 `MongoDbContext` 接口与实现类.

### MongoDbContext 接口

- **推荐** 定义 `MongoDbContext` **接口** 时继承自 `IAbpMongoDbContext`.
- **推荐** 添加 `ConnectionStringName` **attribute** 到 `MongoDbContext` 接口.
- **推荐** 只把聚合根做为 `IMongoCollection<TEntity>` **properties** 添加到 `MongoDbContext` 接口. 示例:

````C#
[ConnectionStringName("AbpIdentity")]
public interface IAbpIdentityMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<IdentityUser> Users { get; }
    IMongoCollection<IdentityRole> Roles { get; }
}
````

### MongoDbContext 类

- **推荐**  `MongoDbContext` 继承自 `AbpMongoDbContext` 类.
- **推荐** 添加 `ConnectionStringName` **attribute** 到 `MongoDbContext` 类.
- **推荐** `MongoDbContext` 类实现相对应的**接口**.  示例:

```c#
[ConnectionStringName("AbpIdentity")]
public class AbpIdentityMongoDbContext : AbpMongoDbContext, IAbpIdentityMongoDbContext
{
    public IMongoCollection<IdentityUser> Users => Collection<IdentityUser>();
    public IMongoCollection<IdentityRole> Roles => Collection<IdentityRole>();

    //code omitted for brevity
}
```

### Collection 前缀

- **推荐** 添加静态 `CollectionPrefix` **property** 到 `DbContext` 类中并使用常量为其设置默认值. 示例:

```c#
public static string CollectionPrefix { get; set; } = AbpIdentityConsts.DefaultDbTablePrefix;
```

在此示例中使用与EF Core集成表前缀相同的常量.

- **推荐** 总是使用简短的 `CollectionPrefix` 值为模块在共享数据库中创建  **unique collection names**. `Abp` collection前缀是为ABP Core模块保留的.

### Collection 映射

- **推荐** 通过重写 `MongoDbContext` 的 `CreateModel` 方法  **配置所有的聚合根** . 示例:

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

- **不推荐** 直接在 `CreateModel` 方法中配置model,而是为 `IMongoModelBuilder` 定义一个 **扩展方法**. 使用Configure*ModuleName*作为方法名称. 示例:

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

- **推荐** 通过继承 `AbpMongoModelBuilderConfigurationOptions` 来创建 **configuration Options** 类. 示例:

```c#
public class IdentityMongoModelBuilderConfigurationOptions
    : AbpMongoModelBuilderConfigurationOptions
{
    public IdentityMongoModelBuilderConfigurationOptions()
        : base(AbpIdentityConsts.DefaultDbTablePrefix)
    {
    }
}
```

* **推荐** 创建一个静态方法, 显示地为所有的实体配置 `BsonClassMap`. 示例:

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

`BsonClassMap` 适用于静态方法. 所以只需要在应用程序配置一次实体. `OneTimeRunner` 以线程安全的方式运行, 并且在应用程序生命周期中只运行一次. 上面代码中的映射确保单元测试可以正确运行. 此代码将由下面的**模块类**调用.

### 仓储实现

- **推荐** 仓储 **继承自** `MongoDbRepository<TMongoDbContext, TEntity, TKey>` 类并且实现其相应的接口. 示例:

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

- **推荐** 使用 `GetCancellationToken` 帮助方法将 `cancellationToken` 传递给MongoDB驱动程序. 示例:

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

如果调用者代码中未提供取消令牌, 则 `GetCancellationToken` 会从`ICancellationTokenProvider.Token` 获取取消令牌
`GetCancellationToken`.

* **推荐** 忽略仓储实现中的 `includeDetails` 参数, 因为MongoDB在默认情况下将聚合根作为一个整体(包括子集合)加载.
* **推荐** 使用 `GetMongoQueryable()` 方法获取 `IQueryable<TEntity>` 以尽可能执行查询use the `GetMongoQueryable()` method to obtain an `IQueryable<TEntity>` to perform queries  wherever possible. 因为;
  *  `GetMongoQueryable()` 方法在内部使用 `ApplyDataFilters` 方法根据当前的过滤器 (如 软删除与多租户)过滤数据.
  * 使用`IQueryable<TEntity>`让代码与EF Core仓储实现类似, 易于使用.
* **推荐** 如果无法使用 `GetMongoQueryable()` 方法, 则应自行实现数据过滤.

### 模块类

- **推荐** 为MongoDB集成包定义一个模块类.
- **推荐** 使用 `AddMongoDbContext<TMongoDbContext>` 方法将 `MongoDbContext` 添加到 `IServiceCollection`.
- **推荐** 将已实现的仓储添加到 `AddMongoDbContext<TMongoDbContext>` 方法options中. 示例:

```c#
[DependsOn(
    typeof(AbpIdentityDomainModule),
    typeof(AbpUsersMongoDbModule)
    )]
public class AbpIdentityMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        AbpIdentityBsonClassMap.Configure();

        context.Services.AddMongoDbContext<AbpIdentityMongoDbContext>(options =>
        {
            options.AddRepository<IdentityUser, MongoIdentityUserRepository>();
            options.AddRepository<IdentityRole, MongoIdentityRoleRepository>();
        });
    }
}
```

需要注意的是, 模块类还调用上面定义的静态 `BsonClassMap` 配置方法.