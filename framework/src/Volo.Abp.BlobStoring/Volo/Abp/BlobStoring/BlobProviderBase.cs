using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.BlobStoring
{
    public abstract class BlobProviderBase : IBlobProvider
    {
        public abstract Task SaveAsync(BlobProviderSaveArgs args);

        public abstract Task<bool> DeleteAsync(BlobProviderDeleteArgs args);

        public abstract Task<bool> ExistsAsync(BlobProviderExistsArgs args);

        public abstract Task<Stream> GetOrNullAsync(BlobProviderGetArgs args);

        protected virtual string NormalizeContainerName(BlobProviderArgs args, IServiceProvider serviceProvider, string containerName)
        {
            if (!args.Configuration.NamingNormalizers.Any())
            {
                return containerName;
            }

            using (var scope = serviceProvider.CreateScope())
            {
                foreach (var normalizerType in args.Configuration.NamingNormalizers)
                {
                    var normalizer = scope.ServiceProvider
                        .GetRequiredService(normalizerType)
                        .As<IBlobNamingNormalizer>();

                    containerName = normalizer.NormalizeContainerName(containerName);
                }

                return containerName;
            }
        }
    }
}
