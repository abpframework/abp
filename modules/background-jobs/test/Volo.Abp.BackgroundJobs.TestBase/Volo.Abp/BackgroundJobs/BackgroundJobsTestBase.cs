using Volo.Abp;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    public abstract class BackgroundJobsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
