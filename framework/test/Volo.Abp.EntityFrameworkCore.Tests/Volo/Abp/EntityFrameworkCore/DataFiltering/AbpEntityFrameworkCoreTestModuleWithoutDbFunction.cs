using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering;

// Disables UseDbFunction so the EF.Property global filter path is exercised
// (covers both ISoftDelete and IMultiTenant filters).
[DependsOn(typeof(AbpEntityFrameworkCoreTestModule))]
public class AbpEntityFrameworkCoreTestModuleWithoutDbFunction : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEfCoreGlobalFilterOptions>(options =>
        {
            options.UseDbFunction = false;
        });
    }
}
