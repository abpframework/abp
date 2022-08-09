using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Dapr.Models;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Dapr;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.AspNetCore.Dapr;

public class AbpDaprPubSubProvider : ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected AbpAspNetCoreDaprOptions Options { get; }
    protected AbpDaprEventBusOptions EventBusOptions { get; }
    protected AbpDistributedEventBusOptions DistributedEventBusOptions { get; }

    public AbpDaprPubSubProvider(
        IServiceProvider serviceProvider,
        IOptions<AbpAspNetCoreDaprOptions> options,
        IOptions<AbpDaprEventBusOptions> eventBusOptions,
        IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions)
    {
        ServiceProvider = serviceProvider;
        EventBusOptions = eventBusOptions.Value;
        Options = options.Value;
        DistributedEventBusOptions = distributedEventBusOptions.Value;
    }

    public virtual async Task<List<DaprSubscriptionDefinition>> GetSubscriptionsAsync()
    {
        var subscriptions = new List<DaprSubscriptionDefinition>();
        foreach (var handler in DistributedEventBusOptions.Handlers)
        {
            foreach (var @interface in handler.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDistributedEventHandler<>)))
            {
                var eventType = @interface.GetGenericArguments()[0];
                var eventName = EventNameAttribute.GetNameOrDefault(eventType);

                subscriptions.Add(new DaprSubscriptionDefinition()
                {
                    PubSubName = EventBusOptions.PubSubName,
                    Topic = eventName,
                    Route = AbpAspNetCoreDaprConsts.DaprEventCallbackUrl
                });
            }
        }

        if (Options.Contributors.Any())
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = new AbpDaprPubSubProviderContributorContext(scope.ServiceProvider, subscriptions);
                foreach (var contributor in Options.Contributors)
                {
                    await contributor.ContributeAsync(context);
                }
            }
        }

        return subscriptions;
    }
}
