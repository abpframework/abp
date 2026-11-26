using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class DerivedEntityFilterTestModuleWithoutDbFunction : DerivedEntityFilterTestModuleBase<DerivedEntityFilterTestDbContext>
{
    protected override bool UseDbFunction => false;
}
