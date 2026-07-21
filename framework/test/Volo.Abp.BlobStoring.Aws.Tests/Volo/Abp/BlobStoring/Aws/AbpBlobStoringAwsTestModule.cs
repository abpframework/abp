using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring.Aws;

/// <summary>
/// This module will not try to connect to aws.
/// </summary>
[DependsOn(
    typeof(AbpBlobStoringAwsModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringAwsTestCommonModule : AbpModule
{
}

[DependsOn(
    typeof(AbpBlobStoringAwsTestCommonModule)
)]
public class AbpBlobStoringAwsTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private readonly string _runId = Guid.NewGuid().ToString("N");
    private readonly string _randomContainerName;
    private readonly string _externalRunPrefix;

    private AwsBlobProviderConfiguration _configuration;
    private string _actualContainerName;
    private bool _externalContainer;

    public AbpBlobStoringAwsTestModule()
    {
        _randomContainerName = "abp-aws-test-container-" + _runId;
        _externalRunPrefix = "abp-aws-test-run-" + _runId + "/";
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        var configuration = context.Services.GetConfiguration();
        var accessKeyId = configuration["Aws:AccessKeyId"];
        var secretAccessKey = configuration["Aws:SecretAccessKey"];

        // No credentials configured (e.g., CI without user secrets) → skip container wiring.
        // `BlobContainerConfiguration.SetConfiguration` rejects nulls, so any attempt to resolve
        // a container with null values from configuration would throw at runtime.
        if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretAccessKey))
        {
            return;
        }

        var region = configuration["Aws:Region"];
        var serviceUrl = configuration["Aws:ServiceURL"];
        var disablePayloadSigning = bool.TryParse(configuration["Aws:DisablePayloadSigning"], out var dps) && dps;
        var configuredContainerName = configuration["Aws:ContainerName"];
        var createContainerIfNotExists = !bool.TryParse(configuration["Aws:CreateContainerIfNotExists"], out var cci) || cci;

        _externalContainer = !string.IsNullOrWhiteSpace(configuredContainerName);
        _actualContainerName = _externalContainer ? configuredContainerName : _randomContainerName;

        if (_externalContainer)
        {
            var prefix = _externalRunPrefix;
            context.Services.Replace(ServiceDescriptor.Transient<IAwsBlobNameCalculator>(sp =>
                new TestRunPrefixedAwsBlobNameCalculator(sp.GetRequiredService<ICurrentTenant>(), prefix)));
        }

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseAws(aws =>
                {
                    aws.AccessKeyId = accessKeyId;
                    aws.SecretAccessKey = secretAccessKey;
                    aws.Region = region;
                    aws.ServiceURL = serviceUrl;
                    aws.DisablePayloadSigning = disablePayloadSigning;
                    aws.CreateContainerIfNotExists = createContainerIfNotExists;
                    aws.ContainerName = _actualContainerName;

                    _configuration = aws;
                });
            });
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        AsyncHelper.RunSync(() => CleanupAsync(context));
    }

    private async Task CleanupAsync(ApplicationShutdownContext context)
    {
        if (_configuration == null ||
            string.IsNullOrWhiteSpace(_configuration.AccessKeyId) ||
            string.IsNullOrWhiteSpace(_configuration.SecretAccessKey) ||
            (string.IsNullOrWhiteSpace(_configuration.Region) && string.IsNullOrWhiteSpace(_configuration.ServiceURL)))
        {
            return;
        }

        try
        {
            using var amazonS3Client = await context.ServiceProvider.GetRequiredService<IAmazonS3ClientFactory>()
                .GetAmazonS3Client(_configuration);

            if (!await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, _actualContainerName))
            {
                return;
            }

            await DeleteObjectsAsync(amazonS3Client, _externalContainer ? _externalRunPrefix : null);

            if (!_externalContainer)
            {
                await amazonS3Client.DeleteBucketAsync(_actualContainerName);
            }
        }
        catch
        {
            // Ignore errors during test cleanup
        }
    }

    private async Task DeleteObjectsAsync(AmazonS3Client client, string? prefix)
    {
        string? continuationToken = null;
        do
        {
            var listResponse = await client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _actualContainerName,
                Prefix = prefix,
                ContinuationToken = continuationToken,
                MaxKeys = 1000
            });

            if (listResponse.S3Objects.Any())
            {
                foreach (var batch in Chunk(listResponse.S3Objects, 1000))
                {
                    await client.DeleteObjectsAsync(new DeleteObjectsRequest
                    {
                        BucketName = _actualContainerName,
                        Objects = batch.Select(o => new KeyVersion { Key = o.Key }).ToList()
                    });
                }
            }

            continuationToken = listResponse.IsTruncated == true ? listResponse.NextContinuationToken : null;
        } while (continuationToken != null);
    }

    private static IEnumerable<List<S3Object>> Chunk(IEnumerable<S3Object> source, int size)
    {
        var bucket = new List<S3Object>(size);
        foreach (var item in source)
        {
            bucket.Add(item);
            if (bucket.Count == size)
            {
                yield return bucket;
                bucket = new List<S3Object>(size);
            }
        }
        if (bucket.Count > 0)
        {
            yield return bucket;
        }
    }
}

internal sealed class TestRunPrefixedAwsBlobNameCalculator : IAwsBlobNameCalculator
{
    private readonly ICurrentTenant _currentTenant;
    private readonly string _runPrefix;

    public TestRunPrefixedAwsBlobNameCalculator(ICurrentTenant currentTenant, string runPrefix)
    {
        _currentTenant = currentTenant;
        _runPrefix = runPrefix;
    }

    public string Calculate(BlobProviderArgs args)
    {
        var baseName = _currentTenant.Id == null
            ? $"host/{args.BlobName}"
            : $"tenants/{_currentTenant.Id.Value:D}/{args.BlobName}";

        return _runPrefix + baseName;
    }
}
