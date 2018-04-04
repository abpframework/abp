using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MemoryDb;

namespace Volo.Abp.MemoryDb
{
    /* TODO: Consider to store objects as binary serialized in the memory, which makes unit tests more realistic.
     */

    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpMemoryDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMemoryDatabaseProvider<>), typeof(UnitOfWorkMemoryDatabaseProvider<>));
            services.AddAssemblyOf<AbpMemoryDbModule>();
        }
    }
}
