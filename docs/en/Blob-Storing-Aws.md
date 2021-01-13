# BLOB Storing Aws Provider

BLOB Storing Aws Provider can store BLOBs in [Amazon Simple Storage Service](https://aws.amazon.com/s3/).

> Read the [BLOB Storing document](Blob-Storing.md) to understand how to use the BLOB storing system. This document only covers how to configure containers to use a Aws BLOB as the storage provider.

## Installation

Use the ABP CLI to add [Volo.Abp.BlobStoring.Aws](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Aws) NuGet package to your project:

* Install the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) if you haven't installed before.
* Open a command line (terminal) in the directory of the `.csproj` file you want to add the `Volo.Abp.BlobStoring.Aws` package.
* Run `abp add-package Volo.Abp.BlobStoring.Aws` command.

If you want to do it manually, install the [Volo.Abp.BlobStoring.Aws](https://www.nuget.org/packages/Volo.Abp.BlobStoring.Aws) NuGet package to your project and add `[DependsOn(typeof(AbpBlobStoringAwsModule))]` to the [ABP module](Module-Development-Basics.md) class inside your project.

## Configuration

Configuration is done in the `ConfigureServices` method of your [module](Module-Development-Basics.md) class, as explained in the [BLOB Storing document](Blob-Storing.md).

**Example: Configure to use the Aws storage provider by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseAws(Aws =>
        {
            Aws.AccessKeyId = "your Aws access key id";
            Aws.SecretAccessKey = "your Aws access key secret";
            Aws.UseCredentials = "set true to use credentials";
            Aws.UseTemporaryCredentials = "set true to use temporary credentials";
            Aws.UseTemporaryFederatedCredentials = "set true to use temporary federated credentials";
            Aws.ProfileName = "the name of the profile to get credentials from";
            Aws.ProfilesLocation = "the path to the aws credentials file to look at";
            Aws.Region = "the system name of the service";
            Aws.Name = "the name of the federated user";
            Aws.Policy = "policy";
            Aws.DurationSeconds = "expiration date";
            Aws.ContainerName = "your Aws container name";
            Aws.CreateContainerIfNotExists = false;
        });
    });
});

````

> See the [BLOB Storing document](Blob-Storing.md) to learn how to configure this provider for a specific container.

### Options

* **AccessKeyId** (string): AWS Access Key ID.
* **SecretAccessKey** (string): AWS Secret Access Key.
* **UseCredentials** (bool): Use [credentials](https://docs.aws.amazon.com/AmazonS3/latest/dev/AuthUsingAcctOrUserCredentials.html) to access AWS services,default : `false`.
* **UseTemporaryCredentials** (bool): Use [temporary credentials](https://docs.aws.amazon.com/AmazonS3/latest/dev/AuthUsingTempSessionToken.html) to access AWS services,default : `false`.
* **UseTemporaryFederatedCredentials** (bool): Use [federated user temporary credentials](https://docs.aws.amazon.com/AmazonS3/latest/dev/AuthUsingTempFederationToken.html) to access AWS services, default : `false`.
* **ProfileName** (string): The [name of the profile]((https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html)) to get credentials from.
* **ProfilesLocation** (string): The path to the aws credentials file to look at.
* **Region** (string): The system name of the service.
* **Policy** (string): An IAM policy in JSON format that you want to use as an inline session policy.
* **DurationSeconds** (int): Validity period(s) of a temporary access certificate,minimum is 900 and the maximum is 3600. **note**: Using subaccounts operated OSS,if the value is 0.
* **ContainerName** (string): You can specify the container name in Aws. If this is not specified, it uses the name of the BLOB container defined with the `BlobContainerName` attribute (see the [BLOB storing document](Blob-Storing.md)). Please note that Aws has some **rules for naming containers**. A container name must be a valid DNS name, conforming to the [following naming rules](https://docs.aws.amazon.com/AmazonS3/latest/dev/BucketRestrictions.html):
    * Bucket names must be between **3** and **63** characters long.
    * Bucket names can consist only of **lowercase** letters, numbers, dots (.), and hyphens (-).
    * Bucket names must begin and end with a letter or number.
    * Bucket names must not be formatted as an IP address (for example, 192.168.5.4).
    * Bucket names can't begin with **xn--** (for buckets created after February 2020).
    * Bucket names must be unique within a partition. 
    * Buckets used with Amazon S3 Transfer Acceleration can't have dots (.) in their names. For more information about transfer acceleration, see Amazon S3 Transfer Acceleration.
* **CreateContainerIfNotExists** (bool): Default value is `false`, If a container does not exist in Aws, `AwsBlobProvider` will try to create it.

## Aws Blob Name Calculator

Aws Blob Provider organizes BLOB name and implements some conventions. The full name of a BLOB is determined by the following rules by default:

* Appends `host` string if [current tenant](Multi-Tenancy.md) is `null` (or multi-tenancy is disabled for the container - see the [BLOB Storing document](Blob-Storing.md) to learn how to disable multi-tenancy for a container).
* Appends `tenants/<tenant-id>` string if current tenant is not `null`.
* Appends the BLOB name.

## Other Services

* `AwsBlobProvider` is the main service that implements the Aws BLOB storage provider, if you want to override/replace it via [dependency injection](Dependency-Injection.md) (don't replace `IBlobProvider` interface, but replace `AwsBlobProvider` class).
* `IAwsBlobNameCalculator` is used to calculate the full BLOB name (that is explained above). It is implemented by the `DefaultAwsBlobNameCalculator` by default.
* `IAmazonS3ClientFactory` is used create OSS client. It is implemented by the `DefaultAmazonS3ClientFactory` by default. You can override/replace it,if you want customize.