# BLOB Storing Minio 提供程序

BLOB Storing Minio提供程序帮助你存储对象到 [MinIO Object storage](https://min.io/),

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统, 本文档仅介绍如何为容器配置Minio提供程序,

## 安装

使用 ABP CLI 来安装 [Volo.Abp.BlobStoring.Minio](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Minio) NuGet 包到你的项目：

* 如果你没有安装ABP CLI,请先安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI),
* 在要添加 `Volo.Abp.BlobStoring.Minio` 包的 `.csproj` 文件目录打开命令行,
* 执行 `abp add-package Volo.Abp.BlobStoring.Minio` 命令,

如果你要手动安装, 通过NuGet安装 [Volo.Abp.BlobStoring.Minio](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Minio) 到你的项目,然后添加 `[DependsOn(typeof(AbpBlobStoringMinioModule))]` 特性到你的 [ABP module](Module-Development-Basics.md) 类上,

## 配置

配置在你的[module](Module-Development-Basics.md)类中的`ConfigureServices`方法中完成,

**例: 配置使用Minio存储**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMinio(minio =>
        {
            minio.EndPoint = "你的 minio endPoint";
            minio.AccessKey = "你的 minio accessKey";
            minio.SecretKey = "你的 minio secretKey";
            minio.BucketName = "你的 minio bucketName";
        });
    });
});
````

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序,

### 选项

* **EndPoint** (string): 你的Minio对象存储服务的URL, 查看文档：https://docs.min.io/docs/dotnet-client-quickstart-guide.html
* **AccessKey** (string): Access key是唯一标识你的账户的用户ID, 
* **SecretKey** (string): Secret Key是你的账户的密码
* **BucketName** (string):你可以指定bucket名称,如果没有指定,将使用 `BlobContainerName` 属性定义的BLOB容器的名称(查阅[BLOB storing document](Blob-Storing.md)),MinIO完全兼容S3标准,所以有一些 **bucket命名规则**,必须符合[规则](https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html):
    * Bucket名称必须介于 3 到 63 个字符之间.
    * Bucket名称只能由小写字母、数字、句点 (.) 和连字符 (-) 组成.
    * Bucket名称必须以字母或数字开头和结尾.
    * Bucket名称不能是ip (例如, 192.168.5.4).
    * Bucket名称不能以 **xn--** 开头, (对于2020年2月以后创建的 Bucket).
    * Bucket名称在分区中必须唯一 .
    * Bucket 与 Amazon S3 Transfer Acceleration 一起使用时名称中不能有句点 (.).
* **WithSSL** (bool): 默认 `false`,代表使用HTTPS,
* **CreateContainerIfNotExists** (bool): 默认 `false`,如果不存在bucket, `MinioBlobProvider` 将会创建一个,


## Minio BLOB 名称计算器

默认情况下BLOB的全名由以下规则确定:

* 如果当前租户为 `null`(或容器禁用多租户 - 请参阅[BLOB存储文档](Blob-Storing.md) 了解如何禁用容器的多租户),则追加 `host` 字符串,
* 如果当前租户不为 `null`,则追加 `tenants/<tenant-id>` 字符串,
* 追加 BLOB 名称,

## 其他服务

* `MinioBlobProvider` 是实现Minio BLOB存储提供程序的主要服务,如果你想要通过[依赖注入](Dependency-Injection.md)覆盖/替换它(不要替换 `IBlobProvider` 接口,而是替换 `MinioBlobProvider` 类).
* `IMinioBlobNameCalculator` 服务用于计算文件路径. 默认实现是 `DefaultMinioBlobNameCalculator`. 如果你想自定义文件路径计算,可以替换/覆盖它.
