using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class AbpMongoDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMongoDatabaseProvider<>), typeof(UnitOfWorkMongoDatabaseProvider<>));
            services.AddAssemblyOf<AbpMongoDbModule>();
        }
    }
}
