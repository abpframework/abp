# Performing Case-Insensitive Search in ABP Based-PostgreSQL Application: Using `citext` and Collation

PostgreSQL, by default, is a case-sensitive database. This means that text data stored in the database is treated as case-sensitive. However, in many cases, users may need to perform searches regardless of case sensitivity. Since PostgreSQL is case-sensitive this creates some questions in the mind, when selecting and using it within an ABP-based application. For example:

* Does not ABP Framework support case-sensitive queries for the PostgreSQL database?
* ABP Framework supports PostgreSQL but does not support case-insensitive search for it?
* [But PostgreSQL still a challenge to work with  Accent and Case Insensitive Searches with ABP](https://twitter.com/iSephit/status/1780568810291913029)
* ...

None of these questions are related to ABP Framework but PostgreSQL is being case-sensitive database by default and in this article, I will try to answer to these questions and I will address two possible solutions to perform case-insensitive operations: **Using the `citext` data type for text fields** and **using collations**. 

> As you would know, ABP Framework provides a [EF Core PostgreSQL Provider package](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-PostgreSQL) called `Volo.Abp.EntityFrameworkCore.PostgreSql`. Throughout this article, I will give the example codes, by assuming that you created an ABP-based application with PostgreSQL as the database option, however all the steps in this article, are also applicable to any .NET-based application.

## Using the `citext` Data Type

`citext` is a PostgreSQL-specific data type that stands for "case-insensitive text". This data type stores text data while ignoring case differences, effectively making searches case-insensitive. When you create an ABP-based application with PostgreSQL as the database option, you can easily use the `citext` data type.

To use the `citext` data type, you mainly need to do two things:

1. The `citext` data type is available in a PostgreSQL-bundled extension, so you'll first have to install it. For that purpose, you should use the [`HasPostgresExtension`](https://www.npgsql.org/efcore/api/Microsoft.EntityFrameworkCore.NpgsqlModelBuilderExtensions.html) method,
2. Then, you should map all of your text fields to the `citext` datatype in your `*DbContext.cs` file as follows:

```csharp
[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class MyProjectNameDbContext :
    AbpDbContext<MyProjectNameDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    //...

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //db configurations...

        //👇 install the citext datatype 👇
        builder.HasPostgresExtension("citext");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // 👇 configure all of the string property types as 'citext' data type 👇
        configurationBuilder.Properties<string>().HaveColumnType("citext");
    }    
}
```

In addition to that, you should configure the `AbpDbContextOptions` in the module class of the `*.EntityFrameworkCore` project to also apply this change in the dependent ABP modules (also for your own modules) as follows:

```csharp
    public class MyProjectEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //other configurations...

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();

                // 👇 configure all of the string property types as 'citext' data type for all of the dependent modules 👇
                options.ConfigureDefaultConvention((_, builder) =>
                {
                    builder.Properties<string>().HaveColumnType("citext");
                });
            });

        }

        //...
    }
```
 
After you make these changes, you can create a new migration and apply it to your database. When you do that, all of the types of text-based fields will be changed as `citext` data type. Then, you can write case-insensitive queries in your application without worry.

## Using Collations

**Collation** is a set of rules that determine how text data is sorted and compared in a dataset. PostgreSQL provides different collation settings for various languages and cultures. These settings can determine how text data is compared and can be configured to ignore case differences.

To perform case-insensitive or accent-insensitive operations, you should choose one of the [non-deterministic collations](https://postgresql.verite.pro/blog/2019/10/14/nondeterministic-collations.html). For example, you can define a collation as follows (in your `*DbContext.cs` file):

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.HasCollation("my_collation", locale: "en-u-ks-primary", provider: "icu", deterministic: false);

    //👇 using collations for a specific entity. -> entity level 👇
    modelBuilder.Entity<Customer>()
        .Property(c => c.Name)
        .UseCollation("my_collation");

    //👇 specify collations at the database level 👇
    modelBuilder.UseCollation("my_collation");
}
```

You can define collations both on entity level and database level like in the example above. If you want to use it in the database layer, you should also configure the collation usage in the `ConfigureConvention` method:

```csharp
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.Properties<string>().UseCollation("my_collation");
}
```

After these configurations, you should create a migration and apply it to your database as always. 

However, this solution comes with some problems, for example, by using non-deterministic collations, it's not yet possible to use pattern-matching operators such as `LIKE` on columns. This is a huge problem because it makes it hard to use LINQ. For example, you can't use the `.EndsWith` or `.StartsWith` methods, because they are [translated to `LIKE` command on the SQL level](https://www.npgsql.org/efcore/mapping/translations.html).

## Conclusion

In PostgreSQL, you can perform case-insensitive searches by using the `citext` data type or by utilizing collation settings. Nevertheless, if you have an ABP-based PostgreSQL application or a plain .NET application with PostgreSQL as the database option, to make a decision to pick one of these options, you can follow the following points:

* If the accent is not important for you and the only thing you want to do, is make the PostgreSQL queries case-insensitive, using the `citext` data type option should be selected
* If the accent is really important to you, and you don't use LINQ methods (such as `StartsWith` and `EndsWith` methods), you can use collations. 
  * Note that, with this approach, queries that are defined in the dependent modules also must not use these LINQ methods. Therefore, this approach is not suitable with the ABP Framework. Because some of the modules use LINQ methods (some pattern-matching operators).

Regardless of the method chosen, you can enable users to perform searches without worrying about case sensitivity. This is crucial for providing a user-friendly experience and making your database queries more flexible.

## References

* https://www.npgsql.org/efcore/misc/collations-and-case-sensitivity.html
* https://postgresql.verite.pro/blog/2019/10/14/nondeterministic-collations.html
* https://www.npgsql.org/efcore/mapping/translations.html
