using System.IO;
using System.Threading;
using JetBrains.Annotations;
using Volo.Abp.BlobStoring.Containers;

namespace Volo.Abp.BlobStoring.Providers
{
    public class BlobProviderSaveArgs
    {
        [NotNull]
        public string ContainerName { get; }
        
        [NotNull]
        public BlobContainerConfiguration Configuration { get; }

        [NotNull]
        public string BlobName { get; }
        
        [NotNull]
        public Stream BlobStream { get; }
        
        public bool OverrideExisting { get; }

        public CancellationToken CancellationToken { get; }
        
        public BlobProviderSaveArgs(
            [NotNull] string containerName,
            [NotNull] BlobContainerConfiguration configuration,
            [NotNull] string blobName,
            [NotNull] Stream blobStream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Configuration = Check.NotNull(configuration, nameof(configuration));
            BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
            BlobStream = Check.NotNull(blobStream, nameof(blobStream));
            OverrideExisting = overrideExisting;
            CancellationToken = cancellationToken;
        }
    }
}