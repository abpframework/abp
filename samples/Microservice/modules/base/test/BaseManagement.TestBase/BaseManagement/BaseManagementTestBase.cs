using Volo.Abp;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    public abstract class BaseManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
