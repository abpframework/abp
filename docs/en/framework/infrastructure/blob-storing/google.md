# BLOB Storing Google Provider

BLOB Storing Google Provider can store BLOBs in [Google Cloud Storage](https://cloud.google.com/storage).

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a Google Cloud Storage as the storage provider.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.Google](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Google) NuGet package to your project:

* Install the [ABP CLI](../../../cli) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.Google` package.
* Run `abp add-package Volo.Abp.BlobStoring.Google` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.Google](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Google) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringGoogleModule))]` to the [ABP module](../../architecture/modularity/basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](../../architecture/modularity/basics.md) class, as explained in the [BLOB Storing document](../blob-storing).

**Example: Configure to use the Google storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseGoogle(google =>
        {
            google.ClientEmail = "your coogle client email";
            google.ProjectId = "your coogle project id";
            google.PrivateKey = "your coogle private key";
            google.Scopes = "your coogle scopes";
            google.ContainerName = "your coogle container name";
            google.CreateContainerIfNotExists = true;
            //google.UseApplicationDefaultCredentials = true; // If you want to use application default credentials
        });
    });
});
````

> See the [BLOB Storing document](../blob-storing) to learn how to configure this provider for a specific container.

### Options

* **ClientEmail** (string): The client email of the Google service account. You can create a service account and get the client email. Please refer to Google documentation: https://cloud.google.com/iam/docs/service-account-overview
* **ProjectId** (string): The project ID of the Google service account.
* **PrivateKey** (string): The private key of the Google service account. You can create a service account and get the private key. Please refer to Google documentation: https://cloud.google.com/iam/docs/keys-create-delete
* **Scopes** (string): The scopes of the Google service account.
* **UseApplicationDefaultCredentials** (bool): If `true`, it uses the application default credentials(ADC). Default value is `false`. Please refer to Google documentation: https://cloud.google.com/docs/authentication/provide-credentials-adc
* **ContainerName** (string): You can specify the container name in Google. If this is not specified, it uses the name of the BLOB container defined with the `BlobContainerName` attribute (see the [BLOB storing document](../blob-storing)). Please note that Google has some **rules for naming containers**. A container name must be a valid DNS name, conforming to the [following naming rules](https://docs.microsoft.com/en-us/rest/api/storageservices/naming-and-referencing-containers--blobs--and-metadata#container-names):
    * Container names must start or end with a letter or number, and can contain only letters, numbers, and the dash (-) character.
    * Every dash (-) character must be immediately preceded and followed by a letter or number; consecutive dashes are not permitted in container names.
    * All letters in a container name must be **lowercase**.
    * Container names must be from **3** through **63** characters long.
* **CreateContainerIfNotExists** (bool): Default value is `false`, If a container does not exist in Google, `GoogleBlobProvider` will try to create it.


## Google Blob Name Calculator

Google Blob Provider organizes BLOB name and implements some conventions. The full name of a BLOB is determined by the following rules by default:

* Appends `host` string if [current tenant](../../architecture/multi-tenancy) is `null` (or multi-tenancy is disabled for the container - see the [BLOB Storing document](../blob-storing) to learn how to disable multi-tenancy for a container).
* Appends `tenants/<tenant-id>` string if current tenant is not `null`.
* Appends the BLOB name.

## Other Services

* `GoogleBlobProvider` is the main service that implements the Google BLOB storage provider, if you want to override/replace it via [dependency injection](../../fundamentals/dependency-injection.md) (don't replace `IBlobProvider` interface, but replace `GoogleBlobProvider` class).
* `IGoogleBlobNameCalculator` is used to calculate the full BLOB name (that is explained above). It is implemented by the `DefaultGoogleBlobNameCalculator` by default.
