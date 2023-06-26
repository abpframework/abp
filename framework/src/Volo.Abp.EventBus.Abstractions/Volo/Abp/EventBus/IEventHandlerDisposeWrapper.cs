using System;

namespace Volo.Abp.EventBus;

public interface IEventHandlerDisposeWrapper : IDisposable
{
    IEventHandler EventHandler { get; }
}
