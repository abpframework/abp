using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    [DependsOn(typeof(AbpCommonModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    public class AbpMongoDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddTransient(typeof(IMongoDatabaseProvider<>), typeof(UnitOfWorkMongoDatabaseProvider<>));
            services.AddAssemblyOf<AbpMongoDbModule>();
        }
    }
}
