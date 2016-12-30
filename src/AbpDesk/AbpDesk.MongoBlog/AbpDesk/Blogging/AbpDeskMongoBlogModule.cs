using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace AbpDesk.Blogging
{
    [DependsOn(typeof(AbpMongoDbModule))]
    public class AbpDeskMongoBlogModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoDbContext<AbpDeskMongoDbContext>(options =>
            {
                options.WithDefaultRepositories();
            });

            services.AddAssemblyOf<AbpDeskMongoBlogModule>();
        }
    }
}
