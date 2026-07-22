```json
//[doc-seo]
{
    "Description": "Learn how to encrypt BLOBs at rest in ABP Framework, using container-specific, tenant-specific or global passphrases."
}
```

# BLOB Encryption

The BLOB Storing system can **encrypt BLOBs at rest**, transparently, on top of the configured [storage provider](../blob-storing): the BLOB stream is encrypted (AES-256-GCM, authenticated) before it reaches the provider and decrypted while it is read back, so the providers themselves need no changes. The combination is covered by automated tests for the File System provider; other providers consume the same standard stream contract, but validate your provider setup before relying on it in production.

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. The encryption is part of the [Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) package; no additional package is needed. It requires a platform with AES-GCM support; it is not available on .NET Standard 2.0 targets (like .NET Framework).

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

    // A single container can still opt out:
    options.Containers.Configure<PublicPictureContainer>(container =>
    {
        container.DisableEncryption();
    });
});
````

Containers that don't enable encryption are not affected at all.

## Resolving the Passphrase

When encryption is enabled, the passphrase for a **new** BLOB is resolved in the following order:

1. **Container-specific passphrase**: If a passphrase is passed to the `UseEncryption` method, it is always used for that container. Calling `UseEncryption()` again without parameters keeps the configured values, so multiple modules can safely compose the configuration; use `ClearEncryptionPassPhrase()` to remove a configured or inherited container passphrase:

````csharp
options.Containers.Configure<ProfilePictureContainer>(container =>
{
    container.UseEncryption("my-container-passphrase");
});
````

2. **Global passphrase**: The `AbpBlobStoringEncryptionOptions.DefaultPassPhrase` is used as the fallback:

````csharp
Configure<AbpBlobStoringEncryptionOptions>(options =>
{
    options.DefaultPassPhrase = "my-global-passphrase";
});
````

If encryption is enabled but no passphrase can be resolved, saving and reading encrypted BLOBs fails with an `AbpException` (on .NET Standard 2.0 targets a `PlatformNotSupportedException` is thrown before that, see above).

> Treat passphrases as production secrets: read them from your configuration/secret store instead of hard-coding them, and prefer long, machine-generated values.

The **source** of the passphrase is recorded in the encrypted BLOB, and only that source is used while decrypting it. So, for example, a BLOB written with the global passphrase stays readable after a container-specific passphrase is configured later.

> Keep your passphrases safe. If the passphrase a BLOB was encrypted with is lost or changed, that BLOB can not be decrypted anymore.

### Customizing the Passphrase Resolution

The passphrase resolution is implemented by the `IBlobEncryptionKeyProvider` service. The default implementation (`DefaultBlobEncryptionKeyProvider`) applies the rules above. You can [replace](../../fundamentals/dependency-injection.md) it with your own implementation to read the passphrases from another source, like a vault or another secret store (the provider must be able to return the passphrase itself; hardware-backed non-exportable keys are not supported).

A custom provider can also supply **tenant-specific** passphrases: return `BlobEncryptionKeySource.Tenant` while encrypting and resolve the same tenant's passphrase when it is requested for decryption. The key source recorded in the BLOB header routes each BLOB back to the provider that can decrypt it. The following implementation gives every tenant its own passphrase and keeps the standard rules for the host side:

````csharp
public class MyTenantBlobEncryptionKeyProvider : DefaultBlobEncryptionKeyProvider
{
    protected ICurrentTenant CurrentTenant { get; }

    public MyTenantBlobEncryptionKeyProvider(
        ICurrentTenant currentTenant,
        IOptions<AbpBlobStoringEncryptionOptions> options)
        : base(options)
    {
        CurrentTenant = currentTenant;
    }

    public override async Task<BlobEncryptionKey> ResolveForEncryptionAsync(
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        // Keep a container-specific passphrase as the highest-priority source
        var containerPassPhrase = GetContainerPassPhraseOrNull(configuration);
        if (string.IsNullOrWhiteSpace(containerPassPhrase) && CurrentTenant.Id.HasValue)
        {
            return new BlobEncryptionKey(
                BlobEncryptionKeySource.Tenant,
                await GetTenantPassPhraseAsync(CurrentTenant.Id.Value, cancellationToken)
            );
        }

        return await base.ResolveForEncryptionAsync(configuration, cancellationToken);
    }

    public override async Task<string> ResolveForDecryptionAsync(
        BlobEncryptionKeySource keySource,
        BlobContainerConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        if (keySource == BlobEncryptionKeySource.Tenant)
        {
            if (!CurrentTenant.Id.HasValue)
            {
                throw new AbpException(
                    "The BLOB was encrypted with a tenant-specific passphrase, " +
                    "but there is no current tenant!");
            }

            return await GetTenantPassPhraseAsync(CurrentTenant.Id.Value, cancellationToken);
        }

        return await base.ResolveForDecryptionAsync(keySource, configuration, cancellationToken);
    }

    private async Task<string> GetTenantPassPhraseAsync(
        Guid tenantId, CancellationToken cancellationToken)
    {
        /* Read the tenant's passphrase from your secret store.
           It must return the same value for the lifetime of the tenant's BLOBs. */
    }
}
````

Notes on this pattern:

* The multi-tenant BLOB containers already isolate tenants physically (see the [BLOB Storing document](../blob-storing)); tenant-specific passphrases add **cryptographic** isolation on top: one tenant's BLOBs can not be decrypted with another tenant's (or the host's) passphrase, and the tenant identity is part of the authenticated data.
* Decryption runs in the tenant context of the BLOB's owner (the container itself switches to the right tenant), so resolving by `CurrentTenant.Id` is correct for both saving and reading.
* Only enable such a provider on containers with `IsMultiTenant = true` (the default). On a shared (`IsMultiTenant = false`) container all tenants read the same BLOBs, so a per-tenant passphrase would make a BLOB readable only by the tenant that happened to write it.

## BLOBs Stored Before Enabling Encryption

By default, reading a BLOB that does not have the encrypted format fails, so a tampered or corrupted BLOB can not silently bypass the authenticity check. If a container already has plaintext BLOBs from before encryption was enabled, allow reading them explicitly:

````csharp
options.Containers.Configure<ProfilePictureContainer>(container =>
{
    container.UseEncryption(allowLegacyPlaintext: true);
});
````

With this option, content without the encrypted format header is returned as-is, **without any authenticity check** — including an encrypted BLOB whose format header got corrupted or tampered. Treat it as a short-term migration switch: new BLOBs are always encrypted, and the option should be disabled once the existing BLOBs are migrated (re-saved).

A typical migration of an existing container:

1. Enable encryption with `UseEncryption(allowLegacyPlaintext: true)` and deploy. New and updated BLOBs are written encrypted; the existing plaintext BLOBs stay readable.
2. Re-save the existing BLOBs (the BLOB storing system has no list operation, so iterate the BLOB names from your own application data):

````csharp
var bytes = await container.GetAllBytesAsync(blobName);
await container.SaveAsync(blobName, bytes, overrideExisting: true);
````

3. Remove the `allowLegacyPlaintext` option, so reading fails closed again for any content that does not have the encrypted format.

> Legacy plaintext content that itself starts with the `ABPE` format magic can not be distinguished from an encrypted BLOB and fails to be read. Re-save such content once to encrypt it. Also note that legacy BLOBs are returned over a non-seekable wrapper stream while this option is enabled (the `Length` stays available when the provider stream knows it).

## Changing a Passphrase

The format does not support key rotation: a BLOB is only readable with the exact passphrase it was written with, and there is no way to keep an old and a new passphrase of the **same source** active at the same time. So changing a passphrase in place makes the BLOBs written with the old one permanently unreadable — migrate the content **before** the change:

* **From the global to a container-specific passphrase**: this direction works without downtime, because the two are different key sources. Configure the new container passphrase; BLOBs recorded with the `Global` source keep decrypting with `DefaultPassPhrase`, while new saves use the container passphrase. Re-save the existing BLOBs (as in the migration steps above) to move them to the new passphrase; the global one can be retired once no BLOB uses it anymore.
* **Any other change**: while the old passphrase is still configured, read the BLOBs and re-save them into a container using a different key source (or export them to a safe location), then apply the change and save them back. Verify the migrated BLOBs are readable before deleting anything.

## Behavioral Changes for Encrypted Containers

* The stream returned for an encrypted BLOB is read-only and non-seekable, and its `Length` is not available; read it sequentially (for example with `CopyToAsync`).
* Opening a BLOB throws an `AbpException` when the content does not have a valid encrypted format; a `CryptographicException` is thrown **while reading** when the content fails authentication (tampered data or a wrong passphrase).
* Each returned chunk is individually authenticated, but the completeness of the whole BLOB (protection against cutting whole chunks off the end) is only verified when the stream is read to its end.
* The file system provider writes an encrypted BLOB in a single attempt (the encrypting stream can not be replayed for I/O retries): a failed save throws, and any partially written content fails closed while reading instead of being returned as damaged data.

## Performance and Cost

Deriving the encryption key from the passphrase is intentionally expensive (PBKDF2-SHA256), so leaked storage can not be brute-forced cheaply. Understand the cost profile before enabling encryption on hot containers:

* One key derivation runs on **every BLOB save** and on **every encrypted BLOB open** (before the stream is returned). The cost does not depend on the BLOB size — it scales with the number of operations, so many small, frequently read BLOBs amplify it the most.
* Every BLOB uses its own random salt, so derivation results can not be cached or reused; reading the same BLOB again derives the key again.
* The default iteration count is 100,000 (tens of milliseconds of CPU per operation, hardware dependent). Measure on your target hardware and concurrency before enabling encryption on high-frequency containers — it is not a microsecond-level transparent overhead.
* Use a long, machine-generated (at least 128 bits of entropy) value from your secret store as the passphrase in production. For low-entropy, human-chosen passphrases you can raise the iteration count — this increases the offline guessing cost and the per-operation CPU cost by the same factor:

````csharp
Configure<AbpBlobStoringEncryptionOptions>(options =>
{
    options.KdfIterations = 600_000; // allowed range: 100,000 - 600,000
});
````

Changing the iteration count only affects newly written BLOBs; existing BLOBs are decrypted with the count recorded in their own header.

## The Encryption Format

* Encryption is authenticated (AES-256-GCM): modified, re-ordered, corrupted or truncated content of a BLOB is detected while reading.
* Every encrypted BLOB is bound to its storage identity (the *normalized* container name, BLOB name and tenant). Copying or renaming an encrypted BLOB at the storage level makes it unreadable at the new location, which also makes substituting one (validly encrypted) BLOB for another detectable. Re-writing an older version of the same BLOB back to its own location is not detectable at this layer.
* Because of the identity binding, the following otherwise-legal operations make existing encrypted BLOBs permanently unreadable: changing the `IsMultiTenant` value of the container, moving BLOBs between tenants or containers, and switching to a storage provider that normalizes container/BLOB names differently (for example, providers that lowercase container names). Decrypt (re-save) the BLOBs before such a change.
* The data is processed in chunks with **constant memory usage**, independent from the BLOB size.
* Every BLOB is encrypted with its own key, derived (PBKDF2-SHA256) from the passphrase and a random per-BLOB salt.
* When the source stream exposes its length, the encrypted stream exposes its exact resulting length for providers that require the object size before uploading.

### What Is (Not) Protected

* Only the BLOB **content** is encrypted. Container names, BLOB names and any provider-level metadata stay in plaintext, so the existence and the approximate size of a BLOB remain visible in the storage.
* The size overhead is small and deterministic: a 39-byte prefix, plus 20 bytes per 64 KB chunk, plus a 20-byte end-of-stream record (about 0.03% for large BLOBs).
* Server-side encryption offered by the storage provider (like S3 or Azure Storage encryption) is complementary, not redundant: it uses provider-managed keys at the storage layer, while this feature encrypts with application-managed passphrases before the content leaves your application. They can be combined for defense in depth.

## Troubleshooting

| Error | Cause and solution |
|---|---|
| `AbpException`: *...is not in the encrypted format* | The BLOB was saved before encryption was enabled (or by an application without encryption). Use `allowLegacyPlaintext: true` during the migration. |
| `AbpException`: *...no passphrase could be resolved* | Encryption is enabled, but neither a container passphrase nor `DefaultPassPhrase` is configured. |
| `AbpException`: *...the default key provider does not supply tenant keys* | The BLOB was encrypted by a custom key provider with a tenant-specific passphrase; the same provider must be registered to read it back. |
| `AbpException`: *...that passphrase is not available anymore* | The passphrase of the key source recorded in the BLOB was removed or cleared from the configuration. Restore it. |
| `CryptographicException` while reading | Wrong passphrase, tampered/corrupted content, or the BLOB was copied, renamed or moved across containers/tenants at the storage level (see the identity binding above). |
| `PlatformNotSupportedException` | The application runs on .NET Standard 2.0 (like .NET Framework) or on a platform without AES-GCM support. |

## See Also

* [BLOB Storing](../blob-storing)
* [Creating a custom BLOB storage provider](./custom-provider.md)
