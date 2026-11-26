using Microsoft.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.DataFiltering.DerivedEntity;

public class DerivedEntityTptFilterTestDbContext : DerivedEntityFilterTestDbContextBase<DerivedEntityTptFilterTestDbContext>
{
    protected override bool UseTpt => true;

    public DerivedEntityTptFilterTestDbContext(DbContextOptions<DerivedEntityTptFilterTestDbContext> options) : base(options)
    {
    }
}

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class DerivedEntityTptFilterTestModule : DerivedEntityFilterTestModuleBase<DerivedEntityTptFilterTestDbContext>
{
    protected override bool UseDbFunction => true;
}

[DependsOn(typeof(AbpEntityFrameworkCoreSqliteModule), typeof(AbpAutofacModule))]
public class DerivedEntityTptFilterTestModuleWithoutDbFunction : DerivedEntityFilterTestModuleBase<DerivedEntityTptFilterTestDbContext>
{
    protected override bool UseDbFunction => false;
}

public class DerivedEntityTptFilter_Tests : DerivedEntityFilterTestBase<DerivedEntityTptFilterTestModule>
{
    protected override bool UseDbFunction => true;
}

public class DerivedEntityTptFilter_WithoutDbFunction_Tests : DerivedEntityFilterTestBase<DerivedEntityTptFilterTestModuleWithoutDbFunction>
{
    protected override bool UseDbFunction => false;
}
