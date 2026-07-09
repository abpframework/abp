using Volo.Abp.Testing;

namespace Volo.Abp.BackgroundJobs;

public abstract class BackgroundJobWorkerTestBase : AbpIntegratedTest<AbpBackgroundJobWorkerTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
