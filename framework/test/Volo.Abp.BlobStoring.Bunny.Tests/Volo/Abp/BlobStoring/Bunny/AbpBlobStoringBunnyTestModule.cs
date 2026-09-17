using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BlobStoring.Bunny;

/// <summary>
/// This module will not try to connect to Bunny.
/// </summary>
[DependsOn(
    typeof(AbpBlobStoringBunnyModule),
    typeof(AbpBlobStoringTestModule)
)]
public class AbpBlobStoringBunnyTestCommonModule : AbpModule
{
}

[DependsOn(
    typeof(AbpBlobStoringBunnyTestCommonModule)
)]
public class AbpBlobStoringBunnyTestModule : AbpModule
{
    private const string UserSecretsId = "9f0d2c00-80c1-435b-bfab-2c39c8249091";

    private readonly string _randomContainerName = "abp-bunny-test-container-" + Guid.NewGuid().ToString("N");

    private BunnyBlobProviderConfiguration _configuration;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
        {
            builder.AddUserSecrets(UserSecretsId);
        }));

        var configuration = context.Services.GetConfiguration();
        var accessKey = configuration["Bunny:AccessKey"];
        var region = configuration["Bunny:Region"];

        Configure<AbpBlobStoringOptions>(options =>
        {
            options.Containers.ConfigureAll((containerName, containerConfiguration) =>
            {
                containerConfiguration.UseBunny(bunny =>
                {
                    bunny.AccessKey = accessKey;
                    bunny.Region = region;
                    bunny.CreateContainerIfNotExists = true;
                    bunny.ContainerName = _randomContainerName;

                    _configuration = bunny;
                });
            });
        });
    }
}
