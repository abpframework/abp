using Volo.Abp.Modularity;

namespace Volo.Abp.TestApp.Testing
{
    public abstract class TestAppTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}