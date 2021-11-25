using Aliyun.OSS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Aliyun;

[DependsOn(
    typeof(AbpBlobStoringAliyunModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringAliyunTestCommonModule : AbpModule
{

}

[DependsOn(
    typeof(AbpBlobStoringAliyunTestCommonModule)
)]
public class AbpBlobStoringAliyunTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private AliyunBlobProviderConfiguration _configuration;
    private readonly string _randomContainerName = "abp-aliyun-test-container-" + Guid.NewGuid().ToString("N");

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        var configuration = context.Services.GetConfiguration();
        var accessKeyId = configuration["Aliyun:AccessKeyId"];
        var accessKeySecret = configuration["Aliyun:AccessKeySecret"];
        var endpoint = configuration["Aliyun:Endpoint"];
        var regionId = configuration["Aliyun:RegionId"];
        var roleArn = configuration["Aliyun:RoleArn"];

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseAliyun(aliyun =>
                {
                    aliyun.AccessKeyId = accessKeyId;
                    aliyun.AccessKeySecret = accessKeySecret;
                    aliyun.Endpoint = endpoint;
                        //STS
                        aliyun.UseSecurityTokenService = true;
                    aliyun.RegionId = regionId;
                    aliyun.RoleArn = roleArn;
                    aliyun.RoleSessionName = Guid.NewGuid().ToString("N");
                    aliyun.DurationSeconds = 900;
                    aliyun.Policy = String.Empty;
                        //Other
                        aliyun.CreateContainerIfNotExists = true;
                    aliyun.ContainerName = _randomContainerName;
                    aliyun.TemporaryCredentialsCacheKey = "297A96094D7048DBB2C28C3FDB20839A";
                    _configuration = aliyun;
                });
            });
        });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var ossClientFactory = context.ServiceProvider.GetService<IOssClientFactory>();
        var ossClient = ossClientFactory.Create(_configuration);
        if (ossClient.DoesBucketExist(_randomContainerName))
        {
            var objects = ossClient.ListObjects(_randomContainerName);
            if (objects.ObjectSummaries.Any())
            {
                ossClient.DeleteObjects(new DeleteObjectsRequest(_randomContainerName,
                    objects.ObjectSummaries.Select(o => o.Key).ToList()));
            }
            ossClient.DeleteBucket(_randomContainerName);
        }
    }

}
