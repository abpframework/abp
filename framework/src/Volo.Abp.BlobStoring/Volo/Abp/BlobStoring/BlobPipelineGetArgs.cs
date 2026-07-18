using System.IO;
using System.Threading;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring;

public class BlobPipelineGetArgs : BlobProviderArgs
{
    [NotNull]
    public Stream BlobStream { get; }

    public BlobPipelineGetArgs(
        [NotNull] string containerName,
        [NotNull] BlobContainerConfiguration configuration,
        [NotNull] string blobName,
        [NotNull] Stream blobStream,
        CancellationToken cancellationToken = default)
        : base(
            containerName,
            configuration,
            blobName,
            cancellationToken)
    {
        BlobStream = Check.NotNull(blobStream, nameof(blobStream));
    }
}
