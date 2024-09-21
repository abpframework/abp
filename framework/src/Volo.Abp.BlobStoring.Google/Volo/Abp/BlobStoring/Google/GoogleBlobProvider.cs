using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Google;

public class GoogleBlobProvider : BlobProviderBase, ITransientDependency
{
    protected IGoogleBlobNameCalculator GoogleBlobNameCalculator { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }
    
    public GoogleBlobProvider(IGoogleBlobNameCalculator googleBlobNameCalculator, IBlobNormalizeNamingService blobNormalizeNamingService)
    {
        GoogleBlobNameCalculator = googleBlobNameCalculator;
        BlobNormalizeNamingService = blobNormalizeNamingService;
    }
    
    public async override Task SaveAsync(BlobProviderSaveArgs args)
    {
        var configuration = args.Configuration.GetGoogleConfiguration();
        var storageClient = await GetStorageClientClientAsync(args);
        var blobName = GoogleBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);
        
        if (await BlobExistsAsync(args, blobName) && !args.OverrideExisting)
        {
            throw new BlobAlreadyExistsException($"Saving BLOB '{args.BlobName}' does already exists in the container '{GetContainerName(args)}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
        }
        
        if (configuration.CreateContainerIfNotExists)
        {
            await CreateContainerIfNotExists(args);
        }
        
        await storageClient.UploadObjectAsync(containerName, blobName, contentType: "application/octet-stream", args.BlobStream);
    }

    public async override Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var storageClient = await GetStorageClientClientAsync(args);
        var blobName = GoogleBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);

        try
        {
            await storageClient.DeleteObjectAsync(containerName, blobName);
        }
        catch (GoogleApiException e) when (e.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return true;
        }

        return true;
    }

    public async override Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var blobName = GoogleBlobNameCalculator.Calculate(args);
        return await BlobExistsAsync(args, blobName);
    }

    public async override Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var storageClient = await GetStorageClientClientAsync(args);
        var blobName = GoogleBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);
         
        if(!await BlobExistsAsync(args, blobName))
        {
            return null;
        }
        
        var stream = new MemoryStream();
        
        await storageClient.DownloadObjectAsync(containerName, blobName, stream);
        
        stream.Seek(0, SeekOrigin.Begin);
        
        return stream;
    }
    
    protected virtual string GetContainerName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetGoogleConfiguration();
        return configuration.ContainerName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.ContainerName!);
    }
    
    protected virtual async Task<bool> BlobExistsAsync(BlobProviderArgs args, string blobName)
    {
        var storageClient = await GetStorageClientClientAsync(args);
        if(!await ContainerExistsAsync(args, storageClient))
        {
            return false;
        }

        try
        {
            await storageClient.GetObjectAsync(GetContainerName(args), blobName);
        }
        catch (GoogleApiException e) when (e.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        return true;
    }
    
    protected virtual async Task CreateContainerIfNotExists(BlobProviderArgs args)
    {
        var storageClient = await GetStorageClientClientAsync(args);
        var configuration = args.Configuration.GetGoogleConfiguration();
        if(await ContainerExistsAsync(args, storageClient))
        {
            return;
        }
       
        await storageClient.CreateBucketAsync(configuration.ProjectId, GetContainerName(args));
    }

    protected virtual async Task<bool> ContainerExistsAsync(BlobProviderArgs args, StorageClient client)
    {
        try
        {
            await client.GetBucketAsync(GetContainerName(args));
        }
        catch (GoogleApiException e) when (e.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        return true;
    }
    
    protected virtual async Task<StorageClient> GetStorageClientClientAsync(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetGoogleConfiguration();
        if (configuration.UseApplicationDefaultCredentials)
        {
            return await StorageClient.CreateAsync();
        }
        
        var googleCredential = GoogleCredential.FromServiceAccountCredential(
            new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(configuration.ClientEmail)
                    {
                        ProjectId = configuration.ProjectId,
                        Scopes = configuration.Scopes
                    }
                    .FromPrivateKey(configuration.PrivateKey)
            ));
        
        return await StorageClient.CreateAsync(googleCredential);
    }
}