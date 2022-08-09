namespace Volo.Abp.AspNetCore.Dapr;

public class AbpAspNetCoreDaprOptions
{
    public List<IAbpDaprPubSubProviderContributor> Contributors { get; }

    public AbpAspNetCoreDaprOptions()
    {
        Contributors = new List<IAbpDaprPubSubProviderContributor>();
    }
}
