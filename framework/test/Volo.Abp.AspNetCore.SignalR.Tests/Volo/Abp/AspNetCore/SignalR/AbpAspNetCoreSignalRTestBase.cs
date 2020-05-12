using Volo.Abp.Testing;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class AbpAspNetCoreSignalRTestBase : AbpIntegratedTest<AbpAspNetCoreSignalRTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}