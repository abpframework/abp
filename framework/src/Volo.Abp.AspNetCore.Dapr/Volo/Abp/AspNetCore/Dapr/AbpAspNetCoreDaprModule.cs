using Microsoft.AspNetCore.Http.Json;
using Volo.Abp.AspNetCore.Dapr.SystemTextJson;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Dapr;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(AbpEventBusDaprModule),
    typeof(AbpJsonModule)
    )]
public class AbpAspNetCoreDaprModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // TODO: Add NewtonsoftJson json converter.

        Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new DaprSubscriptionDefinitionConverter());
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new DaprSubscriptionDefinitionConverter());
        });
    }
}
