using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobProvider : BlobProviderBase, ITransientDependency
    {
        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), args.BlobName);

            if (!args.OverrideExisting && await BlobExistsAsync(blobClient))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            await blobClient.UploadAsync(args.BlobStream, true);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), args.BlobName);
            if (await BlobExistsAsync(blobClient))
            {
                return (await blobClient.DeleteAsync()).Status == 200;
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), args.BlobName);
            return await BlobExistsAsync(blobClient);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), args.BlobName);
            if (!await BlobExistsAsync(blobClient))
            {
                return null;
            }

            var download = await blobClient.DownloadAsync();
            var memoryStream = new MemoryStream();
            await download.Value.Content.CopyToAsync(memoryStream);
            return memoryStream;
        }

        protected virtual BlobClient GetBlobClient(AzureBlobProviderConfiguration configuration, string blobName)
        {
            var blobContainerClient = GetBlobContainerClient(configuration);
            return blobContainerClient.GetBlobClient(blobName);
        }

        protected virtual BlobContainerClient GetBlobContainerClient(AzureBlobProviderConfiguration configuration)
        {
            var blobServiceClient = new BlobServiceClient(configuration.ConnectionString);
            return blobServiceClient.GetBlobContainerClient(configuration.ContainerName);
        }

        private static async Task<bool> BlobExistsAsync(BlobClient blobClient)
        {
            return (await blobClient.ExistsAsync()).Value;
        }
    }
}
