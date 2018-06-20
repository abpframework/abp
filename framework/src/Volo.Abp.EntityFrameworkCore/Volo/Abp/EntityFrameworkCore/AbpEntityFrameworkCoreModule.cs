using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpDbContextOptions>(options =>
            {
                options.PreConfigure(context =>
                {
                    context.DbContextOptions
                        .ConfigureWarnings(warnings =>
                        {
                            warnings.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning);
                        });
                });
            });

            services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            services.AddAssemblyOf<AbpEntityFrameworkCoreModule>();
        }
    }
}
