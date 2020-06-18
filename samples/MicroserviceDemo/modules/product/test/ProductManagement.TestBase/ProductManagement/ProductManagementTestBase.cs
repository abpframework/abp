using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace ProductManagement
{
    public abstract class ProductManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
