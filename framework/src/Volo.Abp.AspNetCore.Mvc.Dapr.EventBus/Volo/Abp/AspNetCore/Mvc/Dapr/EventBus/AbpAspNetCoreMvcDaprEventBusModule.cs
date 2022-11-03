using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.EventBus.Distributed;
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
        var subscribeOptions = context.Services.ExecutePreConfiguredActions<AbpSubscribeOptions>();

        Configure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                var rootServiceProvider = endpointContext.ScopeServiceProvider.GetRequiredService<IRootServiceProvider>();
                subscribeOptions.SubscriptionsCallback = subscriptions =>
                {
                    var daprEventBusOptions = rootServiceProvider.GetRequiredService<IOptions<AbpDaprEventBusOptions>>().Value;
                    foreach (var handler in rootServiceProvider.GetRequiredService<IOptions<AbpDistributedEventBusOptions>>().Value.Handlers)
                    {
                        foreach (var @interface in handler.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)))
                        {
                            var eventType = @interface.GetGenericArguments()[0];
                            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
                            subscriptions.Add(new AbpSubscription()
                            {
                                PubsubName = daprEventBusOptions.PubSubName,
                                Topic = eventName,
                                Route = AbpAspNetCoreMvcDaprPubSubConsts.DaprEventCallbackUrl
                            });
                        }
                    }

                    return Task.CompletedTask;
                };

                endpointContext.Endpoints.MapAbpSubscribeHandler(subscribeOptions);
            });
        });
    }
}
