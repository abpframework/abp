using System;
using System.IO;
using System.Threading;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public class BlobProviderSaveArgs : BlobProviderArgs
    {
        [NotNull]
        public Stream BlobStream { get; }
        
        public bool OverrideExisting { get; }

        public BlobProviderSaveArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            [NotNull] Stream blobStream,
            bool overrideExisting = false,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                tenantId,
                cancellationToken)
        {
            BlobStream = Check.NotNull(blobStream, nameof(blobStream));
            OverrideExisting = overrideExisting;
        }
    }
}