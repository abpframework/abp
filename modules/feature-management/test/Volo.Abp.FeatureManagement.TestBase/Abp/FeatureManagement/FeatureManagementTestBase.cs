using Volo.Abp;
using Volo.Abp.Modularity;

namespace Abp.FeatureManagement
{
    public abstract class FeatureManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
