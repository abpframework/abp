using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring;

public class BlobNormalizeNamingService : IBlobNormalizeNamingService, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }

    public BlobNormalizeNamingService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public BlobNormalizeNaming NormalizeNaming(
        BlobContainerConfiguration configuration,
        string containerName,
        string blobName)
    {

        if (!configuration.NamingNormalizers.Any())
        {
            return new BlobNormalizeNaming(containerName, blobName);
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            foreach (var normalizerType in configuration.NamingNormalizers)
            {
                var normalizer = scope.ServiceProvider
                    .GetRequiredService(normalizerType)
                    .As<IBlobNamingNormalizer>();

                containerName = containerName.IsNullOrWhiteSpace() ? containerName : normalizer.NormalizeContainerName(containerName);
                blobName = blobName.IsNullOrWhiteSpace() ? blobName : normalizer.NormalizeBlobName(blobName);
            }

            return new BlobNormalizeNaming(containerName, blobName);
        }
    }

    public string NormalizeContainerName(BlobContainerConfiguration configuration, string containerName)
    {
        if (!configuration.NamingNormalizers.Any())
        {
            return containerName;
        }

        return NormalizeNaming(configuration, containerName, null).ContainerName;
    }

    public string NormalizeBlobName(BlobContainerConfiguration configuration, string blobName)
    {
        if (!configuration.NamingNormalizers.Any())
        {
            return blobName;
        }

        return NormalizeNaming(configuration, null, blobName).BlobName;
    }
}
