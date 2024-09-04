using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Google;


/// <summary>
/// This module will not try to connect to azure.
/// </summary>
[DependsOn(
    typeof(AbpBlobStoringGoogleModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringGoogleTestCommonModule : AbpModule
{

}

[DependsOn(
    typeof(AbpBlobStoringGoogleTestCommonModule)
)]
public class AbpBlobStoringGoogleTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private string _connectionString;

    private readonly string _randomContainerName = "abp-azure-test-container-" + Guid.NewGuid().ToString("N");

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        // var configuration = context.Services.GetConfiguration();
        // _connectionString = configuration["Azure:ConnectionString"];
        //
        // Configure<AbpBlobStoringOptions>(options =>
        // {
        //     options.Containers.ConfigureAll((containerName, containerConfiguration) =>
        //     {
        //         containerConfiguration.UseAzure(azure =>
        //         {
        //             azure.ConnectionString = _connectionString;
        //             azure.ContainerName = _randomContainerName;
        //             azure.CreateContainerIfNotExists = true;
        //         });
        //     });
        // });
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        
    }
}