namespace Volo.Abp.AspNetCore.Dapr;

public interface IAbpDaprPubSubProviderContributor
{
    Task ContributeAsync(AbpDaprPubSubProviderContributorContext context);
}
