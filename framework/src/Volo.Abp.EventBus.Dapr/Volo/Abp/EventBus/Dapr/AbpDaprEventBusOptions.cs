namespace Volo.Abp.EventBus.Dapr;

public class AbpDaprEventBusOptions
{
    public string PubSubName { get; set; }

    public AbpDaprEventBusOptions()
    {
        PubSubName = "pubsub";
    }
}
