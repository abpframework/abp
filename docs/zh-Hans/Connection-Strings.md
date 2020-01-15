# 连接字符串

ABP框架的设计是[模块化](Module-Development-Basics.md), [微服务兼容](Microservice-Architecture.md) 和 [多租户](Multi-Tenancy.md). 同时设计了连接字符串管理来支持这些场景;

* 允许为每个模块设置单独的连接字符串,这样每个模块都可以有自己的物理数据库. 甚至可以将模块配置为使用不同的DBMS.
* 允许为每个租户设置单独的连接字符串使用单独的数据库(在SaaS应用程序中).

它还支持混合场景;

* 允许将模块分组到数据库 (所有的模块分组到一个共享数据库, 两个模块使用数据库A, 3个模块使用数据库B, 一个数据库使用数据库C其余的数据库使用数据库D...等.)
* 允许将租户分组到数据库中,像模块一样.
* 允许为每个租户每个模块分离数据库 (数据库过多会增加维护成本,但ABP框架支持这种需求).

所有[预构建应用模块](Modules/Index.md)已设计为与以上场景兼容.

## 配置连接字符串

参见以下配置:

````json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=MyMainDb;Trusted_Connection=True;",
  "AbpIdentityServer": "Server=localhost;Database=MyIdsDb;Trusted_Connection=True;",
  "AbpPermissionManagement": "Server=localhost;Database=MyPermissionDb;Trusted_Connection=True;"
}
````

> ABP使用 `IConfiguration` 服务获取应用程序配置. 虽然在 `appsettings.json` 文件中写入配置是最简单的方法, 但它不仅限于此文件. 你可以使用环境变量, user secrets, Azure Key Vault... 等. 更多信息参阅 [配置](Configuration.md) 文档.

以上配置定义了三个不同的连接字符串:

* `MyMainDb` (`Default` 连接字符串)是应用程序的主连接字符串. 如果没有为模块指定连接字符串,则回退到 `Default` 连接字符串. [应用程序启动模板](Startup-Templates/Application.md) 配置为使用单个字符串, 所以所有的模块都使用单个数据库.
* `MyIdsDb` 由 [IdentityServer](Modules/IdentityServer.md) 模块使用.
* `MyPermissionDb` 由 [权限管理](Modules/Permission-Management.md) 模块使用.

[预构建的应用程序模块](Modules/Index.md) 为连接字符串名称定义常量. 例如IdentityServer模块在 `AbpIdentityServerDbProperties` 类(位于 `Volo.Abp.IdentityServer` 命名空间)定义了 `ConnectionStringName` 常量 . 其他的模块类似的定义常量,你可以查看连接字符串的名称.

## 设置连接字符串名称

模块通常使用 `ConnectionStringName` attribute 为 `DbContext` 类关联一个唯一的连接字符串名称. 示例:

````csharp
[ConnectionStringName("AbpIdentityServer")]
public class IdentityServerDbContext
    : AbpDbContext<IdentityServerDbContext>, IIdentityServerDbContext
{
}
````

对于 [Entity Framework Core](Entity-Framework-Core.md) 和 [MongoDB](MongoDB.md), 写入到 `DbContext` 类 (和接口,如果有的话).

> 如果你开发的是与数据库提供程序无关的可重用模块, 请参见 [最佳实践指南](Best-Practices/Index.md).

## Entity Framework Core的数据库迁移

关系数据库需要在使用数据库之前创建数据库和数据库架构 (表, 视图...等).

启动模板(使用 EF Core ORM) 带有一个数据库和一个 `.EntityFrameworkCore.DbMigrations` 项目,其中包含数据库的迁移文件. 该项目主要定义了一个*YourProjectName*MigrationsDbContext,它调用所有模块的 `Configure...()` 方法,例如 `builder.ConfigurePermissionManagement()`.

一旦要分离模块的数据库,通常需要创建第二个迁移路径. 最简单的方法是创建一个带有 `DbContext` 的 `.EntityFrameworkCore.DbMigrations` 项目副本, 更改为只调用需要存储在第二个数据库中的模块的 `Configure...()` 方法并重新创建迁移. 这时你还需要更改 `.DbMigrator` 应用程序使其兼容第二个数据库,这样每个数据库将有一个单独的迁移DbContext.

## 多租户

参阅 [多租户文档](Multi-Tenancy.md)了解如何为租户使用单独的数据库.