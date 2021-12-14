using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace Volo.Abp.FeatureManagement;

public abstract class FeatureManagementTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
