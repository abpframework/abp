using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring.Aws
{
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

        private readonly string _randomContainerName = "abp-aws-test-container-" + Guid.NewGuid().ToString("N");

        private AwsBlobProviderConfiguration _configuration;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets(UserSecretsId);
            }));

            var configuration = context.Services.GetConfiguration();
            var accessKeyId = configuration["Aws:AccessKeyId"];
            var secretAccessKey = configuration["Aws:SecretAccessKey"];
            var region = configuration["Aws:Region"];

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseAws(aws =>
                    {
                        aws.AccessKeyId = accessKeyId;
                        aws.SecretAccessKey = secretAccessKey;
                        aws.Region = region;
                        aws.CreateContainerIfNotExists = true;
                        aws.ContainerName = _randomContainerName;

                        _configuration = aws;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            AsyncHelper.RunSync(() => DeleteBucketAsync(context));
        }

        private async Task DeleteBucketAsync(ApplicationShutdownContext context)
        {
            var amazonS3Client = await context.ServiceProvider.GetRequiredService<IAmazonS3ClientFactory>()
                .GetAmazonS3Client(_configuration);

            if (await AmazonS3Util.DoesS3BucketExistV2Async(amazonS3Client, _randomContainerName))
            {
                var blobs = await amazonS3Client.ListObjectsAsync(_randomContainerName);

                if (blobs.S3Objects.Any())
                {
                    await amazonS3Client.DeleteObjectsAsync(new DeleteObjectsRequest
                    {
                        BucketName = _randomContainerName,
                        Objects = blobs.S3Objects.Select(o => new KeyVersion {Key = o.Key}).ToList()
                    });
                }

                await amazonS3Client.DeleteBucketAsync(_randomContainerName);
            }
        }
    }
}
