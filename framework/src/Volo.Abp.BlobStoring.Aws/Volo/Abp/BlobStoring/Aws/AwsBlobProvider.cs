using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobProvider : BlobProviderBase, ITransientDependency
{
    // Aligned with the TransferUtility threshold under which a non-seekable upload
    // would be sent as a single (non-retryable) request instead of multipart
    protected const long MaxBufferedUploadLength = 16 * 1024 * 1024;

    protected IAwsBlobNameCalculator AwsBlobNameCalculator { get; }
    protected IAmazonS3ClientFactory AmazonS3ClientFactory { get; }
    protected IBlobNormalizeNamingService BlobNormalizeNamingService { get; }

    public AwsBlobProvider(
        IAwsBlobNameCalculator awsBlobNameCalculator,
        IAmazonS3ClientFactory amazonS3ClientFactory,
        IBlobNormalizeNamingService blobNormalizeNamingService)
    {
        AwsBlobNameCalculator = awsBlobNameCalculator;
        AmazonS3ClientFactory = amazonS3ClientFactory;
        BlobNormalizeNamingService = blobNormalizeNamingService;
    }

    public override async Task SaveAsync(BlobProviderSaveArgs args)
    {
        var blobName = AwsBlobNameCalculator.Calculate(args);
        var configuration = args.Configuration.GetAwsConfiguration();
        var containerName = GetContainerName(args);

        using (var amazonS3Client = await GetAmazonS3Client(args))
        {
            if (!args.OverrideExisting && await BlobExistsAsync(amazonS3Client, containerName, blobName))
            {
                throw new BlobAlreadyExistsException(
                    $"Saving BLOB '{args.BlobName}' does already exists in the container '{containerName}'! Set {nameof(args.OverrideExisting)} if it should be overwritten.");
            }

            if (configuration.CreateContainerIfNotExists)
            {
                await CreateContainerIfNotExists(amazonS3Client, containerName);
            }

            if (!RequiresRetrySafeUpload(args))
            {
                await PutObjectAsync(amazonS3Client, containerName, blobName, args.BlobStream, configuration, args.CancellationToken);
                return;
            }

            // The SDK can not retry the upload of a non-seekable stream (like an encrypting
            // stream). A small source with a known length is buffered in memory and uploaded
            // as a retryable PutObject; anything larger (or with an unknown length) goes
            // through a TransferUtility multipart upload, which buffers and retries part by part
            var remainingLength = GetRemainingLengthOrNull(args.BlobStream);
            if (remainingLength != null && remainingLength <= MaxBufferedUploadLength)
            {
                using (var bufferedStream = new MemoryStream((int)remainingLength.Value))
                {
                    await args.BlobStream.CopyToAsync(bufferedStream, 81920, args.CancellationToken);
                    bufferedStream.Position = 0;
                    await PutObjectAsync(amazonS3Client, containerName, blobName, bufferedStream, configuration, args.CancellationToken);
                }

                return;
            }

            await UploadMultipartAsync(amazonS3Client, containerName, blobName, args.BlobStream, configuration, args.CancellationToken);
        }
    }

    public override async Task<bool> DeleteAsync(BlobProviderDeleteArgs args)
    {
        var blobName = AwsBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);

        using (var amazonS3Client = await GetAmazonS3Client(args))
        {
            if (!await BlobExistsAsync(amazonS3Client, containerName, blobName))
            {
                return false;
            }

            await amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = containerName,
                Key = blobName
            });

            return true;
        }
    }

    public override async Task<bool> ExistsAsync(BlobProviderExistsArgs args)
    {
        var blobName = AwsBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);

        using (var amazonS3Client = await GetAmazonS3Client(args))
        {
            return await BlobExistsAsync(amazonS3Client, containerName, blobName);
        }
    }

    public override async Task<Stream?> GetOrNullAsync(BlobProviderGetArgs args)
    {
        var blobName = AwsBlobNameCalculator.Calculate(args);
        var containerName = GetContainerName(args);

        using (var amazonS3Client = await GetAmazonS3Client(args))
        {
            if (!await BlobExistsAsync(amazonS3Client, containerName, blobName))
            {
                return null;
            }

            var response = await amazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = containerName,
                Key = blobName
            });

            return response.ResponseStream;
        }
    }

    /// <summary>
    /// The retry-safe (buffered/multipart) upload only applies to non-seekable streams
    /// of containers using the encryption or the content pipeline; other containers
    /// keep the plain PutObject behavior they always had.
    /// </summary>
    protected virtual bool RequiresRetrySafeUpload(BlobProviderSaveArgs args)
    {
        if (args.BlobStream.CanSeek)
        {
            return false;
        }

        return args.Configuration.IsEncryptionEnabled() ||
               args.Configuration.GetEffectivePipelineContributors().Any();
    }

    protected virtual long? GetRemainingLengthOrNull(Stream stream)
    {
        try
        {
            var remainingLength = stream.Length - stream.Position;
            return remainingLength >= 0 ? remainingLength : null;
        }
        catch (Exception ex) when (ex is NotSupportedException || ex is IOException)
        {
            // The length is optional; a probe failure must not fail the save
            return null;
        }
    }

    protected virtual async Task PutObjectAsync(
        AmazonS3Client amazonS3Client,
        string containerName,
        string blobName,
        Stream blobStream,
        AwsBlobProviderConfiguration configuration,
        CancellationToken cancellationToken)
    {
        await amazonS3Client.PutObjectAsync(CreatePutObjectRequest(containerName, blobName, blobStream, configuration), cancellationToken);
    }

    protected virtual PutObjectRequest CreatePutObjectRequest(
        string containerName,
        string blobName,
        Stream blobStream,
        AwsBlobProviderConfiguration configuration)
    {
        return new PutObjectRequest
        {
            BucketName = containerName,
            Key = blobName,
            InputStream = blobStream,
            AutoCloseStream = false,
            DisablePayloadSigning = configuration.DisablePayloadSigning
        };
    }

    protected virtual async Task UploadMultipartAsync(
        AmazonS3Client amazonS3Client,
        string containerName,
        string blobName,
        Stream blobStream,
        AwsBlobProviderConfiguration configuration,
        CancellationToken cancellationToken)
    {
        using (var transferUtility = new TransferUtility(amazonS3Client))
        {
            await transferUtility.UploadAsync(CreateMultipartUploadRequest(containerName, blobName, blobStream, configuration), cancellationToken);
        }
    }

    protected virtual TransferUtilityUploadRequest CreateMultipartUploadRequest(
        string containerName,
        string blobName,
        Stream blobStream,
        AwsBlobProviderConfiguration configuration)
    {
        return new TransferUtilityUploadRequest
        {
            BucketName = containerName,
            Key = blobName,
            // The unseekable multipart path of the SDK ignores AutoCloseStream
            // and disposes the input, so the ownership is protected by a wrapper
            InputStream = new LeaveOpenStreamWrapper(blobStream),
            AutoCloseStream = false,
            DisablePayloadSigning = configuration.DisablePayloadSigning
        };
    }

    protected virtual async Task<AmazonS3Client> GetAmazonS3Client(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetAwsConfiguration();
        return await AmazonS3ClientFactory.GetAmazonS3Client(configuration);
    }

    protected virtual async Task<bool> BlobExistsAsync(AmazonS3Client amazonS3Client, string containerName, string blobName)
    {
        // Make sure Blob Container exists.
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, containerName))
        {
            return false;
        }

        try
        {
            await amazonS3Client.GetObjectMetadataAsync(containerName, blobName);
        }
        catch (Exception ex)
        {
            if (ex is AmazonS3Exception)
            {
                return false;
            }

            throw;
        }

        return true;
    }

    protected virtual async Task CreateContainerIfNotExists(AmazonS3Client amazonS3Client, string containerName)
    {
        if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, containerName))
        {
            await amazonS3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = containerName
            });
        }
    }

    protected virtual string GetContainerName(BlobProviderArgs args)
    {
        var configuration = args.Configuration.GetAwsConfiguration();
        return configuration.ContainerName.IsNullOrWhiteSpace()
            ? args.ContainerName
            : BlobNormalizeNamingService.NormalizeContainerName(args.Configuration, configuration.ContainerName!);
    }
}
