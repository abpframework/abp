using Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;

namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

public class AbpAspNetCoreMvcDaprPubSubProviderContributorContext
{
    public IServiceProvider ServiceProvider { get; }

    public List<AbpAspNetCoreMvcDaprSubscriptionDefinition> Subscriptions { get; }

    public AbpAspNetCoreMvcDaprPubSubProviderContributorContext(IServiceProvider serviceProvider, List<AbpAspNetCoreMvcDaprSubscriptionDefinition> daprSubscriptionModels)
    {
        ServiceProvider = serviceProvider;
        Subscriptions = daprSubscriptionModels;
    }
}
