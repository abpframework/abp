using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Volo.Abp.EntityFrameworkCore.Interceptors;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Sqlite;

[DependsOn(
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpEntityFrameworkCoreSqliteModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpSqliteOptions>(options =>
        {
            options.BusyTimeout = 5000;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEfCoreGlobalFilterOptions>(options =>
        {
            options.UseDbFunction = true;
        });

        var sqliteOptions = context.Services.ExecutePreConfiguredActions<AbpSqliteOptions>();
        if (sqliteOptions.BusyTimeout.HasValue)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.ConfigureDefaultOnConfiguring((dbContext, dbContextOptionsBuilder) =>
                {
                    dbContextOptionsBuilder.AddInterceptors(new SqliteBusyTimeoutSaveChangesInterceptor(sqliteOptions.BusyTimeout.Value));
                }, overrideExisting: false);
            });
        }
    }
}
