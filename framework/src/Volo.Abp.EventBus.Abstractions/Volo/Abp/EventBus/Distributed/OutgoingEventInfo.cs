using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed;

public class OutgoingEventInfo : IOutgoingEventInfo
{
    public static int MaxEventNameLength { get; set; } = 256;

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public Guid Id { get; }

    public string EventName { get; } = default!;

    public byte[] EventData { get; } = default!;

    public DateTime CreationTime { get; }

    protected OutgoingEventInfo()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public OutgoingEventInfo(
        Guid id,
        string eventName,
        byte[] eventData,
        DateTime creationTime)
    {
        Id = id;
        EventName = Check.NotNullOrWhiteSpace(eventName, nameof(eventName), MaxEventNameLength);
        EventData = eventData;
        CreationTime = creationTime;
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public void SetCorrelationId(string correlationId)
    {
        ExtraProperties[EventBusConsts.CorrelationIdHeaderName] = correlationId;
    }

    public string? GetCorrelationId()
    {
        return ExtraProperties.GetOrDefault(EventBusConsts.CorrelationIdHeaderName)?.ToString();
    }
}
