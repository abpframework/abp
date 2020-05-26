using System;
using System.Threading;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobProviderExistsArgs : BlobProviderArgs
    {
        public BlobProviderExistsArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        : base(
            containerName,
            configuration,
            blobName,
            tenantId,
            cancellationToken)
        {
        }
    }
}