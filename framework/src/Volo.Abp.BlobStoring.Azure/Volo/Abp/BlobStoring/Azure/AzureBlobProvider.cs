using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IAzureBlobNameCalculator AzureBlobNameCalculator { get; }

        public AzureBlobProvider(IAzureBlobNameCalculator azureBlobNameCalculator)
        {
            AzureBlobNameCalculator = azureBlobNameCalculator;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAzureConfiguration();

            if (!args.OverrideExisting && await BlobExistsAsync(configuration, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{configuration.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateContainerIfNotExists)
            {
                await CreateContainerIfNotExists(configuration);
            }

            await GetBlobClient(configuration, blobName).UploadAsync(args.BlobStream, true);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAzureConfiguration();

            if (await BlobExistsAsync(configuration, blobName))
            {
                return await GetBlobClient(configuration, blobName).DeleteIfExistsAsync();
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAzureConfiguration();

            return await BlobExistsAsync(configuration, blobName);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAzureConfiguration();

            if (!await BlobExistsAsync(configuration, blobName))
            {
                return null;
            }

            var blobClient = GetBlobClient(configuration, blobName);
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

        protected virtual async Task CreateContainerIfNotExists(AzureBlobProviderConfiguration configuration)
        {
            var blobContainerClient = GetBlobContainerClient(configuration);
            await blobContainerClient.CreateIfNotExistsAsync();
        }

        private async Task<bool> BlobExistsAsync(AzureBlobProviderConfiguration configuration, string blobName)
        {
            // Make sure Blob Container exists.
            return await ContainerExistsAsync(GetBlobContainerClient(configuration)) &&
                   (await GetBlobClient(configuration, blobName).ExistsAsync()).Value;
        }

        private static async Task<bool> ContainerExistsAsync(BlobContainerClient blobContainerClient)
        {
            return (await blobContainerClient.ExistsAsync()).Value;
        }
    }
}
