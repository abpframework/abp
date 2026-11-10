using Bench.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Bench.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BenchEntityFrameworkCoreModule),
    typeof(BenchApplicationContractsModule)
)]
public class BenchDbMigratorModule : AbpModule
{
}
