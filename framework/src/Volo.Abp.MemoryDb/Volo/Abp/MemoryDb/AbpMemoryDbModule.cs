using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Repositories.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MemoryDb;

namespace Volo.Abp.MemoryDb
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpMemoryDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient(typeof(IMemoryDatabaseProvider<>), typeof(UnitOfWorkMemoryDatabaseProvider<>));
            context.Services.TryAddTransient(typeof(IMemoryDatabaseCollection<>), typeof(MemoryDatabaseCollection<>));
        }
    }
}
