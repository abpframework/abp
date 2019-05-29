namespace Volo.Abp.BackgroundJobs
{
    public abstract class BackgroundJobsTestBase : AbpIntegratedTest<BackgroundJobsTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}