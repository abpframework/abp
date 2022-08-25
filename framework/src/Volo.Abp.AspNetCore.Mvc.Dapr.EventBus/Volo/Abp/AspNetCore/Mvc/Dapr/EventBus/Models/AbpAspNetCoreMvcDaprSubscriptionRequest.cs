namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;

public class AbpAspNetCoreMvcDaprSubscriptionRequest
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }
}
