using System;
using Volo.Abp.Data;

namespace Volo.Abp.EventBus.Distributed;

public interface IOutgoingEventInfo : IHasExtraProperties
{
    Guid Id { get; }

    string EventName { get; }

    byte[] EventData { get; }

    DateTime CreationTime { get; }
}
