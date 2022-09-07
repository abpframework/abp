namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus;

public interface IAbpAspNetCoreMvcDaprPubSubProviderContributor
{
    Task ContributeAsync(AbpAspNetCoreMvcDaprPubSubProviderContributorContext context);
}
