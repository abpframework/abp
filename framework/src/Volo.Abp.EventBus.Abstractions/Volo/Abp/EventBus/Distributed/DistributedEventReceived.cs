namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventReceived
{
    public DistributedEventSource Source { get; set; }

    public string EventName { get; set; }

    public object EventData { get; set; }
}
