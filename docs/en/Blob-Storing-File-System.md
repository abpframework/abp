# BLOB Storing File System Provider

File System Storage Provider is used to store BLOBs in the local file system as standard files inside a folder.

> Read the [BLOB Storing document](Blob-Storing.md) to understand how to use the BLOB storing system. This document only covers how to configure containers to use the file system.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.FileSystem](https://www.nuget.org/packages/Volo.Abp.BlobStoring.FileSystem) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.FileSystem` package.
* Run `abp add-package Volo.Abp.BlobStoring.FileSystem` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.FileSystem](https://www.nuget.org/packages/Volo.Abp.BlobStoring.FileSystem) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringFileSystemModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](Module-Development-Basics.md) class, as explained in the [BLOB Storing document](Blob-Storing.md).

**Example: Configure to use the File System storage provider by default**

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

`UseFileSystem` extension method is used to set the File System Provider for a container and configure the file system options.

> See the [BLOB Storing document](Blob-Storing.md) to learn how to configure this provider for a specific container.

### Options

* **BasePath** (string): The base folder path to store BLOBs. It is required to set this option.
* **AppendContainerNameToBasePath** (bool; default: `true`): Indicates whether to create a folder with the container name inside the base folder. If you store multiple containers in the same `BaseFolder`, leave this as `true`. Otherwise, you can set it to `false` if you don't like an unnecessarily deeper folder hierarchy.

## File Path Calculation

File System Provider organizes BLOB files inside folders and implements some conventions. The full path of a BLOB file is determined by the following rules by default:

* It starts with the `BasePath` configured as shown above.
* Appends `host` folder if [current tenant](Multi-Tenancy.md) is `null` (or multi-tenancy is disabled for the container - see the [BLOB Storing document](Blob-Storing.md) to learn how to disable multi-tenancy for a container).
* Appends `tenants/<tenant-id>` folder if current tenant is not `null`.
* Appends the container's name if `AppendContainerNameToBasePath` is `true`. If container name contains `/`, this will result with nested folders.
* Appends the BLOB name. If the BLOB name contains `/` it creates folders. If the BLOB name contains `.` it will have a file extension.

## Extending the File System BLOB Provider

* `FileSystemBlobProvider` is the main service that implements the File System storage. You can inherit from this class and [override](Customizing-Application-Modules-Overriding-Services.md) methods to customize it.

* The `IBlobFilePathCalculator` service is used to calculate the file paths. Default implementation is the `DefaultBlobFilePathCalculator`. You can replace/override it if you want to customize the file path calculation.