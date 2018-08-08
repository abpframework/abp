using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Volo.Abp.Storage
{
    public interface IStorageProvider
    {
        Task SaveBlobStreamAsync(string containerName, string blobName, Stream source, BlobProperties properties = null, bool closeStream = true);

        Task<Stream> GetBlobStreamAsync(string containerName, string blobName);

        Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName);

        Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName);

        Task DeleteBlobAsync(string containerName, string blobName);

        Task DeleteContainerAsync(string containerName);

        Task CopyBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName, string destinationBlobName = null);

        Task MoveBlobAsync(string sourceContainerName, string sourceBlobName, 
            string destinationContainerName, string destinationBlobName = null);

        Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties);

        string GetBlobUrl(string containerName, string blobName);

        string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false,
            string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read);
    }
}