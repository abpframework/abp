using System;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Azure
{
    [DependsOn(
        typeof(AbpBlobStoringAzureModule),
        typeof(AbpBlobStoringTestModule)
    )]
    public class AbpBlobStoringAzureTestModule : AbpModule
    {
        private readonly string _randomContainerName = "abp-azure-test-container-" + Guid.NewGuid().ToString("N");

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                builder.AddUserSecrets("9f0d2c00-80c1-435b-bfab-2c39c8249091");
            }));

            var configuration = context.Services.GetConfiguration();

            var blobServiceClient = new BlobServiceClient(configuration["Azure:ConnectionString"]);
            blobServiceClient.CreateBlobContainer(_randomContainerName);

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureAll((containerName, containerConfiguration) =>
                {
                    containerConfiguration.UseAzure(azure =>
                    {
                        azure.ConnectionString = configuration["Azure:ConnectionString"];
                        azure.ContainerName = _randomContainerName;
                    });
                });
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            var blobServiceClient = new BlobServiceClient(configuration["Azure:ConnectionString"]);
            blobServiceClient.DeleteBlobContainer(_randomContainerName);
        }
    }
}
