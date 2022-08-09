using Volo.Abp.AspNetCore.Dapr.Models;

namespace Volo.Abp.AspNetCore.Dapr;

public class AbpDaprPubSubProviderContributorContext
{
    public IServiceProvider ServiceProvider { get; }

    public List<DaprSubscriptionDefinition> Subscriptions { get; }

    public AbpDaprPubSubProviderContributorContext(IServiceProvider serviceProvider, List<DaprSubscriptionDefinition> daprSubscriptionModels)
    {
        ServiceProvider = serviceProvider;
        Subscriptions = daprSubscriptionModels;
    }
}
