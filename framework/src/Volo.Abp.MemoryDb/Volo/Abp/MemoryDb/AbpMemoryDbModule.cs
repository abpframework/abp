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
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient(typeof(IMemoryDatabaseProvider<>), typeof(UnitOfWorkMemoryDatabaseProvider<>));
        }
    }
}
