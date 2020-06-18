using Volo.Abp.Testing;

namespace Volo.Abp.BackgroundJobs
{
    public abstract class BackgroundJobsTestBase : AbpIntegratedTest<AbpBackgroundJobsTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}