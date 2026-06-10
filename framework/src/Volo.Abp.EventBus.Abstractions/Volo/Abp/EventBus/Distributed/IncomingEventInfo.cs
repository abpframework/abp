using System;
using System.Collections.Generic;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed;

public class IncomingEventInfo : IIncomingEventInfo
{
    public static int MaxEventNameLength { get; set; } = 256;

    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public Guid Id { get; }

    public string MessageId { get; } = default!;

    public string EventName { get; } = default!;

    public byte[] EventData { get; } = default!;

    public DateTime CreationTime { get; }

    public IncomingEventStatus Status { get; set; } = IncomingEventStatus.Pending;

    public DateTime? HandledTime { get; set; }

    public int RetryCount { get; set; } = 0;

    public DateTime? NextRetryTime { get; set; } = null;

    protected IncomingEventInfo()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public IncomingEventInfo(
        Guid id,
        string messageId,
        string eventName,
        byte[] eventData,
        DateTime creationTime,
        IncomingEventStatus status = IncomingEventStatus.Pending,
        DateTime? handledTime = null,
        int retryCount = 0,
        DateTime? nextRetryTime = null)
    {
        Id = id;
        MessageId = messageId;
        EventName = Check.NotNullOrWhiteSpace(eventName, nameof(eventName), MaxEventNameLength);
        EventData = eventData;
        CreationTime = creationTime;
        Status = status;
        HandledTime = handledTime;
        RetryCount = retryCount;
        NextRetryTime = nextRetryTime;
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
