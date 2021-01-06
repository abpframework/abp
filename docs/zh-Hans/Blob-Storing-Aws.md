# BLOB Storing Aws提供程序

BLOB存储Aws提供程序可以将BLOB存储在[Amazon Simple Storage Service](https://aws.amazon.com/cn/s3/)中.

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何为容器配置Aws提供程序.

## 安装

使用ABP CLI添加[Volo.Abp.BlobStoring.Aws](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Aws)NuGet包到你的项目:

* 安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), 如果你还没有安装.
* 在要添加 `Volo.Abp.BlobStoring.Aws` 包的 `.csproj` 文件目录打开命令行.
* 运行 `Volo.Abp.BlobStoring.Aws` 命令.

如果要手动安装,在你的项目中安装 `Volo.Abp.BlobStoring.Aws` NuGet包然后将`[DependsOn(typeof(AbpBlobStoringAwsModule))]`添加到项目内的[ABP模块](Module-Development-Basics.md)类中.

## 配置

如同[BLOB存储文档](Blob-Storing.md)所述,配置是在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法完成的.

**示例: 配置为默认使用Aws存储提供程序**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseAws(Aws =>
        {
            Aws.AccessKeyId = "your Aws access key id";
            Aws.SecretAccessKey = "your Aws access key secret";
            Aws.UseCredentials = "set true to use credentials";
            Aws.UseTemporaryCredentials = "set true to use temporary credentials";
            Aws.UseTemporaryFederatedCredentials = "set true to use temporary federated credentials";
            Aws.ProfileName = "the name of the profile to get credentials from";
            Aws.ProfilesLocation = "the path to the aws credentials file to look at";
            Aws.Region = "the system name of the service";
            Aws.Name = "the name of the federated user";
            Aws.Policy = "policy";
            Aws.DurationSeconds = "expiration date";
            Aws.ContainerName = "your Aws container name";
            Aws.CreateContainerIfNotExists = false;
        });
    });
});
````

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序.

### 选项

* **AccessKeyId** (string): AWS Access Key ID.
* **SecretAccessKey** (string): AWS Secret Access Key.
* **UseCredentials** (bool): 使用[本地凭证](https://docs.aws.amazon.com/zh_cn/AmazonS3/latest/dev/AuthUsingAcctOrUserCredentials.html)访问AWS服务,默认: `false`.
* **UseTemporaryCredentials** (bool): 使用[临时凭证](https://docs.aws.amazon.com/zh_cn/AmazonS3/latest/dev/AuthUsingTempSessionToken.html)访问AWS服务,默认: `false`.
* **UseTemporaryFederatedCredentials** (bool): 使用[联合身份用户临时凭证](https://docs.aws.amazon.com/zh_cn/AmazonS3/latest/dev/AuthUsingTempFederationToken.html)访问AWS服务, 默认: `false`.
* **ProfileName** (string): [本地凭证配置文件](https://docs.aws.amazon.com/zh_cn/sdk-for-net/v3/developer-guide/net-dg-config-creds.html)名称.
* **ProfilesLocation** (string): 本地配置文件位置.
* **Region** (string): 服务的地区名称.
* **Policy** (string): JSON格式的IAM策略.
* **DurationSeconds** (int): 设置临时访问凭证的有效期,单位是s,最小为900,最大为129600.
* **ContainerName** (string): 你可以在Aws中指定容器名称. 如果没有指定它将使用 `BlobContainerName` 属性定义的BLOB容器的名称(请参阅[BLOB存储文档](Blob-Storing.md)). 请注意Aws有一些**命名容器的规则**,容器名称必须是有效的DNS名称,[符合以下命名规则](https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html):
    * Bucket名称必须介于 3 到 63 个字符之间.
    * Bucket名称只能由小写字母、数字、句点 (.) 和连字符 (-) 组成.
    * Bucket名称必须以字母或数字开头和结尾.
    * Bucket名称不能是ip (例如, 192.168.5.4).
    * Bucket名称不能以 **xn--** 开头, (对于2020年2月以后创建的 Bucket).
    * Bucket名称在分区中必须唯一 .
    * Bucket 与 Amazon S3 Transfer Acceleration 一起使用时名称中不能有句点 (.).
* **CreateContainerIfNotExists** (bool): 默认值为 `false`, 如果Aws中不存在容器, `AwsBlobProvider` 将尝试创建它.

## Aws BLOB 名称计算器

Aws BLOB提供程序组织BLOB名称并实现一些约定. 默认情况下BLOB的全名由以下规则确定:

* 如果当前租户为 `null`(或容器禁用多租户 - 请参阅[BLOB存储文档](Blob-Storing.md) 了解如何禁用容器的多租户),则追加 `host` 字符串.
* 如果当前租户不为 `null`,则追加 `tenants/<tenant-id>` 字符串.
* 追加 BLOB 名称.

## 其他服务

* `AwsBlobProvider` 是实现Aws BLOB存储提供程序的主要服务,如果你想要通过[依赖注入](Dependency-Injection.md)覆盖/替换它(不要替换 `IBlobProvider` 接口,而是替换 `AwsBlobProvider` 类).
* `IAwsBlobNameCalculator` 服务用于计算文件路径. 默认实现是 `DefaultAwsBlobNameCalculator`. 如果你想自定义文件路径计算,可以替换/覆盖它.
* `IAmazonS3ClientFactory` 服务用于生成AWS S3客户端. 默认实现是 `DefaultAmazonS3ClientFactory` . 如果你想自定义AWS S3客户端生成,可以替换/覆盖它.
