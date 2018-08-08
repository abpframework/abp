using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google;
using Google.Apis.Upload;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Blob = Google.Apis.Storage.v1.Data.Object;
using PredefinedAcl = Google.Apis.Storage.v1.ObjectsResource.InsertMediaUpload.PredefinedAclEnum;
using Google.Apis.Requests;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Google.Cloud.Storage.V1;
using BlobObject = Google.Apis.Storage.v1.Data.Object;
using Google.Apis.Storage.v1.Data;
using System.Net.Http;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.Google
{
    public sealed class GoogleStorageProvider : IStorageProvider, ITransientDependency
    {
        private const string BlobNameRegex = @"(?<Container>[^/]+)/(?<Blob>.+)";
        private const string DefaultContentType = "application/octet-stream";
        private readonly StorageClient _client;
        private readonly UrlSigner _urlSigner = null;
        private readonly string _bucket;
        private readonly string _serviceEmail;

        public GoogleStorageProvider(GoogleCredential credential, GoogleProviderOptions options)
        {
            if (credential == null) throw new ArgumentNullException(nameof(credential));

            _client = StorageClient.Create(credential);
            _serviceEmail = options.Email;
            _bucket = options.Bucket;

            if (credential.UnderlyingCredential is ServiceAccountCredential cred) _urlSigner = UrlSigner.FromServiceAccountCredential(cred);
        }

        public async Task SaveBlobStreamAsync(string containerName, string blobName, Stream source,
            BlobProperties properties = null, bool closeStream = true)
        {
            try
            {
                await _client.UploadObjectAsync(_bucket,
                    ObjectName(containerName, blobName), properties?.ContentType, source);
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            try
            {
                var stream = new MemoryStream();
                await _client.DownloadObjectAsync(_bucket,
                    ObjectName(containerName, blobName), stream);

                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            try
            {
                return _client.GetObject(_bucket, ObjectName(containerName, blobName)).MediaLink;
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry,
            bool isDownload = false,
            string fileName = null, string contentType = null, BlobUrlAccess access = BlobUrlAccess.Read)
        {
            if (_urlSigner == null)
                throw new StorageException(StorageErrorCode.InvalidCredentials,
                    "URL Signer requires ServiceAccountCredentials");

            var headers = new Dictionary<string, IEnumerable<string>>();

            if (!string.IsNullOrEmpty(fileName))
            {
                var header = $"filename=\"{fileName}\"";

                headers["Content-Disposition"] = new[] {isDownload ? "attachment; " + header : header};
            }
            else if (isDownload)
            {
                headers["Content-Disposition"] = new[] {"attachment;"};
            }

            if (!string.IsNullOrEmpty(contentType)) headers["Content-Type"] = new[] {contentType};

            return _urlSigner.Sign(
                _bucket,
                ObjectName(containerName, blobName),
                expiry,
                access == BlobUrlAccess.Read ? HttpMethod.Get : HttpMethod.Put,
                headers);
        }

        public async Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            try
            {
                var obj = await _client.GetObjectAsync(_bucket, ObjectName(containerName, blobName),
                    new GetObjectOptions {Projection = Projection.Full});

                return GetBlobDescriptor(obj);
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public async Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            try
            {
                return await _client.ListObjectsAsync(_bucket, containerName,
                        new ListObjectsOptions {Projection = Projection.Full})
                    .Select(GetBlobDescriptor)
                    .ToList();
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            try
            {
                await _client.DeleteObjectAsync(_bucket, ObjectName(containerName, blobName));
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            try
            {
                var batch = new BatchRequest(_client.Service);

                foreach (var blob in await ListBlobsAsync(containerName))
                    batch.Queue<string>(_client.Service.Objects.Delete(_bucket, ObjectName(blob.Container, blob.Name)),
                        (content, error, i, message) => { });

                await batch.ExecuteAsync();
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        public Task CopyBlobAsync(string sourceContainerName, string sourceBlobName, string destinationContainerName,
            string destinationBlobName = null)
        {
            return _client.CopyObjectAsync(_bucket, ObjectName(sourceContainerName, sourceBlobName),
                _bucket, ObjectName(destinationContainerName, destinationBlobName ?? sourceBlobName));
        }

        public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName,
            string destinationBlobName = null)
        {
            await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
            await DeleteBlobAsync(sourceContainerName, sourceBlobName);
        }

        public async Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));

            try
            {
                await _client.UpdateObjectAsync(new BlobObject
                {
                    Bucket = _bucket,
                    Name = ObjectName(containerName, blobName),
                    ContentType = properties.ContentType,
                    ContentDisposition = properties.ContentDisposition,
                    Metadata = properties.Metadata,
                }, new UpdateObjectOptions
                {
                    PredefinedAcl = properties.Security == BlobSecurity.Public
                        ? PredefinedObjectAcl.PublicRead
                        : PredefinedObjectAcl.Private,
                });
            }
            catch (GoogleApiException gae)
            {
                throw Error(gae);
            }
        }

        #region Helpers

        private Blob CreateBlob(string containerName, string blobName, BlobProperties properties = null)
        {
            return new Blob
            {
                Name = $"{containerName}/{blobName}",
                ContentType = properties?.ContentType ?? DefaultContentType
            };
        }

        private string ObjectName(string containerName, string blobName)
        {
            return $"{containerName}/{blobName}";
        }

        private List<ObjectAccessControl> GetPublicAcl()
        {
            return new List<ObjectAccessControl>
            {
                new ObjectAccessControl
                {
                    Role = "OWNER",
                    Entity = "allUsers"
                }
            };
        }

        private BlobDescriptor GetBlobDescriptor(Blob blob)
        {
            var match = Regex.Match(blob.Name, BlobNameRegex);
            if (!match.Success) throw new InvalidOperationException("Unable to match blob name with regex; all blob names");

            var blobDescriptor = new BlobDescriptor
            {
                Container = match.Groups["Container"].Value,
                ContentMD5 = blob.Md5Hash,
                ContentType = blob.ContentType,
                ETag = blob.ETag,
                LastModified = blob.Updated,
                Length = (long) blob.Size.GetValueOrDefault(),
                Name = match.Groups["Blob"].Value,
                Security = blob.Acl != null
                           && blob.Acl.Any(acl => acl.Entity.ToLowerInvariant() == "allusers")
                    ? BlobSecurity.Public
                    : BlobSecurity.Private,
                Url = blob.MediaLink
            };

            return blobDescriptor;
        }

        private StorageException Error(GoogleApiException gae, int code = 1001, string message = null)
        {
            return new StorageException(new StorageError
            {
                Code = code,
                Message =
                    message ?? "Encountered an error when making a request to Google's Cloud API.",
                ProviderMessage = gae?.Message
            }, gae);
        }

        #endregion
    }
}