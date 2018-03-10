using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.EntityFrameworkCore;

namespace Volo.Abp.EntityFrameworkCore
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddApplicationModule))] //TODO: Is it possible to not depend DDD and seperate to another module?
    public class AbpEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
            services.AddAssemblyOf<AbpEntityFrameworkCoreModule>();
        }
    }
}
