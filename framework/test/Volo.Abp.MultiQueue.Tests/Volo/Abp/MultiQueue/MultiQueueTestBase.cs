using Volo.Abp.Testing;

namespace Volo.Abp.MultiQueue;
public class MultiQueueTestBase : AbpIntegratedTest<MultiQueueTestModule>
{
    protected IAbpMultiQueueFactory MultiQueueFactory { get; }
    public MultiQueueTestBase()
    {
        MultiQueueFactory = GetRequiredService<IAbpMultiQueueFactory>();
    }

    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
