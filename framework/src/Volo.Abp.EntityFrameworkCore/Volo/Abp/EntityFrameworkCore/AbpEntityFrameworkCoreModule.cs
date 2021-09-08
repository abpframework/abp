using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDbContextOptions>(options =>
            {
                options.PreConfigure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions
                        .ConfigureWarnings(warnings =>
                        {
                            warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
                        });
                });
            });

            context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            context.Services.AddTransient(typeof(DbContextEventOutbox<>));
        }
    }
}
