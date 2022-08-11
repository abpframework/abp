namespace Volo.Abp.AspNetCore.Dapr;

public interface IAbpAspNetCoreDaprPubSubProviderContributor
{
    Task ContributeAsync(AbpAspNetCoreDaprPubSubProviderContributorContext context);
}
