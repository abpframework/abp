using System.IO;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    public class HuaweiyunBlobProvider : BlobProviderBase, ITransientDependency
    {
        protected IObsClientFactory ObsClientFactory { get; }
        protected IHuaweiyunBlobNameCalculator HuaweiyunBlobNameCalculator { get; }

        public HuaweiyunBlobProvider(
            IObsClientFactory obsClientFactory,
            IHuaweiyunBlobNameCalculator huaweiyunBlobNameCalculator)
        {
            ObsClientFactory = obsClientFactory;
            HuaweiyunBlobNameCalculator = huaweiyunBlobNameCalculator;
        }

        protected virtual IObsClient GetObsClient(HuaweiyunBlobProviderConfiguration huaweiyunConfig)
        {
            return ObsClientFactory.Create(huaweiyunConfig);
        }

        public async override Task SaveAsync(BlobProviderSaveArgs args)
        {
            var containerName = GetContainerName(args);
            var blobName = HuaweiyunBlobNameCalculator.Calculate(args);
            var huaweiyunConfig = args.Configuration.GetHuaweiyunConfiguration();
            var obsClient = GetObsClient(huaweiyunConfig);

            if (!args.OverrideExisting && await BlobExistsAsync(obsClient, containerName, blobName))
            {
                throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }
            if (huaweiyunConfig.CreateContainerIfNotExists)
            {
                if (!await obsClient.DoesContainerExistAsync(containerName))
                {
                    await obsClient.CreateContainer(containerName, huaweiyunConfig.ContainerLocation);
                }
            }
            await obsClient.PutObjectAsync(containerName, blobName, args.BlobStream);
        }

        public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
        {
            var containerName = GetContainerName(args);
            var blobName = HuaweiyunBlobNameCalculator.Calculate(args);
            var huaweiyunConfig = args.Configuration.GetHuaweiyunConfiguration();
            var obsClient = GetObsClient(huaweiyunConfig);
            if (!await BlobExistsAsync(obsClient, containerName, blobName))
            {
                return false;
            }
            await obsClient.DeleteObjectAsync(containerName, blobName);
            return true;
        }

        public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
        {
            var containerName = GetContainerName(args);
            var blobName = HuaweiyunBlobNameCalculator.Calculate(args);
            var huaweiyunConfig = args.Configuration.GetHuaweiyunConfiguration();
            var obsClient = GetObsClient(huaweiyunConfig);

            return await BlobExistsAsync(obsClient, containerName, blobName);
        }

        public async override Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
        {
            var containerName = GetContainerName(args);
            var blobName = HuaweiyunBlobNameCalculator.Calculate(args);
            var huaweiyunConfig = args.Configuration.GetHuaweiyunConfiguration();
            var obsClient = GetObsClient(huaweiyunConfig);
            if (!await BlobExistsAsync(obsClient, containerName, blobName))
            {
                return null;
            }

            return await obsClient.GetObjectAsync(containerName, blobName);
        }

        private static string GetContainerName(BlobProviderArgs args)
        {
            var configuration = args.Configuration.GetHuaweiyunConfiguration();
            return configuration.ContainerName.IsNullOrWhiteSpace()
                ? args.ContainerName
                : configuration.ContainerName;
        }

        private async Task<bool> BlobExistsAsync(IObsClient client, string containerName, string blobName)
        {
            return await client.DoesContainerExistAsync(containerName) &&
                    await client.DoesBlobExistAsync(containerName, blobName);
        }
    }
}
