using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BunnyCDN.Net.Storage;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Bunny;

public class BunnyBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IBunnyBlobNameCalculator BunnyBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }
    protected IBunnyClientFactory BunnyClientFactory { get; }

    public BunnyBlobProvider(
        IBunnyBlobNameCalculator bunnyBlobNameCalculator,
        IBlobNormalizeNamingService blobNormalizeNamingService,
        IBunnyClientFactory bunnyClientFactory)
    {
        BunnyBlobNameCalculator = bunnyBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
        BunnyClientFactory = bunnyClientFactory;
    }

    public async override Task SaveAsync(BlobProviderSaveArgs args)
    {
        var configuration = args.Configuration.GetBunnyConfiguration();
        var containerName = GetContainerName(args);
        var blobName = BunnyBlobNameCalculator.Calculate(args);

        await ValidateContainerExistsAsync(containerName, configuration);

        var bunnyStorage = await GetBunnyCDNStorageAsync(args);

        if (!args.OverrideExisting && await BlobExistsAsync(bunnyStorage, containerName, blobName))
        {
            throw new BlobAlreadyExistsException(
                $"Blob '{args.BlobName}' already exists in container '{containerName}'. " +
                $"Set {nameof(args.OverrideExisting)} to true to overwrite.");
        }

        using var memoryStream = new MemoryStream();
        await args.BlobStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        await bunnyStorage.UploadAsync(memoryStream, $"{containerName}/{blobName}");
    }

    public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var blobName = BunnyBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);
        var bunnyStorage = await GetBunnyCDNStorageAsync(args);

        if (!await BlobExistsAsync(bunnyStorage, containerName, blobName))
        {
            return false;
        }

        try
        {
            return await bunnyStorage.DeleteObjectAsync($"{containerName}/{blobName}");
        }
        catch (BunnyCDNStorageException ex) when (ex.Message.Contains("404"))
        {
            return false;
        }
    }

    public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var blobName = BunnyBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);
        var bunnyStorage = await GetBunnyCDNStorageAsync(args);

        return await BlobExistsAsync(bunnyStorage, containerName, blobName);
    }

    public async override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var blobName = BunnyBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);
        var bunnyStorage = await GetBunnyCDNStorageAsync(args);

        if (!await BlobExistsAsync(bunnyStorage, containerName, blobName))
        {
            return null;
        }

        try
        {
            return await bunnyStorage.DownloadObjectAsStreamAsync($"{containerName}/{blobName}");
        }
        catch (WebException ex) when ((HttpStatusCode)ex.Status == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    protected virtual async Task<bool> BlobExistsAsync(BunnyCDNStorage bunnyStorage, string containerName, string blobName)
    {
        try
        {
            var fullBlobPath = $"/{containerName}/{blobName}";
            var directoryPath = Path.GetDirectoryName(fullBlobPath)?.Replace('\\', '/') + "/";

            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new Exception("Invalid directory path generated from blob name.");
            }

            var objects = await bunnyStorage.GetStorageObjectsAsync(directoryPath);
            return objects?.Any(o => o.FullPath == fullBlobPath) == true;
        }
        catch (BunnyCDNStorageException ex) when (ex.Message.Contains("404"))
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error while checking blob existence: {ex.Message}", ex);
        }
    }

    protected virtual async Task<BunnyCDNStorage> GetBunnyCDNStorageAsync(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetBunnyConfiguration();
        var containerName = GetContainerName(args);
        var region = configuration.Region ?? "de";

        return await BunnyClientFactory.CreateAsync(
            configuration.AccessKey,
            containerName,
            region);
    }

    protected virtual string GetContainerName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetBunnyConfiguration();
        return configuration.ContainerName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.ContainerName!);
    }

    protected virtual async Task ValidateContainerExistsAsync(
        string containerName,
        BunnyBlobProviderConfiguration configuration
        )
    {
        try
        {
            await BunnyClientFactory.EnsureStorageZoneExistsAsync(
                configuration.AccessKey,
                containerName,
                configuration.Region ?? "de",
                configuration.CreateContainerIfNotExists);
        }
        catch (Exception ex)
        {
            throw new AbpException(
                $"Failed to validate storage zone '{containerName}': {ex.Message}",
                ex);
        }
    }
}
