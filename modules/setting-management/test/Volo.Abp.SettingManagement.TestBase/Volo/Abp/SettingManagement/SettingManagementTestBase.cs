using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement
{
    public class SettingManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
