using Volo.Abp.Testing;

namespace Volo.Abp.OperationRateLimiting;

public class OperationRateLimitingTestBase : AbpIntegratedTest<AbpOperationRateLimitingTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
