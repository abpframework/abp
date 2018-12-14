using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core;
using Volo.Abp.Storage.Configuration;

namespace Volo.Abp.Storage.Azure
{
    public class AbpAzureStore : IAbpStore
    {
        private readonly AzureStoreOptions _storeOptions;
        private readonly Lazy<CloudBlobClient> _client;
        private readonly Lazy<CloudBlobContainer> _container;

        public AbpAzureStore(AzureStoreOptions storeOptions)
        {
            storeOptions.Validate();

            _storeOptions = storeOptions;
            
            _client = new Lazy<CloudBlobClient>(() =>
                CloudStorageAccount.Parse(storeOptions.ConnectionString).CreateCloudBlobClient());
            
            _container =
                new Lazy<CloudBlobContainer>(() => _client.Value.GetContainerReference(storeOptions.FolderName));
        }

        public string Name => _storeOptions.Name;

        public Task InitAsync()
        {
            BlobContainerPublicAccessType accessType;
            switch (_storeOptions.AccessLevel)
            {
                case AbpStorageAccessLevel.Public:
                    accessType = BlobContainerPublicAccessType.Container;
                    break;
                case AbpStorageAccessLevel.Confidential:
                    accessType = BlobContainerPublicAccessType.Blob;
                    break;
                case AbpStorageAccessLevel.Private:
                    accessType = BlobContainerPublicAccessType.Off;
                    break;
                default:
                    accessType = BlobContainerPublicAccessType.Off;
                    break;
            }

            return _container.Value.CreateIfNotExistsAsync(accessType, null, null);
        }

        public async ValueTask<IFileReference[]> ListAsync(string path, bool recursive, bool withMetadata)
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

            return results.OfType<ICloudBlob>().Select(blob => new AzureFileReference(blob, withMetadata))
                .ToArray();
        }

        public async ValueTask<IFileReference[]> ListAsync(string path, string searchPattern, bool recursive,
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

            var matcher = new Microsoft.Extensions.FileSystemGlobbing.Matcher(StringComparison.Ordinal);
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
                .Select(blob => new AzureFileReference(blob, withMetadata)).ToDictionary(x => x.Path);

            var filteredResults = matcher.Execute(
                new AzureListDirectoryWrapper(path,
                    pathMap));

            return filteredResults.Files.Select(x => pathMap[path + x.Path]).ToArray();
        }

        public async ValueTask<IFileReference> GetAsync(IPrivateFileReference file, bool withMetadata)
        {
            return await InternalGetAsync(file, withMetadata);
        }

        public async ValueTask<IFileReference> GetAsync(Uri uri, bool withMetadata)
        {
            return await InternalGetAsync(uri, withMetadata);
        }

        public async Task DeleteAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            await fileReference.DeleteAsync();
        }

        public async ValueTask<Stream> ReadAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadInMemoryAsync();
        }

        public async ValueTask<byte[]> ReadAllBytesAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadAllBytesAsync();
        }

        public async ValueTask<string> ReadAllTextAsync(IPrivateFileReference file)
        {
            var fileReference = await InternalGetAsync(file);
            return await fileReference.ReadAllTextAsync();
        }

        public async ValueTask<IFileReference> SaveAsync(byte[] data, IPrivateFileReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            using (var stream = new SyncMemoryStream(data, 0, data.Length))
            {
                return await SaveAsync(stream, file, contentType, overwritePolicy, metadata);
            }
        }

        public async ValueTask<IFileReference> SaveAsync(Stream data, IPrivateFileReference file, string contentType,
            OverwritePolicy overwritePolicy = OverwritePolicy.Always, IDictionary<string, string> metadata = null)
        {
            var uploadBlob = true;
            var blockBlob = _container.Value.GetBlockBlobReference(file.Path);
            var blobExists = await blockBlob.ExistsAsync();

            if (blobExists)
            {
                if (overwritePolicy == OverwritePolicy.Never) throw new Exceptions.FileAlreadyExistsException(Name, file.Path);

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
                foreach (var kvp in metadata) blockBlob.Metadata.Add(kvp.Key, kvp.Value);

            if (uploadBlob) await blockBlob.UploadFromStreamAsync(data);

            var reference = new AzureFileReference(blockBlob, true);

            if (reference.Properties.ContentType == contentType)
                return reference;

            reference.Properties.ContentType = contentType;
            await reference.SavePropertiesAsync();

            return reference;
        }

        public ValueTask<string> GetSharedAccessSignatureAsync(ISharedAccessPolicy policy)
        {
            var adHocPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = policy.StartTime,
                SharedAccessExpiryTime = policy.ExpiryTime,
                Permissions = FromGenericToAzure(policy.Permissions),
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

        private ValueTask<AzureFileReference> InternalGetAsync(IPrivateFileReference file, bool withMetadata = false)
        {
            return InternalGetAsync(new Uri(file.Path, UriKind.Relative), withMetadata);
        }

        private async ValueTask<AzureFileReference> InternalGetAsync(Uri uri, bool withMetadata)
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

                return new AzureFileReference(blob, withMetadata);
            }
            catch (StorageException storageException)
            {
                if (storageException.RequestInformation.HttpStatusCode == 404) return null;

                throw;
            }
        }
    }
}