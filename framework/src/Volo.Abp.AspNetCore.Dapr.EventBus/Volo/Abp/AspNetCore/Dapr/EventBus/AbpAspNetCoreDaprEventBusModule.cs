using Microsoft.AspNetCore.Http.Json;
using Volo.Abp.AspNetCore.Dapr.SystemTextJson;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Dapr;

[DependsOn(
    typeof(AbpAspNetCoreDaprModule),
    typeof(AbpEventBusDaprModule),
    typeof(AbpJsonModule)
    )]
public class AbpAspNetCoreDaprEventBusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // TODO: Add NewtonsoftJson json converter.

        Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new AbpAspNetCoreDaprSubscriptionDefinitionConverter());
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new AbpAspNetCoreDaprSubscriptionDefinitionConverter());
        });
    }
}
