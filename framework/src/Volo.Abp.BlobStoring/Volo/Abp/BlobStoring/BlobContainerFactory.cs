using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.BlobStoring.Providers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerFactory : IBlobContainerFactory, ITransientDependency
    {
        public IEnumerable<IBlobProvider> BlobProviders { get; }
        
        protected AbpBlobStoringOptions Options { get; }

        public BlobContainerFactory(
            IOptions<AbpBlobStoringOptions> options,
            IEnumerable<IBlobProvider> blobProviders)
        {
            BlobProviders = blobProviders;
            Options = options.Value;
        }
        
        public virtual IBlobContainer Create(string name, CancellationToken cancellationToken = default)
        {
            var configuration = Options.Containers.GetConfiguration(name);
            return new BlobContainerToProviderAdapter(
                name,
                configuration,
                GetProvider(name, configuration)
            );
        }

        [NotNull]
        protected virtual IBlobProvider GetProvider(
            string containerName,
            BlobContainerConfiguration configuration)
        {
            if (!BlobProviders.Any())
            {
                throw new AbpException("No BLOB Storage provider was registered! At least one provider must be registered to be able to use the Blog Storing System.");
            }
            
            foreach (var provider in BlobProviders)
            {
                if (ProxyHelper.GetUnProxiedType(provider).IsAssignableTo(configuration.ProviderType))
                {
                    return provider;
                }
            }

            throw new AbpException(
                $"Could not find the BLOB Storage provider with the type ({configuration.ProviderType.AssemblyQualifiedName}) configured for the container {containerName} and no default provider was set."
            );
        }
    }
}