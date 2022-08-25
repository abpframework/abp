namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

public class AbpAspNetCoreMvcDaprEventBusOptions
{
    public List<IAbpAspNetCoreMvcDaprPubSubProviderContributor> Contributors { get; }

    public AbpAspNetCoreMvcDaprEventBusOptions()
    {
        Contributors = new List<IAbpAspNetCoreMvcDaprPubSubProviderContributor>();
    }
}
