namespace Volo.Abp.AspNetCore.Dapr.Models;

public class DaprSubscriptionDefinition
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }

    public string Route { get; set; }
}
