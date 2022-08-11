using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Dapr.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.AspNetCore.Dapr;

public class AbpAspNetCoreDaprPubSubProvider : ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAspNetCoreDaprEventBusOptions AspNetCoreDaprEventBusOptions { get; }
    protected AbpDaprEventBusOptions DaprEventBusOptions { get; }
    protected AbpDistributedEventBusOptions DistributedEventBusOptions { get; }

    public AbpAspNetCoreDaprPubSubProvider(
        IServiceProvider serviceProvider,
        IOptions<AbpAspNetCoreDaprEventBusOptions> aspNetCoreDaprEventBusOptions,
        IOptions<AbpDaprEventBusOptions> daprEventBusOptions,
        IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions)
    {
        ServiceProvider = serviceProvider;
        AspNetCoreDaprEventBusOptions = aspNetCoreDaprEventBusOptions.Value;
        DaprEventBusOptions = daprEventBusOptions.Value;
        DistributedEventBusOptions = distributedEventBusOptions.Value;
    }

    public virtual async Task<List<AbpAspNetCoreDaprSubscriptionDefinition>> GetSubscriptionsAsync()
    {
        var subscriptions = new List<AbpAspNetCoreDaprSubscriptionDefinition>();
        foreach (var handler in DistributedEventBusOptions.Handlers)
        {
            foreach (var @interface in handler.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)))
            {
                var eventType = @interface.GetGenericArguments()[0];
                var eventName = EventNameAttribute.GetNameOrDefault(eventType);

                subscriptions.Add(new AbpAspNetCoreDaprSubscriptionDefinition()
                {
                    PubSubName = DaprEventBusOptions.PubSubName,
                    Topic = eventName,
                    Route = AbpAspNetCoreDaprPubSubConsts.DaprEventCallbackUrl
                });
            }
        }

        if (AspNetCoreDaprEventBusOptions.Contributors.Any())
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new AbpAspNetCoreDaprPubSubProviderContributorContext(scope.ServiceProvider, subscriptions);
                foreach (var contributor in AspNetCoreDaprEventBusOptions.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        return subscriptions;
    }
}
