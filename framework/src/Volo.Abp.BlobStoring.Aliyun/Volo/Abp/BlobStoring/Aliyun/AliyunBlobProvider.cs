using Aliyun.OSS;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aliyun
{
    public class AliyunBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IOssClientFactory OssClientFactory { get; }
        protected IAliyunBlobNameCalculator AliyunBlobNameCalculator { get; }

        public AliyunBlobProvider(
            IOssClientFactory ossClientFactory,
            IAliyunBlobNameCalculator aliyunBlobNameCalculator)
        {
            OssClientFactory = ossClientFactory;
            AliyunBlobNameCalculator = aliyunBlobNameCalculator;
        }

        private IOss GetOssClient(BlobContainerConfiguration blobContainerConfiguration)
        {
            var aliyunConfig = blobContainerConfiguration.GetAliyunConfiguration();
            return OssClientFactory.Create(aliyunConfig);
        }

        private IOss GetOssClient(AliyunBlobProviderConfiguration aliyunConfig)
        {
            return OssClientFactory.Create(aliyunConfig);
        }

        public override async Task SaveAsync(BlobProviderSaveArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);
            if (!args.OverrideExisting && await ExistsAsync(new BlobProviderExistsArgs(args.ContainerName, args.Configuration, args.BlobName)))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{args.ContainerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }
            var aliyunConfig = args.Configuration.GetAliyunConfiguration();
            var OssClient = GetOssClient(aliyunConfig);
            if (aliyunConfig.CreateContainerIfNotExists)
            {
                if (!OssClient.DoesBucketExist(args.ContainerName))
                {
                    OssClient.CreateBucket(args.ContainerName);
                }
            }
            OssClient.PutObject(args.ContainerName, blobName, args.BlobStream);
        }

        public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);
            var OssClient = GetOssClient(args.Configuration);
            var result = OssClient.DeleteObject(args.ContainerName, blobName);
            //TODO: undifend delete flag
            //https://help.aliyun.com/document_detail/91924.html
            return Task.FromResult(true);
        }

        public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);
            var OssClient = GetOssClient(args.Configuration);
            return Task.FromResult(OssClient.DoesObjectExist(args.ContainerName, blobName));
        }

        public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var blobName = AliyunBlobNameCalculator.Calculate(args);

            var OssClient = GetOssClient(args.Configuration);
            if (!await ExistsAsync(new BlobProviderExistsArgs(args.ContainerName, args.Configuration, args.BlobName)))
            {
                return null;
            }
            var result = OssClient.GetObject(args.ContainerName, blobName);
            return result.Content;
        }
    }
}
