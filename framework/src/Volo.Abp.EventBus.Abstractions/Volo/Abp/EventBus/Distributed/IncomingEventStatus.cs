namespace Volo.Abp.EventBus.Distributed;

public enum IncomingEventStatus
{
    Pending = 0,

    Discarded = 1,

    Processed = 2
}
