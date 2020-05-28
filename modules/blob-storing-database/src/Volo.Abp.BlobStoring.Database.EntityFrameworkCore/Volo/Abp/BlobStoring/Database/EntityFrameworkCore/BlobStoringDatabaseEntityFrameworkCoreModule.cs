using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BlobStoring.Database.EntityFrameworkCore
{
    [DependsOn(
        typeof(BlobStoringDatabaseDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BlobStoringDatabaseEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BlobStoringDatabaseDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}