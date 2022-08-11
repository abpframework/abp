using Volo.Abp.AspNetCore.Dapr.Models;

namespace Volo.Abp.AspNetCore.Dapr;

public class AbpAspNetCoreDaprPubSubProviderContributorContext
{
    public IServiceProvider ServiceProvider { get; }

    public List<AbpAspNetCoreDaprSubscriptionDefinition> Subscriptions { get; }

    public AbpAspNetCoreDaprPubSubProviderContributorContext(IServiceProvider serviceProvider, List<AbpAspNetCoreDaprSubscriptionDefinition> daprSubscriptionModels)
    {
        ServiceProvider = serviceProvider;
        Subscriptions = daprSubscriptionModels;
    }
}
