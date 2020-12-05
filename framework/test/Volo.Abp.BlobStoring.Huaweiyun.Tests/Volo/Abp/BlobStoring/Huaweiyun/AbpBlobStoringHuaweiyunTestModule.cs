using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring.Huaweiyun
{
    [DependsOn(
        typeof(AbpBlobStoringHuaweiyunModule),
        typeof(AbpBlobStoringTestModule)
    )]
    public class AbpBlobStoringHuaweiyunTestCommonModule : AbpModule
    {

    }

    [DependsOn(
        typeof(AbpBlobStoringHuaweiyunTestCommonModule)
    )]
    public class AbpBlobStoringHuaweiyunTestModule : AbpModule
    {
        private const string UserSecretsId = "1ce25a52-8e70-49c4-8d5e-8450b6b4a8b9";

        private HuaweiyunBlobProviderConfiguration _configuration;
        private readonly string _randomContainerName = "abp-huaweiyun-test-container-" + Guid.NewGuid().ToString("N");

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets(UserSecretsId);
            }));

            var configuration = context.Services.GetConfiguration();
            var accessKeyId = configuration["Huaweiyun:AccessKeyId"];
            var secretAccessKey = configuration["Huaweiyun:SecretAccessKey"];
            var endpoint = configuration["Huaweiyun:Endpoint"];
            var containerLocation = configuration["Huaweiyun:ContainerLocation"];
            var createContainerIfNotExists = configuration["Huaweiyun:CreateContainerIfNotExists"];

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseHuaweiyun(huaweiyun =>
                    {
                        huaweiyun.AccessKeyId = accessKeyId;
                        huaweiyun.SecretAccessKey = secretAccessKey;
                        huaweiyun.Endpoint = endpoint;
                        huaweiyun.ContainerName = _randomContainerName;
                        huaweiyun.ContainerLocation = containerLocation;
                        huaweiyun.CreateContainerIfNotExists = Convert.ToBoolean(createContainerIfNotExists);
                        _configuration = huaweiyun;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            AsyncHelper.RunSync(() => DeleteContainerAsync(context));
        }


        private async Task DeleteContainerAsync(ApplicationShutdownContext context)
        {
            var obsClientFactory = context.ServiceProvider.GetService<IObsClientFactory>();
            var obsClient = obsClientFactory.Create(_configuration);
            if (await obsClient.DoesContainerExistAsync(_randomContainerName))
            {
                var objects = await obsClient.GetObjectNamesAsync(_randomContainerName);
                if (objects.Any())
                {
                    foreach (var obj in objects)
                    {
                        await obsClient.DeleteObjectAsync(_randomContainerName, obj);
                    }
                }
                await obsClient.DeleteContainerAsync(_randomContainerName);
            }
        }

    }
}
