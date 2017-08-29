using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MemoryDb;

namespace Volo.Abp.MemoryDb
{
    public class AbpMemoryDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMemoryDatabaseProvider<>), typeof(UnitOfWorkMemoryDatabaseProvider<>));
            services.AddAssemblyOf<AbpMemoryDbModule>();
        }
    }
}
