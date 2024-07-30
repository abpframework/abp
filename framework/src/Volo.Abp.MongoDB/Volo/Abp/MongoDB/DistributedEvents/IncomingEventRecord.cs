using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.MongoDB.DistributedEvents;

public class IncomingEventRecord :
    BasicAggregateRoot<Guid>,
    IHasExtraProperties,
    IHasCreationTime
{
    public static int MaxEventNameLength { get; set; } = 256;

    public ExtraPropertyDictionary ExtraProperties { get; private set; }

    public string MessageId { get; private set; } = default!;

    public string EventName { get; private set; } = default!;

    public byte[] EventData { get; private set; } = default!;

    public DateTime CreationTime { get; private set; }

    public bool Processed { get; set; }

    public DateTime? ProcessedTime { get; set; }

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
            CreationTime
        );

        foreach (var property in ExtraProperties)
        {
            info.SetProperty(property.Key, property.Value);
        }

        return info;
    }

    public void MarkAsProcessed(DateTime processedTime)
    {
        Processed = true;
        ProcessedTime = processedTime;
    }
}
