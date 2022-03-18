namespace Volo.Abp.EventBus.Azure;

public class AbpAzureEventBusOptions
{
    public string ConnectionName { get; set; }

    public string SubscriberName { get; set; }

    public string TopicName { get; set; }

    public bool IsServiceBusDisabled { get; set; }
}
