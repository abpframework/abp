using System;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Distributed;

namespace Volo.Abp.EntityFrameworkCore.DistributedEvents;

public class OutgoingEventRecord :
    BasicAggregateRoot<Guid>,
    IHasExtraProperties,
    IHasCreationTime
{
    public static int MaxEventNameLength { get; set; } = 256;

    public ExtraPropertyDictionary ExtraProperties { get; private set; }

    public string EventName { get; private set; }

    public byte[] EventData { get; private set; }

    public DateTime CreationTime { get; private set; }

    protected OutgoingEventRecord()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public OutgoingEventRecord(
        OutgoingEventInfo eventInfo)
        : base(eventInfo.Id)
    {
        EventName = eventInfo.EventName;
        EventData = eventInfo.EventData;
        CreationTime = eventInfo.CreationTime;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public OutgoingEventInfo ToOutgoingEventInfo()
    {
        return new OutgoingEventInfo(
            Id,
            EventName,
            EventData,
            CreationTime
        );
    }
}
