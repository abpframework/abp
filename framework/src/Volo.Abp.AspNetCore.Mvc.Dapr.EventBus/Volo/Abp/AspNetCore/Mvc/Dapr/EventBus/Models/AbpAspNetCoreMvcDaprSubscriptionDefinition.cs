namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;

public class AbpAspNetCoreMvcDaprSubscriptionDefinition
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }

    public string Route { get; set; }
}
