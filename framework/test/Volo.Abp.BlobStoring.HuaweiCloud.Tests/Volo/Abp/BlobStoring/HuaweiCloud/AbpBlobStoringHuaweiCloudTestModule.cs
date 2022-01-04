using System;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BlobStoring.HuaweiCloud;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.HuaweiCloud;

[DependsOn(
    typeof(AbpBlobStoringHuaweiCloudModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringHuaweiCloudTestCommonModule : AbpModule
{

}

[DependsOn(
    typeof(AbpBlobStoringHuaweiCloudTestCommonModule)
)]
public class AbpBlobStoringHuaweiCloudTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private string _endPoint;
    private string _accessKey;
    private string _secretKey;

    private readonly string _randomContainerName = "abp-huaweicloud-test-container-" + Guid.NewGuid().ToString("N");

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        var configuration = context.Services.GetConfiguration();
        _endPoint = configuration["HuaweiCloud:EndPoint"];
        _accessKey = configuration["HuaweiCloud:AccessKey"];
        _secretKey = configuration["HuaweiCloud:SecretKey"];

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseHuaweiCloud(huaweicloud =>
                {
                    huaweicloud.EndPoint = _endPoint;
                    huaweicloud.AccessKey = _accessKey;
                    huaweicloud.SecretKey = _secretKey;

                    huaweicloud.BucketName = _randomContainerName;
                    huaweicloud.CreateBucketIfNotExists = true;
                });
            });
        });
    }

    public override async void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        var config = new AmazonS3Config();
        config.ServiceURL = _endPoint;

        var huaweicloudclient = new AmazonS3Client(_accessKey, _secretKey, config);
    
        if (await AmazonS3Util.DoesS3BucketExistV2Async(huaweicloudclient, _randomContainerName))
        {
            var objects = (await huaweicloudclient.ListObjectsAsync(_randomContainerName)).S3Objects;

            foreach (var item in objects)
            {
                await huaweicloudclient.DeleteObjectAsync(_randomContainerName, item.Key);
            }

            await huaweicloudclient.DeleteBucketAsync(_randomContainerName);
        }

    }
}
