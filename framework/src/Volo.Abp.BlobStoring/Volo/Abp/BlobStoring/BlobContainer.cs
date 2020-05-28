using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainer<TContainer> : IBlobContainer<TContainer>
        where TContainer : class
    {
        private readonly IBlobContainer _container;

        public BlobContainer(IBlobContainerFactory blobContainerFactory)
        {
            _container = blobContainerFactory.Create<TContainer>();
        }

        public Task SaveAsync(
            string name,
            Stream stream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            return _container.SaveAsync(
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
            return _container.DeleteAsync(
                name,
                cancellationToken
            );
        }

        public Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(
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

        public BlobContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            ICurrentTenant currentTenant,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            ContainerName = containerName;
            Configuration = configuration;
            Provider = provider;
            CurrentTenant = currentTenant;
            CancellationTokenProvider = cancellationTokenProvider;
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
                    Configuration,
                    name,
                    stream,
                    overrideExisting,
                    GetTenantIdOrNull(),
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
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
                    Configuration,
                    name,
                    GetTenantIdOrNull(),
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
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
                    Configuration,
                    name,
                    GetTenantIdOrNull(),
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }

        public virtual async Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var stream = await Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    ContainerName,
                    Configuration,
                    name,
                    GetTenantIdOrNull(),
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
            
            if (stream == null)
            {
                //TODO: Consider to throw some type of "not found" exception and handle on the HTTP status side
                throw new AbpException($"Could not found the requested BLOB '{name}' in the container '{ContainerName}'!");
            }

            return stream;
        }

        public virtual Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    ContainerName,
                    Configuration,
                    name,
                    GetTenantIdOrNull(),
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }
        
        protected virtual Guid? GetTenantIdOrNull()
        {
            if (!Configuration.IsMultiTenant)
            {
                return null;
            }
            
            return CurrentTenant.Id;
        }
    }
}