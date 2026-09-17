using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Minio;

public class MinioBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected IMinioBlobNameCalculator MinioBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

    public MinioBlobProvider(
        IHttpClientFactory httpClientFactory,
        IMinioBlobNameCalculator minioBlobNameCalculator,
        IBlobNormalizeNamingService blobNormalizeNamingService)
    {
        HttpClientFactory = httpClientFactory;
        MinioBlobNameCalculator = minioBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
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

        await client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(containerName)
            .WithObject(blobName)
            .WithStreamData(args.BlobStream)
            .WithObjectSize(args.BlobStream.Length));
    }

    public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var blobName = MinioBlobNameCalculator.Calculate(args);
        var client = GetMinioClient(args);
        var containerName = GetContainerName(args);

        if (!await BlobExistsAsync(client, containerName, blobName))
        {
            return false;
        }

        await client.RemoveObjectAsync(new RemoveObjectArgs().WithBucket(containerName).WithObject(blobName));
        return true;

    }

    public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var blobName = MinioBlobNameCalculator.Calculate(args);
        var client = GetMinioClient(args);
        var containerName = GetContainerName(args);

        return await BlobExistsAsync(client, containerName, blobName);
    }

    public async override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var blobName = MinioBlobNameCalculator.Calculate(args);
        var client = GetMinioClient(args);
        var containerName = GetContainerName(args);

        if (!await BlobExistsAsync(client, containerName, blobName))
        {
            return null;
        }

        var configuration = args.Configuration.GetMinioConfiguration();
        var downloadUrl = await client.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(containerName)
                .WithObject(blobName)
                .WithExpiry(configuration.PresignedGetExpirySeconds));

        var httpClient = HttpClientFactory.CreateMinioHttpClient();

        return await httpClient.GetStreamAsync(downloadUrl, args.CancellationToken);
    }

    protected virtual IMinioClient GetMinioClient(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetMinioConfiguration();

        var client = new MinioClient()
            .WithEndpoint(configuration.EndPoint)
            .WithCredentials(configuration.AccessKey, configuration.SecretKey);

        if (configuration.WithSSL)
        {
            client.WithSSL();
        }

        return client.Build();
    }

    protected virtual async Task CreateBucketIfNotExists(IMinioClient client, string containerName)
    {
        if (!await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(containerName)))
        {
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(containerName));
        }
    }

    protected virtual async Task<bool> BlobExistsAsync(IMinioClient client, string containerName, string blobName)
    {
        // Make sure Blob Container exists.
        if (await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(containerName)))
        {
            try
            {
                await client.StatObjectAsync(new StatObjectArgs().WithBucket(containerName).WithObject(blobName));
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
            : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.BucketName!);
    }
}
