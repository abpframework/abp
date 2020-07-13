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
                        aliyun.Endpoint = _endpoint;//eg:https://oss-cn-beijing.aliyuncs.com
                        //STS
                        aliyun.RegionId = _regionId;//eg:cn-beijing
                        aliyun.RoleArn = _roleArn;//eg:acs:ram::1320235309887297:role/role-oss-xxxxx
                        aliyun.RoleSessionName = Guid.NewGuid().ToString("N");
                        aliyun.DurationSeconds = 3600;
                        aliyun.Policy = String.Empty;
                        //Other
                        aliyun.CreateContainerIfNotExists = true;
                    });
                });
            });
        }

    }
}
