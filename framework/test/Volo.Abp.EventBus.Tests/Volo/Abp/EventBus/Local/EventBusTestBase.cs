using Volo.Abp.Testing;

namespace Volo.Abp.EventBus.Local
{
    public abstract class EventBusTestBase : AbpIntegratedTest<EventBusTestModule>
    {
        protected ILocalEventBus LocalEventBus;

        protected EventBusTestBase()
        {
            LocalEventBus = GetRequiredService<ILocalEventBus>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}