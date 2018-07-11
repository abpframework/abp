using Volo.Abp;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyModuleName
{
    public abstract class MyModuleNameTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
