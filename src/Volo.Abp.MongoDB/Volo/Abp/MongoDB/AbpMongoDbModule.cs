using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddModule))] //TODO: Is it possible to not depend DDD and seperate to another module?
    public class AbpMongoDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMongoDatabaseProvider<>), typeof(UnitOfWorkMongoDatabaseProvider<>));
            services.AddAssemblyOf<AbpMongoDbModule>();
        }
    }
}
