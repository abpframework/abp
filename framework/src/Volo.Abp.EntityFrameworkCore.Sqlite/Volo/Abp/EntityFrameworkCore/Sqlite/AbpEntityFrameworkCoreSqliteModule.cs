using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.Sqlite
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AbpEntityFrameworkCoreSqliteModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Transient(typeof(IDbContextProvider<>),
                typeof(AbpSqliteUnitOfWorkDbContextProvider<>)));
        }
    }
}
