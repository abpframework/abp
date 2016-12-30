using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Uow.MongoDB;

namespace Volo.Abp.MongoDB
{
    public class AbpMongoDbModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //TODO: ServiceCollection extensions to register mongodbcontext!

            services.TryAddTransient(typeof(IMongoDatabaseProvider<>), typeof(UnitOfWorkMongoDatabaseProvider<>));
            services.AddAssemblyOf<AbpMongoDbModule>();
        }
    }
}
