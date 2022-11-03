namespace Volo.Abp.AspNetCore.Mvc.Dapr.EventBus.Models;

public class AbpDaprSubscriptionRequest<T>
    where T : class
{
    public string PubSubName { get; set; }

    public string Topic { get; set; }

    public T Data { get; set; }
}
