# BLOB存储数据库提供程序

BLOB存储数据库提供程序可以将BLOB存储在关系或非关系数据库中.

有两个数据库提供程序实现;

* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore) 包实现[EF Core](Entity-Framework-Core.md), 它可以通过EF Core存储BLOB在[任何支持的DBMS](https://docs.microsoft.com/en-us/ef/core/providers/)中.
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB) 包实现了[MongoDB](MongoDB.md).

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何为容器配置数据库提供程序.

## 安装

### 自动安装

如果你已基于[应用程序启动模板](Startup-Templates/Application.md)创建了解决方案,则可以使用 `abp add-module` [CLI](CLI.md)命令将相关软件包自动添加到解决方案中.

在包含解决方案(`.sln`)文件的文件夹中打开命令行运行以下命令:

````bash
abp add-module Volo.Abp.BlobStoring.Database
````

此命令将所有NuGet软件包添加到解决方案的相应层. 如果使用的是EF Core,它会添加必要的配置,添加新的数据库迁移并更新数据库.

### 手动安装

这里是此提供程序定义的所有包:

* [Volo.Abp.BlobStoring.Database.Domain.Shared](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain.Shared)
* [Volo.Abp.BlobStoring.Database.Domain](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.Domain)
* [Volo.Abp.BlobStoring.Database.EntityFrameworkCore](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.EntityFrameworkCore)
* [Volo.Abp.BlobStoring.Database.MongoDB](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Database.MongoDB)

你可以只安装 `Volo.Abp.BlobStoring.Database.EntityFrameworkCore` 或 `Volo.Abp.BlobStoring.Database.MongoDB` (根据你的偏好),因为它们依赖其他包.

安装完成后,添加 `DepenedsOn` 属性到相关[模块](Module-Development-Basics.md).下面是由上面列出的相关NuGet包定义的模块类列表:

* `BlobStoringDatabaseDomainModule`
* `BlobStoringDatabaseDomainSharedModule`
* `BlobStoringDatabaseEntityFrameworkCoreModule`
* `BlobStoringDatabaseMongoDbModule`

如果你正在使用EF Core,还需要配置你的**Migration DbContext**将BLOB存储表添加到你的数据库. 在 `OnModelCreating` 方法中调用 `builder.ConfigureBlobStoring()` 扩展方法来包含到DbContext的映射. 你可以使用标准的 `Add-Migration` 和 `Update-Database` [命令](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)在数据库中创建必要的表.

## 配置

### 连接字符串

如果你要使用你的 `Default` 连接字符串,则不需要做任何其他配置.

如果要将BLOB存储到单独的数据库,请在配置文件(`appsettings.json`)中将 `AbpBlobStoring` 用作连接字符串名称. 请阅读[EF Core Migrations](Entity-Framework-Core-Migrations.md)文档了解如何为所需模块创建和使用其他数据库.

### 配置容器

如果只使用数据库存储提供程序,则不需要手动配置,因为它是自动完成的. 如果使用多个存储提供程序,可能需要对其进行配置.

如同[BLOB存储文档](Blob-Storing.md)所述,配置是在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法完成的.

**示例: 配置为默认使用数据库系统存储提供程序**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseDatabase();
    });
});
````

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序.

## 附加信息

它需要使用[BLOB存储服务](Blob-Storing.md)来使用BLOB存储系统. 但是如果要处理数据库表/实体,可以使用以下信息.

### 实体

此模块定义的实体:

* `DatabaseBlobContainer` (aggregate root) 表示存储在数据库中的容器.
* `DatabaseBlob` (aggregate root) 表示数据库中的BLOB.

参阅[实体文档](Entities.md)了解什么是实体和聚合根.

### 仓储

* `IDatabaseBlobContainerRepository`
* `IDatabaseBlobRepository`

你还可以使用 `IRepository` 和 `IRepository` 来获得 `IQueryable` 能力. 更多信息请参阅[仓储文档](Repositories.md).

### 其他服务

* `DatabaseBlobProvider` 是实现数据库BLOB存储提供程序的主要服务,如果你想要通过[依赖注入](Dependency-Injection.md)覆盖/替换它(不要替换 `IBlobProvider` 接口,而是替换 `DatabaseBlobProvider` 类).