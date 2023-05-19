using Volo.Abp.Testing;

namespace Volo.Abp.Json;

public abstract class AbpJsonSystemTextJsonTestBase : AbpIntegratedTest<AbpJsonSystemTextJsonTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}

public abstract class AbpJsonNewtonsoftJsonTestBase : AbpIntegratedTest<AbpJsonNewtonsoftTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
