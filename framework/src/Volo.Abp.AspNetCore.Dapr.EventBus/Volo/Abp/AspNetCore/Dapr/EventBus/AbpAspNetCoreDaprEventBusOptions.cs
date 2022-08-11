namespace Volo.Abp.AspNetCore.Dapr;

public class AbpAspNetCoreDaprEventBusOptions
{
    public List<IAbpAspNetCoreDaprPubSubProviderContributor> Contributors { get; }

    public AbpAspNetCoreDaprEventBusOptions()
    {
        Contributors = new List<IAbpAspNetCoreDaprPubSubProviderContributor>();
    }
}
