using Aliyun.OSS;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aliyun;

public class AliyunBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IOssClientFactory OssClientFactory { get; }
    protected IAliyunBlobNameCalculator AliyunBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

    public AliyunBlobProvider(
        IOssClientFactory ossClientFactory,
        IAliyunBlobNameCalculator aliyunBlobNameCalculator,
        IBlobNormalizeNamingService blobNormalizeNamingService)
    {
        OssClientFactory = ossClientFactory;
        AliyunBlobNameCalculator = aliyunBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
    }

    protected virtual IOss GetOssClient(BlobContainerConfiguration blobContainerConfiguration)
    {
        var aliyunConfig = blobContainerConfiguration.GetAliyunConfiguration();
        return OssClientFactory.Create(aliyunConfig);
    }

    protected virtual IOss GetOssClient(AliyunBlobProviderConfiguration aliyunConfig)
    {
        return OssClientFactory.Create(aliyunConfig);
    }


    public override Task SaveAsync(BlobProviderSaveArgs args)
    {
        var containerName = GetContainerName(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);
        var aliyunConfig = args.Configuration.GetAliyunConfiguration();
        var ossClient = GetOssClient(aliyunConfig);
        if (!args.OverrideExisting && BlobExists(ossClient, containerName, blobName))
        {
            throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
        }
        if (aliyunConfig.CreateContainerIfNotExists)
        {
            if (!ossClient.DoesBucketExist(containerName))
            {
                ossClient.CreateBucket(containerName);
            }
        }
        ossClient.PutObject(containerName, blobName, args.BlobStream);
        return Task.CompletedTask;
    }

    public override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var containerName = GetContainerName(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);
        var ossClient = GetOssClient(args.Configuration);
        if (!BlobExists(ossClient, containerName, blobName))
        {
            return Task.FromResult(false);
        }
        ossClient.DeleteObject(containerName, blobName);
        return Task.FromResult(true);
    }

    public override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var containerName = GetContainerName(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);
        var ossClient = GetOssClient(args.Configuration);
        return Task.FromResult(BlobExists(ossClient, containerName, blobName));
    }

    public override async Task<Stream> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var containerName = GetContainerName(args);
        var blobName = AliyunBlobNameCalculator.Calculate(args);
        var ossClient = GetOssClient(args.Configuration);
        if (!BlobExists(ossClient, containerName, blobName))
        {
            return null;
        }
        var result = ossClient.GetObject(containerName, blobName);
        return await TryCopyToMemoryStreamAsync(result.Content, args.CancellationToken);
    }

    protected virtual string GetContainerName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetAliyunConfiguration();
        return configuration.ContainerName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.ContainerName);
    }

    protected virtual bool BlobExists(IOss ossClient, string containerName, string blobName)
    {
        // Make sure Blob Container exists.
        return ossClient.DoesBucketExist(containerName) &&
               ossClient.DoesObjectExist(containerName, blobName);
    }

}
