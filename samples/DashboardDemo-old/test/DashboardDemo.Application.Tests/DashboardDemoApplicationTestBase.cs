using Volo.Abp;

namespace DashboardDemo
{
    public abstract class DashboardDemoApplicationTestBase : AbpIntegratedTest<DashboardDemoApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
