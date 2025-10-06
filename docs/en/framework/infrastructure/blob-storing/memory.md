# BLOB Storing Memory Provider

Memory Storage Provider is used to store BLOBs in the memory. This is mainly used for unit testing purposes.

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use the memory.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.Memory](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Memory) NuGet package to your project:

* Install the [ABP CLI](../../../cli) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.Memory` package.
* Run `abp add-package Volo.Abp.BlobStoring.Memory` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.Memory](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Memory) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringMemoryModule))]` to the [ABP module](../../architecture/modularity/basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](../../architecture/modularity/basics.md) class, as explained in the [BLOB Storing document](../blob-storing).

**Example: Configure to use the Memory storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMemory();
    });
});
````

`UseMemory` extension method is used to set the Memory Provider for a container.

> See the [BLOB Storing document](../blob-storing) to learn how to configure this provider for a specific container.
