using Microsoft.AspNetCore.Http.Json;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.SystemTextJson;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

[DependsOn(
    typeof(AbpAspNetCoreMvcDaprModule),
    typeof(AbpEventBusDaprModule)
)]
public class AbpAspNetCoreMvcDaprEventBusModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // TODO: Add NewtonsoftJson json converter.

        Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new AbpAspNetCoreMvcDaprSubscriptionDefinitionConverter());
        });

        Configure<AbpSystemTextJsonSerializerOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new AbpAspNetCoreMvcDaprSubscriptionDefinitionConverter());
        });
    }
}
