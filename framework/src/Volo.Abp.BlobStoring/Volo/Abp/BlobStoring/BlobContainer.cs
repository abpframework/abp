using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            IServiceScope? contributorScope = null;
            try
            {
                if (contributorTypes.Count > 0)
                {
                    contributorScope = ServiceProvider.CreateScope();
                    stream = await ApplyPipelineOnSaveAsync(
                        blobNormalizeNaming,
                        stream,
                        fallbackCancellationToken,
                        contributorTypes,
                        contributorScope.ServiceProvider
                    );
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
            }
            finally
            {
                contributorScope?.Dispose();
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

            var contributorTypes = Configuration.GetEffectivePipelineContributors().ToList();
            if (contributorTypes.Count == 0)
            {
                return stream;
            }

            var contributorScope = ServiceProvider.CreateScope();
            try
            {
                var transformedStream = await ApplyPipelineOnGetAsync(
                    blobNormalizeNaming,
                    stream,
                    fallbackCancellationToken,
                    contributorTypes,
                    contributorScope.ServiceProvider
                );

                return new ScopeDisposingStream(transformedStream, contributorScope);
            }
            catch
            {
                contributorScope.Dispose();
                stream.Dispose();
                throw;
            }
        }
    }

    protected virtual async Task<Stream> ApplyPipelineOnSaveAsync(
        BlobNormalizeNaming blobNormalizeNaming,
        Stream stream,
        CancellationToken cancellationToken,
        IReadOnlyList<Type> contributorTypes,
        IServiceProvider contributorServiceProvider)
    {
        foreach (var contributorType in contributorTypes)
        {
            var contributor = contributorServiceProvider
                .GetRequiredService(contributorType)
                .As<IBlobPipelineContributor>();

            stream = await contributor.OnSaveAsync(
                new BlobPipelineSaveArgs(
                    blobNormalizeNaming.ContainerName!,
                    Configuration,
                    blobNormalizeNaming.BlobName!,
                    stream,
                    cancellationToken
                )
            );
        }

        return stream;
    }

    protected virtual async Task<Stream> ApplyPipelineOnGetAsync(
        BlobNormalizeNaming blobNormalizeNaming,
        Stream stream,
        CancellationToken cancellationToken,
        IReadOnlyList<Type> contributorTypes,
        IServiceProvider contributorServiceProvider)
    {
        // Execute in reverse order, so the contributor that transformed the stream
        // last on save is the first to transform it back on read.
        foreach (var contributorType in contributorTypes.Reverse())
        {
            var contributor = contributorServiceProvider
                .GetRequiredService(contributorType)
                .As<IBlobPipelineContributor>();

            stream = await contributor.OnGetAsync(
                new BlobPipelineGetArgs(
                    blobNormalizeNaming.ContainerName!,
                    Configuration,
                    blobNormalizeNaming.BlobName!,
                    stream,
                    cancellationToken
                )
            );
        }

        return stream;
    }

    protected virtual Guid? GetTenantIdOrNull()
    {
        if (!Configuration.IsMultiTenant)
        {
            return null;
        }

        return CurrentTenant.Id;
    }

    private sealed class ScopeDisposingStream : Stream
    {
        private readonly Stream _stream;
        private readonly IServiceScope _scope;
        private bool _disposed;

        public ScopeDisposingStream(Stream stream, IServiceScope scope)
        {
            _stream = stream;
            _scope = scope;
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                try
                {
                    _stream.Dispose();
                }
                finally
                {
                    _scope.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
