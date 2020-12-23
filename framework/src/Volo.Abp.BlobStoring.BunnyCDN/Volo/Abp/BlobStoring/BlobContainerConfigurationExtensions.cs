using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.BlobStoring.BunnyCDN.Volo.Abp.BlobStoring
{
    public static class BlobContainerConfigurationExtensions
    {
        public static BlobContainerConfiguration UseBunnyCdn(
           this BlobContainerConfiguration containerConfiguration,
           Action<BunnyCdnProviderConfiguration> configureAction)
        {
            if (configureAction is null)
            {
                throw new ArgumentNullException(nameof(configureAction));
            }

            containerConfiguration.ProviderType = typeof(BunnyCdnBlobProvider);

            configureAction.Invoke(new BunnyCdnProviderConfiguration(containerConfiguration));

            return containerConfiguration;
        }

        public static BunnyCdnProviderConfiguration GetBunnyCdnConfiguration(this BlobContainerConfiguration containerConfiguration)
        {
            return new BunnyCdnProviderConfiguration(containerConfiguration);
        }
    }
}
