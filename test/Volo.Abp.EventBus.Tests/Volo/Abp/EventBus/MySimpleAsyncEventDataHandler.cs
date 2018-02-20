using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus
{
    public class MySimpleAsyncEventDataHandler : IAsyncEventHandler<MySimpleEventData>, ISingletonDependency
    {
        public int TotalData { get; private set; }

        public Task HandleEventAsync(MySimpleEventData eventData)
        {
            TotalData += eventData.Value;
            return Task.CompletedTask;
        }
    }
}