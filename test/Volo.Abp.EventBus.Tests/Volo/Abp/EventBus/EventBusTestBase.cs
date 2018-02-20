using Volo.Abp.TestBase;

namespace Volo.Abp.EventBus
{
    public abstract class EventBusTestBase : AbpIntegratedTest<EventBusTestModule>
    {
        protected IEventBus EventBus;

        protected EventBusTestBase()
        {
            EventBus = GetRequiredService<IEventBus>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}