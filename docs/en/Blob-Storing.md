# BLOB Storing

It is typical to **store file contents** in an application and read these file contents on need. Not only files, but you may also need to save various types of **large binary objects**, a.k.a. **[BLOB](https://en.wikipedia.org/wiki/Binary_large_object)** into a **storage**. For example, you may want to save user profile pictures.

A BLOB is typically a **byte array**. Storing a BLOB in the local file system, in a shared database or on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/) can be an option.

ABP Framework provides an abstraction to work with BLOBs and provides some pre-built storage providers that you can easily integrate to. Having such an abstraction has some benefits;

* You can **easily integrate** to your favorite BLOB storage provides with a few lines of configuration.
* You can then **easily change** your BLOB storage without changing your application code.
* If you want to create **reusable application modules**, you don't need to make assumption about how the BLOBs are stored.

### BLOB Storage Providers

ABP Framework has the following storage provider implementations;

* [File System](Blob-Storing-File-System.md): Stores BLOBs in a folder of the local file system, as standard files.
* [Database](Blob-Storing-Database.md): Stores BLOBs in a database.
* [Azure](Blob-Storing-Azure.md): Stores BLOBs on the [Azure BLOB storage](https://azure.microsoft.com/en-us/services/storage/blobs/).

More providers will be implemented by the time. You can [request](https://github.com/abpframework/abp/issues/new) it for your favorite provider or [contribute](Contribution/Index.md) it yourself (see the "*Implementing Your Own BLOB Storage Provider*" section).

Multiple providers **can be used together** by the help of the **container system**, where each container can use a different provider (will be explained below).

> BLOB storing system can not work unless you **configure a storage provider**. Refer to the linked documents for the storage providers.

## Volo.Abp.BlobStoring Package

[Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) is the main package defines the BLOB storing services. You can use this package to use the BLOB Storing system without depending a specific storage provider.

### Installation

Use the ABP CLI to add this package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring` package.
* Run `abp add-package Volo.Abp.BlobStoring` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

### The IBlobContainer

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
        
        public async Task<byte[]> GetBytesAsync(byte[] bytes)
        {
            return await _blobContainer.GetAllBytesOrNullAsync("my-blob-1");
        }
    }
}
````

This service saves given bytes with the `my-blob-1` name and then reads the previously saved bytes with the same name.

> A BLOB is a named object and **each BLOB should have a unique name**, which is an arbitrary string.

#### Saving BLOBs

TODO

#### Reading/Getting BLOBs

#### Deleting BLOBs

#### Other Methods

## Implementing Your Own BLOB Storage Provider

TODO