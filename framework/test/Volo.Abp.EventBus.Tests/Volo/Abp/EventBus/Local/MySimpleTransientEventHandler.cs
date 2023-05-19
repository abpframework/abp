using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Local;

public class MySimpleTransientEventHandler : ILocalEventHandler<MySimpleEventData>, IDisposable
{
    public static int HandleCount { get; set; }

    public static int DisposeCount { get; set; }

    public Task HandleEventAsync(MySimpleEventData eventData)
    {
        ++HandleCount;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        ++DisposeCount;
    }
}
