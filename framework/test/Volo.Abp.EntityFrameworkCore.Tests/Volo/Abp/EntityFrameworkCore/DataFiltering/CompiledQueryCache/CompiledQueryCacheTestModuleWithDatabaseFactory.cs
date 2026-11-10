using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.CompiledQueryCache;

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class CompiledQueryCacheTestModuleWithDatabaseFactory : CompiledQueryCacheTestModuleBase
{
    protected override bool UseDbFunction => true;

    protected override bool AddCustomDatabase => true;
}
