namespace Volo.Abp.EventBus.Distributed
{
    public abstract class LocalDistributedEventBusTestBase : AbpIntegratedTest<EventBusTestModule>
    {
        protected LocalDistributedEventBus LocalEventBus;

        protected LocalDistributedEventBusTestBase()
        {
            LocalEventBus = GetRequiredService<LocalDistributedEventBus>();
        }

        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}