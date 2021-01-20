using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Minio
{

    [DependsOn(
        typeof(AbpBlobStoringMinioModule),
        typeof(AbpBlobStoringTestModule)
    )]
    public class AbpBlobStoringMinioTestCommonModule : AbpModule
    {

    }

    [DependsOn(
        typeof(AbpBlobStoringMinioTestCommonModule)
    )]
    public class AbpBlobStoringMinioTestModule : AbpModule
    {
        private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

        private string _endPoint;
        private string _accessKey;
        private string _secretKey;

        private readonly string _randomContainerName = "abp-minio-test-container-" + Guid.NewGuid().ToString("N");

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets(UserSecretsId);
            }));

            var configuration = context.Services.GetConfiguration();
            _endPoint = configuration["Minio:EndPoint"];
            _accessKey = configuration["Minio:AccessKey"];
            _secretKey = configuration["Minio:SecretKey"];

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseMinio(minio =>
                    {
                        minio.EndPoint = _endPoint;
                        minio.AccessKey = _accessKey;
                        minio.SecretKey = _secretKey;
                        minio.WithSSL = false;
                        minio.BucketName = _randomContainerName;
                        minio.CreateBucketIfNotExists = true;
                    });
                });
            });
        }

        public override async void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var minioClient = new MinioClient(_endPoint, _accessKey, _secretKey);
            if (await minioClient.BucketExistsAsync(_randomContainerName))
            {
                var objects = await minioClient.ListObjectsAsync(_randomContainerName, null, true).ToList();

                foreach (var item in objects)
                {
                    await minioClient.RemoveObjectAsync(_randomContainerName, item.Key);
                }

                await minioClient.RemoveBucketAsync(_randomContainerName);
            }

        }
    }

}
