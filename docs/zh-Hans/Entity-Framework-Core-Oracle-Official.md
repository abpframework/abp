# 切换到EF Core Oracle提供程序

本文介绍如何将预配置为SqlServer提供程序的 **[应用程序启动模板](Startup-Templates/Application.md)** 切换到 **Oracle** 数据库提供程序

> 本文档使用[Devart](https://www.devart.com/dotconnect/oracle/)公司的付费库,因为它是oracle唯一支持EF Core 3.x的库

## 替换Volo.Abp.EntityFrameworkCore.SqlServer包

解决方案中的 `.EntityFrameworkCore` 项目依赖于 [Volo.Abp.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.SqlServer) NuGet包. 删除这个包并且添加相同版本的 [Volo.Abp.EntityFrameworkCore.Oracle.Devart](https://www.nuget.org/packages/Volo.Abp.EntityFrameworkCore.Oracle.Devart) 包.

## 替换模块依赖项

在 `.EntityFrameworkCore` 项目中找到 **YourProjectName*EntityFrameworkCoreModule** 类, 删除 `DependsOn` attribute 上的`typeof(AbpEntityFrameworkCoreSqlServerModule)`, 添加 `typeof(AbpEntityFrameworkCoreOracleDevartModule)` (并且替换 `using Volo.Abp.EntityFrameworkCore.SqlServer;` 为 `using Volo.Abp.EntityFrameworkCore.Oracle.Devart;`).

## UseOracle()

Find `UseSqlServer()` calls in your solution, replace with `UseOracle()`. Check the following files:

* *YourProjectName*EntityFrameworkCoreModule.cs inside the `.EntityFrameworkCore` project.
* *YourProjectName*MigrationsDbContextFactory.cs inside the `.EntityFrameworkCore.DbMigrations` project.

In the `CreateDbContext()` method of the *YourProjectName*MigrationsDbContextFactory.cs, replace the following code block

查找你的解决方案中 `UseSqlServer()`调用,替换为 `UseOracle()`. 检查下列文件:

* `.EntityFrameworkCore` 项目中的*YourProjectName*EntityFrameworkCoreModule.cs.
* `.EntityFrameworkCore.DbMigrations` 项目中的*YourProjectName*MigrationsDbContextFactory.cs.

使用以下代码替换*YourProjectName*MigrationsDbContextFactory.cs中的  `CreateDbContext()` 方法:

```csharp
var builder = new DbContextOptionsBuilder<YourProjectNameMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));
```

与这个

```csharp
var builder = (DbContextOptionsBuilder<YourProjectNameMigrationsDbContext>)
	new DbContextOptionsBuilder<YourProjectNameMigrationsDbContext>().UseOracle
	(
		configuration.GetConnectionString("Default")
	);
```

> 根据你的解决方案的结构,你可能发现更多需要改变代码的文件.

## 更改连接字符串

Oracle连接字符串与SQL Server连接字符串不同. 所以检查你的解决方案中所有的 `appsettings.json` 文件,更改其中的连接字符串. 有关oracle连接字符串选项的详细内容请参见[connectionstrings.com](https://www.connectionstrings.com/oracle/).

通常需要更改 `.DbMigrator` 和 `.Web` 项目里面的 `appsettings.json` ,但它取决于你的解决方案结构.

## 重新生成迁移

启动模板使用[Entity Framework Core的Code First迁移](https://docs.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/). EF Core迁移取决于所选的DBMS提供程序. 因此更改DBMS提供程序会导致迁移失败.

* 删除 `.EntityFrameworkCore.DbMigrations` 项目下的Migrations文件夹,并重新生成解决方案.
* 在包管理控制台中运行 `Add-Migration "Initial"`(在解决方案资源管理器选择 `.DbMigrator`  (或 `.Web`) 做为启动项目并且选择 `.EntityFrameworkCore.DbMigrations` 做为默认项目).

这将创建一个配置所有数据库对象(表)的数据库迁移.

运行 `.DbMigrator` 项目创建数据库和初始种子数据.

## 运行应用程序

它已准备就绪, 只需要运行该应用程序与享受编码.