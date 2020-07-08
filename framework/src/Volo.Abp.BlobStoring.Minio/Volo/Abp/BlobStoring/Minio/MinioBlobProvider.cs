using Minio;
using Minio.Exceptions;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Minio
{
    public class MinioBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IMinioBlobNameCalculator MinioBlobNameCalculator { get; }

        public MinioBlobProvider(IMinioBlobNameCalculator minioBlobNameCalculator)
        {
            MinioBlobNameCalculator = minioBlobNameCalculator;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetMinioConfiguration();

            if (!args.OverrideExisting && await BlobExistsAsync(args, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{GetContainerName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(args);
            }

            await GetMinioClient(args).PutObjectAsync(GetContainerName(args), blobName, args.BlobStream, args.BlobStream.Length);
        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);

            if (await BlobExistsAsync(args, blobName))
            {
                var client = GetMinioClient(args);
                await client.RemoveObjectAsync(GetContainerName(args), blobName);
                return true;
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);

            return await BlobExistsAsync(args, blobName);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);

            if (!await BlobExistsAsync(args, blobName))
            {
                return null;
            }
            try
            {
                var client = GetMinioClient(args);
             
               var stat = await client.StatObjectAsync(GetContainerName(args), blobName);

                MemoryStream returnStream = new MemoryStream();

                await client.GetObjectAsync(GetContainerName(args), blobName,
                                                 (stream) =>
                                                 {
                                                     if (stream != null)
                                                     {
                                                         stream.CopyTo(returnStream);

                                                            // returnStream = new MemoryStream(stream.GetAllBytes());

                                                     }
                                                 });

                return returnStream;
            }
            catch (MinioException ex)
            {

            }
            return null;

        }


        private MinioClient GetMinioClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetMinioConfiguration();
            var client = new MinioClient(configuration.EndPoint, configuration.AccessKey, configuration.SecretKey);
            if (configuration.WithSSL)
            {
                client.WithSSL();
            }

            return client;

        }



        protected virtual async Task CreateBucketIfNotExists(BlobProviderArgs args)
        {
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);
            if (!await client.BucketExistsAsync(containerName))
            {
                await client.MakeBucketAsync(containerName);
            }

        }

        private async Task<bool> BlobExistsAsync(BlobProviderArgs args, string blobName)
        {
            // Make sure Blob Container exists.
            if (await ContainerExistsAsync(args))
            {
                try
                {
                    await GetMinioClient(args).StatObjectAsync(GetContainerName(args), blobName);
                    return true;
                }
                catch (MinioException ex)
                {

                }
            }
            return false;

        }
        private static string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetMinioConfiguration();

            //minio bucket name must be lower
            return configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName.ToLower()
                : configuration.BucketName.ToLower();
        }

        private async Task<bool> ContainerExistsAsync(BlobProviderArgs args)
        {
            var client = GetMinioClient(args);

            return await client.BucketExistsAsync(GetContainerName(args));
        }
    }
}
