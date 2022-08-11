namespace Volo.Abp.AspNetCore.Dapr.Models;

public class AbpAspNetCoreDaprSubscriptionDefinition
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }

    public string Route { get; set; }
}
