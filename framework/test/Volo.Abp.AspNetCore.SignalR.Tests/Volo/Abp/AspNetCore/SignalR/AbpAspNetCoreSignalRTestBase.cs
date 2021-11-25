using Volo.Abp.Testing;

namespace Volo.Abp.AspNetCore.SignalR;

public abstract class AbpAspNetCoreSignalRTestBase : AbpIntegratedTest<AbpAspNetCoreSignalRTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
