using System;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed;

public interface IIncomingEventInfo : IHasExtraProperties
{
    Guid Id { get; }

    string MessageId { get; }

    string EventName { get; }

    byte[] EventData { get; }

    DateTime CreationTime { get; }
}
