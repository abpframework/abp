# BLOB Storing Bunny Provider

BLOB Storing Bunny Provider can store BLOBs in [bunny.net Storage](https://bunny.net/storage/).

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a Bunny BLOB as the storage provider.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.Bunny](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Bunny) NuGet package to your project:

* Install the [ABP CLI](../../../cli) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.Bunny` package.
* Run `abp add-package Volo.Abp.BlobStoring.Bunny` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.Bunny](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Bunny) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringBunnyModule))]` to the [ABP module](../../architecture/modularity/basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](../../architecture/modularity/basics.md) class, as explained in the [BLOB Storing document](../blob-storing).

**Example: Configure to use the Bunny storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseBunny(Bunny =>
        {
            Bunny.AccessKey = "your Bunny account access key";
            Bunny.Region = "the code of the main storage zone region";  // "de" is the default value
            Bunny.ContainerName = "your bunny storage zone name";
            Bunny.CreateContainerIfNotExists = true;
        });
    });
});

````

> See the [BLOB Storing document](../blob-storing) to learn how to configure this provider for a specific container.

### Options

* **AccessKey** (string): Bunny Account Access Key. [Where do I find my Access key?](https://support.bunny.net/hc/en-us/articles/360012168840-Where-do-I-find-my-API-key)
* **Region** (string?): The code of the main storage zone region (Possible values: DE, NY, LA, SG).
* **ContainerName** (string): You can specify the container name in Bunny. If this is not specified, it uses the name of the BLOB container defined with the `BlobContainerName` attribute (see the [BLOB storing document](../blob-storing)). Please note that Bunny has some **rules for naming containers**:
    * Storage Zone names must be a globaly unique.
    * Storage Zone names must be between **4** and **64** characters long.
    * Storage Zone names can consist only of **lowercase** letters, numbers, and hyphens (-).
* **CreateContainerIfNotExists** (bool): Default value is `false`, If a container does not exist in Bunny, `BunnyBlobProvider` will try to create it.

## Bunny Blob Name Calculator

Bunny Blob Provider organizes BLOB name and implements some conventions. The full name of a BLOB is determined by the following rules by default:

* Appends `host` string if [current tenant](../../architecture/multi-tenancy) is `null` (or multi-tenancy is disabled for the container - see the [BLOB Storing document](../blob-storing) to learn how to disable multi-tenancy for a container).
* Appends `tenants/<tenant-id>` string if current tenant is not `null`.
* Appends the BLOB name.

## Other Services

* `BunnyBlobProvider` is the main service that implements the Bunny BLOB storage provider, if you want to override/replace it via [dependency injection](../../fundamentals/dependency-injection.md) (don't replace `IBlobProvider` interface, but replace `BunnyBlobProvider` class).
* `IBunnyBlobNameCalculator` is used to calculate the full BLOB name (that is explained above). It is implemented by the `DefaultBunnyBlobNameCalculator` by default.
* `IBunnyClientFactory` is implemented by `DefaultBunnyClientFactory` by default. You can override/replace it,if you want customize.
