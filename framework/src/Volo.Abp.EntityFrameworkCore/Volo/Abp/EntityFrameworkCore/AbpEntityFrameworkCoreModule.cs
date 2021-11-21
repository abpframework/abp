using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore;

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
        context.Services.AddTransient(typeof(IDbContextEventOutbox<>), typeof(DbContextEventOutbox<>));
        context.Services.AddTransient(typeof(IDbContextEventInbox<>), typeof(DbContextEventInbox<>));

        context.Services.AddTransient(typeof(ISqlRawDbContextEventOutbox<>), typeof(SqlRawDbContextEventOutbox<>));
        context.Services.AddTransient(typeof(ISqlRawDbContextEventInbox<>), typeof(SqlRawDbContextEventInbox<>));
    }
}
