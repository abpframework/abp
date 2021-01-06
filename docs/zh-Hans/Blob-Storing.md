# BLOB 存储

通常将文件内容存储在应用程序中并根据需要读取这些文件内容. 不仅是文件你可能还需要将各种类型的[BLOB](https://en.wikipedia.org/wiki/Binary_large_object)(大型二进制对象)保存到存储中. 例如你可能要保存用户个人资料图片.

BLOB通常是一个**字节数组**. 有很多地方可以存储BLOB项. 可以选择将其存储在本地文件系统中,共享数据库中或[Azure BLOB存储](https://azure.microsoft.com/zh-cn/services/storage/blobs/)中.

ABP框架为BLOB提供了抽象,并提供了一些可以轻松集成到的预构建存储提供程序. 抽象有一些好处;

* 你可以通过几行配置**轻松的集成**你喜欢的BLOB存储提供程序.
* 你可以**轻松的更改**BLOB存储,而不用改变你的应用程序代码.
* 如果你想创建**可重用的应用程序模块**,无需假设BLOB的存储方式.

ABP BLOB存储系统兼容ABP框架其他功能,如[多租户](Multi-Tenancy.md).

## BLOB 存储提供程序

ABP框架已经有以下存储提供程序的实现;

* [File System](Blob-Storing-File-System.md):将BLOB作为标准文件存储在本地文件系统的文件夹中.
* [Database](Blob-Storing-Database.md): 将BLOB存储在数据库中.
* [Azure](Blob-Storing-Azure.md): 将BLOB存储在 [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/)中.
* [Aliyun](Blob-Storing-Aliyun.md): 将BLOB存储在[Aliyun Storage Service](https://help.aliyun.com/product/31815.html)中.
* [Minio](Blob-Storing-Minio.md): 将BLOB存储在[MinIO Object storage](https://min.io/)中.
* [Aws](Blob-Storing-Aws.md): 将BLOB存储在[Amazon Simple Storage Service](https://aws.amazon.com/s3/)中.

以后会实现更多的提供程序,你可以为自己喜欢的提供程序创建[请求](https://github.com/abpframework/abp/issues/new),或者你也可以[自己实现](Blob-Storing-Custom-Provider.md)它并[贡献](Contribution/Index.md)到ABP框架.

可以在**容器系统**的帮助下一起**使用多个提供程序**,其中每个容器可以使用不同的提供程序.

> 除非你**配置存储提供程序**否则BLOB存储系统无法工作. 有关存储提供程序配置请参考链接的文档.

## 安装

[Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring)是定义BLOB存储服务的主要包. 你可以用此包使用BLOB存储系统而不依赖特定存储提供程序.

使用ABP CLI这个包添加到你的项目:

* 安装 [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), 如果你还没有安装.
* 在要添加 `Volo.Abp.BlobStoring` 包的 `.csproj` 文件目录打开命令行.
* 运行 `abp add-package Volo.Abp.BlobStoring` 命令.

如果要手动安装,在你的项目中安装 `Volo.Abp.BlobStoring` NuGet包然后将`[DependsOn(typeof(AbpBlobStoringModule))]`添加到项目内的[ABP模块](Module-Development-Basics.md)类中.

## IBlobContainer

`IBlobContainer` 是存储和读取BLOB的主要接口. 应用程序可能有多个容器,每个容器都可以单独配置. 有一个**默认容器**可以通过注入 `IBlobContainer` 来简单使用.

**示例: 简单地保存和读取命名BLOB的字节**

````csharp
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly IBlobContainer _blobContainer;

        public MyService(IBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public async Task SaveBytesAsync(byte[] bytes)
        {
            await _blobContainer.SaveAsync("my-blob-1", bytes);
        }

        public async Task<byte[]> GetBytesAsync()
        {
            return await _blobContainer.GetAllBytesOrNullAsync("my-blob-1");
        }
    }
}
````

该服务用 `my-blob-1` 名称保存给定的字节,然后以相同的名称获取先前保存的字节.

> 一个BLOB是一个命名对象,**每个BLOB都应该有一个唯一的名称**,它是一个任意的字符串.

`IBlobContainer` 可以处理 `Stream` 和 `byte[]` 对象,在下一节中将详细介绍.

### 保存 BLOB

`SaveAsync` 方法用于保存新的或替换现有的BLOB. 默认情况下,它可以保存 `Stream`,但是有一个快捷的扩展方法来保存字节数组.

`SaveAsync` 有以下参数:

* **name** (string): 唯一的BLOB名称.
* **stream** (Stream) or **byte** (byte[]): 读取BLOB内容或字节数组的流.
* **overrideExisting** (bool): 设置为 `true`,如果BLOB内容已经存在,则替换它. 默认值为 `false`,则抛出 `BlobAlreadyExistsException` 异常.

### 读取/获取 BLOB

* `GetAsync`: 返回给定BLOB名称可用于读取BLOB内容的 `Stream` 对象. 使用后始终要**dispose流**. 如果找不到具有给定名称的BLOB,则抛出异常.
* `GetOrNullAsync`: 与 `GetAsync` 方法相反,如果未找到给定名称的BLOB,则返回 `null`.
* `GetAllBytesAsync`: 返回 `byte[]` 而不是 `Stream`. 如果找不到具有给定名称的BLOB,则抛出异常.
* `GetAllBytesOrNullAsync`: 与 `GetAllBytesAsync` 方法相反,如果未找到给定名称的BLOB,则返回 `null`.

### 删除 BLOB

`DeleteAsync` 使用给定BLOB名称删除BLOB数据. 如果找不到给定的BLOB不会引发任何异常. 相反如果你关心BLOB,它会返回一个 `bool`,表示BLOB实际上是否已删除.

### 其他方法

* `ExistsAsync` 方法简单的检查容器中是否存在具有给定名称的BLOB.

### 关于命名BLOB

没有命名BLOB的规则. BLOB名称只是每个容器(和每个租户-参见"*多租户*"部分)唯一的字符串. 但是不同的存储提供程序可能会按惯例实施某些做法. 例如[文件系统提供程序](Blob-Storing-File-System.md)在BLOB名称中使用目录分隔符 (`/`) 和文件扩展名(如果BLOB名称为 `images/common/x.png` ,则在根容器文件夹下的 `images/common` 文件夹中存储 `x.png`).

## 类型化 IBlobContainer

类型化BLOB容器系统是一种在应用程序中创建和管理**多个容器**的方法;

* **每个容器分别存储**. 这意味着BLOB名称在一个容器中应该是唯一的,两个具有相同名称的BLOB可以存在不同的容器中不会互相影响.
* **每个容器可以单独配置**,因此每个容器可以根据你的配置使用不同的存储提供程序.

要创建类型化容器,需要创建一个简单的用 `BlobContainerName` 属性装饰的类:

````csharp
using Volo.Abp.BlobStoring;

namespace AbpDemo
{
    [BlobContainerName("profile-pictures")]
    public class ProfilePictureContainer
    {

    }
}
````

> 如果不使用 `BlobContainerName` attribute,ABP Framework将使用类的全名(带有名称空间),但是始终建议使用稳定的容器名称,即使重命名该类也不会被更改.

创建容器类后,可以为容器类型注入 `IBlobContainer<T>`.

**示例: 用于保存和读取[当前用户](CurrentUser.md)的个人资料图片的[应用服务](Application-Services.md)**

````csharp
[Authorize]
public class ProfileAppService : ApplicationService
{
    private readonly IBlobContainer<ProfilePictureContainer> _blobContainer;

    public ProfileAppService(IBlobContainer<ProfilePictureContainer> blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveProfilePictureAsync(byte[] bytes)
    {
        var blobName = CurrentUser.GetId().ToString();
        await _blobContainer.SaveAsync(blobName, bytes);
    }

    public async Task<byte[]> GetProfilePictureAsync()
    {
        var blobName = CurrentUser.GetId().ToString();
        return await _blobContainer.GetAllBytesOrNullAsync(blobName);
    }
}
````

`IBlobContainer<T>` 有与 `IBlobContainer` 相同的方法.

> 在开发可重复使用的模块时,**始终使用类型化的容器是一个好习惯**,这样最终的应用程序就可以为你的容器配置提供程序,而不会影响其他容器.

### 默认容器

如果不使用泛型参数,直接注入 `IBlobContainer` (如上所述),会得到默认容器. 注入默认容器的另一种方法是使用 `IBlobContainer<DefaultContainer>`,它返回完全相同的容器.

默认容器的名称是  `default`.

### 命令容器

类型容器只是命名容器的快捷方式. 你可以注入并使用 `IBlobContainerFactory` 来获得一个BLOB容器的名称:

````csharp
public class ProfileAppService : ApplicationService
{
    private readonly IBlobContainer _blobContainer;

    public ProfileAppService(IBlobContainerFactory blobContainerFactory)
    {
        _blobContainer = blobContainerFactory.Create("profile-pictures");
    }

    //...
}
````

## IBlobContainerFactory

`IBlobContainerFactory` 是用于创建BLOB容器的服务. 上面提供了一个示例.

**示例: 通过名称创建容器**

````csharp
var blobContainer = blobContainerFactory.Create("profile-pictures");
````

**示例: 通过类型创建容器**

````csharp
var blobContainer = blobContainerFactory.Create<ProfilePictureContainer>();
````

> 通常你不需要使用 `IBlobContainerFactory`, 因为在注入 `IBlobContainer` 或`IBlobContainer<T>` 时会在内部使用它.

## 配置容器

在使用容器之前应先对其进行配置. 最基本的配置是选择一个 **BLOB存储提供程序**(请参阅上面的"*BLOB存储提供程序*"部分).

`AbpBlobStoringOptions` 是用于配置容器的[选项类](Options.md). 你可以在[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法中配置选项.

### 配置单个容器

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        //TODO...
    });
});
````

这个例子配置 `ProfilePictureContainer`. 你还可以通过容器名称进行配置:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure("profile-pictures", container =>
    {
        //TODO...
    });
});
````

### 配置默认容器

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        //TODO...
    });
});
````

> 默认容器有一个特殊情况;如果不为容器指定配置,则**返回到默认容器配置**. 这是一种为所有容器配置默认值并在需要时专门针对特定容器进行配置的好方法.

### 配置所有容器

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureAll((containerName, containerConfiguration) =>
    {
        //TODO...
    });
});
````

这是配置所有容器的方式.

> 与配置默认容器的主要区别在于, `ConfigureAll` 会覆盖配置,即使它是专门为特定容器配置的.

## 多租户


如果你的应用程序是多租户的,BLOB存储系统可以**与[多租户](Multi-Tenancy.md)无缝协作**. 所有提供程序都将多租户实现为标准功能. 它们将不同租户的**BLOB彼此隔离**,因此它们只能访问自己的BLOB. 这意味着你可以**为不同的租户使用相同的BLOB名称**.

如果应用程序是多租户的,则可能需要单独控制容器的**多租户行为**. 例如你可能希望**禁用特定容器的多租户**,这样容器中的BLOB将对**所有租户可用**. 这是在所有租户之间共享BLOB的一种方法.

**示例: 禁用特定容器的多租户**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        container.IsMultiTenant = false;
    });
});
````

> 如果你的应用程序不是多租户的,不用担心,它会正常工作. 你不需要配置 `IsMultiTenant` 选项.

## 扩展BLOB存储系统

大多数时候除了创建定制的BLOB存储提供程序外,你不需要[自定义BLOB存储系统](Blob-Storing-Custom-Provider.md).但是如果需要,你可以替换任何服务(通过[依赖注入](Dependency-Injection.md)). 这里有一些上面没有提到的其他服务,但你可能想知道:

* `IBlobProviderSelector` 用于通过容器名称获取 `IBlobProvider` 实例. 默认实现(`DefaultBlobProviderSelector`)使用配置选择提供程序.
* `IBlobContainerConfigurationProvider` 用于获取给定容器名称的`BlobContainerConfiguration`. 默认实现(`DefaultBlobContainerConfigurationProvider`)从上述 `AbpBlobStoringOptions` 获取配置.

## BLOB 存储 vs 文件管理系统

注意BLOB存储不是一个文件管理系统. 它是一个用于保存,获取和删除命名BLOB的低级别系统. 它不提供目录那样的层次结构,这是典型文件系统所期望的.

如果你想创建文件夹并在文件夹之间移动文件,为文件分配权限并在用户之间共享文件,那么你需要在BLOB存储系统上实现你自己的应用程序.

## 另请参阅

* [创建自定义BLOB存储提供程序](Blob-Storing-Custom-Provider.md)
