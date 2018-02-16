using Volo.Abp.Modularity;
using Volo.Abp.TestBase;

namespace Volo.Abp.Identity
{
    public abstract class AbpIdentityCommonTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
