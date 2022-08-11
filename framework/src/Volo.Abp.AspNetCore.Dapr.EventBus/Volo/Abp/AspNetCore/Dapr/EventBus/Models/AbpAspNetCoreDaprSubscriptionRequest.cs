namespace Volo.Abp.AspNetCore.Dapr.Models;

public class AbpAspNetCoreDaprSubscriptionRequest
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }
}
