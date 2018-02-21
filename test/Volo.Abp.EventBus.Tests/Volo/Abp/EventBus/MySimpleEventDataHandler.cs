using Volo.Abp.DependencyInjection;

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