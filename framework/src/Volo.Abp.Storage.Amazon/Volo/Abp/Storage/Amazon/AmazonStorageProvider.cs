using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Storage.Amazon
{
    public class AmazonStorageProvider : IStorageProvider, ISingletonDependency
    {
        private const string DefaultServiceUrl = "https://s3.amazonaws.com";
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucket;
        private readonly string _serverSideEncryptionMethod;
        private readonly string _serviceUrl;

        public AmazonStorageProvider(AmazonProviderOptions options)
        {
            _serviceUrl = string.IsNullOrEmpty(options.ServiceUrl) ? DefaultServiceUrl : options.ServiceUrl;
            _bucket = options.Bucket;
            _serverSideEncryptionMethod = options.ServerSideEncryptionMethod;
            var s3Config = new AmazonS3Config
            {
                ServiceURL = _serviceUrl,
                Timeout = options.Timeout ?? ClientConfig.MaxTimeout,
            };
            _s3Client = new AmazonS3Client(ReadAwsCredentials(options), s3Config);
        }

        private AWSCredentials ReadAwsCredentials(AmazonProviderOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.ProfileName))
            {
                var credentialProfileStoreChain = new CredentialProfileStoreChain();
                if (credentialProfileStoreChain.TryGetAWSCredentials(options.ProfileName, out var defaultCredentials))
                {
                    return defaultCredentials;
                }

                throw new AmazonClientException("Unable to find a default profile in CredentialProfileStoreChain.");
            }

            if (!string.IsNullOrEmpty(options.PublicKey) && !string.IsNullOrWhiteSpace(options.SecretKey))
                return new BasicAWSCredentials(options.PublicKey, options.SecretKey);
            return new EnvironmentVariablesAWSCredentials();
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var key = GenerateKeyName(containerName, blobName);
            var objectDeleteRequest = new DeleteObjectRequest() {BucketName = _bucket, Key = key};
            try
            {
                await _s3Client.DeleteObjectAsync(objectDeleteRequest);
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task CopyBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName, string destinationBlobName = null)
        {
            if (string.IsNullOrEmpty(sourceContainerName))
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(sourceContainerName)}");
            if (string.IsNullOrEmpty(sourceBlobName))
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(sourceBlobName)}");
            if (string.IsNullOrEmpty(destinationContainerName))
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(destinationContainerName)}");
            if (destinationBlobName == string.Empty)
                throw new StorageException(StorageErrorCode.InvalidName, $"Invalid {nameof(destinationBlobName)}");
            var sourceKey = GenerateKeyName(sourceContainerName, sourceBlobName);
            var destinationKey = GenerateKeyName(destinationContainerName, destinationBlobName ?? sourceBlobName);
            try
            {
                var request = new CopyObjectRequest
                {
                    SourceBucket = _bucket,
                    SourceKey = sourceKey,
                    DestinationBucket = _bucket,
                    DestinationKey = destinationKey,
                    ServerSideEncryptionMethod = _serverSideEncryptionMethod
                };
                var response = await _s3Client.CopyObjectAsync(request);
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new StorageException(StorageErrorCode.GenericException, "Copy failed.");
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task MoveBlobAsync(string sourceContainerName, string sourceBlobName,
            string destinationContainerName, string destinationBlobName = null)
        {
            await CopyBlobAsync(sourceContainerName, sourceBlobName, destinationContainerName, destinationBlobName);
            await DeleteBlobAsync(sourceContainerName, sourceBlobName);
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            var objectsRequest =
                new ListObjectsRequest {BucketName = _bucket, Prefix = containerName, MaxKeys = 100000};
            var keys = new List<KeyVersion>();
            try
            {
                do
                {
                    var objectsResponse = await _s3Client.ListObjectsAsync(objectsRequest);
                    keys.AddRange(
                        objectsResponse.S3Objects.Select(x => new KeyVersion() {Key = x.Key, VersionId = null}));

                    // If response is truncated, set the marker to get the next set of keys.
                    if (objectsResponse.IsTruncated)
                        objectsRequest.Marker = objectsResponse.NextMarker;
                    else
                        objectsRequest = null;
                } while (objectsRequest != null);

                if (keys.Count > 0)
                {
                    var objectsDeleteRequest = new DeleteObjectsRequest() {BucketName = _bucket, Objects = keys};
                    await _s3Client.DeleteObjectsAsync(objectsDeleteRequest);
                }
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task<BlobDescriptor> GetBlobDescriptorAsync(string containerName, string blobName)
        {
            var key = GenerateKeyName(containerName, blobName);
            try
            {
                var objectMetaRequest = new GetObjectMetadataRequest() {BucketName = _bucket, Key = key};
                var objectMetaResponse = await _s3Client.GetObjectMetadataAsync(objectMetaRequest);
                var objectAclRequest = new GetACLRequest() {BucketName = _bucket, Key = key};
                var objectAclResponse = await _s3Client.GetACLAsync(objectAclRequest);
                var isPublic = objectAclResponse.AccessControlList.Grants.Any(x =>
                    x.Grantee.URI == "http://acs.amazonaws.com/groups/global/AllUsers");
                return new BlobDescriptor
                {
                    Name = blobName,
                    Container = containerName,
                    Length = objectMetaResponse.Headers.ContentLength,
                    ETag = objectMetaResponse.ETag,
                    ContentMD5 = objectMetaResponse.ETag,
                    ContentType = objectMetaResponse.Headers.ContentType,
                    ContentDisposition = objectMetaResponse.Headers.ContentDisposition,
                    LastModified = objectMetaResponse.LastModified,
                    Security = isPublic ? BlobSecurity.Public : BlobSecurity.Private,
                    Metadata = objectMetaResponse.Metadata.ToMetadata(),
                };
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task<Stream> GetBlobStreamAsync(string containerName, string blobName)
        {
            try
            {
                return await _s3Client.GetObjectStreamAsync(_bucket, GenerateKeyName(containerName, blobName), null);
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public string GetBlobUrl(string containerName, string blobName)
        {
            return $"{_serviceUrl}/{_bucket}/{GenerateKeyName(containerName, blobName)}";
        }

        public string GetBlobSasUrl(string containerName, string blobName, DateTimeOffset expiry,
            bool isDownload = false, string fileName = null, string contentType = null,
            BlobUrlAccess access = BlobUrlAccess.Read)
        {
            var headers = new ResponseHeaderOverrides();
            if (isDownload) headers.ContentDisposition = "attachment;";
            if (!string.IsNullOrEmpty(fileName)) headers.ContentDisposition += "filename=\"" + fileName + "\"";
            if (!string.IsNullOrEmpty(contentType)) headers.ContentType = contentType;
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _bucket,
                Key = GenerateKeyName(containerName, blobName),
                Expires = expiry.UtcDateTime,
                ResponseHeaderOverrides = headers,
                Verb = access == BlobUrlAccess.Read ? HttpVerb.GET : HttpVerb.PUT
            };
            try
            {
                return _s3Client.GetPreSignedURL(urlRequest);
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task<IList<BlobDescriptor>> ListBlobsAsync(string containerName)
        {
            var descriptors = new List<BlobDescriptor>();
            var objectsRequest =
                new ListObjectsRequest {BucketName = _bucket, Prefix = containerName, MaxKeys = 100000};
            try
            {
                do
                {
                    var objectsResponse = await _s3Client.ListObjectsAsync(objectsRequest);
                    foreach (var entry in objectsResponse.S3Objects)
                    {
                        var objectMetaRequest = new GetObjectMetadataRequest() {BucketName = _bucket, Key = entry.Key};
                        var objectMetaResponse = await _s3Client.GetObjectMetadataAsync(objectMetaRequest);
                        var objectAclRequest = new GetACLRequest() {BucketName = _bucket, Key = entry.Key};
                        var objectAclResponse = await _s3Client.GetACLAsync(objectAclRequest);
                        var isPublic = objectAclResponse.AccessControlList.Grants.Any(x =>
                            x.Grantee.URI == "http://acs.amazonaws.com/groups/global/AllUsers");
                        descriptors.Add(new BlobDescriptor
                        {
                            Name = entry.Key.Remove(0, containerName.Length + 1),
                            Container = containerName,
                            Length = entry.Size,
                            ETag = entry.ETag,
                            ContentMD5 = entry.ETag,
                            ContentType = objectMetaResponse.Headers.ContentType,
                            LastModified = entry.LastModified,
                            Security = isPublic ? BlobSecurity.Public : BlobSecurity.Private,
                            ContentDisposition = objectMetaResponse.Headers.ContentDisposition,
                            Metadata = objectMetaResponse.Metadata.ToMetadata(),
                        });
                    }

                    // If response is truncated, set the marker to get the next set of keys.
                    if (objectsResponse.IsTruncated)
                        objectsRequest.Marker = objectsResponse.NextMarker;
                    else
                        objectsRequest = null;
                } while (objectsRequest != null);

                return descriptors;
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        public async Task SaveBlobStreamAsync(string containerName, string blobName, Stream source,
            BlobProperties properties = null, bool closeStream = true)
        {
            if (source.Length >= 100000000)
            {
                var fileTransferUtilityRequest =
                    CreateChunkedUpload(containerName, blobName, source, properties, closeStream);
                try
                {
                    await new TransferUtility(_s3Client).UploadAsync(fileTransferUtilityRequest);
                }
                catch (AmazonS3Exception asex)
                {
                    throw asex.ToStorageException();
                }
            }
            else
            {
                var putRequest = CreateUpload(containerName, blobName, source, properties, closeStream);
                try
                {
                    await _s3Client.PutObjectAsync(putRequest);
                }
                catch (AmazonS3Exception asex)
                {
                    throw asex.ToStorageException();
                }
            }
        }

        public async Task UpdateBlobPropertiesAsync(string containerName, string blobName, BlobProperties properties)
        {
            var updateRequest = CreateUpdateRequest(containerName, blobName, properties);
            try
            {
                await _s3Client.CopyObjectAsync(updateRequest);
            }
            catch (AmazonS3Exception asex)
            {
                throw asex.ToStorageException();
            }
        }

        private S3CannedACL GetCannedACL(BlobProperties properties)
        {
            return properties?.Security == BlobSecurity.Public ? S3CannedACL.PublicRead : S3CannedACL.Private;
        }

        private static string GenerateKeyName(string containerName, string blobName)
        {
            return $"{containerName}/{blobName}";
        }

        private CopyObjectRequest CreateUpdateRequest(string containerName, string blobName, BlobProperties properties)
        {
            var updateRequest = new CopyObjectRequest()
            {
                SourceBucket = _bucket,
                SourceKey = GenerateKeyName(containerName, blobName),
                DestinationBucket = _bucket,
                DestinationKey = GenerateKeyName(containerName, blobName),
                ContentType = properties?.ContentType,
                CannedACL = GetCannedACL(properties),
                MetadataDirective = S3MetadataDirective.REPLACE,
                ServerSideEncryptionMethod = _serverSideEncryptionMethod
            };
            updateRequest.Headers.ContentDisposition = properties?.ContentDisposition;
            updateRequest.Metadata.AddMetadata(properties?.Metadata);
            return updateRequest;
        }

        private TransferUtilityUploadRequest CreateChunkedUpload(string containerName, string blobName, Stream source,
            BlobProperties properties, bool closeStream)
        {
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = _bucket,
                InputStream = source,
                PartSize = 6291456,
                Key = GenerateKeyName(containerName, blobName),
                ContentType = properties?.ContentType,
                CannedACL = GetCannedACL(properties),
                AutoCloseStream = closeStream,
                ServerSideEncryptionMethod = _serverSideEncryptionMethod
            };
            fileTransferUtilityRequest.Headers.ContentDisposition = properties?.ContentDisposition;
            fileTransferUtilityRequest.Metadata.AddMetadata(properties?.Metadata);
            return fileTransferUtilityRequest;
        }

        private PutObjectRequest CreateUpload(string containerName, string blobName, Stream source,
            BlobProperties properties, bool closeStream)
        {
            var putRequest = new PutObjectRequest()
            {
                BucketName = _bucket,
                Key = GenerateKeyName(containerName, blobName),
                InputStream = source,
                ContentType = properties?.ContentType,
                CannedACL = GetCannedACL(properties),
                AutoCloseStream = closeStream,
                ServerSideEncryptionMethod = _serverSideEncryptionMethod
            };
            putRequest.Headers.ContentDisposition = properties?.ContentDisposition;
            putRequest.Metadata.AddMetadata(properties?.Metadata);
            return putRequest;
        }
    }
}