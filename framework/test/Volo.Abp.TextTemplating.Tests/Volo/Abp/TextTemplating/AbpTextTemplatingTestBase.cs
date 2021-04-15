using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.TextTemplating
{
    public abstract class AbpTextTemplatingTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
