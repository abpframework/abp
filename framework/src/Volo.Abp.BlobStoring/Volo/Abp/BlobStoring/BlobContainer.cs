using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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

        protected IServiceProvider ServiceProvider { get; }

        public BlobContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            ICurrentTenant currentTenant,
            ICancellationTokenProvider cancellationTokenProvider,
            IServiceProvider serviceProvider)
        {
            ContainerName = containerName;
            Configuration = configuration;
            Provider = provider;
            CurrentTenant = currentTenant;
            CancellationTokenProvider = cancellationTokenProvider;
            ServiceProvider = serviceProvider;
        }

        public virtual async Task SaveAsync(
            string name,
            Stream stream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull()))
            {
                var (normalizedContainerName, normalizedBlobName) = NormalizeNaming(ContainerName, name);

                await Provider.SaveAsync(
                    new BlobProviderSaveArgs(
                        normalizedContainerName,
                        Configuration,
                        normalizedBlobName,
                        stream,
                        overrideExisting,
                        CancellationTokenProvider.FallbackToProvider(cancellationToken)
                    )
                );
            }
        }

        public virtual async Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull()))
            {
                var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

                return await Provider.DeleteAsync(
                    new BlobProviderDeleteArgs(
                        normalizedContainerName,
                        Configuration,
                        normalizedBlobName,
                        CancellationTokenProvider.FallbackToProvider(cancellationToken)
                    )
                );
            }
        }

        public virtual async Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull()))
            {
                var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

                return await Provider.ExistsAsync(
                    new BlobProviderExistsArgs(
                        normalizedContainerName,
                        Configuration,
                        normalizedBlobName,
                        CancellationTokenProvider.FallbackToProvider(cancellationToken)
                    )
                );
            }
        }

        public virtual async Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var stream = await GetOrNullAsync(name, cancellationToken);

            if (stream == null)
            {
                //TODO: Consider to throw some type of "not found" exception and handle on the HTTP status side
                throw new AbpException(
                    $"Could not found the requested BLOB '{name}' in the container '{ContainerName}'!");
            }

            return stream;
        }

        public virtual async Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            using (CurrentTenant.Change(GetTenantIdOrNull()))
            {
                var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

                return await Provider.GetOrNullAsync(
                    new BlobProviderGetArgs(
                        normalizedContainerName,
                        Configuration,
                        normalizedBlobName,
                        CancellationTokenProvider.FallbackToProvider(cancellationToken)
                    )
                );
            }
        }

        protected virtual Guid? GetTenantIdOrNull()
        {
            if (!Configuration.IsMultiTenant)
            {
                return null;
            }

            return CurrentTenant.Id;
        }
        
        protected virtual (string, string) NormalizeNaming(string containerName,  string blobName)
        {
            if (!Configuration.NamingNormalizers.Any())
            {
                return (containerName, blobName);
            }

            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var normalizerType in Configuration.NamingNormalizers)
                {
                    var normalizer = scope.ServiceProvider
                        .GetRequiredService(normalizerType)
                        .As<IBlobNamingNormalizer>();

                    containerName = normalizer.NormalizeContainerName(containerName);
                    blobName = normalizer.NormalizeBlobName(blobName);
                }

                return (containerName, blobName);
            }
        }
    }
}
