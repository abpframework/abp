using Volo.Abp;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    public abstract class DocsTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
