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
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), blobName);

            if (!args.OverrideExisting && await BlobExistsAsync(blobClient))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            await blobClient.UploadAsync(args.BlobStream, true);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), blobName);
            return await blobClient.DeleteIfExistsAsync();
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), blobName);
            return await BlobExistsAsync(blobClient);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var blobClient = GetBlobClient(args.Configuration.GetAzureConfiguration(), blobName);
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

        private static async Task<bool> BlobExistsAsync(BlobBaseClient blobClient)
        {
            return (await blobClient.ExistsAsync()).Value;
        }
    }
}
