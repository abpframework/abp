using Minio;
using Minio.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Minio
{
    public class MinioBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IMinioBlobNameCalculator MinioBlobNameCalculator { get; }
        protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

        public MinioBlobProvider(
            IMinioBlobNameCalculator minioBlobNameCalculator,
            IBlobNormalizeNamingService blobNormalizeNamingService)
        {
            MinioBlobNameCalculator = minioBlobNameCalculator;
            BlobNormalizeNamingService = blobNormalizeNamingService;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetMinioConfiguration();
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            if (!args.OverrideExisting && await BlobExistsAsync(client, containerName, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(client, containerName);
            }

            await client.PutObjectAsync(containerName, blobName, args.BlobStream, args.BlobStream.Length);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            if (await BlobExistsAsync(client, containerName, blobName))
            {
                await client.RemoveObjectAsync(containerName, blobName);
                return true;
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            return await BlobExistsAsync(client, containerName, blobName);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            if (!await BlobExistsAsync(client, containerName, blobName))
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await client.GetObjectAsync(containerName, blobName, (stream) =>
            {
                if (stream != null)
                {
                    stream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                }
                else
                {
                    memoryStream = null;
                }
            });

            return memoryStream;
        }

        protected virtual MinioClient GetMinioClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetMinioConfiguration();
            var client = new MinioClient(configuration.EndPoint, configuration.AccessKey, configuration.SecretKey);

            if (configuration.WithSSL)
            {
                client.WithSSL();
            }

            return client;
        }

        protected virtual async Task CreateBucketIfNotExists(MinioClient client,string containerName)
        {
            if (!await client.BucketExistsAsync(containerName))
            {
                await client.MakeBucketAsync(containerName);
            }
        }

        protected virtual async Task<bool> BlobExistsAsync(MinioClient client, string containerName , string blobName)
        {
            // Make sure Blob Container exists.
            if (await client.BucketExistsAsync(containerName))
            {
                try
                {
                    await client.StatObjectAsync(containerName, blobName);
                }
                catch (Exception e)
                {
                    if (e is ObjectNotFoundException)
                    {
                        return false;
                    }

                    throw;
                }

                return true;
            }

            return false;
        }

        protected virtual string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetMinioConfiguration();

            return configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.BucketName);
        }
    }
}
