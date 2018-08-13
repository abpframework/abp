using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core;
using Volo.Abp.Storage.Azure.Configuration;
using Volo.Abp.Storage.Azure.Internal;
using Volo.Abp.Storage.Configuration;
using Volo.Abp.Storage.Exceptions;

namespace Volo.Abp.Storage.Azure
{
    public class AbpAzureStore : IAbpStore
    {
        private readonly Lazy<CloudBlobClient> _client;
        private readonly Lazy<CloudBlobContainer> _container;
        private readonly OperationContext _context;
        private readonly BlobRequestOptions _requestOptions;
        private readonly AbpAzureStoreOptions _storeOptions;

        public AbpAzureStore(AbpAzureStoreOptions storeOptions)
        {
            if (storeOptions == null)
                throw new ArgumentNullException(nameof(storeOptions));

            storeOptions.Validate();

            _storeOptions = storeOptions;
            _client = new Lazy<CloudBlobClient>(() =>
                CloudStorageAccount.Parse(storeOptions.ConnectionString).CreateCloudBlobClient());
            _container =
                new Lazy<CloudBlobContainer>(() => _client.Value.GetContainerReference(storeOptions.ContainerName));
            _requestOptions = new BlobRequestOptions();
            _context = new OperationContext();
        }

        public string Name => _storeOptions.Name;

        public Task InitAsync()
        {
            BlobContainerPublicAccessType accessType;
            switch (_storeOptions.AccessLevel)
            {
                case BlobAccessLevel.Public:
                    accessType = BlobContainerPublicAccessType.Container;
                    break;
                case BlobAccessLevel.Confidential:
                    accessType = BlobContainerPublicAccessType.Blob;
                    break;
                case BlobAccessLevel.Private:
                default:
                    accessType = BlobContainerPublicAccessType.Off;
                    break;
            }

            return _container.Value.CreateIfNotExistsAsync(accessType, null, null);
        }

        public async ValueTask<IBlobReference[]> ListBlobAsync(string path, bool recursive, bool withMetadata)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = null;
            }
            else
            {
                if (!path.EndsWith("/")) path = path + "/";
            }

            BlobContinuationToken continuationToken = null;

            var results = new List<IListBlobItem>();

            do
            {
                var response = await _container.Value.ListBlobsSegmentedAsync(path, recursive,
                    withMetadata ? BlobListingDetails.Metadata : BlobListingDetails.None, null, continuationToken,
                    new BlobRequestOptions(), new OperationContext());
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            } while (continuationToken != null);

            return results.OfType<ICloudBlob>().Select(blob => new AzureBlobReference(blob, withMetadata))
                .ToArray();
        }

        public async ValueTask<IBlobReference[]> ListBlobAsync(string path, string searchPattern, bool recursive,
            bool withMetadata)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = null;
            }
            else
            {
                if (!path.EndsWith("/")) path = path + "/";
            }

            var prefix = path;
            var firstWildCard = searchPattern.IndexOf('*');

            if (firstWildCard >= 0)
            {
                prefix += searchPattern.Substring(0, firstWildCard);
                searchPattern = searchPattern.Substring(firstWildCard);
            }

            var matcher = new Matcher(StringComparison.Ordinal);
            matcher.AddInclude(searchPattern);

            var operationContext = new OperationContext();
            BlobContinuationToken continuationToken = null;
            var results = new List<IListBlobItem>();

            do
            {
                var response = await _container.Value.ListBlobsSegmentedAsync(prefix, recursive,
                    withMetadata ? BlobListingDetails.Metadata : BlobListingDetails.None, null, continuationToken,
                    new BlobRequestOptions(), new OperationContext());
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            } while (continuationToken != null);

            var pathMap = results.OfType<ICloudBlob>()
                .Select(blob => new AzureBlobReference(blob, withMetadata)).ToDictionary(x => x.Path);

            var filteredResults = matcher.Execute(
                new AzureListDirectoryWrapper(path,
                    pathMap));

            return filteredResults.Files.Select(x => pathMap[path + x.Path]).ToArray();
        }

        public async ValueTask<IBlobReference> GetBlobAsync(IPrivateBlobReference file, bool withMetadata)
        {
            return await InternalGetAsync(file, withMetadata);
        }

        public async ValueTask<IBlobReference> GetBlobAsync(Uri uri, bool withMetadata)
        {
            return await InternalGetAsync(uri, withMetadata);
        }

        public async Task DeleteBlobAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            await fileReference.DeleteBlobAsync();
        }

        public async Task CopyBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName,
            string destinationBlobName = null)
        {
            var sourceContainer = _client.Value.GetContainerReference(sourceContainerName);
            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceBlobName);

            var destContainer = _client.Value.GetContainerReference(destinationContainerName);

            await destContainer.CreateIfNotExistsAsync(sourceContainer.Properties.PublicAccess ??
                                                       BlobContainerPublicAccessType.Off, _requestOptions, _context)
                .ConfigureAwait(false);

            var destBlob = destContainer.GetBlockBlobReference(destinationBlobName ?? sourceBlobName);

            await destBlob.StartCopyAsync(sourceBlob).ConfigureAwait(false);

            while (destBlob.CopyState.Status == CopyStatus.Pending)
            {
                await Task.Delay(500).ConfigureAwait(false);
                await destBlob.FetchAttributesAsync().ConfigureAwait(false);
            }

            if (destBlob.CopyState.Status != CopyStatus.Success)
                throw new Exception("Copy failed: " + destBlob.CopyState.Status);
        }

        public async ValueTask<Stream> ReadBlobAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadInMemoryAsync();
        }

        public async ValueTask<byte[]> ReadBlobBytesAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadBlobBytesAsync();
        }

        public async ValueTask<string> ReadBlobTextAsync(IPrivateBlobReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadBlobTextAsync();
        }

        public async ValueTask<IBlobReference> SaveBlobAsync(byte[] data, IPrivateBlobReference file,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            using (var stream = new SyncMemoryStream(data, 0, data.Length))
            {
                return await SaveBlobAsync(stream, file, contentType, overwritePolicy, metadata);
            }
        }

        public async ValueTask<IBlobReference> SaveBlobAsync(Stream data, IPrivateBlobReference file,
            string contentType, OverwritePolicy overwritePolicy = OverwritePolicy.Always,
            IDictionary<string, string> metadata = null)
        {
            var uploadBlob = true;
            var blockBlob = _container.Value.GetBlockBlobReference(file.Path);
            var blobExists = await blockBlob.ExistsAsync();

            if (blobExists)
            {
                if (overwritePolicy == OverwritePolicy.Never) throw new FileAlreadyExistsException(Name, file.Path);

                await blockBlob.FetchAttributesAsync();

                if (overwritePolicy == OverwritePolicy.IfContentModified)
                    using (var md5 = MD5.Create())
                    {
                        data.Seek(0, SeekOrigin.Begin);
                        var contentMd5 = Convert.ToBase64String(md5.ComputeHash(data));
                        data.Seek(0, SeekOrigin.Begin);
                        uploadBlob = contentMd5 != blockBlob.Properties.ContentMD5;
                    }
            }

            if (metadata != null)
                foreach (var kvp in metadata)
                    blockBlob.Metadata.Add(kvp.Key, kvp.Value);

            if (uploadBlob) await blockBlob.UploadFromStreamAsync(data);

            var reference = new AzureBlobReference(blockBlob, true);

            if (reference.BlobDescriptor.ContentType == contentType) return reference;

            reference.BlobDescriptor.ContentType = contentType;
            await reference.SaveBlobDescriptorAsync();

            return reference;
        }

        public ValueTask<string> GetBlobSasUrlAsync(ISharedAccessPolicy policy)
        {
            var adHocPolicy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = policy.StartTime,
                SharedAccessExpiryTime = policy.ExpiryTime,
                Permissions = FromGenericToAzure(policy.Permissions)
            };

            return new ValueTask<string>(_container.Value.GetSharedAccessSignature(adHocPolicy));
        }

        internal static SharedAccessBlobPermissions FromGenericToAzure(SharedAccessPermissions permissions)
        {
            var result = SharedAccessBlobPermissions.None;

            if (permissions.HasFlag(SharedAccessPermissions.Add)) result |= SharedAccessBlobPermissions.Add;

            if (permissions.HasFlag(SharedAccessPermissions.Create)) result |= SharedAccessBlobPermissions.Create;

            if (permissions.HasFlag(SharedAccessPermissions.Delete)) result |= SharedAccessBlobPermissions.Delete;

            if (permissions.HasFlag(SharedAccessPermissions.List)) result |= SharedAccessBlobPermissions.List;

            if (permissions.HasFlag(SharedAccessPermissions.Read)) result |= SharedAccessBlobPermissions.Read;

            if (permissions.HasFlag(SharedAccessPermissions.Write)) result |= SharedAccessBlobPermissions.Write;

            return result;
        }

        private ValueTask<AzureBlobReference> InternalGetAsync(IPrivateBlobReference file, bool withMetadata = false)
        {
            return InternalGetAsync(new Uri(file.Path, UriKind.Relative), withMetadata);
        }

        private async ValueTask<AzureBlobReference> InternalGetAsync(Uri uri, bool withMetadata)
        {
            try
            {
                ICloudBlob blob;

                if (uri.IsAbsoluteUri)
                {
                    blob = await _client.Value.GetBlobReferenceFromServerAsync(uri);
                    withMetadata = true;
                }
                else
                {
                    if (withMetadata)
                    {
                        blob = await _container.Value.GetBlobReferenceFromServerAsync(uri.ToString());
                    }
                    else
                    {
                        blob = _container.Value.GetBlockBlobReference(uri.ToString());
                        if (!await blob.ExistsAsync()) return null;
                    }
                }

                return new AzureBlobReference(blob, withMetadata);
            }
            catch (StorageException storageException)
            {
                if (storageException.RequestInformation.HttpStatusCode == 404) return null;

                throw;
            }
        }
    }
}