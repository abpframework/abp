using System.Threading;
using JetBrains.Annotations;
using Volo.Abp.BlobStoring.Containers;

namespace Volo.Abp.BlobStoring.Providers
{
    public class BlobProviderExistsArgs : BlobProviderArgs
    {
        public BlobProviderExistsArgs(
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