using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Sqlite
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreSqliteModule : AbpModule
    {
    }
}
