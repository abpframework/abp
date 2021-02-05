using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aws
{
    public class AwsBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IAwsBlobNameCalculator AwsBlobNameCalculator { get; }
        protected IAmazonS3ClientFactory AmazonS3ClientFactory { get; }

        public AwsBlobProvider(IAwsBlobNameCalculator awsBlobNameCalculator,
            IAmazonS3ClientFactory amazonS3ClientFactory)
        {
            AwsBlobNameCalculator = awsBlobNameCalculator;
            AmazonS3ClientFactory = amazonS3ClientFactory;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = AwsBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetAwsConfiguration();
            var containerName = GetContainerName(args);

            using (var amazonS3Client = await GetAmazonS3Client(args))
            {
                if (!args.OverrideExisting && await BlobExistsAsync(amazonS3Client, containerName, blobName))
                {
                    throw new BlobAlreadyExistsException(
                        $"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
                }

                if (configuration.CreateContainerIfNotExists)
                {
                    await CreateContainerIfNotExists(amazonS3Client, containerName);
                }

                await amazonS3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = containerName,
                    Key = blobName,
                    InputStream = args.BlobStream
                });
            }
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AwsBlobNameCalculator.Calculate(args);
            var containerName = GetContainerName(args);

            using (var amazonS3Client = await GetAmazonS3Client(args))
            {
                if (!await BlobExistsAsync(amazonS3Client, containerName, blobName))
                {
                    return false;
                }

                await amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = containerName,
                    Key = blobName
                });

                return true;
            }
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AwsBlobNameCalculator.Calculate(args);
            var containerName = GetContainerName(args);

            using (var amazonS3Client = await GetAmazonS3Client(args))
            {
                return await BlobExistsAsync(amazonS3Client, containerName, blobName);
            }
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AwsBlobNameCalculator.Calculate(args);
            var containerName = GetContainerName(args);

            using (var amazonS3Client = await GetAmazonS3Client(args))
            {
                if (!await BlobExistsAsync(amazonS3Client, containerName, blobName))
                {
                    return null;
                }

                var response = await amazonS3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = containerName,
                    Key = blobName
                });

                var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                return memoryStream;
            }
        }

        protected virtual async Task<AmazonS3Client> GetAmazonS3Client(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAwsConfiguration();
            return await AmazonS3ClientFactory.GetAmazonS3Client(configuration);
        }

        private async Task<bool> BlobExistsAsync(AmazonS3Client amazonS3Client, string containerName, string blobName)
        {
            // Make sure Blob Container exists.
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, containerName))
            {
                return false;
            }

            try
            {
                await amazonS3Client.GetObjectMetadataAsync(containerName, blobName);
            }
            catch (Exception ex)
            {
                if (ex is AmazonS3Exception)
                {
                    return false;
                }

                throw;
            }

            return true;
        }

        protected virtual async Task CreateContainerIfNotExists(AmazonS3Client amazonS3Client, string containerName)
        {
            if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, containerName))
            {
                await amazonS3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = containerName
                });
            }
        }

        private static string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetAwsConfiguration();
            return configuration.ContainerName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.ContainerName;
        }
    }
}
