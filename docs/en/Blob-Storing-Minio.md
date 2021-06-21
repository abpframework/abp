# BLOB Storing Minio Provider

BLOB Storing Minio Provider can store BLOBs in [MinIO Object storage](https://min.io/).

> Read the [BLOB Storing document](Blob-Storing.md) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a Minio BLOB as the storage provider.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.Minio](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Minio) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.Minio` package.
* Run `abp add-package Volo.Abp.BlobStoring.Minio` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.Minio](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Minio) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringMinioModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](Module-Development-Basics.md) class, as explained in the [BLOB Storing document](Blob-Storing.md).

**Example: Configure to use the minio storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseMinio(minio =>
        {
            minio.EndPoint = "your minio endPoint";
            minio.AccessKey = "your minio accessKey";
            minio.SecretKey = "your minio secretKey";
            minio.BucketName = "your minio bucketName";
        });
    });
});
````

> See the [BLOB Storing document](Blob-Storing.md) to learn how to configure this provider for a specific container.

### Options

* **EndPoint** (string): URL to object storage service. Please refer to MinIO Client SDK for .NET: https://docs.min.io/docs/dotnet-client-quickstart-guide.html
* **AccessKey** (string): Access key is the user ID that uniquely identifies your account. 
* **SecretKey** (string): Secret key is the password to your account.
* **BucketName** (string): You can specify the bucket name in MinIO. If this is not specified, it uses the name of the BLOB container defined with the `BlobContainerName` attribute (see the [BLOB storing document](Blob-Storing.md)).MinIO is the defacto standard for S3 compatibility, So MinIO has some **rules for naming bucket**.  The [following rules](https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html) apply for naming MinIO buckets:
    * Bucket names must be between **3** and **63** characters long.
    * Bucket names can consist only of **lowercase** letters, numbers, dots (.), and hyphens (-).
    * Bucket names must begin and end with a letter or number.
    * Bucket names must not be formatted as an IP address (for example, 192.168.5.4).
    * Bucket names can't begin with **xn--** (for buckets created after February 2020).
    * Bucket names must be unique within a partition. 
    * Buckets used with Amazon S3 Transfer Acceleration can't have dots (.) in their names. For more information about transfer acceleration, see Amazon S3 Transfer Acceleration.
* **WithSSL** (bool): Default value is `false`,Chain to MinIO Client object to use https instead of http.
* **CreateContainerIfNotExists** (bool): Default value is `false`, If a bucket does not exist in minio, `MinioBlobProvider` will try to create it.


## Minio Blob Name Calculator

Minio Blob Provider organizes BLOB name and implements some conventions. The full name of a BLOB is determined by the following rules by default:

* Appends `host` string if [current tenant](Multi-Tenancy.md) is `null` (or multi-tenancy is disabled for the container - see the [BLOB Storing document](Blob-Storing.md) to learn how to disable multi-tenancy for a container).
* Appends `tenants/<tenant-id>` string if current tenant is not `null`.
* Appends the BLOB name.

## Other Services

* `MinioBlobProvider` is the main service that implements the Minio BLOB storage provider, if you want to override/replace it via [dependency injection](Dependency-Injection.md) (don't replace `IBlobProvider` interface, but replace `MinioBlobProvider` class).
* `IMinioBlobNameCalculator` is used to calculate the full BLOB name (that is explained above). It is implemented by the `DefaultMinioBlobNameCalculator` by default.
