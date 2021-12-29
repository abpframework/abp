using Volo.Abp.Testing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed;

public abstract class LocalDistributedEventBusTestBase : AbpIntegratedTest<EventBusTestModule>
{
    protected IUnitOfWorkManager UnitOfWorkManager;
    protected IDistributedEventBus DistributedEventBus;

    protected LocalDistributedEventBusTestBase()
    {
        UnitOfWorkManager = GetRequiredService<IUnitOfWorkManager>();
        DistributedEventBus = GetRequiredService<LocalDistributedEventBus>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
