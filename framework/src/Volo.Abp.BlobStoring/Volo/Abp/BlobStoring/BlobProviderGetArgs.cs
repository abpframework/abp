using System.Threading;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobProviderGetArgs : BlobProviderArgs
    {
        public BlobProviderGetArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
        }
    }
}