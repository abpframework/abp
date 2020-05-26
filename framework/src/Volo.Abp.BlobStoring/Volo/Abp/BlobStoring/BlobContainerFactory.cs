using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring
{
    public class BlobContainerFactory : IBlobContainerFactory, ITransientDependency
    {
        protected AbpBlobStoringOptions Options { get; }

        protected  IEnumerable<IBlobProvider> BlobProviders { get; }

        protected ICurrentTenant CurrentTenant { get; }
        
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public BlobContainerFactory(
            IOptions<AbpBlobStoringOptions> options,
            IEnumerable<IBlobProvider> blobProviders,
            ICurrentTenant currentTenant,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            Options = options.Value;
            BlobProviders = blobProviders;
            CurrentTenant = currentTenant;
            CancellationTokenProvider = cancellationTokenProvider;
        }
        
        public virtual IBlobContainer Create(string name, CancellationToken cancellationToken = default)
        {
            var configuration = Options.Containers.GetConfiguration(name);
            return new BlobContainer(
                name,
                configuration,
                GetProvider(name, configuration),
                CurrentTenant,
                CancellationTokenProvider
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