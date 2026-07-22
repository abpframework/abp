using System;
using System.IO;
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
            Stream? encryptingStream = null;
            if (BlobEncryptionConfiguration.IsEnabled(Configuration))
            {
                encryptingStream = await CreateEncryptingStreamAsync(blobNormalizeNaming, stream, fallbackCancellationToken);
                stream = encryptingStream;
            }

            try
            {
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
            }
            finally
            {
                // Disposing the wrapper zeroes the derived key; the caller's stream is untouched
                encryptingStream?.Dispose();
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

            if (!BlobEncryptionConfiguration.IsEnabled(Configuration))
            {
                return stream;
            }

            try
            {
                return await CreateDecryptingStreamAsync(blobNormalizeNaming, stream, fallbackCancellationToken);
            }
            catch
            {
                stream.Dispose();
                throw;
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
        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            return await scope.ServiceProvider
                .GetRequiredService<BlobEncryptionCodec>()
                .CreateEncryptingStreamAsync(
                    Configuration,
                    blobNormalizeNaming.ContainerName!,
                    blobNormalizeNaming.BlobName!,
                    GetTenantIdOrNull(),
                    stream,
                    cancellationToken
                );
        }
    }

    /// <summary>
    /// Wraps the provider stream for decryption; the returned stream owns it.
    /// Opening throws <see cref="AbpException"/> for format violations; reading throws
    /// <see cref="System.Security.Cryptography.CryptographicException"/> on failed authentication.
    /// </summary>
    protected virtual async Task<Stream> CreateDecryptingStreamAsync(BlobNormalizeNaming blobNormalizeNaming, Stream stream, CancellationToken cancellationToken)
    {
        await using (var scope = ServiceProvider.CreateAsyncScope())
        {
            return await scope.ServiceProvider
                .GetRequiredService<BlobEncryptionCodec>()
                .CreateDecryptingStreamAsync(
                    Configuration,
                    blobNormalizeNaming.ContainerName!,
                    blobNormalizeNaming.BlobName!,
                    GetTenantIdOrNull(),
                    stream,
                    cancellationToken
                );
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
