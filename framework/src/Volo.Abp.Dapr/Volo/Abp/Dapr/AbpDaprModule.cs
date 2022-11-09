using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Dapr;

[DependsOn(typeof(AbpJsonModule))]
public class AbpDaprModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureDaprOptions(configuration);

        context.Services.TryAddSingleton(
            serviceProvider => serviceProvider
                .GetRequiredService<IAbpDaprClientFactory>()
                .Create()
        );
    }

    private void ConfigureDaprOptions(IConfiguration configuration)
    {
        Configure<AbpDaprOptions>(configuration.GetSection("Dapr"));
        Configure<AbpDaprOptions>(options =>
        {
            if (options.DaprApiToken.IsNullOrWhiteSpace())
            {
                var confEnv = configuration["DAPR_API_TOKEN"];
                if (!confEnv.IsNullOrWhiteSpace())
                {
                    options.DaprApiToken = confEnv;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("DAPR_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.DaprApiToken = env;
                    }
                }
            }

            if (options.AppApiToken.IsNullOrWhiteSpace())
            {
                var confEnv = configuration["APP_API_TOKEN"];
                if (!confEnv.IsNullOrWhiteSpace())
                {
                    options.AppApiToken = confEnv;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("APP_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.AppApiToken = env;
                    }
                }
            }
        });
    }
}
