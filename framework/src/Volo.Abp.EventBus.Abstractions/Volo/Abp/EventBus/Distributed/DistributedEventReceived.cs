namespace Volo.Abp.EventBus.Distributed;

public class DistributedEventReceived
{
    public DistributedEventSource Source { get; set; }

    public string EventName { get; set; } = default!;

    public object EventData { get; set; } = default!;
}
