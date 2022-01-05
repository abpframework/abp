# BLOB Storing Azure提供程序

BLOB存储Azure提供程序可以将BLOB存储在[Azure Blob storage](https://azure.microsoft.com/en-us/services/storage/blobs/)中.

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何为容器配置Azure提供程序.

## 安装

使用ABP CLI添加[Volo.Abp.BlobStoring.Azure](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Azure)NuGet包到你的项目:

* 安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), 如果你还没有安装.
* 在要添加 `Volo.Abp.BlobStoring.Azure` 包的 `.csproj` 文件目录打开命令行.
* 运行 `Volo.Abp.BlobStoring.Azure` 命令.

如果要手动安装,在你的项目中安装 `Volo.Abp.BlobStoring.Azure` NuGet包然后将`[DependsOn(typeof(AbpBlobStoringAzureModule))]`添加到项目内的[ABP模块](Module-Development-Basics.md)类中.

## 配置

如同[BLOB存储文档](Blob-Storing.md)所述,配置是在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法完成的.

**示例: 配置为默认使用Azure存储提供程序**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseAzure(azure =>
        {
            azure.ConnectionString = "your azure connection string";
            azure.ContainerName = "your azure container name";
            azure.CreateContainerIfNotExists = true;
        });
    });
});
````

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序.

### 选项

* **ConnectionString** (string): 连接字符串包括应用程序在运行时使用共享密钥授权访问Azure存储帐户中的数据所需的授权信息. 请参考[Azure文档](https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string).
* **ContainerName** (string): 你可以在azure中指定容器名称. 如果没有指定它将使用 `BlobContainerName` 属性定义的BLOB容器的名称(请参阅[BLOB存储文档](Blob-Storing.md)). 请注意Azure有一些**命名容器的规则**,容器名称必须是有效的DNS名称,[符合以下命名规则](https://docs.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata#container-names):
    * 容器名称必须以字母或数字开头或结尾,并且只能包含字母,数字和破折号(-)字符.
    * 每个破折号(-)必须紧跟在字母或数字之后;容器名称中不允许使用连续的破折号.
    * 容器名称中的所有字母都必须**小写**.
    * 容器名称的长度必须在**3**到**63**个字符之间.
* **CreateContainerIfNotExists** (bool): 默认值为 `false`, 如果azure中不存在容器, `AzureBlobProvider` 将尝试创建它.

## Azure BLOB 名称计算器

Azure BLOB提供程序组织BLOB名称并实现一些约定. 默认情况下BLOB的全名由以下规则确定:

* 如果当前租户为 `null`(或容器禁用多租户 - 请参阅[BLOB存储文档](Blob-Storing.md) 了解如何禁用容器的多租户),则追加 `host` 字符串.
* 如果当前租户不为 `null`,则追加 `tenants/<tenant-id>` 字符串.
* 追加 BLOB 名称.

## 其他服务

* `AzureBlobProvider` 是实现Azure BLOB存储提供程序的主要服务,如果你想要通过[依赖注入](Dependency-Injection.md)覆盖/替换它(不要替换 `IBlobProvider` 接口,而是替换 `AzureBlobProvider` 类).
* `IAzureBlobNameCalculator` 服务用于计算文件路径. 默认实现是 `DefaultAzureBlobNameCalculator` . 如果你想自定义文件路径计算,可以替换/覆盖它.
