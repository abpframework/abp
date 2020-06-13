# BLOB Storing: Creating a Custom Provider

This document explains how you can create a new storage provider for the BLOB storing system with an example.

> Read the [BLOB Storing document](Blob-Storing.md) to understand how to use the BLOB storing system. This document only covers how to create a new storage provider.

## Example Implementation

The first step is to create a class implements the `IBlobProvider` interface or inherit from the `BlobProviderBase` abstract class.

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

* `MyCustomBlobProvider` inherits from the `BlobProviderBase` and overrides the `abstract` methods. The actual implementation is up to you.
* Implementing `ITransientDependency` registers this class to the [Dependency Injection](Dependency-Injection.md) system as a transient service.

> **Notice: Naming conventions are important**. If your class name doesn't end with `BlobProvider`, you must manually register/expose your service for the `IBlobProvider`.

That's all. Now, you can configure containers (inside the `ConfigureServices` method of your [module](Module-Development-Basics.md)) to use the `MyCustomBlobProvider` class:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.ProviderType = typeof(MyCustomBlobProvider);
    });
});
````

> See the [BLOB Storing document](Blob-Storing.md) if you want to configure a specific container.

### BlobContainerConfiguration Extension Method

If you want to provide a simpler configuration, create an extension method for the `BlobContainerConfiguration` class:

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

Then you can configure containers easier using the extension method:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMyCustomBlobProvider();
    });
});
````

### Extra Configuration Options

`BlobContainerConfiguration` allows to add/remove provider specific configuration objects. If your provider needs to additional configuration, you can create a wrapper class to the `BlobContainerConfiguration` for a type-safe configuration option:

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

Then you can change the `MyBlobContainerConfigurationExtensions` class like that:

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

* Added an action parameter to the `UseMyCustomBlobProvider` method to allow developers to set the additional options.
* Added a new `GetMyCustomBlobProviderConfiguration` method to be used inside `MyCustomBlobProvider` class to obtain the configured values.

Then anyone can set the `MyOption1` as shown below:

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

Finally, you can access to the extra options using the `GetMyCustomBlobProviderConfiguration` method:

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

## Contribute?

If you create a new provider and you think it can be useful for other developers, please consider to [contribute](Contribution/Index.md) to the ABP Framework on GitHub.
