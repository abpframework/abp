namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventSent
{
    public DistributedEventSource Source { get; set; }

    public string EventName { get; set; }

    public object EventData { get; set; }
}
