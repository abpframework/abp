using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring;

/// <summary>
/// The context an <see cref="IBlobPipelineContributor"/> works on. A contributor
/// transforms the content by replacing <see cref="BlobStream"/> with a wrapper;
/// see <see cref="IBlobPipelineContributor"/> for the stream ownership contract.
/// </summary>
public class BlobPipelineContext : IServiceProviderAccessor
{
    /// <summary>
    /// The scoped service provider of the pipeline. While saving, the scope stays
    /// alive until the save operation completes; while getting, until the stream
    /// returned to the caller is disposed — so lazily transforming wrappers can
    /// keep using their scoped services.
    /// </summary>
    [NotNull]
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// The normalized container name.
    /// </summary>
    [NotNull]
    public string ContainerName { get; }

    /// <summary>
    /// The normalized BLOB name.
    /// </summary>
    [NotNull]
    public string BlobName { get; }

    /// <summary>
    /// The configuration of the container the BLOB belongs to.
    /// </summary>
    [NotNull]
    public BlobContainerConfiguration Configuration { get; }

    /// <summary>
    /// The tenant of the BLOB operation (null for the host or a shared container).
    /// </summary>
    public Guid? TenantId { get; }

    /// <summary>
    /// The cancellation token of the BLOB operation. Pass it to any I/O the contributor
    /// performs while <see cref="IBlobPipelineContributor.OnSavingAsync"/> /
    /// <see cref="IBlobPipelineContributor.OnGettingAsync"/> runs. A lazy read wrapper
    /// returned from <c>OnGettingAsync</c> must instead honor the token passed to each of
    /// its own <c>Read</c>/<c>ReadAsync</c> calls (this token is captured once at
    /// <c>GetAsync</c> time and is not updated per read).
    /// </summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    /// The content stream. Replace it with a (typically lazily transforming,
    /// read-only) wrapper to transform the content.
    /// </summary>
    [NotNull]
    public Stream BlobStream {
        get => _blobStream;
        set
        {
            _blobStream = Check.NotNull(value, nameof(value));
            TrackCreatedStream(value);
        }
    }
    private Stream _blobStream;

    private readonly Stream _initialStream;

    // While saving, every stream the pipeline creates is collected here (at
    // assignment, so intermediate replacements within one contributor call are
    // not lost) to be disposed after the save; the initial (caller-owned)
    // stream is never collected. Null while getting.
    internal List<Stream>? CreatedStreams { get; set; }

    /// <summary>
    /// Creates the context; the names are expected in their normalized form and
    /// <paramref name="blobStream"/> is the initial (untransformed) content.
    /// </summary>
    public BlobPipelineContext(
        [NotNull] IServiceProvider serviceProvider,
        [NotNull] string containerName,
        [NotNull] string blobName,
        [NotNull] BlobContainerConfiguration configuration,
        Guid? tenantId,
        [NotNull] Stream blobStream,
        CancellationToken cancellationToken = default)
    {
        ServiceProvider = Check.NotNull(serviceProvider, nameof(serviceProvider));
        ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
        BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
        Configuration = Check.NotNull(configuration, nameof(configuration));
        TenantId = tenantId;
        _initialStream = _blobStream = Check.NotNull(blobStream, nameof(blobStream));
        CancellationToken = cancellationToken;
    }

    private void TrackCreatedStream(Stream stream)
    {
        if (CreatedStreams == null || ReferenceEquals(stream, _initialStream))
        {
            return;
        }

        foreach (var existingStream in CreatedStreams)
        {
            if (ReferenceEquals(existingStream, stream))
            {
                return;
            }
        }

        CreatedStreams.Add(stream);
    }
}
