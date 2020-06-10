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

````
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

