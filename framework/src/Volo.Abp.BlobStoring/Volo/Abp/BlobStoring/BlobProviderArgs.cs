using System;
using System.Threading;
using JetBrains.Annotations;

namespace Volo.Abp.BlobStoring
{
    public abstract class BlobProviderArgs
    {
        [NotNull]
        public string ContainerName { get; }
        
        [NotNull]
        public BlobContainerConfiguration Configuration { get; }

        [NotNull]
        public string BlobName { get; }
        
        public CancellationToken CancellationToken { get; }
        
        public Guid? TenantId { get; }

        protected BlobProviderArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            [CanBeNull] Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Configuration = Check.NotNull(configuration, nameof(configuration));
            BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
            TenantId = tenantId;
            CancellationToken = cancellationToken;
        }
    }
}