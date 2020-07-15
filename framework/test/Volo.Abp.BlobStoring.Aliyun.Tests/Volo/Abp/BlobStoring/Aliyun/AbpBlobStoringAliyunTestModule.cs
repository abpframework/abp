using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Aliyun
{
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
        private const string UserSecretsId = "fe9a87da-3584-40e0-a06c-aa499936015d";

        private AliyunBlobProviderConfiguration _configuration;
        private readonly string _randomContainerName = "abp-aliyun-test-container-" + Guid.NewGuid().ToString("N");

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets(UserSecretsId);
            }));

            var configuration = context.Services.GetConfiguration();
            var _accessKeyId = configuration["Aliyun:AccessKeyId"];
            var _accessKeySecret = configuration["Aliyun:AccessKeySecret"];
            var _endpoint = configuration["Aliyun:Endpoint"];
            var _regionId = configuration["Aliyun:RegionId"];
            var _roleArn = configuration["Aliyun:RoleArn"];

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseAliyun(aliyun =>
                    {
                        aliyun.AccessKeyId = _accessKeyId;
                        aliyun.AccessKeySecret = _accessKeySecret;
                        aliyun.Endpoint = _endpoint;
                        //STS
                        aliyun.RegionId = _regionId;
                        aliyun.RoleArn = _roleArn;
                        aliyun.RoleSessionName = Guid.NewGuid().ToString("N");
                        aliyun.DurationSeconds = 3600;
                        aliyun.Policy = String.Empty;
                        //Other
                        aliyun.CreateContainerIfNotExists = true;
                        aliyun.ContainerName = _randomContainerName;
                        _configuration = aliyun;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var ossClientFactory = context.ServiceProvider.GetService<IOssClientFactory>();
            var ossClient = ossClientFactory.Create(_configuration);
            ossClient.DeleteBucket(_randomContainerName);
        }

    }
}
