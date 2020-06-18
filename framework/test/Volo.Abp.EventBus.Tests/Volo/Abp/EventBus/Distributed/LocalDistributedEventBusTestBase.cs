using Volo.Abp.Testing;

namespace Volo.Abp.EventBus.Distributed
{
    public abstract class LocalDistributedEventBusTestBase : AbpIntegratedTest<EventBusTestModule>
    {
        protected IDistributedEventBus DistributedEventBus;

        protected LocalDistributedEventBusTestBase()
        {
            DistributedEventBus = GetRequiredService<LocalDistributedEventBus>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}