# BLOB Storing Aliyun提供程序

BLOB存储Aliyun提供程序可以将BLOB存储在[Aliyun Blob storage](https://help.aliyun.com/product/31815.html)中.

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何为容器配置Aliyun提供程序.

## 安装

使用ABP CLI添加[Volo.Abp.BlobStoring.Aliyun](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Aliyun)NuGet包到你的项目:

* 安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), 如果你还没有安装.
* 在要添加 `Volo.Abp.BlobStoring.Aliyun` 包的 `.csproj` 文件目录打开命令行.
* 运行 `Volo.Abp.BlobStoring.Aliyun` 命令.

如果要手动安装,在你的项目中安装 `Volo.Abp.BlobStoring.Aliyun` NuGet包然后将`[DependsOn(typeof(AbpBlobStoringAliyunModule))]`添加到项目内的[ABP模块](Module-Development-Basics.md)类中.

## 配置

如同[BLOB存储文档](Blob-Storing.md)所述,配置是在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法完成的.

**示例: 配置为默认使用Aliyun存储提供程序**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseAliyun(aliyun =>
        {
            aliyun.AccessKeyId = "your aliyun access key id";
            aliyun.AccessKeySecret = "your aliyun access key secret";
            aliyun.Endpoint = "your oss endpoint";
            aliyun.RegionId = "your sts region id";
            aliyun.RoleArn = "the arn of ram role";
            aliyun.RoleSessionName = "the name of the certificate";
            aliyun.Policy = "policy";
            aliyun.DurationSeconds = "expiration date";
            aliyun.ContainerName = "your aliyun container name";
            aliyun.CreateContainerIfNotExists = true;
        });
    });
});
````

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序.

### 选项

* **AccessKeyId** ([NotNull]string): 云账号AccessKey是访问阿里云API的密钥,具有该账户完全的权限,请你务必妥善保管!强烈建议遵循[阿里云安全最佳实践](https://help.aliyun.com/document_detail/102600.html),使用RAM子用户AccessKey来进行API调用.
* **AccessKeySecret** ([NotNull]string): 同上.
* **Endpoint** ([NotNull]string): Endpoint表示OSS对外服务的访问域名. [访问域名和数据中心](https://help.aliyun.com/document_detail/31837.html)
* **UseSecurityTokenService** (bool): 是否使用STS临时授权访问OSS,默认false. [STS临时授权访问OSS](https://help.aliyun.com/document_detail/100624.html)
* **RegionId** (string): STS服务的接入地址,每个地址的功能都相同,请尽量在同地域进行调用. [接入地址](https://help.aliyun.com/document_detail/66053.html)
* **RoleArn** ([NotNull]string): STS所需角色ARN. 
* **RoleSessionName** ([NotNull]string): 用来标识临时访问凭证的名称,建议使用不同的应用程序用户来区分.
* **Policy** (string): 在扮演角色的时候额外添加的权限限制. 请参见[基于RAM Policy的权限控制](https://help.aliyun.com/document_detail/100680.html).
* **DurationSeconds** (int): 设置临时访问凭证的有效期,单位是s,最小为900,最大为3600. 
* **ContainerName** (string): 你可以在aliyun中指定容器名称. 如果没有指定它将使用 `BlobContainerName` 属性定义的BLOB容器的名称(请参阅[BLOB存储文档](Blob-Storing.md)). 请注意Aliyun有一些**命名容器的规则**,容器名称必须是有效的DNS名称,[符合以下命名规则](https://help.aliyun.com/knowledge_detail/39668.html):
    * 只能包含小写字母,数字和短横线(-)
    * 必须以小写字母和数字开头和结尾
    * Bucket名称的长度限制在**3**到**63**个字符之间
* **CreateContainerIfNotExists** (bool): 默认值为 `false`, 如果aliyun中不存在容器, `AliyunBlobProvider` 将尝试创建它.
* **TemporaryCredentialsCacheKey** (bool): STS凭证缓存Key,默认Guid.NewGuid().ToString("N").

## Aliyun BLOB 名称计算器

Aliyun BLOB提供程序组织BLOB名称并实现一些约定. 默认情况下BLOB的全名由以下规则确定:

* 如果当前租户为 `null`(或容器禁用多租户 - 请参阅[BLOB存储文档](Blob-Storing.md) 了解如何禁用容器的多租户),则追加 `host` 字符串.
* 如果当前租户不为 `null`,则追加 `tenants/<tenant-id>` 字符串.
* 追加 BLOB 名称.

## 其他服务

* `AliyunBlobProvider` 是实现Aliyun BLOB存储提供程序的主要服务,如果你想要通过[依赖注入](Dependency-Injection.md)覆盖/替换它(不要替换 `IBlobProvider` 接口,而是替换 `AliyunBlobProvider` 类).
* `IAliyunBlobNameCalculator` 服务用于计算文件路径. 默认实现是 `DefaultAliyunBlobNameCalculator`. 如果你想自定义文件路径计算,可以替换/覆盖它.
* `IOssClientFactory` 服务用于生成OSS客户端. 默认实现是 `DefaultOssClientFactory` . 如果你想自定义OSS客户端生成,可以替换/覆盖它.
