
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.HuaweiCloud
{
    public class HuaweiCloudBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IHuaweiCloudBlobNameCalculator HuaweiCloudBlobNameCalculator { get; }
        protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

        public HuaweiCloudBlobProvider(
            IHuaweiCloudBlobNameCalculator huaweiCloudBlobNameCalculator,
            IBlobNormalizeNamingService blobNormalizeNamingService)
        {
            HuaweiCloudBlobNameCalculator = huaweiCloudBlobNameCalculator;
            BlobNormalizeNamingService = blobNormalizeNamingService;
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = HuaweiCloudBlobNameCalculator.Calculate(args);
            var configuration = args.Configuration.GetHuaweiCloudConfiguration();
            var client = GetHuaweiCloudClient(args);
            var containerName = GetContainerName(args);

            if (!args.OverrideExisting && await BlobExistsAsync(client, containerName, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateBucketIfNotExists)
            {
                await CreateBucketIfNotExists(client, containerName);
            }
            await client.PutObjectAsync(
                new PutObjectRequest()
                {
                    BucketName = containerName,
                    Key = blobName,
                    InputStream = args.BlobStream
                }
            );




        }

        public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = HuaweiCloudBlobNameCalculator.Calculate(args);
            var client = GetHuaweiCloudClient(args);
            var containerName = GetContainerName(args);

            try
            {
                if (await BlobExistsAsync(client, containerName, blobName))
                {

                    var result = await client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = args.ContainerName, Key = args.BlobName });


                    return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = HuaweiCloudBlobNameCalculator.Calculate(args);
            var client = GetHuaweiCloudClient(args);
            var containerName = GetContainerName(args);

            return await BlobExistsAsync(client, containerName, blobName);
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = HuaweiCloudBlobNameCalculator.Calculate(args);
            var client = GetHuaweiCloudClient(args);
            var containerName = GetContainerName(args);

            if (!await BlobExistsAsync(client, containerName, blobName))
            {
                return null;
            }

            var memoryStream = new MemoryStream();

            var result = await client.GetObjectAsync(new GetObjectRequest() { BucketName = containerName, Key = blobName });
            var stream = result.ResponseStream;
            if (stream != null)
            {

                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                memoryStream = null;
            }



            return memoryStream;
        }

        protected virtual IAmazonS3 GetHuaweiCloudClient(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetHuaweiCloudConfiguration();
            var config = new AmazonS3Config();
            config.ServiceURL = configuration.EndPoint;

            var client = new AmazonS3Client(configuration.AccessKey, configuration.SecretKey, config);


            return client;
        }

        protected virtual async Task CreateBucketIfNotExists(IAmazonS3 client, string containerName)
        {
            await client.EnsureBucketExistsAsync(containerName);
        }

        protected virtual async Task<bool> BlobExistsAsync(IAmazonS3 client, string containerName, string blobName)
        {
            try
            {
                var response = await client.GetObjectMetadataAsync(new GetObjectMetadataRequest()
                {
                    BucketName = containerName,
                    Key = blobName
                });

                return true;
            }

            catch (Amazon.S3.AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                //status wasn't not found, so throw the exception
                throw;
            }
        }

        protected virtual string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetHuaweiCloudConfiguration();

            return configuration.BucketName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.BucketName);
        }
    }
}
