```json
//[doc-seo]
{
    "Description": "Learn how to encrypt BLOBs at rest with the BLOB pipeline in ABP Framework, using tenant-specific or global passphrases, and how to build custom pipeline contributors."
}
```

# BLOB Encryption & Pipeline

The BLOB Storing system provides a **pipeline** between the `IBlobContainer` and the storage provider. Pipeline contributors can transform the BLOB stream on save and on read, regardless of which [storage provider](../blob-storing) is configured. The most common use case is **encrypting BLOBs at rest**, which is provided out of the box. Compression, content validation and similar cross-cutting concerns can be implemented the same way.

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. This document covers the pipeline and the built-in encryption, which are part of the [Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) package; no additional package is needed.

## Enabling Encryption

Encryption is enabled **per container**, with the `UseEncryption` extension method:

**Example: Encrypt the BLOBs of a specific container**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        container.UseEncryption();
    });
});
````

**Example: Encrypt all containers by default**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.UseEncryption();
    });
});
````

Containers that don't enable encryption are not affected at all; there is no performance overhead for them.

## Resolving the Passphrase

When encryption is enabled, the passphrase is resolved in the following order:

1. **Container-specific passphrase**: If a passphrase is passed to the `UseEncryption` method, it is always used for that container:

````csharp
options.Containers.Configure<ProfilePictureContainer>(container =>
{
    container.UseEncryption("my-container-passphrase");
});
````

2. **Tenant-specific passphrase**: If the current tenant is available, the `Abp.BlobStoring.Encryption.TenantPassPhrase` setting is checked. This encrypted setting is restricted to the tenant setting provider, preventing a user-level value from changing the key for other users in the same tenant. You can set it per tenant, for example using the `ISettingManager`:

````csharp
await _settingManager.SetForTenantAsync(
    tenantId,
    "Abp.BlobStoring.Encryption.TenantPassPhrase",
    "tenant-secret-passphrase"
);
````

3. **Global passphrase**: The `AbpBlobStoringEncryptionOptions.DefaultPassPhrase` is used as the fallback (when there is no current tenant, or the tenant has no passphrase defined):

````csharp
Configure<AbpBlobStoringEncryptionOptions>(options =>
{
    options.DefaultPassPhrase = "my-global-passphrase";
});
````

If encryption is enabled but no passphrase can be resolved, an `AbpException` is thrown on save/read.

> Since the passphrase resolution is performed in the current tenant context, a multi-tenant container automatically encrypts each tenant's BLOBs with the tenant's own key (when defined), and falls back to the global key otherwise. If multi-tenancy is disabled for a container (`IsMultiTenant = false`), the global passphrase is used.

### Customizing the Passphrase Resolution

The passphrase resolution is implemented by the `IBlobEncryptionKeyProvider` service. The default implementation (`DefaultBlobEncryptionKeyProvider`) applies the rules above. You can [replace](../../fundamentals/dependency-injection.md) it with your own implementation to read the keys from another source, like a vault or a key management service (KMS):

````csharp
[Dependency(ReplaceServices = true)]
public class MyEncryptionKeyProvider : IBlobEncryptionKeyProvider
{
    public Task<string?> GetPassPhraseOrNullAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        // TODO: Resolve the passphrase from your own key store
    }
}
````

## The Encryption Format & Compatibility

* Encryption is performed with authenticated encryption (AEAD) by the `IByteArrayEncryptionService` (AES-256-GCM where available, AES-256-CBC + HMAC-SHA256 on .NET Standard 2.0), so tampered, corrupted, or truncated BLOBs are detected while reading. An authenticated terminal record protects the end of the ciphertext.
* The data is processed in chunks with **constant memory usage**, independent from the BLOB size.
* When the source stream exposes its length, the encrypted stream exposes its exact resulting length for providers that require the object size before uploading.
* Every encrypted BLOB starts with an `ABPE` magic header and a format version byte. BLOBs **without** this header (stored before the encryption was enabled) are returned as-is, so you can enable encryption on a container that already has BLOBs. New BLOBs are encrypted from that point on.

> Keep your passphrases safe. If the passphrase of a container is lost or changed, the BLOBs encrypted with it can not be decrypted anymore.

## Creating Custom Pipeline Contributors

The encryption itself is implemented as a pipeline contributor. You can write your own contributors by implementing the `IBlobPipelineContributor` interface:

**Example: A contributor that compresses BLOBs on save and decompresses on read**

````csharp
public class CompressionPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public Task<Stream> OnSaveAsync(BlobPipelineSaveArgs args)
    {
        // Return a stream that compresses args.BlobStream while being read
    }

    public Task<Stream> OnGetAsync(BlobPipelineGetArgs args)
    {
        // Return a stream that decompresses args.BlobStream while being read
    }
}
````

Then, add it to the container configuration:

````csharp
options.Containers.Configure<ProfilePictureContainer>(container =>
{
    container.PipelineContributors.Add(typeof(CompressionPipelineContributor));
    container.UseEncryption(); // Multiple contributors can be combined
});
````

Contributors are resolved from the [dependency injection](../../fundamentals/dependency-injection.md) and executed in the order they are added on save, and in the **reverse order** on read. So, the contributor added last is the first one to transform the stream back on read (in the example above, the BLOB is compressed first, then encrypted while saving; decrypted first, then decompressed while reading).

A few notes for implementers:

* Return the input stream as-is if your contributor has nothing to do for the given BLOB.
* Prefer **stream wrappers** over buffering the whole content in memory, so large BLOBs can be processed with constant memory usage.
* The contributors run inside the current tenant context and get the normalized container/BLOB names over the `args` parameter.

## See Also

* [BLOB Storing](../blob-storing)
* [Creating a custom BLOB storage provider](./custom-provider.md)
