using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

public class AbpAspNetCoreMvcDaprPubSubProvider : ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAspNetCoreMvcDaprEventBusOptions AspNetCoreMvcDaprEventBusOptions { get; }
    protected AbpDaprEventBusOptions DaprEventBusOptions { get; }
    protected AbpDistributedEventBusOptions DistributedEventBusOptions { get; }

    public AbpAspNetCoreMvcDaprPubSubProvider(
        IServiceProvider serviceProvider,
        IOptions<AbpAspNetCoreMvcDaprEventBusOptions> aspNetCoreDaprEventBusOptions,
        IOptions<AbpDaprEventBusOptions> daprEventBusOptions,
        IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions)
    {
        ServiceProvider = serviceProvider;
        AspNetCoreMvcDaprEventBusOptions = aspNetCoreDaprEventBusOptions.Value;
        DaprEventBusOptions = daprEventBusOptions.Value;
        DistributedEventBusOptions = distributedEventBusOptions.Value;
    }

    public virtual async Task<List<AbpAspNetCoreMvcDaprSubscriptionDefinition>> GetSubscriptionsAsync()
    {
        var subscriptions = new List<AbpAspNetCoreMvcDaprSubscriptionDefinition>();
        foreach (var handler in DistributedEventBusOptions.Handlers)
        {
            foreach (var @interface in handler.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)))
            {
                var eventType = @interface.GetGenericArguments()[0];
                var eventName = EventNameAttribute.GetNameOrDefault(eventType);

                subscriptions.Add(new AbpAspNetCoreMvcDaprSubscriptionDefinition()
                {
                    PubSubName = DaprEventBusOptions.PubSubName,
                    Topic = eventName,
                    Route = AbpAspNetCoreMvcDaprPubSubConsts.DaprEventCallbackUrl
                });
            }
        }

        if (AspNetCoreMvcDaprEventBusOptions.Contributors.Any())
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new AbpAspNetCoreMvcDaprPubSubProviderContributorContext(scope.ServiceProvider, subscriptions);
                foreach (var contributor in AspNetCoreMvcDaprEventBusOptions.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        return subscriptions;
    }
}
