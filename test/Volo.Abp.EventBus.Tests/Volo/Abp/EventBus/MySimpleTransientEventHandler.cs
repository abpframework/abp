using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    public class MySimpleTransientEventHandler : IAsyncEventHandler<MySimpleEventData>, IDisposable
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
}