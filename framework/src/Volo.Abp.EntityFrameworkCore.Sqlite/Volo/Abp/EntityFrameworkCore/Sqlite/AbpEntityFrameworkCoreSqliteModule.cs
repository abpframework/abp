using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Sqlite;

[DependsOn(
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpEntityFrameworkCoreSqliteModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEfCoreGlobalFilterOptions>(options =>
        {
            options.UseDbFunction = true;
        });
    }
}
