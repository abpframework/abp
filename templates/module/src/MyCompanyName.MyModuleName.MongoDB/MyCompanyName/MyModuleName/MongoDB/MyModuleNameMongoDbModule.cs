using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyModuleName.MongoDB
{
    [DependsOn(
        typeof(MyModuleNameDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class MyModuleNameMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            MyModuleNameBsonClassMap.Configure();

            context.Services.AddMongoDbContext<MyModuleNameMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });

            context.Services.AddAssemblyOf<MyModuleNameMongoDbModule>();
        }
    }
}
