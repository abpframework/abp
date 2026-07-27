```json
//[doc-seo]
{
    "Description": "Learn how to transform BLOB content transparently (compression, validation, watermarking...) with pipeline contributors in ABP Framework."
}
```

# BLOB Content Pipeline

The BLOB Storing system can pass the BLOB content through a **pipeline of contributors** while it is saved and read. A contributor transforms the content stream transparently, on top of the configured [storage provider](../blob-storing): compression, watermarking, content validation or any other stream transformation can be implemented without changing the storage provider or the application code that works with `IBlobContainer`.

> Read the [BLOB Storing document](../blob-storing) to understand how to use the BLOB storing system. The pipeline is part of the [Volo.Abp.BlobStoring](https://www.nuget.org/packages/Volo.Abp.BlobStoring) package; no additional package is needed.

## Creating a Pipeline Contributor

A pipeline contributor implements the `IBlobPipelineContributor` interface. The following example compresses the BLOBs with GZip:

````csharp
public class GZipBlobPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public async Task OnSavingAsync(BlobPipelineContext context)
    {
        var compressedStream = new MemoryStream();
        try
        {
            using (var gzipStream = new GZipStream(compressedStream, CompressionLevel.Fastest, leaveOpen: true))
            {
                await context.BlobStream.CopyToAsync(gzipStream, context.CancellationToken);
            }
        }
        catch
        {
            // A stream is only tracked for disposal by the pipeline once it is assigned
            // to context.BlobStream, so dispose it here if the eager work fails first
            compressedStream.Dispose();
            throw;
        }

        compressedStream.Position = 0;
        context.BlobStream = compressedStream;
    }

    public Task OnGettingAsync(BlobPipelineContext context)
    {
        context.BlobStream = new GZipStream(context.BlobStream, CompressionMode.Decompress);
        return Task.CompletedTask;
    }
}
````

* `OnSavingAsync` is called before the BLOB reaches the storage provider. Replace `context.BlobStream` with the transformed content; it is also allowed to materialize the content eagerly, like the example does (a lazily transforming, read-only wrapper keeps the memory usage constant instead, which is preferable for large BLOBs).
* `OnGettingAsync` is called after the BLOB was read from the storage provider, in the reverse direction of `OnSavingAsync`.
* The `BlobPipelineContext` also provides the normalized container/BLOB names, the container configuration, the tenant id and a scoped `ServiceProvider`. Contributors are resolved from the [dependency injection](../../fundamentals/dependency-injection.md) system (register them like any other service, for example with `ITransientDependency`). While saving, the scope stays alive until the save operation completes; while getting, until the stream returned to the caller is disposed.

### The Stream Ownership Contract

* If a stream (or the DI scope) fails to dispose **after** the storage provider already saved the BLOB, `SaveAsync` still throws that cleanup error even though the data is committed — a retry with the default `overrideExisting: false` would then get a `BlobAlreadyExistsException`.
* **While saving**, do not dispose the stream you received (notice the `leaveOpen: true` in the example): every stream you assign to `context.BlobStream` is disposed after the save, while the original stream stays owned by the caller. A stream is only tracked from the moment it is assigned, so if you create a stream and then do work that may fail (like the eager copy above) before assigning it, dispose it yourself on the failure path.
* **While getting**, the stream you set must dispose the stream you received when it is disposed (a `GZipStream` already does that by default), because the composed stream is returned to the caller as a whole.

## Configuring Containers

Contributors are configured **per container**, like the other container options:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        container.PipelineContributors.Add<GZipBlobPipelineContributor>();
    });
});
````

Configuring the default container applies the contributor to all containers; a named container can add its own contributors on top of them:

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.ConfigureDefault(container =>
    {
        container.PipelineContributors.Add<GZipBlobPipelineContributor>();
    });

    options.Containers.Configure<ProfilePictureContainer>(container =>
    {
        // Runs after the GZip contributor while saving
        container.PipelineContributors.Add<WatermarkPipelineContributor>();
    });
});
````

Think of the composition as **global stages plus container stages**: the contributors of the default container run first while saving, then the own ones of the container (each contributor type runs once). The inherited contributors are kept even when a container overrides its storage provider; set `InheritPipelineContributors` to `false` on a container to opt out of the global stages completely:

````csharp
options.Containers.Configure<PublicPictureContainer>(container =>
{
    container.InheritPipelineContributors = false;
});
````

## Execution Order and Encryption

* While **saving**, the contributors run in the configuration order, and the built-in [encryption](./encryption.md) always runs **after** them (immediately before the storage provider).
* While **getting**, the decryption runs first and the contributors run in the **reverse** order.

So, contributors always work on the plain content, a compressing contributor always compresses before the encryption (encrypted data can not be compressed), and the stored form is always ciphertext when the encryption is enabled.

## Behavioral Notes

* The stream returned for a container with contributors is generally read-only and non-seekable, and its `Length` is only available when the transformation can provide it. See the behavioral notes of the [BLOB Encryption document](./encryption.md) — the same stream semantics apply to the pipeline.
* When a contributor changes the content size lazily, the final length is unknown to the storage provider; providers that require the object size before uploading need an eagerly materialized (or length-aware) stream.
* Some storage providers consume the stream **synchronously** (like the Aliyun provider); they require contributor streams that also support synchronous reads, exactly like they do without the pipeline.
* Containers without contributors are not affected at all.

> **A contributor that transforms the content is part of the persisted data format.** A BLOB is only readable with the same transforming contributors, in the same order, it was saved with: adding, removing or re-ordering **transforming** contributors on a container that already has BLOBs makes the existing content fail to be read (or, for transformations without an own format check, silently return wrong content). A **metadata-only** contributor that neither consumes nor replaces `context.BlobStream` does not change the stored format, so it can be added to a container with existing BLOBs. A contributor that reads the content to validate it must return a pass-through wrapper (it still counts as consuming the stream); not replacing the stream after reading it would leave an empty/truncated stream for the provider. To change transforming contributors, migrate by reading the BLOBs **with the old configuration** and exporting the plain content, applying the change, and then writing the content back; re-saving in place under the old configuration does not change the stored form.

## See Also

* [BLOB Storing](../blob-storing)
* [BLOB Encryption](./encryption.md)
* [Creating a custom BLOB storage provider](./custom-provider.md)
