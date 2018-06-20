using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    public abstract class TenantManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
