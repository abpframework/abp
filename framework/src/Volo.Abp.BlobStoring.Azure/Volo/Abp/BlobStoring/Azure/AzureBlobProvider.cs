﻿using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
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

        public async override Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAzureConfiguration();

            if (!args.OverrideExisting && await BlobExistsAsync(args, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{GetContainerName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateContainerIfNotExists)
            {
                await CreateContainerIfNotExists(args);
            }

            await GetBlobClient(args, blobName).UploadAsync(args.BlobStream, true);
        }

        public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);

            if (await BlobExistsAsync(args, blobName))
            {
                return await GetBlobClient(args, blobName).DeleteIfExistsAsync();
            }

            return false;
        }

        public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);

            return await BlobExistsAsync(args, blobName);
        }

        public async override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AzureBlobNameCalculator.Calculate(args);

            if (!await BlobExistsAsync(args, blobName))
            {
                return null;
            }

            var blobClient = GetBlobClient(args, blobName);
            var download = await blobClient.DownloadAsync();
            var memoryStream = new MemoryStream();
            await download.Value.Content.CopyToAsync(memoryStream);
            return memoryStream;
        }

        protected virtual BlobClient GetBlobClient(BlobProviderArgs args, string blobName)
        {
            var blobContainerClient = GetBlobContainerClient(args);
            return blobContainerClient.GetBlobClient(blobName);
        }

        protected virtual BlobContainerClient GetBlobContainerClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAzureConfiguration();
            var blobServiceClient = new BlobServiceClient(configuration.ConnectionString);
            return blobServiceClient.GetBlobContainerClient(GetContainerName(args));
        }

        protected virtual async Task CreateContainerIfNotExists(BlobProviderArgs args)
        {
            var blobContainerClient = GetBlobContainerClient(args);
            await blobContainerClient.CreateIfNotExistsAsync();
        }

        private async Task<bool> BlobExistsAsync(BlobProviderArgs args, string blobName)
        {
            // Make sure Blob Container exists.
            return await ContainerExistsAsync(GetBlobContainerClient(args)) &&
                   (await GetBlobClient(args, blobName).ExistsAsync()).Value;
        }

        private static string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAzureConfiguration();
            return configuration.ContainerName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.ContainerName;
        }

        private static async Task<bool> ContainerExistsAsync(BlobContainerClient blobContainerClient)
        {
            return (await blobContainerClient.ExistsAsync()).Value;
        }
    }
}
