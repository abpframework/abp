using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(typeof(DddDomainModule))]
    public class MongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.TryAddTransient(
                typeof(IMongoDbContextProvider<>),
                typeof(UnitOfWorkMongoDbContextProvider<>)
            );
        }
    }
}
