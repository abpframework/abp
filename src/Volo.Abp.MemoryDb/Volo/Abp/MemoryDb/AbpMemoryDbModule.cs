using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MemoryDb;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddModule))] //TODO: Is it possible to not depend DDD and seperate to another module?
    public class AbpMemoryDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMemoryDatabaseProvider<>), typeof(UnitOfWorkMemoryDatabaseProvider<>));
            services.AddAssemblyOf<AbpMemoryDbModule>();
        }
    }
}
