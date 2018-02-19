using System;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus
{
    public class MySimpleTransientAsyncEventHandler : IAsyncEventHandler<MySimpleEventData>, IDisposable
    {
        public static int HandleCount { get; set; }

        public static int DisposeCount { get; set; }

        public Task HandleEventAsync(MySimpleEventData eventData)
        {
            ++HandleCount;
            return Task.FromResult(0);
        }

        public void Dispose()
        {
            ++DisposeCount;
        }
    }
}