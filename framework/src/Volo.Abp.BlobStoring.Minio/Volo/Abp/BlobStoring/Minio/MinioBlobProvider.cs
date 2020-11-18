﻿using Minio;
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

        public MinioBlobProvider(IMinioBlobNameCalculator minioBlobNameCalculator)
        {
            MinioBlobNameCalculator = minioBlobNameCalculator;
        }

        public async override Task SaveAsync(BlobProviderSaveArgs args)
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

        public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
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

        public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            return await BlobExistsAsync(client, containerName, blobName);
        }

        public async override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = MinioBlobNameCalculator.Calculate(args);
            var client = GetMinioClient(args);
            var containerName = GetContainerName(args);

            if (!await BlobExistsAsync(client, containerName, blobName))
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await client.GetObjectAsync(containerName, blobName,  (stream) =>
            {
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream);
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

        private async Task<bool> BlobExistsAsync(MinioClient client, string containerName , string blobName)
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

        private static string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetMinioConfiguration();

            return configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.BucketName;
        }
    }
}
