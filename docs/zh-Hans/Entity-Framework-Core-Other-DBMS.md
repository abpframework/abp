# 切换到EF Core 其它DBMS提供程序

**[应用程序启动模板](Startup-Templates/Application.md)** 为EF Core预配置了Sql Server提供程序,EF Core支持许多其它DBMS,你可以在基于ABP的应用程序使用它们.

ABP框架为一些常见的DMBS提供了简化配置的集成包(有关可用集成包的列表,请参阅[EF Core文档](Entity-Framework-Core.md)),你也可以不使用集成包配置DBMS提供程序.

虽然总是建议使用集成包(它也使不同模块之间的依赖版本成为标准版本),但是如果没有用于DBMS提供程序的集成包,也可以手动集成.

本文介绍了如何在不使用[MySQL集成包](Entity-Framework-Core-MySQL.md)的情况下切换到MySQL.

## 替换SQL Server依赖

* 删除 `.EntityFrameworkCore` 项目依赖的 [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet 包.
* 添加 [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/) NuGet 包到 `.EntityFrameworkCore` 项目.

## 删除模块依赖项

从 ***YourProjectName*EntityFrameworkCoreModule** 类的依赖列表中删除`AbpEntityFrameworkCoreSqlServerModule`.

## 更改UseSqlServer()调用

在*YourProjectName*EntityFrameworkCoreModule类中找到以下代码:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.UseSqlServer();
});
````

替换成以下代码:

````csharp
Configure<AbpDbContextOptions>(options =>
{
    options.Configure(ctx =>
    {
        if (ctx.ExistingConnection != null)
        {
            ctx.DbContextOptions.UseMySql(ctx.ExistingConnection);
        }
        else
        {
            ctx.DbContextOptions.UseMySql(ctx.ConnectionString);
        }
    });
});
````

* 调用的 `UseMySql` 代码是在 Pomelo.EntityFrameworkCore.MySql 包中定义的,方法还有附加选项,如果需要可以使用它.
* 这段代码首先检查当前请求中是否存在到相同数据库的现有(活动)连接,并在可能的情况下重用它. 这允许在不同的DbContext类型之间共享单个事务. ABP处理其余的事情.
* 如果没有活动的连接,它将把 `ctx.ConnectionString` 传递给UseMySql(这将创建新的数据库连接). 这里使用 `ctx.ConnectionString` 很重要. 不要传递静态连接字符串(或配置中的连接字符串). 因为ABP在多数据库或[多租户](Multi-Tenancy.md)环境中[动态确定正确的连接字符串](Connection-Strings.md).

## 更改连接连接字符串

MySQL连接字符串与SQL Server连接字符串不同. 所以检查你的解决方案中所有的 `appsettings.json` 文件,更改其中的连接字符串. 有关MySQL连接字符串选项的详细内容请参见[connectionstrings.com](https://www.connectionstrings.com/mysql/).

通常需要更改 `.DbMigrator` 和 `.Web` 项目里面的 `appsettings.json` ,但它取决于你的解决方案结构.

## 更改迁移DbContext

MySQL DBMS与SQL Server有一些细微的差异. 某些模块数据库映射配置(尤其是字段长度)会导致MySQL出现问题. 例如某些[IdentityServer模块](Modules/IdentityServer.md)表就存在这样的问题,它提供了一个选项可以根据您的DBMS配置字段.

启动模板包含*YourProjectName*MigrationsDbContext,它负责维护和迁移数据库架构. 此DbContext基本上调用依赖模块的扩展方法来配置其数据库表.

打开 *YourProjectName*MigrationsDbContext 更改 `builder.ConfigureIdentityServer();` 行,如下所示:

````csharp
builder.ConfigureIdentityServer(options =>
{
    options.DatabaseProvider = EfCoreDatabaseProvider.MySql;
});
````

然后 `ConfigureIdentityServer()` 方法会将字段长度设置为超过MySQL的限制. 如果在创建或执行数据库迁移时遇到任何问题请参考相关的模块文档.

## 重新生成迁移

启动模板使用[Entity Framework Core的Code First迁移](https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/). EF Core迁移取决于所选的DBMS提供程序. 因此更改DBMS提供程序会导致迁移失败.

* 删除 `.EntityFrameworkCore.DbMigrations` 项目下的Migrations文件夹,并重新生成解决方案.
* 在包管理控制台中运行 `Add-Migration "Initial"`(在解决方案资源管理器选择 `.DbMigrator`  (或 `.Web`) 做为启动项目并且选择 `.EntityFrameworkCore.DbMigrations` 做为默认项目).

这将创建一个配置所有数据库对象(表)的数据库迁移.

运行 `.DbMigrator` 项目创建数据库和初始种子数据.

## 运行应用程序

它已准备就绪, 只需要运行该应用程序与享受编码.