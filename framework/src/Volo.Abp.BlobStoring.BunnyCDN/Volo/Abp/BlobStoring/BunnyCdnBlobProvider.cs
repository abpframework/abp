using BunnyCDN.Net.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.BunnyCDN.Volo.Abp.BlobStoring
{
    public class BunnyCdnBlobProvider : BlobProviderBase, ITransientDependency
    {
        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var bunny = GetBunnyCdnClient(args.Configuration.GetBunnyCdnConfiguration());

            return bunny.DeleteObjectAsync(args.ContainerName + "/" + args.BlobName);
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            try
            {
                var bunny = GetBunnyCdnClient(args.Configuration.GetBunnyCdnConfiguration());

                // TODO: Find better solution to check if blob exists.
                using (var stream = await bunny.DownloadObjectAsStreamAsync(args.ContainerName + "/" + args.BlobName))
                {
                    return true;
                }
            }
            catch (BunnyCDNStorageFileNotFoundException)
            {
                return false;
            }
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            try
            {
                var bunny = GetBunnyCdnClient(args.Configuration.GetBunnyCdnConfiguration());
                return await bunny.DownloadObjectAsStreamAsync(args.ContainerName + "/" + args.BlobName);
            }
            catch (BunnyCDNStorageFileNotFoundException)
            {
                return null;
            }
        }

        public override Task SaveAsync(BlobProviderSaveArgs args)
        {
            var configuration = args.Configuration.GetBunnyCdnConfiguration();
            var bunny = GetBunnyCdnClient(configuration);

            return bunny.UploadAsync(args.BlobStream, "/" + configuration.StorageZoneName + "/" + args.ContainerName + "/" + args.BlobName);
        }

        private BunnyCDNStorage GetBunnyCdnClient(BunnyCdnProviderConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.MainApplicationRegion))
            {
                return new BunnyCDNStorage(configuration.StorageZoneName, configuration.ApiAccessKey);
            }

            return new BunnyCDNStorage(configuration.StorageZoneName, configuration.ApiAccessKey, configuration.MainApplicationRegion);
        }
    }
}
