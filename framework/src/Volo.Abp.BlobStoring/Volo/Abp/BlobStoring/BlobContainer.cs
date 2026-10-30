using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring;

public class BlobContainer<TContainer> : IBlobContainer<TContainer>
    where TContainer : class
{
    protected readonly IBlobContainer Container;

    public BlobContainer(IBlobContainerFactory blobContainerFactory)
    {
        Container = blobContainerFactory.Create<TContainer>();
    }

    public Task SaveAsync(
        string name,
        Stream stream,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default)
    {
        return Container.SaveAsync(
            name,
            stream,
            overrideExisting,
            cancellationToken
        );
    }

    public Task<bool> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Container.DeleteAsync(
            name,
            cancellationToken
        );
    }

    public Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Container.ExistsAsync(
            name,
            cancellationToken
        );
    }

    public Task<Stream> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Container.GetAsync(
            name,
            cancellationToken
        );
    }

    public Task<Stream?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return Container.GetOrNullAsync(
            name,
            cancellationToken
        );
    }
}

public class BlobContainer : IBlobContainer
{
    protected string ContainerName { get; }

    protected BlobContainerConfiguration Configuration { get; }

    protected IBlobProvider Provider { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    protected IServiceProvider ServiceProvider { get; }

    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

    public BlobContainer(
        string containerName,
        BlobContainerConfiguration configuration,
        IBlobProvider provider,
        ICurrentTenant currentTenant,
        ICancellationTokenProvider cancellationTokenProvider,
        IBlobNormalizeNamingService blobNormalizeNamingService,
        IServiceProvider serviceProvider)
    {
        ContainerName = containerName;
        Configuration = configuration;
        Provider = provider;
        CurrentTenant = currentTenant;
        CancellationTokenProvider = cancellationTokenProvider;
        BlobNormalizeNamingService = blobNormalizeNamingService;
        ServiceProvider = serviceProvider;
    }

    public virtual async Task SaveAsync(
        string name,
        Stream stream,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(GetTenantIdOrNull()))
        {
            var blobNormalizeNaming = BlobNormalizeNamingService.NormalizeNaming(Configuration, ContainerName, name);

            var fallbackCancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);
            var contributorTypes = Configuration.GetEffectivePipelineContributors().ToList();

            // Every stream the pipeline creates is disposed after the save (disposing the
            // encrypting wrapper also zeroes the derived key); the caller keeps the original
            var pipelineStreams = new List<Stream>();
            AsyncServiceScope? contributorScope = null;
            var completed = false;
            try
            {
                if (contributorTypes.Count > 0)
                {
                    contributorScope = ServiceProvider.CreateAsyncScope();
                    var context = new BlobPipelineContext(
                        contributorScope.Value.ServiceProvider,
                        blobNormalizeNaming.ContainerName!,
                        blobNormalizeNaming.BlobName!,
                        Configuration,
                        GetTenantIdOrNull(),
                        stream,
                        fallbackCancellationToken
                    );

                    context.CreatedStreams = pipelineStreams;
                    await RunPipelineAsync(context, contributorTypes, saving: true);
                    stream = context.BlobStream;
                }

                if (BlobEncryptionConfiguration.IsEnabled(Configuration))
                {
                    stream = await CreateEncryptingStreamAsync(blobNormalizeNaming, stream, fallbackCancellationToken);
                    pipelineStreams.Add(stream);
                }

                await Provider.SaveAsync(
                    new BlobProviderSaveArgs(
                        blobNormalizeNaming.ContainerName!,
                        Configuration,
                        blobNormalizeNaming.BlobName!,
                        stream,
                        overrideExisting,
                        fallbackCancellationToken
                    )
                );

                completed = true;
            }
            finally
            {
                // Best-effort cleanup: a failing Dispose must not prevent releasing the
                // remaining streams/scope or hide the exception that is already propagating
                Exception? disposeException = null;
                for (var i = pipelineStreams.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        await DisposeStreamAsync(pipelineStreams[i]);
                    }
                    catch (Exception ex)
                    {
                        disposeException ??= ex;
                    }
                }

                if (contributorScope != null)
                {
                    try
                    {
                        await contributorScope.Value.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        disposeException ??= ex;
                    }
                }

                if (completed && disposeException != null)
                {
                    ExceptionDispatchInfo.Capture(disposeException).Throw();
                }
            }
        }
    }

    public virtual async Task<bool> DeleteAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(GetTenantIdOrNull()))
        {
            var blobNormalizeNaming =
                BlobNormalizeNamingService.NormalizeNaming(Configuration, ContainerName, name);

            return await Provider.DeleteAsync(
                new BlobProviderDeleteArgs(
                    blobNormalizeNaming.ContainerName!,
                    Configuration,
                    blobNormalizeNaming.BlobName!,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }
    }

    public virtual async Task<bool> ExistsAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(GetTenantIdOrNull()))
        {
            var blobNormalizeNaming =
                BlobNormalizeNamingService.NormalizeNaming(Configuration, ContainerName, name);

            return await Provider.ExistsAsync(
                new BlobProviderExistsArgs(
                    blobNormalizeNaming.ContainerName!,
                    Configuration,
                    blobNormalizeNaming.BlobName!,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }
    }

    public virtual async Task<Stream> GetAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var stream = await GetOrNullAsync(name, cancellationToken);

        if (stream == null)
        {
            //TODO: Consider to throw some type of "not found" exception and handle on the HTTP status side
            throw new AbpException(
                $"Could not find the requested BLOB '{name}' in the container '{ContainerName}'!");
        }

        return stream;
    }

    public virtual async Task<Stream?> GetOrNullAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(GetTenantIdOrNull()))
        {
            var blobNormalizeNaming =
                BlobNormalizeNamingService.NormalizeNaming(Configuration, ContainerName, name);

            var fallbackCancellationToken = CancellationTokenProvider.FallbackToProvider(cancellationToken);

            var stream = await Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    blobNormalizeNaming.ContainerName!,
                    Configuration,
                    blobNormalizeNaming.BlobName!,
                    fallbackCancellationToken
                )
            );

            if (stream == null)
            {
                return null;
            }

            // The provider stream is now owned by the container: reading the configuration
            // (a mis-typed value can throw) must not leak it
            List<Type> contributorTypes;
            bool encryptionEnabled;
            try
            {
                contributorTypes = Configuration.GetEffectivePipelineContributors().ToList();
                encryptionEnabled = BlobEncryptionConfiguration.IsEnabled(Configuration);
            }
            catch
            {
                await TryDisposeStreamAsync(stream);
                throw;
            }

            // Captured so the pipeline can verify the decryption reached its authenticated
            // end (the terminal record) when the composed stream reaches EOF, even if a
            // contributor stops reading the decrypted content early. A custom
            // CreateDecryptingStreamAsync override that wraps the stream should implement
            // IBlobAuthenticatedEndStream (and forward), or this verification is skipped.
            IBlobAuthenticatedEndStream? authenticatedEndSource = null;
            if (encryptionEnabled)
            {
                // The method owns the provider stream: it is disposed there on any failure
                stream = await CreateDecryptingStreamAsync(blobNormalizeNaming, stream, fallbackCancellationToken);
                authenticatedEndSource = stream as IBlobAuthenticatedEndStream;
            }

            if (contributorTypes.Count == 0)
            {
                // Without contributors the caller reads the decrypting stream directly, so
                // reading it to its end verifies the terminal record on its own
                return stream;
            }

            // Contributors run in the reverse order after the decryption; the scope stays
            // alive until the returned (lazily transforming) stream is disposed. Resolve the
            // tenant id once, before the scope is created, so nothing between scope creation
            // and the successful return can throw and leak the scope or the provider stream
            Guid? tenantId;
            try
            {
                tenantId = GetTenantIdOrNull();
            }
            catch
            {
                await TryDisposeStreamAsync(stream);
                throw;
            }

            contributorTypes.Reverse();
            AsyncServiceScope contributorScope;
            try
            {
                contributorScope = ServiceProvider.CreateAsyncScope();
            }
            catch
            {
                await TryDisposeStreamAsync(stream);
                throw;
            }

            var context = new BlobPipelineContext(
                contributorScope.ServiceProvider,
                blobNormalizeNaming.ContainerName!,
                blobNormalizeNaming.BlobName!,
                Configuration,
                tenantId,
                stream,
                fallbackCancellationToken
            );

            try
            {
                await RunPipelineAsync(context, contributorTypes, saving: false);

                return new BlobPipelineScopeStream(context.BlobStream, contributorScope, CurrentTenant, tenantId, authenticatedEndSource);
            }
            catch
            {
                // Best-effort cleanup that keeps the original exception: the current
                // context stream owns the whole chain down to the provider stream
                await TryDisposeStreamAsync(context.BlobStream);

                try
                {
                    await contributorScope.DisposeAsync();
                }
                catch
                {
                    // ignored
                }

                throw;
            }
        }
    }

    /// <summary>
    /// Runs the pipeline contributors on the context stream. While saving, the context
    /// collects every stream a contributor creates (at assignment, so intermediate
    /// replacements within one contributor call are not lost) to be disposed by the
    /// caller; while reading, the composed stream is returned to the caller as a whole,
    /// so each wrapper owns the stream it received.
    /// </summary>
    protected virtual async Task RunPipelineAsync(
        BlobPipelineContext context,
        IReadOnlyList<Type> contributorTypes,
        bool saving)
    {
        foreach (var contributorType in contributorTypes)
        {
            var contributor = (IBlobPipelineContributor)context.ServiceProvider.GetRequiredService(contributorType);

            if (saving)
            {
                await contributor.OnSavingAsync(context);
            }
            else
            {
                await contributor.OnGettingAsync(context);
            }
        }
    }

    /// <summary>
    /// Wraps the stream for encryption. The caller keeps the ownership of
    /// <paramref name="stream"/>; the wrapper is disposed after the provider call.
    /// </summary>
    protected virtual async Task<Stream> CreateEncryptingStreamAsync(BlobNormalizeNaming blobNormalizeNaming, Stream stream, CancellationToken cancellationToken)
    {
        // The key is fully resolved before returning, so the scope can be released here
        var scope = ServiceProvider.CreateAsyncScope();
        Stream? encryptingStream = null;
        var completed = false;
        try
        {
            encryptingStream = await scope.ServiceProvider
                .GetRequiredService<IBlobEncryptionCodec>()
                .CreateEncryptingStreamAsync(
                    Configuration,
                    blobNormalizeNaming.ContainerName!,
                    blobNormalizeNaming.BlobName!,
                    GetTenantIdOrNull(),
                    stream,
                    cancellationToken
                );
            completed = true;
        }
        finally
        {
            try
            {
                await scope.DisposeAsync();
            }
            catch
            {
                // A failing scope dispose must not leak the created stream (it zeroes
                // the key) and must not replace an exception already propagating
                if (encryptingStream != null)
                {
                    await TryDisposeStreamAsync(encryptingStream);
                }

                if (completed)
                {
                    throw;
                }
            }
        }

        return encryptingStream!;
    }

    /// <summary>
    /// Wraps the provider stream for decryption. The method owns <paramref name="stream"/>:
    /// the returned stream disposes it, and it is also disposed when this method fails.
    /// Opening throws <see cref="AbpException"/> for format violations; reading throws
    /// <see cref="System.Security.Cryptography.CryptographicException"/> on failed authentication.
    /// The returned stream implements <see cref="IBlobAuthenticatedEndStream"/>, which the
    /// content pipeline uses to verify the authenticated end on EOF. An override that wraps
    /// the returned stream should implement that interface too (and forward), otherwise the
    /// pipeline can not run the end verification for this container.
    /// </summary>
    protected virtual async Task<Stream> CreateDecryptingStreamAsync(BlobNormalizeNaming blobNormalizeNaming, Stream stream, CancellationToken cancellationToken)
    {
        AsyncServiceScope scope;
        try
        {
            scope = ServiceProvider.CreateAsyncScope();
        }
        catch
        {
            await TryDisposeStreamAsync(stream);
            throw;
        }

        Stream? decryptingStream = null;
        var completed = false;
        try
        {
            decryptingStream = await scope.ServiceProvider
                .GetRequiredService<IBlobEncryptionCodec>()
                .CreateDecryptingStreamAsync(
                    Configuration,
                    blobNormalizeNaming.ContainerName!,
                    blobNormalizeNaming.BlobName!,
                    GetTenantIdOrNull(),
                    stream,
                    cancellationToken
                );
            completed = true;
        }
        catch
        {
            // Best-effort cleanup that keeps the original exception
            await TryDisposeStreamAsync(stream);
            throw;
        }
        finally
        {
            try
            {
                await scope.DisposeAsync();
            }
            catch
            {
                // A failing scope dispose must not leak the created stream (it zeroes
                // the key and disposes the provider stream — exactly once, since the
                // caller does not dispose after this method) and must not replace an
                // exception already propagating
                if (decryptingStream != null)
                {
                    await TryDisposeStreamAsync(decryptingStream);
                }

                if (completed)
                {
                    throw;
                }
            }
        }

        return decryptingStream!;
    }

    protected virtual async Task DisposeStreamAsync(Stream stream)
    {
#if NETSTANDARD2_0
        // Stream has no DisposeAsync on netstandard2.0, but a stream may still implement
        // IAsyncDisposable (via Microsoft.Bcl.AsyncInterfaces) for its async-only cleanup
        if (stream is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync();
        }
        else
        {
            stream.Dispose();
        }
#else
        // Also covers wrappers that only implement DisposeAsync
        await stream.DisposeAsync();
#endif
    }

    protected virtual async Task TryDisposeStreamAsync(Stream stream)
    {
        try
        {
            await DisposeStreamAsync(stream);
        }
        catch
        {
            // ignored: best-effort cleanup during exception handling
        }
    }

    protected virtual Guid? GetTenantIdOrNull()
    {
        if (!Configuration.IsMultiTenant)
        {
            return null;
        }

        return CurrentTenant.Id;
    }
}
