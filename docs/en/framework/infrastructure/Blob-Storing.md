# BLOB Storing

It is typical to **store file contents** in an application and read these file contents on need. Not only files, but you may also need to save various types of **large binary objects**, a.k.a. [BLOB](https://en.wikipedia.org/wiki/Binary_large_object)s, into a **storage**. For example, you may want to save user profile pictures.

A BLOB is a typically **byte array**. There are various places to store a BLOB item; storing in the local file system, in a shared database or on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/) can be options.

The ABP Framework provides an abstraction to work with BLOBs and provides some pre-built storage providers that you can easily integrate to. Having such an abstraction has some benefits;

* You can **easily integrate** to your favorite BLOB storage provides with a few lines of configuration.
* You can then **easily change** your BLOB storage without changing your application code.
* If you want to create **reusable application modules**, you don't need to make assumption about how the BLOBs are stored.

ABP BLOB Storage system is also compatible to other ABP Framework features like [multi-tenancy](Multi-Tenancy.md).

## BLOB Storage Providers

The ABP Framework has already the following storage provider implementations:

* [File System](Blob-Storing-File-System.md): Stores BLOBs in a folder of the local file system, as standard files.
* [Database](Blob-Storing-Database.md): Stores BLOBs in a database.
* [Azure](Blob-Storing-Azure.md): Stores BLOBs on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/).
* [Aliyun](Blob-Storing-Aliyun.md): Stores BLOBs on the [Aliyun Storage Service](https://help.aliyun.com/product/31815.html).
* [Minio](Blob-Storing-Minio.md): Stores BLOBs on the [MinIO Object storage](https://min.io/).
* [Aws](Blob-Storing-Aws.md): Stores BLOBs on the [Amazon Simple Storage Service](https://aws.amazon.com/s3/).

More providers will be implemented by the time. You can [request](https://github.com/abpframework/abp/issues/new) it for your favorite provider or [create it yourself](Blob-Storing-Custom-Provider.md) and [contribute](Contribution/Index.md) to the ABP Framework.

Multiple providers **can be used together** by the help of the **container system**, where each container can uses a different provider.

> BLOB storing system can not work unless you **configure a storage provider**. Refer to the linked documents for the storage provider configurations.

## Installation

[Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) is the main package that defines the BLOB storing services. You can use this package to use the BLOB Storing system without depending a specific storage provider.

Use the ABP CLI to add this package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI), if you haven't installed it.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring` package.
* Run `abp add-package Volo.Abp.BlobStoring` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## The IBlobContainer

`IBlobContainer` is the main interface to store and read BLOBs. Your application may have multiple containers and each container can be separately configured. But, there is a **default container** that can be simply used by [injecting](Dependency-Injection.md) the `IBlobContainer`.

**Example: Simply save and read bytes of a named BLOB**

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

This service saves the given bytes with the `my-blob-1` name and then gets the previously saved bytes with the same name.

> A BLOB is a named object and **each BLOB should have a unique name**, which is an arbitrary string.

`IBlobContainer` can work with `Stream` and `byte[]` objects, which will be detailed in the next sections.

### Saving BLOBs

`SaveAsync` method is used to save a new BLOB or replace an existing BLOB. It can save a `Stream` by default, but there is a shortcut extension method to save byte arrays.

`SaveAsync` gets the following parameters:

* **name** (string): Unique name of the BLOB.
* **stream** (Stream) or **bytes** (byte[]): The stream to read the BLOB content or a byte array.
* **overrideExisting** (bool): Set `true` to replace the BLOB content if it does already exists. Default value is `false` and throws `BlobAlreadyExistsException` if there is already a BLOB in the container with the same name.

### Reading/Getting BLOBs

* `GetAsync`: Only gets a BLOB name and returns a `Stream` object that can be used to read the BLOB content. Always **dispose the stream** after using it. This method throws exception, if it can not find the BLOB with the given name.
* `GetOrNullAsync`: In opposite to the `GetAsync` method, this one returns `null` if there is no BLOB found with the given name.
* `GetAllBytesAsync`: Returns a `byte[]` instead of a `Stream`. Still throws exception if can not find the BLOB with the given name.
* `GetAllBytesOrNullAsync`: In opposite to the `GetAllBytesAsync` method, this one returns `null` if there is no BLOB found with the given name.

### Deleting BLOBs

`DeleteAsync` method gets a BLOB name and deletes the BLOB data. It doesn't throw any exception if given BLOB was not found. Instead, it returns a `bool` indicating that the BLOB was actually deleted or not, if you care about it.

### Other Methods

* `ExistsAsync` method simply checks if there is a BLOB in the container with the given name.

### About Naming the BLOBs

There is not a rule for naming the BLOBs. A BLOB name is just a string that is unique per container (and per tenant - see the "*Multi-Tenancy*" section). However, different storage providers may conventionally implement some practices. For example, the [File System Provider](Blob-Storing-File-System.md) use directory separators (`/`) and file extensions in your BLOB name (if your BLOB name is `images/common/x.png` then it is saved as `x.png` in the `images/common` folder inside the root container folder).

## Typed IBlobContainer

Typed BLOB container system is a way of creating and managing **multiple containers** in an application;

* **Each container is separately stored**. That means the BLOB names should be unique in a container and two BLOBs with the same name can live in different containers without effecting each other.
* **Each container can be separately configured**, so each container can use a different storage provider based on your configuration.

To create a typed container, you need to create a simple class decorated with the `BlobContainerName` attribute:

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

> If you don't use the `BlobContainerName` attribute, ABP Framework uses the full name of the class (with namespace), but it is always recommended to use a container name which is stable and does not change even if you rename the class.

Once you create the container class, you can inject `IBlobContainer<T>` for your container type.

**Example: An [application service](Application-Services.md) to save and read profile picture of the [current user](CurrentUser.md)**

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

`IBlobContainer<T>` has the same methods with the `IBlobContainer`.

> It is a good practice to **always use a typed container while developing re-usable modules**, so the final application can configure the provider for your container without effecting the other containers.

### The Default Container

If you don't use the generic argument and directly inject the `IBlobContainer` (as explained before), you get the default container. Another way of injecting the default container is using `IBlobContainer<DefaultContainer>`, which returns exactly the same container.

The name of the default container is `default`.

### Named Containers

Typed containers are just shortcuts for named containers. You can inject and use the `IBlobContainerFactory` to get a BLOB container by its name:

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

`IBlobContainerFactory` is the service that is used to create the BLOB containers. One example was shown above.

**Example: Create a container by name**

````csharp
var blobContainer = blobContainerFactory.Create("profile-pictures");
````

**Example: Create a container by type**

````csharp
var blobContainer = blobContainerFactory.Create<ProfilePictureContainer>();
````

> You generally don't need to use the `IBlobContainerFactory` since it is used internally, when you inject a `IBlobContainer` or `IBlobContainer<T>`.

## Configuring the Containers

Containers should be configured before using them. The most fundamental configuration is to **select a BLOB storage provider** (see the "*BLOB Storage Providers*" section above).

`AbpBlobStoringOptions` is the [options class](Options.md) to configure the containers. You can configure the options inside the `ConfigureServices` method of your [module](Module-Development-Basics.md).

### Configure a Single Container

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        //TODO...
    });
});
````

This example configures the `ProfilePictureContainer`. You can also configure by the container name:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure("profile-pictures", container =>
    {
        //TODO...
    });
});
````

### Configure the Default Container

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        //TODO...
    });
});
````

> There is a special case about the default container; If you don't specify a configuration for a container, it **fallbacks to the default container configuration**. This is a good way to configure defaults for all containers and specialize configuration for a specific container when needed.

### Configure All Containers

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureAll((containerName, containerConfiguration) =>
    {
        //TODO...
    });
});
````

This is a way to configure all the containers.

> The main difference from configuring the default container is that `ConfigureAll` overrides the configuration even if it was specialized for a specific container.

## Multi-Tenancy

If your application is set as multi-tenant, the BLOB Storage system **works seamlessly with the [multi-tenancy](Multi-Tenancy.md)**. All the providers implement multi-tenancy as a standard feature. They **isolate BLOBs** of different tenants from each other, so they can only access to their own BLOBs. It means you can use the **same BLOB name for different tenants**.

If your application is multi-tenant, you may want to control **multi-tenancy behavior** of the containers individually. For example, you may want to **disable multi-tenancy** for a specific container, so the BLOBs inside it will be **available to all the tenants**. This is a way to share BLOBs among all tenants.

**Example: Disable multi-tenancy for a specific container**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        container.IsMultiTenant = false;
    });
});
````

> If your application is not multi-tenant, no worry, it works as expected. You don't need to configure the `IsMultiTenant` option.

## Extending the BLOB Storing System

Most of the times, you won't need to customize the BLOB storage system except [creating a custom BLOB storage provider](Blob-Storing-Custom-Provider.md). However, you can replace any service (injected via [dependency injection](Dependency-Injection.md)), if you need. Here, some other services not mentioned above, but you may want to know:

* `IBlobProviderSelector` is used to get a `IBlobProvider` instance by a container name. Default implementation (`DefaultBlobProviderSelector`) selects the provider using the configuration.
* `IBlobContainerConfigurationProvider` is used to get the `BlobContainerConfiguration` for a given container name. Default implementation (`DefaultBlobContainerConfigurationProvider`) gets the configuration from the `AbpBlobStoringOptions` explained above.

## BLOB Storing vs File Management System

Notice that BLOB storing is not a file management system. It is a low level system that is used to save, get and delete named BLOBs. It doesn't provide a hierarchical structure like directories, you may expect from a typical file system.

If you want to create folders and move files between folders, assign permissions to files and share files between users then you need to implement your own application on top of the BLOB Storage system.

## See Also

* [Creating a custom BLOB storage provider](Blob-Storing-Custom-Provider.md)
