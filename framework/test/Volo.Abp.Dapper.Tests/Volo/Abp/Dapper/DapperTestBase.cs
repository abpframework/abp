using Volo.Abp.Testing;

namespace Volo.Abp.Dapper;

public abstract class DapperTestBase : AbpIntegratedTest<AbpDapperTestModule>
{
    protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
    {
        options.UseAutofac();
    }
}
