# BLOB存储文件系统提供程序

文件系统存储提供程序用于将BLOB作为文件夹中的标准文件存储在本地文件系统中.

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何为容器配置文件系统.

## 介绍

使用ABP CLI添加[Volo.Abp.BlobStoring.FileSystem](https://www.nuget.org/packages/Volo.Abp.BlobStoring.FileSystem)NuGet包到你的项目:

* 安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), 如果你还没有安装.
* 在要添加 `Volo.Abp.BlobStoring.FileSystem` 包的 `.csproj` 文件目录打开命令行.
* 运行 `abp add-package Volo.Abp.BlobStoring.FileSystem` 命令.

如果要手动安装,在你的项目中安装 `Volo.Abp.BlobStoring.FileSystem` NuGet包然后将`[DependsOn(typeof(AbpBlobStoringFileSystemModule))]`添加到项目内的[ABP模块](Module-Development-Basics.md)类中.

## 配置

如同[BLOB存储文档](Blob-Storing.md)所述,配置是在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法完成的.

**示例: 配置为默认使用文件系统存储提供程序**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseFileSystem(fileSystem =>
        {
            fileSystem.BasePath = "C:\\my-files";
        });
    });
});
````

`UseFileSystem` 扩展方法用于为容器设置文件系统提供程序并配置文件系统选项.

> 参阅[BLOB存储文档](Blob-Storing.md) 学习如何为指定容器配置提供程序.

### 选项

* **BasePath** (string): 存储BLOB的基本文件夹路径,它是必选的.
* **AppendContainerNameToBasePath** (bool; 默认: `true`): 指定是否在基本文件夹中创建具有容器名称的文件夹. 如果你在同一个 `BaseFolder` 中存储多个容器,请将其保留为`true`. 如果你不喜欢不必要的更深层次的文件夹,你可以将它设置为 `false`.

## 文件路径计算

文件系统提供程序在文件夹中组织BLOB文件并实现一些约定. 默认情况下,BLOB文件的完整路径由以下规则确定:

* 它以如上所述配置的 `BasePath` 开始.
* 如果当前租户为 `null`(或容器禁用多租户 - 请参阅[BLOB存储文档](Blob-Storing.md) 了解如何禁用容器的多租户),则追加 `host` 文件夹.
* 如果当前租户不为 `null`,则追加 `tenants/<tenant-id>` 文件夹.
* 如果 `AppendContainerNameToBasePath` 为`true`,则追加容器的名称. 如果容器名称包含 `/`,将导致文件夹嵌套.
* 追加BLOB名称,如果BLOB名称包含 `/` 它创建文件夹. 如果BLOB名称包含 `.` 它将有一个文件扩展名.

## 扩展文件系统提供程序

* `FileSystemBlobProvider` 是实现文件系统存储的主要服务. 你可以从这个类继承并[覆盖](Customizing-Application-Modules-Overriding-Services.md)方法进行自定义.
* `IBlobFilePathCalculator` 服务用于计算文件路径. 默认实现是 `DefaultBlobFilePathCalculator` . 如果你想自定义文件路径计算,可以替换/覆盖它.