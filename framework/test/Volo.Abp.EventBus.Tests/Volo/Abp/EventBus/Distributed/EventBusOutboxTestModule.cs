using Volo.Abp.Modularity;

namespace Volo.Abp.EventBus.Distributed;

[DependsOn(typeof(EventBusTestModule))]
public class EventBusOutboxTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDistributedEventBusOptions>(options =>
        {
            options.Outboxes.Configure(config =>
            {
                config.ImplementationType = typeof(FakeEventOutbox);
            });
        });
    }
}
