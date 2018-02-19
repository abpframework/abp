namespace Volo.Abp.EventBus
{
    public abstract class EventBusTestBase
    {
        protected IEventBus EventBus;

        protected EventBusTestBase()
        {
            EventBus = new EventBus();
        }
    }
}