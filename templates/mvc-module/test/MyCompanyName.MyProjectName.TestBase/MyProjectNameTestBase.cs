using Volo.Abp;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
