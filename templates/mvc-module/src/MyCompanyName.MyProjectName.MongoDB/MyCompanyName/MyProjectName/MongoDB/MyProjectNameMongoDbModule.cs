using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    [DependsOn(
        typeof(MyProjectNameDomainModule),
        typeof(AbpMongoDbModule)
        )]
    public class MyProjectNameMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<MyProjectNameMongoDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
            });
        }
    }
}
