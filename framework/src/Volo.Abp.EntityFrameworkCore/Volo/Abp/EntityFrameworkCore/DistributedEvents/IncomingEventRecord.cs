using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class IncomingEventRecord :
    BasicAggregateRoot<Guid>,
    IIncomingEventInfo,
    IHasExtraProperties,
    IHasCreationTime
{
    public static int MaxEventNameLength { get; set; } = 256;

    public ExtraPropertyDictionary ExtraProperties { get; private set; }

    public string MessageId { get; private set; } = default!;

    public string EventName { get; private set; } = default!;

    public byte[] EventData { get; private set; } = default!;

    public DateTime CreationTime { get; private set; }

    public IncomingEventStatus Status { get; set; } = IncomingEventStatus.Pending;

    public DateTime? HandledTime { get; set; }

    public int RetryCount { get; set; } = 0;

    public DateTime? NextRetryTime { get; set; } = null;

    protected IncomingEventRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public IncomingEventRecord(
        IncomingEventInfo eventInfo)
        : base(eventInfo.Id)
    {
        MessageId = eventInfo.MessageId;
        EventName = eventInfo.EventName;
        EventData = eventInfo.EventData;
        CreationTime = eventInfo.CreationTime;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
        foreach (var property in eventInfo.ExtraProperties)
        {
            this.SetProperty(property.Key, property.Value);
        }
    }

    public IncomingEventInfo ToIncomingEventInfo()
    {
        var info = new IncomingEventInfo(
            Id,
            MessageId,
            EventName,
            EventData,
            CreationTime,
            Status,
            HandledTime,
            RetryCount,
            NextRetryTime
        );

        foreach (var property in ExtraProperties)
        {
            info.SetProperty(property.Key, property.Value);
        }

        return info;
    }

    public void MarkAsProcessed(DateTime processedTime)
    {
        Status = IncomingEventStatus.Processed;
        HandledTime = processedTime;
    }

    public void MarkAsDiscarded(DateTime discardedTime)
    {
        Status = IncomingEventStatus.Discarded;
        HandledTime = discardedTime;
    }

    public void RetryLater(int retryCount, DateTime nextRetryTime)
    {
        Status = IncomingEventStatus.Pending;
        NextRetryTime = nextRetryTime;
        RetryCount = retryCount;
    }
}
