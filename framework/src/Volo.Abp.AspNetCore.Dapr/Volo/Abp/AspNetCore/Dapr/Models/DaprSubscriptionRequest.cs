namespace Volo.Abp.AspNetCore.Dapr.Models;

public class DaprSubscriptionRequest
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }
}
