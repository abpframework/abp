using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.Azure
{
    public sealed class AzureStorageProvider : IStorageProvider, ITransientDependency
    {
        private readonly CloudBlobClient _blobClient;
        private readonly BlobRequestOptions _requestOptions;
        private readonly OperationContext _context;

        public AzureStorageProvider(AzureProviderOptions options)
        {
            _blobClient = CloudStorageAccount
                .Parse(options.ConnectionString)
                .CreateCloudBlobClient();
            _requestOptions = new BlobRequestOptions();
            _context = new OperationContext();
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            try
            {
                await _blobClient.GetContainerReference(containerName)
                    .GetBlobReference(blobName)
                    .DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, _requestOptions, _context)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            try
            {
                await _blobClient.GetContainerReference(containerName)
                    .DeleteIfExistsAsync(null, _requestOptions, _context);
            }
            catch(Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
        }

        public async Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            var sourceContainer = _blobClient.GetContainerReference(sourceContainerName);
            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceBlobName);

            var destContainer = _blobClient.GetContainerReference(destinationContainerName);

            await destContainer.CreateIfNotExistsAsync(sourceContainer.Properties.PublicAccess ?? 
                BlobContainerPublicAccessType.Off, _requestOptions, _context).ConfigureAwait(false);

            var destBlob = destContainer.GetBlockBlobReference(destinationBlobName ?? sourceBlobName);

            await destBlob.StartCopyAsync(sourceBlob).ConfigureAwait(false);
            
            while (destBlob.CopyState.Status == CopyStatus.Pending)
            {
                await Task.Delay(500).ConfigureAwait(false);
                await destBlob.FetchAttributesAsync().ConfigureAwait(false);
            }

            if (destBlob.CopyState.Status != CopyStatus.Success) throw new Exception("Copy failed: " +    destBlob.CopyState.Status);
        }

        public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
            await DeleteBlobAsync(sourceContainerName, sourceBlobName);
        }

        public async Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            try
            {
                await blob.FetchAttributesAsync(null, _requestOptions, _context).ConfigureAwait(false);
                var props = blob.Properties;
                var perm = (await container.GetPermissionsAsync(null, _requestOptions, _context).ConfigureAwait(false)).PublicAccess;

                return new BlobDescriptor
                {
                    Name = blob.Name,
                    Container = containerName,
                    Url = blob.Uri.ToString(),
                    Security = perm == BlobContainerPublicAccessType.Off ? BlobSecurity.Private : BlobSecurity.Public,
                    ContentType = props.ContentType,
                    ContentMD5 = props.ContentMD5,
                    ETag = props.ETag,
                    LastModified = props.LastModified,
                    Length = props.Length,
                };
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            var blob = _blobClient.GetContainerReference(containerName)
                .GetBlobReference(blobName);

            try
            {
                return await blob.OpenReadAsync(null, _requestOptions, _context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            return _blobClient.GetContainerReference(containerName)
                .GetBlockBlobReference(blobName)
                .Uri.ToString();
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry, bool isDownload = false, 
            string filename = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            var blob = _blobClient.GetContainerReference(containerName)
                .GetBlockBlobReference(blobName);

            var builder = new UriBuilder(blob.Uri);
            var headers = new SharedAccessBlobHeaders();
            var hasFilename = !string.IsNullOrEmpty(filename);

            if (hasFilename || isDownload) headers.ContentDisposition = "attachment" + (hasFilename ? "; filename=\"" + filename + "\"" : string.Empty);

            if (!string.IsNullOrEmpty(contentType)) headers.ContentType = contentType;

            builder.Query = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                Permissions = access.ToPermissions(),
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = expiry,
            }, headers).TrimStart('?');

            return builder.Uri.ToString();
        }

        public async Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            var list = new List<BlobDescriptor>();
            var container = _blobClient.GetContainerReference(containerName);
            var security = BlobSecurity.Public;

            BlobContinuationToken token = null;

            try
            {
                do
                {
                    var results = await container
                        .ListBlobsSegmentedAsync(null, true, BlobListingDetails.Metadata, 100, token, _requestOptions, _context)
                        .ConfigureAwait(false);
                    
                    token = results.ContinuationToken;

                    list.AddRange(results.Results.OfType<CloudBlockBlob>().Select(blob =>
                    {
                        return new BlobDescriptor
                        {
                            Name = blob.Name,
                            Container = containerName,
                            Url = blob.Uri.ToString(),
                            ContentType = blob.Properties.ContentType,
                            ContentMD5 = blob.Properties.ContentMD5,
                            ETag = blob.Properties.ETag,
                            Length = blob.Properties.Length,
                            LastModified = blob.Properties.LastModified,
                            Security = security,
                        };
                    }));
                }
                while (token != null);
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }

            return list;
        }

        public async Task SaveBlobStreamAsync(string containerName, string blobName, Stream source, BlobProperties properties = null, bool closeStream = true)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var props = properties ?? BlobProperties.Empty;
            var security = props.Security == BlobSecurity.Public ? BlobContainerPublicAccessType.Blob : BlobContainerPublicAccessType.Off;

            try
            {
                var created = await container.CreateIfNotExistsAsync(security, _requestOptions, _context).ConfigureAwait(false);

                var blob = container.GetBlockBlobReference(blobName);
                blob.Properties.ContentType = props.ContentType;                
                blob.Properties.ContentDisposition = props.ContentDisposition;                

                if (props.Metadata != null) blob.Metadata.SetMetadata(props.Metadata);

                await blob.UploadFromStreamAsync(source, null, _requestOptions, _context).ConfigureAwait(false);

                // Hack to deal with issue https://github.com/Azure/azure-storage-net/issues/353
                if (!string.IsNullOrEmpty(props.ContentDisposition)) await blob.SetPropertiesAsync(null, _requestOptions, _context);

                // Check if container permission elevation is necessary
                if (!created)
                {
                    var perms = await container.GetPermissionsAsync(null, _requestOptions, _context).ConfigureAwait(false);

                    if (properties != null && properties.Security == BlobSecurity.Public && perms.PublicAccess == BlobContainerPublicAccessType.Off) await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = security }, null, _requestOptions, _context).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
            finally
            {
                if (closeStream) source.Dispose();
            }
        }

        public async Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(blobName);

            try
            {
                await blob.FetchAttributesAsync(null, _requestOptions, _context).ConfigureAwait(false);
                blob.Properties.ContentType = properties.ContentType;
                blob.Properties.ContentDisposition = properties.ContentDisposition;

                await blob.SetPropertiesAsync(null, _requestOptions, _context).ConfigureAwait(false);

                if (properties.Metadata != null)
                {
                    blob.Metadata.SetMetadata(properties.Metadata);
                    await blob.SetMetadataAsync();
                }
                
                var perms = await container.GetPermissionsAsync(null, _requestOptions, _context).ConfigureAwait(false);

                // Elevate container permissions if necessary.
                if (properties.Security == BlobSecurity.Public && perms.PublicAccess == BlobContainerPublicAccessType.Off) await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }, null, _requestOptions, _context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (e.IsAzureStorageException()) throw e.Convert();
                throw;
            }
        }
    }
}