using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus
{
    public class MySimpleEventDataHandler : IEventHandler<MySimpleEventData>, ISingletonDependency
    {
        public int TotalData { get; private set; }

        public void HandleEvent(MySimpleEventData eventData)
        {
            TotalData += eventData.Value;
        }
    }
}