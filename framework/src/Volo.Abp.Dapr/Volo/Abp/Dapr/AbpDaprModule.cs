using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Dapr;

[DependsOn(
    typeof(AbpJsonModule),
    typeof(AbpMultiTenancyAbstractionsModule),
    typeof(AbpHttpClientModule)
)]
public class AbpDaprModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        ConfigureDaprOptions(configuration);
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
                    options.DaprApiToken = confEnv!;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("DAPR_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.DaprApiToken = env!;
                    }
                }
            }

            if (options.AppApiToken.IsNullOrWhiteSpace())
            {
                var confEnv = configuration["APP_API_TOKEN"];
                if (!confEnv.IsNullOrWhiteSpace())
                {
                    options.AppApiToken = confEnv!;
                }
                else
                {
                    var env = Environment.GetEnvironmentVariable("APP_API_TOKEN");
                    if (!env.IsNullOrWhiteSpace())
                    {
                        options.AppApiToken = env!;
                    }
                }
            }
        });
    }
}
