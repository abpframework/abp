using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerToProviderAdapter : IBlobContainer
    {
        protected string ContainerName { get; }
        
        protected BlobContainerConfiguration ContainerConfiguration { get; }
        
        protected IBlobProvider Provider { get; }

        public BlobContainerToProviderAdapter(
            string containerName,
            BlobContainerConfiguration containerConfiguration,
            IBlobProvider provider)
        {
            ContainerName = containerName;
            ContainerConfiguration = containerConfiguration;
            Provider = provider;
        }

        public virtual Task SaveAsync(
            string name,
            Stream stream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            return Provider.SaveAsync(
                new BlobProviderSaveArgs(
                    ContainerName,
                    ContainerConfiguration,
                    name,
                    stream,
                    overrideExisting,
                    cancellationToken
                )
            );
        }

        public virtual Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Provider.DeleteAsync(
                new BlobProviderDeleteArgs(
                    ContainerName,
                    ContainerConfiguration,
                    name,
                    cancellationToken
                )
            );
        }

        public virtual Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Provider.ExistsAsync(
                new BlobProviderExistsArgs(
                    ContainerName,
                    ContainerConfiguration,
                    name,
                    cancellationToken
                )
            );
        }

        public virtual Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Provider.GetAsync(
                new BlobProviderGetArgs(
                    ContainerName,
                    ContainerConfiguration,
                    name,
                    cancellationToken
                )
            );
        }

        public virtual Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    ContainerName,
                    ContainerConfiguration,
                    name,
                    cancellationToken
                )
            );
        }
    }
}