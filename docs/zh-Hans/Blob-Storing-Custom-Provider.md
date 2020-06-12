# BLOB 存储: 创建自定义提供程序

本文档通过一个示例说明如何为BLOB存储系统创建新的存储提供程序.

> 阅读[BLOB存储文档](Blob-Storing.md)了解如何使用BLOB存储系统. 本文档仅介绍如何创建新存储提供程序.

## 示例实现

第一步是创建一个实现 `IBlobProvider` 接口或 `BlobProviderBase` 抽象类继承的类.

````csharp
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.BlobStoring;
using Volo.Abp.DependencyInjection;

namespace AbpDemo
{
    public class MyCustomBlobProvider : BlobProviderBase, ITransientDependency
    {
        public override Task SaveAsync(BlobProviderSaveArgs args)
        {
            //TODO...
        }

        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            //TODO...
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            //TODO...
        }

        public override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            //TODO...
        }
    }
}
````

* `MyCustomBlobProvider` 继承 `BlobProviderBase` 并覆盖 `abstract` 方法. 实际的实现取决于你.
* 实现 `ITransientDependency` 接口将这个类注做为瞬态服务注册到[依赖注入](Dependency-Injection.md)系统.

> **注意: 命名约定很重要**. 如果类名没有以 `BlobProvider` 结尾,则必须手动注册/公开你的服务为 `IBlobProvider`.

这是所有. 现在你可以配置容器(在[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法中)使用 `MyCustomBlobProvider` 类:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.ProviderType = typeof(MyCustomBlobProvider);
    });
});
````

> 如果你想配置特定的容器,请参阅[BLOB存储文档](Blob-Storing.md).

### BlobContainerConfiguration 扩展方法

如果你想提供一个更简单的配置方式,可以为 `BlobContainerConfiguration` 类创建一个扩展方法:

````csharp
public static class MyBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseMyCustomBlobProvider(
        this BlobContainerConfiguration containerConfiguration)
    {
        containerConfiguration.ProviderType = typeof(MyCustomBlobProvider);
        return containerConfiguration;
    }
}
````

然后你可以使用扩展方法更容易地配置容器:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMyCustomBlobProvider();
    });
});
````

### 额外的配置选项

`BlobContainerConfiguration` 允许添加/删除提供程序特定的配置对象. 如果你的提供者需要额外的配置,你可以为 `BlobContainerConfiguration` 创建一个包装类提供的类型安全配置选项:

````csharp
    public class MyCustomBlobProviderConfiguration
    {
        public string MyOption1
        {
            get => _containerConfiguration
                .GetConfiguration<string>("MyCustomBlobProvider.MyOption1");
            set => _containerConfiguration
                .SetConfiguration("MyCustomBlobProvider.MyOption1", value);
        }

        private readonly BlobContainerConfiguration _containerConfiguration;

        public MyCustomBlobProviderConfiguration(
            BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }
    }
````

然后你可以这样更改 `MyBlobContainerConfigurationExtensions` 类:

````csharp
public static class MyBlobContainerConfigurationExtensions
{
    public static BlobContainerConfiguration UseMyCustomBlobProvider(
        this BlobContainerConfiguration containerConfiguration,
        Action<MyCustomBlobProviderConfiguration> configureAction)
    {
        containerConfiguration.ProviderType = typeof(MyCustomBlobProvider);
        
        configureAction.Invoke(
            new MyCustomBlobProviderConfiguration(containerConfiguration)
        );
        
        return containerConfiguration;
    }
    
    public static MyCustomBlobProviderConfiguration GetMyCustomBlobProviderConfiguration(
        this BlobContainerConfiguration containerConfiguration)
    {
        return new MyCustomBlobProviderConfiguration(containerConfiguration);
    }
}
````

* 向 `UseMyCustomBlobProvider` 方法添加了一个参数,允许开发人员设置其他选项.
* 添加了一个新的 `GetMyCustomBlobProviderConfiguration` 方法,该方法将在 `MyCustomBlobProvider` 类内使用获取配置的值.

然后任何人都可以如下设置  `MyOption1`:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMyCustomBlobProvider(provider =>
        {
            provider.MyOption1 = "my value";
        });
    });
});
````

最后你可以使用 `GetMyCustomBlobProviderConfiguration` 方法访问额外的选项:

````csharp
public class MyCustomBlobProvider : BlobProviderBase, ITransientDependency
{
    public override Task SaveAsync(BlobProviderSaveArgs args)
    {
        var config = args.Configuration.GetMyCustomBlobProviderConfiguration();
        var value = config.MyOption1;
        
        //...
    }
}
````

## 贡献?

如果你创建了一个新的提供程序,并且认为它对其他开发者有用,请考虑为GitHub上的ABP框架做出[贡献](Contribution/Index.md).